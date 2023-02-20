using CourseWork.Domain.ViewModels.Item;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	[Authorize]
	public class ItemController : Controller
	{
		private readonly IItemService _itemService;

		public ItemController(IItemService itemService)
		{
			_itemService = itemService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Items()
		{
			var response = await _itemService.GetItems();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetItem(string id)
		{
			var response = await _itemService.GetItem(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> MyItems(int? pageId)
		{
			var response = await _itemService.GetItems();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data.Where(x => x.Author == GetCurrentUsername()));
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

			var response = await _itemService.Create(model, GetCurrentUsername(), file);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Redirect($"/Item/GetItem/{response.Data.Id}");
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> EditItem(string id)
		{
			var response = await _itemService.GetItem(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> EditItem(ItemViewModel model, string image)
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

			var response = await _itemService.Edit(model, file);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Redirect($"/Item/GetItem/{response.Data.Id}");
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteItem(string id)
		{
			var response = await _itemService.Delete(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Redirect("/");
			}
			return View("Error", response.Description);
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
