using CourseWork.Domain.ViewModels.Collection;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	[Authorize]
	public class CollectionController : Controller
	{
		private readonly ICollectionService _collectionService;

		public CollectionController(ICollectionService collectionService)
		{
			_collectionService = collectionService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetCollection(string id, int? pageId)
		{
			var response = await _collectionService.GetCollection(id, pageId ?? 1);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> MyCollections(int? pageId)
		{
			var response = await _collectionService.GetCollections();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data.Where(x => x.Author == GetCurrentUsername()));
			}
			return View("Error");
		}

		[HttpGet]
		public IActionResult CreateCollection() => View();

		[HttpPost]
		public async Task<IActionResult> CreateCollection(CreateCollectionViewModel model, IFormFile image)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var response = await _collectionService.Create(model, GetCurrentUsername(), image);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Redirect($"/Collection/EditCollection/{response.Data.Id}");
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> EditCollection(string id)
		{
			var response = await _collectionService.GetCollection(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> EditCollection(CollectionViewModel model, string[] selectedItems, IFormFile image, string[] tags)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var response = await _collectionService.Edit(model.Id, model, selectedItems, tags, image);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Redirect($"/Collection/GetCollection/{response.Data.Id}");
			}

			return View("Error", model.Description);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteCollection(string id)
		{
			var response = await _collectionService.Delete(id);

			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return RedirectToAction("Index", "Home");
			}

			return View("Error", response.Description);
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
