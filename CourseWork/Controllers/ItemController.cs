using AutoMapper;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using CourseWork.ViewModels.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	[Authorize]
	public class ItemController : Controller
	{
		private readonly IItemService _itemService;
		private readonly IMapper _mapper;

		public ItemController(IItemService itemService, IMapper mapper)
		{
			_itemService = itemService;
			_mapper = mapper;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Items()
		{
			var items = await _itemService.GetItems();
			if (items.Any())
			{
				return View(items);
			}
			return View("Error", "Ошибка");
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetItem(string id)
		{
			var item = await _itemService.GetItem(id);
			if (item != null)
			{
				return View(item);
			}
			return View("Error", "Ошибка");
		}

		[HttpGet]
		public async Task<IActionResult> MyItems(int? pageId)
		{
			var items = await _itemService.GetItems();
			if (items.Any())
			{
				items = items.Where(x => x.Author == GetCurrentUsername());
				return View(_mapper.Map<IEnumerable<ItemViewModel>>(items));
			}
			return View("Error");
		}

		[HttpGet]
		public IActionResult CreateItem() => View();

		[HttpPost]
		public async Task<IActionResult> CreateItem(CreateItemViewModel model, string image)
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

			var item = await _itemService.Create(_mapper.Map<ItemModel>(model), GetCurrentUsername(), file);
			if (item != null)
			{
				return Redirect($"/Item/GetItem/{item.Id}");
			}
			return View("Error", "Error");
		}

		[HttpGet]
		public async Task<IActionResult> EditItem(string id)
		{
			var item = await _itemService.GetItem(id);
			if (item != null)
			{
				return View(_mapper.Map<EditItemViewModel>(item));
			}
			return View("Error", "Error");
		}

		[HttpPost]
		public async Task<IActionResult> EditItem(EditItemViewModel model, string image)
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

			var item = await _itemService.Edit(_mapper.Map<ItemModel>(model), file);
			if (item != null)
			{
				return Redirect($"/Item/GetItem/{item.Id}");
			}
			return View("Error","Error");
		}

		[HttpGet]
		public async Task<IActionResult> DeleteItem(string id)
		{
			var deleted = await _itemService.Delete(id);
			if (deleted)
			{
				return Redirect("/");
			}
			return View("Error", "Error");
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
