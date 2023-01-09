using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Collection;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class CollectionService : ICollectionService
	{
		private readonly IRepository<Collection> _collectionRepository;
		private readonly IRepository<Item> _itemRepository;
		private readonly ICloudStorageService _cloudStorageService;
		private readonly IRepository<Tag> _tagRepository;

		public CollectionService(IRepository<Collection> repository,
								 IRepository<Item> itemRepository,
								 ICloudStorageService cloudStorageService,
								 IRepository<Tag> tagRepository)
		{
			_collectionRepository = repository;
			_itemRepository = itemRepository;
			_cloudStorageService = cloudStorageService;
			_tagRepository = tagRepository;
		}

		public async Task<IBaseResponse<List<Collection>>> GetCollections()
		{
			try
			{
				var collections = await _collectionRepository.GetAll().ToListAsync();

				if (collections.Count == 0)
				{
					return new BaseResponse<List<Collection>>
					{
						Description = "Найдено 0 элементов",
						StatusCode = StatusCode.NotFound
					};
				}

				return new BaseResponse<List<Collection>>
				{
					Data = collections.OrderByDescending(c => c.Date).ToList(),
					StatusCode = StatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Collection>>
				{
					Description = $"[GetCollections] : {ex.Message}",
					StatusCode = StatusCode.InternalServerError
				};
			}
		}

		public async Task<IBaseResponse<List<Collection>>> SearchByValue(string value)
		{
			try
			{
				var collections = await _collectionRepository.GetAll().ToListAsync();

				if (collections == null)
				{
					return new BaseResponse<List<Collection>>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Найдено 0 элементов"
					};
				}

				var sortedCollections = collections.Where(c => c.Name.Contains(value)
				|| c.Author.Contains(value)
				|| c.Description.Contains(value));

				return new BaseResponse<List<Collection>>
				{
					StatusCode = StatusCode.OK,
					Data = sortedCollections.OrderByDescending(c => c.Date).ToList()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Collection>>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[SearchByValue] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<CollectionViewModel>> GetCollection(string id)
		{
			try
			{
				var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

				if (collection == null)
				{
					return new BaseResponse<CollectionViewModel>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Коллекция не найдена"
					};
				}

				var items = await _itemRepository.GetAll().ToListAsync();

				var allTags = await _tagRepository.GetAll().ToListAsync();
				var tagsInCollection = collection.Tags;

				var data = new CollectionViewModel
				{
					Id = collection.Id,
					Name = collection.Name,
					Description = collection.Description,
					Theme = collection.Theme,
					ImgRef = collection.ImgRef,
					Items = collection.Items,
					Date = collection.Date,
					Author = collection.Author,
					FreeItems = items,
					AllTags = allTags,
					Tags = tagsInCollection,
				};

				return new BaseResponse<CollectionViewModel>
				{
					StatusCode = StatusCode.OK,
					Data = data
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<CollectionViewModel>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[GetCollection] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<Collection>> Create(CreateCollectionViewModel model, string username, IFormFile image)
		{
			try
			{
				var collection = new CourseWork.Domain.Entities.Collection
				{
					Id = Guid.NewGuid().ToString(),
					Name = model.Name,
					Author = username,
					Description = model.Description,
					Theme = model.Theme,
					Date = DateTime.Now,
				};

				collection.ImgRef = await _cloudStorageService.UploadAsync(image, "/collections", collection.Id);

				await _collectionRepository.Create(collection);

				return new BaseResponse<Collection>
				{
					StatusCode = StatusCode.OK,
					Data = collection
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Collection>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[CreateCollection] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<Collection>> Edit(string id, CollectionViewModel model, string[] selectedItems, string[] tags, IFormFile image)
		{
			try
			{
				var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

				if (collection == null)
				{
					return new BaseResponse<Collection>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Collection not found"
					};
				}

				var items = await _itemRepository.GetAll()
					.Where(i => selectedItems.Contains(i.Id))
					.ToListAsync();

				var tagList = await GetTags(tags);

				collection.Name = model.Name;
				collection.Description = model.Description;
				collection.Theme = model.Theme;
				collection.Items.Clear();
				collection.Items.AddRange(items);
				collection.Tags.Clear();
				collection.Tags.AddRange(tagList);

				// если картинка не пустая то обновить, а если пустая то останется старая картинка
				if (image != null)
				{
					collection.ImgRef = await _cloudStorageService.UpdateAsync(image, "/collections", collection.Id);
				}

				await _collectionRepository.Update(collection);

				return new BaseResponse<Collection>
				{
					StatusCode = StatusCode.OK,
					Data = collection
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Collection>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[EditCollection] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> Delete(string id)
		{
			try
			{
				var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

				if (collection == null)
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.NotFound,
						Data = false,
						Description = "Collection not found"
					};
				}

				await _cloudStorageService.DeleteAsync("/collections", collection.Id);
				await _collectionRepository.Delete(collection);

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
					Description = $"[DeleteCollection] : {ex.Message}"
				};
			}
		}

		private async Task<List<Tag>> GetTags(string[] selectedTags)
		{
			if (selectedTags == null)
			{
				return new List<Tag>();
			}

			var tags = selectedTags.Select(x =>
			{
				x = x.ToLower();
				x = x.Replace(" ", "_");
				return x;
			}).ToList();

			var allTags = await _tagRepository.GetAll().ToListAsync();

			var result = allTags.Where(t => tags.Contains(t.Name)).ToList();

			tags.ForEach(x =>
			{
				if (allTags.FirstOrDefault(t => t.Name == x) == null)
				{
					result.Add(new Tag() { Name = x });
				}
			});

			return result;
		}
	}
}
