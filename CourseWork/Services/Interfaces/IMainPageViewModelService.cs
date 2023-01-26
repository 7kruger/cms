using CourseWork.Domain.Enum;
using CourseWork.ViewModels.Index;
using System.Threading.Tasks;

namespace CourseWork.Services.Interfaces
{
	public interface IMainPageViewModelService
	{
		Task<IndexViewModel> GetIndexViewModel(int page, string searchString, Theme? theme, SortState? sort, SearchIn? searchIn, string? hashtag);
	}
}
