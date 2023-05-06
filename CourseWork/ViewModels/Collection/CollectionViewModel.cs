using CourseWork.Domain.Enum;
using CourseWork.ViewModels.Item;
using CourseWork.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Collection;

public class CollectionViewModel
{
	[HiddenInput(DisplayValue = false)]
	public string Id { get; set; }
	public string Title { get; set; }
	public Theme Theme { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
	public Pagination Pagination { get; set; }
	public IEnumerable<ItemViewModel> Items { get; set; }
	public IEnumerable<string> Tags { get; set; }
}
