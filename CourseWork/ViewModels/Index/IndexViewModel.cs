using CourseWork.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CourseWork.ViewModels.Index
{
	public class IndexViewModel
	{
		public List<Collection> Collections { get; set; }
		public Pagination Pagination { get; set; }
		public List<SelectListItem> Themes { get; set; }
		public int? ThemeFilterApplied { get; set; }
		public string SearchString { get; set; }
	}
}
