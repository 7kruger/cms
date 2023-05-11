using AutoMapper;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using CourseWork.ViewModels.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	[Authorize]
	public class CollectionController : Controller
	{
		private readonly ICollectionService _collectionService;
		private readonly IMapper _mapper;

		public CollectionController(ICollectionService collectionService, IMapper mapper)
		{
			_collectionService = collectionService;
			_mapper = mapper;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetCollection(string id, int? pageId)
		{
			var collection = await _collectionService.GetCollection(id);

			if (collection == null)
			{
				return View("Error", "Ошибка! Коллекция не найдена");
			}

			var page = pageId ?? 1;
			collection.Items = collection.Items
				.Skip((page - 1) * Constants.ITEMS_PER_PAGE)
				.Take(Constants.ITEMS_PER_PAGE)
				.ToList();

			return View(_mapper.Map<CollectionViewModel>(collection));
		}

		[HttpGet]
		public async Task<IActionResult> MyCollections(int? pageId)
		{
			var collections = await _collectionService.GetCollections();
			if (!collections.Any())
			{
				return View("Error");
			}

			collections = collections.Where(x => x.Author == GetCurrentUsername());
			var result = _mapper.Map<IEnumerable<CollectionViewModel>>(collections);

			return View(result);
		}

		[HttpGet]
		public IActionResult CreateCollection() => View();

		[HttpPost]
		public async Task<IActionResult> CreateCollection(CreateCollectionViewModel model, string? image)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			IFormFile file = null;

			if (!string.IsNullOrWhiteSpace(image))
			{
				image = image.Replace("data:image/jpeg;base64,", string.Empty);
				var fileBytes = Convert.FromBase64String(image);
				var ms = new MemoryStream(fileBytes);
				file = new FormFile(ms, 0, fileBytes.Length, GetCurrentUsername(), GetCurrentUsername() + ".jpg");
			}

			var result = await _collectionService.Create(_mapper.Map<CollectionModel>(model), GetCurrentUsername(), file);
			if (result != null)
			{
				return Redirect($"/Collection/EditCollection/{result.Id}");
			}
			return View("Error", "Не удалось создать коллекцию");
		}

		[HttpGet]
		public async Task<IActionResult> EditCollection(string id)
		{
			var collection = await _collectionService.GetCollection(id);
			if (collection != null)
			{
				return View(_mapper.Map<EditCollectionViewModel>(collection));
			}
			return View("Error", "Ошибка! Не удалось найти коллекцию");
		}

		[HttpPost]
		public async Task<IActionResult> EditCollection(EditCollectionViewModel model, string[] selectedItems, string image, string[] tags)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			IFormFile file = null;

			if (!string.IsNullOrWhiteSpace(image))
			{
				image = image.Replace("data:image/jpeg;base64,", string.Empty);
				var fileBytes = Convert.FromBase64String(image);
				var ms = new MemoryStream(fileBytes);
				file = new FormFile(ms, 0, fileBytes.Length, GetCurrentUsername(), GetCurrentUsername() + ".jpg");
			}

			var result = await _collectionService.Edit(model.Id, _mapper.Map<CollectionModel>(model), selectedItems, tags, file);
			if (result != null)
			{
				return Redirect($"/Collection/GetCollection/{result.Id}");
			}

			return View("Error", "Ошибка при сохранении изменений");
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCollection(string id)
		{
			var deleted = await _collectionService.Delete(id);

			if (deleted)
			{
				return RedirectToAction("Index", "Home");
			}

			return View("Error", "Ошибка при удалении коллекции");
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
