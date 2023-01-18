using CourseWork.Domain.Enum;
using CourseWork.ViewModels.Index;
using System.Threading.Tasks;

namespace CourseWork.Services.Interfaces
{
	public interface IMainPageViewModelService
	{
		Task<IndexViewModel> GetIndexViewModel(int page, int pageSize, string searchString, Theme? theme, SortState? sort);
	}
}
