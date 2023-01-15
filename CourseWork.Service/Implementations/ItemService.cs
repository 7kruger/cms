using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Item;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class ItemService : IItemService
	{
		private readonly IRepository<Item> _itemRepository;
		private readonly IRepository<Collection> _collectionRepository;
		private readonly ICloudStorageService _cloudStorageService;
		private readonly ILikeRepository _likeRepository;
		private readonly ICommentRepository _commentRepository;

		public ItemService(IRepository<Item> itemRepository,
						   IRepository<Collection> collectionRepository,
						   ICloudStorageService cloudStorageService,
						   ILikeRepository likeRepository,
						   ICommentRepository commentRepository)
		{
			_itemRepository = itemRepository;
			_collectionRepository = collectionRepository;
			_cloudStorageService = cloudStorageService;
			_likeRepository = likeRepository;
			_commentRepository = commentRepository;
		}

		public async Task<IBaseResponse<List<ItemViewModel>>> GetItems()
		{
			try
			{
				var items = await _itemRepository.GetAll().ToListAsync();

				if (items == null)
				{
					return new BaseResponse<List<ItemViewModel>>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Найдено 0 элементов"
					};
				}

				var data = items.Select(x =>
				{
					return new ItemViewModel()
					{
						Id = x.Id,
						Name = x.Name,
						Author = x.Author,
						Content = x.Content,
						Date = x.Date,
						ImgRef = x.ImgRef,
						CollectionId = x.CollectionId,
						LikesCount = _likeRepository.GetAll().Where(l => l.SrcId == x.Id).Count(),
						CommentsCount  = _commentRepository.GetAll().Where(c => c.SrcId == x.Id).Count(),
					};
                }).OrderByDescending(i => i.Date)
				  .ToList();

				return new BaseResponse<List<ItemViewModel>>
				{
					StatusCode = StatusCode.OK,
					Data = data
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<ItemViewModel>>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetItems] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<ItemViewModel>> GetItem(string id)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == id);

				if (item == null)
				{
					return new BaseResponse<ItemViewModel>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Item not found"
					};
				}

				var collectionName = (await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == item.CollectionId))?.Name;

				var data = new ItemViewModel
				{
					Id = item.Id,
					CollectionId = item.CollectionId,
					Name = item.Name,
					Author = item.Author,
					Content = item.Content,
					Date = item.Date,
					ImgRef = item.ImgRef,
					CollectionName = collectionName,
					LikesCount = _likeRepository.GetAll().Where(l=> l.SrcId == item.Id).Count()
				};

				return new BaseResponse<ItemViewModel>
				{
					StatusCode = StatusCode.OK,
					Data = data
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<ItemViewModel>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetItem] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<Item>> Create(CreateItemViewModel model, string username, IFormFile image)
		{
			try
			{
				var item = new Item
				{
					Id = Guid.NewGuid().ToString(),
					Name = model.Name,
					Content = model.Content,
					Author = username,
					Date = DateTime.Now,
				};

				item.ImgRef = await _cloudStorageService.UploadAsync(image, "/items", item.Id);

				await _itemRepository.Create(item);

				return new BaseResponse<Item>
				{
					StatusCode = StatusCode.OK,
					Data = item
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Item>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[CreateItem] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<Item>> Edit(ItemViewModel model, IFormFile image)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == model.Id);

				if (item == null)
				{
					return new BaseResponse<Item>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Item not found"
					};
				}

				item.Name = model.Name;
				item.Content = model.Content;

				if (image != null)
				{
					item.ImgRef = await _cloudStorageService.UpdateAsync(image, "/items", item.Id);
				}

				await _itemRepository.Update(item);

				return new BaseResponse<Item>
				{
					StatusCode = StatusCode.OK,
					Data = item
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Item>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[EditItem] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> Delete(string id)
		{
			try
			{
				var item = await _itemRepository.GetAll().FirstOrDefaultAsync(i => i.Id == id);

				if (item == null)
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Item not found"
					};
				}

				await _cloudStorageService.DeleteAsync("/items", item.Id);
				await _itemRepository.Delete(item);

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
					Description = $"[DeleteItem] : {ex.Message}"
				};
			}
		}
	}
}
