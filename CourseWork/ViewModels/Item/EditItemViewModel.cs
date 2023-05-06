using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Item;

public class EditItemViewModel
{
	[HiddenInput(DisplayValue = false)]
	public int Id { get; set; }
	public int? CollectionId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
