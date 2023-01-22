using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class AdminService : IAdminService
	{
		private readonly IRepository<User> _userRepository;

		public AdminService(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<IBaseResponse<List<User>>> GetUsers()
		{
			try
			{
				var users = await _userRepository.GetAll().ToListAsync();

				return new BaseResponse<List<User>>
				{
					StatusCode = StatusCode.OK,
					Data = users
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<User>>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"{ex.Message}",
				};
			}
		}

		public async Task<IBaseResponse<bool>> Do(ActionType type, int[] selectedUsers)
		{
			try
			{
				var users = await _userRepository.GetAll().Where(x => selectedUsers.Contains(x.Id)).ToListAsync();
				
				switch (type)
				{
					case ActionType.Block:
						await Block(users);
						break;
					case ActionType.Unlock:
						await Unlock(users);
						break;
					case ActionType.AddToAdmin:
						await AddToAdmin(users);
						break;
					case ActionType.RemoveFromAdmin:
						await RemoveFromAdmin(users);
						break;
					case ActionType.Delete:
						await Delete(users);
						break;
				}

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"{ex.Message}",
				};
			}
		}

		private async Task Unlock(List<User> users)
		{
			foreach (var user in users)
			{
				user.IsBlocked = false;
				await _userRepository.Update(user);
			}
		}
		private async Task AddToAdmin(List<User> users)
		{
			foreach (var user in users)
			{
				user.Role = Role.Admin;
				await _userRepository.Update(user);
			}
		}
		private async Task Block(List<User> users)
		{
			foreach (var user in users)
			{
				user.IsBlocked = true;
				await _userRepository.Update(user);
			}
		}
		private async Task RemoveFromAdmin(List<User> users)
		{
			foreach (var user in users)
			{
				user.Role = Role.User;
				await _userRepository.Update(user);
			}
		}
		private async Task Delete(List<User> users)
		{
			foreach (var user in users)
			{
				await _userRepository.Delete(user);
			}
		}
	}
}
