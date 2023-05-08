using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Enum;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Service.Implementations
{
	public class AdminService : IAdminService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<User> _userRepository;

		public AdminService(IRepository<User> userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<UserModel>> GetUsers()
		{
			try
			{
				var users = await _userRepository.GetAll().ToListAsync();

				return _mapper.Map<IEnumerable<UserModel>>(users);
			}
			catch (Exception)
			{
				return new List<UserModel>();
			}
		}

		public async Task<bool> Do(ActionType type, int[] selectedUsers)
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

				return true;
			}
			catch (Exception)
			{
				return false;
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
