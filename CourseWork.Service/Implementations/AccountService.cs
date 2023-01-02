using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Account;
using CourseWork.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IRepository<User> _userRepository;

		public AccountService(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<IBaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
		{
			try
			{
				var user = (await _userRepository.GetAll()).FirstOrDefault(u => u.Name == model.Name);

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
					Password = model.Password,
					RegistrationDate= DateTime.Now,
					Role = Role.User,
					IsBlocked = false,
				};

				await _userRepository.Create(user);

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
				var user = (await _userRepository.GetAll()).FirstOrDefault(u => u.Name == model.Name);

				if (user == null)
				{
					return new BaseResponse<ClaimsIdentity>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Пользователя не найден"
					};
				}
				if (user.Password != model.Password)
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
				var user = (await _userRepository.GetAll()).FirstOrDefault(x => x.Name == model.Name);
				if (user == null)
				{
					return new BaseResponse<bool>()
					{
						StatusCode = StatusCode.NotFound,
						Description = "Пользователь не найден"
					};
				}

				user.Password = model.NewPassword;

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
