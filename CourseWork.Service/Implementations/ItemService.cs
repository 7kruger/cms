using System;
using System.Collections.Generic;
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
	public class ItemService : IItemService
	{
		private readonly IRepository<Item> _itemRepository;
		private readonly IRepository<Collection> _collectionRepository;
		private readonly ICloudStorageService _cloudStorageService;
		private readonly ILikeRepository _likeRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _mapper;

		public ItemService(IRepository<Item> itemRepository,
						   IRepository<Collection> collectionRepository,
						   ICloudStorageService cloudStorageService,
						   ILikeRepository likeRepository,
						   ICommentRepository commentRepository,
						   IMapper mapper)
		{
			_itemRepository = itemRepository;
			_collectionRepository = collectionRepository;
			_cloudStorageService = cloudStorageService;
			_likeRepository = likeRepository;
			_commentRepository = commentRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ItemModel>> GetItems()
		{
			try
			{
				var items = await _itemRepository.GetAll().ToListAsync();

				if (items == null)
				{
					return null;
				}

				var model = _mapper.Map<List<ItemModel>>(items);

				var likes = await _likeRepository.GetAll().ToListAsync();
				var comments = await _commentRepository.GetAll().ToListAsync();

				model.ForEach(item =>
				{
					item.LikesCount = likes.Where(l => l.SrcId == item.Id).Count();
					item.CommentsCount = comments.Where(c => c.SrcId == item.Id).Count();
				});

				return model;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<ItemModel> GetItem(string id)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == id);

				if (item == null)
				{
					return null;
				}

				var collectionTitle = (await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == item.CollectionId))?.Title;

				var model = _mapper.Map<ItemModel>(item);

				var likes = await _likeRepository.GetAll().ToListAsync();
				var comments = await _commentRepository.GetAll().ToListAsync();

				model.LikesCount = likes.Where(l => l.SrcId == item.Id).Count();
				model.CommentsCount = comments.Where(c => c.SrcId == item.Id).Count();

				return model;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<ItemModel> Create(ItemModel model, string username, IFormFile image)
		{
			try
			{
				model.Id = Guid.NewGuid().ToString();
				model.Author = username;
				model.Date = DateTime.Now;

				model.ImgUrl = await _cloudStorageService.UploadAsync(image, "/items", model.Id);

				var item = _mapper.Map<Item>(model);

				await _itemRepository.Create(item);

				return _mapper.Map<ItemModel>(item);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<ItemModel> Edit(ItemModel model, IFormFile image)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);

				if (item == null)
				{
					return null;
				}

				item.Title = model.Title;
				item.Description = model.Description;

				if (image != null)
				{
					item.ImgUrl = await _cloudStorageService.UpdateAsync(image, "/items", item.Id);
				}

				await _itemRepository.Update(item);

				return _mapper.Map<ItemModel>(item);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<bool> Delete(string id)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == id);

				if (item == null)
				{
					return false;
				}

				await _cloudStorageService.DeleteAsync("/items", item.Id);
				await _itemRepository.Delete(item);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
