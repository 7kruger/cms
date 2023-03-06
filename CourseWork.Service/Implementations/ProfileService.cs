using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Profile;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
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
		private readonly IRepository<User> _userRepository;
		private readonly ICloudStorageService _cloudStorageService;

		public ProfileService(IRepository<Profile> profileRepository,
							  IRepository<Collection> collectionRepository,
							  IRepository<Item> itemRepository,
							  ILikeRepository likeRepository,
							  IRepository<User> userRepository,
							  ICloudStorageService cloudStorageService)
		{
			_profileRepository = profileRepository;
			_collectionRepository = collectionRepository;
			_itemRepository = itemRepository;
			_likeRepository = likeRepository;
			_userRepository = userRepository;
			_cloudStorageService = cloudStorageService;
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
					Id = profile.Id,
					Username = profile.User.Name,
					ImgRef = profile.ImgRef,
					RegistrationDate = profile.User.RegistrationDate,
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

		public async Task<IBaseResponse<bool>> Update(ProfileViewModel model, IFormFile image)
		{
			try
			{
				var profile = await _profileRepository.GetAll().FirstOrDefaultAsync(p => p.User.Name == model.Username);

				if (profile == null)
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.OK,
						Description = "Профиль не найден"
					};
				}

				if (image != null)
				{
					profile.ImgRef = await _cloudStorageService.UpdateAsync(image, "/profiles", model.Id.ToString());
				}

				await _profileRepository.Update(profile);

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[ProfileService] : {ex.Message}"
				};
			}
		}

		public async Task Create(string username)
		{
			var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == username);

			if (user != null)
			{
				var profile = new Profile
				{
					ImgRef = "/images/person.svg",
					UserId = user.Id,
				};

				await _profileRepository.Create(profile);
			}
		}
	}
}
