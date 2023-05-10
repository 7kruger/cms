using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Helpers;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Service.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<User> _userRepository;
		private readonly IProfileService _profileService;

		public AccountService(IRepository<User> userRepository, 
			IProfileService profileService, 
			IMapper mapper)
		{
			_userRepository = userRepository;
			_profileService = profileService;
			_mapper = mapper;
		}

		public async Task<IdentityResult> Register(UserModel model)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == model.Name);

				if (user != null)
				{
					return new IdentityResult(
						errors: new List<string>()
						{
							"Пользователь с таким именем уже существует"
						},
						succeeded: false,
						claims: null);
				}

				model.Password = HashPasswordHelper.HashPassword(model.Password);
				model.RegistrationDate = DateTime.Now;
				model.Role = Role.User;
				model.IsBlocked = false;

				await _userRepository.Create(_mapper.Map<User>(model));
				await _profileService.Create(user.Name);

				var result = GetClaimsIdentity(user);

				return new IdentityResult(
					errors: new List<string>(),
					succeeded: true,
					claims: result);
			}
			catch (Exception)
			{
				return new IdentityResult(
					errors: new List<string>
					{
						"Не удалось зарегистрировать пользователя, попробуйте позже"
					},
					succeeded: false,
					claims: null);
			}
		}

		public async Task<IdentityResult> Login(UserModel model)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == model.Name);

				if (user == null)
				{
					return new IdentityResult(
							errors: new List<string>
							{
								"Пользователя с таким именем не существует"
							},
							succeeded: false,
							claims: null);
				}
				if (user.Password != HashPasswordHelper.HashPassword(model.Password))
				{
					return new IdentityResult(
							errors: new List<string>
							{
								"Неверный логин или пароль"
							},
							succeeded: false,
							claims: null);
				}
				if (user.IsBlocked)
				{
					return new IdentityResult(
							errors: new List<string>
							{
								"Пользователь заблокирован"
							},
							succeeded: false,
							claims: null);
				}

				var result = GetClaimsIdentity(user);

				return new IdentityResult(
					errors: null,
					succeeded: true,
					claims: result);
			}
			catch (Exception)
			{
				return new IdentityResult(
					errors: new List<string>
					{
						"Не удалось войти в аккаунт, попробуйте позже"
					},
					succeeded: false,
					claims: null);
			}
		}

		public async Task<bool> ChangePassword(string username, string newPassword)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == username);

				if (user == null)
				{
					return false;
				}

				user.Password = HashPasswordHelper.HashPassword(newPassword);
				await _userRepository.Update(user);

				return true;
			}
			catch (Exception)
			{
				return false;
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
