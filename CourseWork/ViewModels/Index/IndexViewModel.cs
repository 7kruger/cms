using CourseWork.Domain.Enum;
using CourseWork.ViewModels.Item;
using CourseWork.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseWork.ViewModels.Index
{
	public class IndexViewModel
	{
		public List<DAL.Entities.Collection> Collections { get; set; }
		public List<ItemViewModel> Items { get; set; }
		public Pagination Pagination { get; set; }
		public List<SelectListItem> Themes { get; set; }
		public List<SelectListItem> SortStates { get; set; }
		public Theme? ThemeFilterApplied { get; set; }
		public SortState? SortFilterApplied { get; set; }
		public SearchIn? SearchInFilterApplied { get; set; }
		public string SearchString { get; set; }
	}
}
