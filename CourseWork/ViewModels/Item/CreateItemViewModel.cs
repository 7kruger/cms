using System.ComponentModel.DataAnnotations;

namespace CourseWork.ViewModels.Item;

public class CreateItemViewModel
{
	[Required(ErrorMessage = "Нет названия айтема")]
	public string Title { get; set; }
	[Required(ErrorMessage = "Нет содержания")]
	public string Description { get; set; }
	public string? CollectionId { get; set; }
}
