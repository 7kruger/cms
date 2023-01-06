using CourseWork.Services.Interfaces;
using CourseWork.ViewModels.Index;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CourseWork.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IMainPageViewModelService _mainPageViewModelService;

		public IndexModel(IMainPageViewModelService mainPageViewModelService)
		{
			_mainPageViewModelService = mainPageViewModelService;
		}

		public IndexViewModel IndexViewModel { get; set; } = new IndexViewModel();

		public async Task OnGetAsync(IndexViewModel model, string value, int? pageId, int? themeId)
		{
			IndexViewModel = await _mainPageViewModelService.GetIndexViewModel(pageId ?? 1, Constants.ITEMS_PER_PAGE, value, model, themeId);
		}
	}
}
