using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICollectionService _collectionService;
		public HomeController(ICollectionService collectionService)
		{
			_collectionService = collectionService;
		}

		public async Task<IActionResult> Index()
		{
			var reponse = await _collectionService.GetCollections();
			if (reponse.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(reponse.Data);
			}
			return View("Error", $"{reponse.Description}");
		}

		public async Task<IActionResult> Search(string value)
		{
			var response = await _collectionService.SearchByValue(value);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}
	}
}
