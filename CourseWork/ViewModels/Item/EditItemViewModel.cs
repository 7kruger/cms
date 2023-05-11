using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Item;

public class EditItemViewModel
{
	[HiddenInput(DisplayValue = false)]
	public string Id { get; set; }
	public string? CollectionId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
