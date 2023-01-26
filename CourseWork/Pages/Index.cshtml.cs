using CourseWork.Domain.Enum;
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

		public async Task OnGetAsync(int? pageId, 
									 Theme? theme, 
									 SortState? sortState, 
									 SearchIn? searchIn,
									 string search, 
									 string hashtag)
		{
			IndexViewModel = await _mainPageViewModelService.GetIndexViewModel(pageId ?? 1, search, theme, sortState, searchIn, hashtag);
		}
	}
}
