using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Helpers;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Account;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IRepository<User> _userRepository;
		private readonly IProfileService _profileService;

		public AccountService(IRepository<User> userRepository, IProfileService profileService)
		{
			_userRepository = userRepository;
			_profileService = profileService;
		}

		public async Task<IBaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == model.Name);

				if (user != null)
				{
					return new BaseResponse<ClaimsIdentity>
					{
						Description = "Пользователь с таким именем уже существует"
					};
				}

				user = new User
				{
					Name = model.Name,
					Password = HashPasswordHelper.HashPassword(model.Password),
					RegistrationDate = DateTime.Now,
					Role = Role.User,
					IsBlocked = false,
				};

				await _userRepository.Create(user);
				await _profileService.Create(user.Name);

				var result = GetClaimsIdentity(user);

				return new BaseResponse<ClaimsIdentity>
				{
					StatusCode = StatusCode.OK,
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<ClaimsIdentity>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[Register] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == model.Name);

				if (user == null)
				{
					return new BaseResponse<ClaimsIdentity>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Пользователя с таким именем не существует"
					};
				}
				if (user.Password != HashPasswordHelper.HashPassword(model.Password))
				{
					return new BaseResponse<ClaimsIdentity>
					{
						Description = "Неверный логин или пароль"
					};
				}

				var result = GetClaimsIdentity(user);

				return new BaseResponse<ClaimsIdentity>
				{
					StatusCode = StatusCode.OK,
					Data = result
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<ClaimsIdentity>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[Login] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == model.Name);
				if (user == null)
				{
					return new BaseResponse<bool>()
					{
						StatusCode = StatusCode.NotFound,
						Description = "Пользователь не найден"
					};
				}

				user.Password = HashPasswordHelper.HashPassword(model.NewPassword);

				await _userRepository.Update(user);

				return new BaseResponse<bool>()
				{
					Data = true,
					StatusCode = StatusCode.OK,
					Description = "Пароль обновлен"
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					Description = $"[ChangePassword] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		private ClaimsIdentity GetClaimsIdentity(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString().ToLower())
			};
			return new ClaimsIdentity(claims, "ApplicationCookie",
				ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		}
	}
}
