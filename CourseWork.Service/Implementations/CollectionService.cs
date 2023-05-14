using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Service.Implementations;

public class CollectionService : ICollectionService
{
	private readonly IRepository<Collection> _collectionRepository;
	private readonly IRepository<Item> _itemRepository;
	private readonly ICloudStorageService _cloudStorageService;
	private readonly IRepository<Tag> _tagRepository;
	private readonly ILikeRepository _likeRepository;
	private readonly ICommentRepository _commentRepository;
	private readonly IItemService _itemService;
	private readonly IMapper _mapper;

	public CollectionService(IRepository<Collection> repository,
							 IRepository<Item> itemRepository,
							 ICloudStorageService cloudStorageService,
							 IRepository<Tag> tagRepository,
							 ILikeRepository likeRepository,
							 ICommentRepository commentRepository,
							 IItemService itemService,
							 IMapper mapper)
	{
		_collectionRepository = repository;
		_itemRepository = itemRepository;
		_cloudStorageService = cloudStorageService;
		_tagRepository = tagRepository;
		_likeRepository = likeRepository;
		_commentRepository = commentRepository;
		_itemService = itemService;
		_mapper = mapper;
	}

	public async Task<IEnumerable<CollectionModel>> GetCollections()
	{
		try
		{
			var collections = await _collectionRepository.GetAll().ToListAsync();

			if (!collections.Any())
			{
				return new List<CollectionModel>();
			}

			return _mapper.Map<IEnumerable<CollectionModel>>(collections.OrderByDescending(c => c.Date));
		}
		catch (Exception)
		{
			return new List<CollectionModel>();
		}
	}

	public async Task<IEnumerable<CollectionModel>> SearchByValue(string value)
	{
		try
		{
			var collections = await _collectionRepository.GetAll().ToListAsync();

			if (collections == null)
			{
				return new List<CollectionModel>();
			}

			var sortedCollections = collections.Where(c => c.Title.Contains(value)
			|| c.Author.Contains(value)
			|| c.Description.Contains(value));

			return _mapper.Map<IEnumerable<CollectionModel>>(sortedCollections);
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public async Task<CollectionModel> GetCollection(string id)
	{
		try
		{
			var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

			if (collection == null)
			{
				return null;
			}

			var model = _mapper.Map<CollectionModel>(collection);

			var likes = await _likeRepository.GetAll().ToListAsync();
			var comments = await _commentRepository.GetAll().ToListAsync();

			model.Items.ForEach(x =>
			{
				x.LikesCount = likes.Where(l => l.SrcId == x.Id).Count();
				x.CommentsCount = comments.Where(c => c.SrcId == x.Id).Count();
			});

			return model;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public async Task<CollectionModel> Create(CollectionModel model, string username, IFormFile image)
	{
		try
		{
			model.Id = Guid.NewGuid().ToString();
			model.Author = username;
			model.Date = DateTime.Now;

			model.ImgUrl = await _cloudStorageService.UploadAsync(image, "/collections", model.Id);

			var collection = _mapper.Map<Collection>(model);

			await _collectionRepository.Create(collection);

			return _mapper.Map<CollectionModel>(collection);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async Task<CollectionModel> Edit(string id, CollectionModel model, string[] selectedItems, string[] tags, IFormFile image)
	{
		try
		{
			var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

			if (collection == null)
			{
				return null;
			}

			var items = await _itemRepository.GetAll()
				.Where(i => selectedItems.Contains(i.Id))
				.ToListAsync();

			collection.Title = model.Title;
			collection.Description = model.Description;
			collection.Theme = model.Theme;
			collection.Items.Clear();
			collection.Items.AddRange(items);

			await SetTags(tags, collection);

			// если картинка не пустая то обновить, а если пустая то останется старая картинка
			if (image != null)
			{
				collection.ImgUrl = await _cloudStorageService.UpdateAsync(image, "/collections", collection.Id);
			}

			await _collectionRepository.Update(collection);

			return _mapper.Map<CollectionModel>(collection);
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
			var collection = await _collectionRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

			if (collection == null)
			{
				return false;
			}

			await _cloudStorageService.DeleteAsync("/collections", collection.Id);
			await _collectionRepository.Delete(collection);

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private async Task SetTags(string[] selectedTags, Collection collection)
	{
		if (selectedTags == null)
		{
			return;
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

		collection.Tags.Clear();
		collection.Tags.AddRange(result);
	}
}
