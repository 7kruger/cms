using CourseWork.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Collection;

public class EditCollectionViewModel
{
	[HiddenInput(DisplayValue = false)]
	public string Id { get; set; }
	public string Title { get; set; }
	public Theme Theme { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public string ImgUrl { get; set; }
	public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}
