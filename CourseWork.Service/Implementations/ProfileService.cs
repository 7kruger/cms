using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Profile;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class ProfileService : IProfileService
	{
		private readonly IRepository<Profile> _profileRepository;
		private readonly IRepository<Collection> _collectionRepository;
		private readonly IRepository<Item> _itemRepository;
		private readonly ILikeRepository _likeRepository;

		public ProfileService(IRepository<Profile> profileRepository,
							  IRepository<Collection> collectionRepository,
							  IRepository<Item> itemRepository,
							  ILikeRepository likeRepository)
		{
			_profileRepository = profileRepository;
			_collectionRepository = collectionRepository;
			_itemRepository = itemRepository;
			_likeRepository = likeRepository;
		}

		public async Task<IBaseResponse<ProfileViewModel>> Get(string name)
		{
			try
			{
				var profile = await _profileRepository.GetAll()
					.FirstOrDefaultAsync(p => p.User.Name == name);

				if (profile == null)
				{
					return new BaseResponse<ProfileViewModel>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Профиль не найден"
					};
				}

				var collectionsCount = _collectionRepository.GetAll().Where(c => c.Author == name).Count();
				var itemsCount = _itemRepository.GetAll().Where(i => i.Author == name).Count();
				var likesCount = _likeRepository.GetAll().Where(l => l.UserName == name).Count();

				var profileViewModel = new ProfileViewModel
				{
					Username = profile.User.Name,
					ImgRef = profile.ImgRef,
					CollectionsCount = collectionsCount,
					ItemsCount = itemsCount,
					LikesCount = likesCount
				};

				return new BaseResponse<ProfileViewModel>
				{
					StatusCode = StatusCode.OK,
					Data = profileViewModel
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<ProfileViewModel>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[ProfileService] : {ex.Message}"
				};
			}
		}
		public Task<IBaseResponse<bool>> Create(Profile profile)
		{
			throw new System.NotImplementedException();
		}

		public Task<IBaseResponse<bool>> Delete(int id)
		{
			throw new System.NotImplementedException();
		}

		public Task<IBaseResponse<IEnumerable<ProfileViewModel>>> GetAll()
		{
			throw new System.NotImplementedException();
		}

		public Task<IBaseResponse<bool>> Update(Profile profile)
		{
			throw new System.NotImplementedException();
		}
	}
}
