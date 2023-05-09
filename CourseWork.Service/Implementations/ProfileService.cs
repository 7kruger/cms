using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Response;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Service.Implementations
{
	public class ProfileService : IProfileService
	{
		private readonly IRepository<DAL.Entities.Profile> _profileRepository;
		private readonly IRepository<Collection> _collectionRepository;
		private readonly IRepository<Item> _itemRepository;
		private readonly ILikeRepository _likeRepository;
		private readonly IRepository<User> _userRepository;
		private readonly ICloudStorageService _cloudStorageService;
		private readonly IMapper _mapper;

		public ProfileService(IRepository<DAL.Entities.Profile> profileRepository,
							  IRepository<Collection> collectionRepository,
							  IRepository<Item> itemRepository,
							  ILikeRepository likeRepository,
							  IRepository<User> userRepository,
							  ICloudStorageService cloudStorageService,
							  IMapper mapper)
		{
			_profileRepository = profileRepository;
			_collectionRepository = collectionRepository;
			_itemRepository = itemRepository;
			_likeRepository = likeRepository;
			_userRepository = userRepository;
			_cloudStorageService = cloudStorageService;
			_mapper = mapper;
		}

		public async Task<ProfileModel> Get(string name)
		{
			try
			{
				var profile = await _profileRepository.GetAll()
					.FirstOrDefaultAsync(p => p.User.Name == name);

				if (profile == null)
				{
					return null;
				}

				var collectionsCount = _collectionRepository.GetAll().Where(c => c.Author == name).Count();
				var itemsCount = _itemRepository.GetAll().Where(i => i.Author == name).Count();
				var likesCount = _likeRepository.GetAll().Where(l => l.UserName == name).Count();

				var profileViewModel = new ProfileModel
				{
					Id = profile.Id,
					Username = profile.User.Name,
					ImgUrl = profile.ImgUrl,
					CollectionsCreated = collectionsCount,
					ItemsCreated = itemsCount,
					LikesCount = likesCount
				};

				return profileViewModel;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<bool> Update(ProfileModel model, IFormFile image)
		{
			try
			{
				var profile = await _profileRepository.GetAll().FirstOrDefaultAsync(p => p.User.Name == model.Username);

				if (profile == null)
				{
					return false;
				}

				if (image != null)
				{
					profile.ImgUrl = await _cloudStorageService.UpdateAsync(image, "/profiles", model.Id.ToString());
				}

				await _profileRepository.Update(profile);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task Create(string username)
		{
			var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == username);

			if (user != null)
			{
				var profile = new DAL.Entities.Profile
				{
					ImgUrl = "/images/person.svg",
					UserId = user.Id,
				};

				await _profileRepository.Create(profile);
			}
		}
	}
}
