using System.ComponentModel.DataAnnotations;
using CourseWork.Domain.Enum;

namespace CourseWork.ViewModels.Collection;

public class CreateCollectionViewModel
{
	[Required(ErrorMessage = "Не указано названия коллекции")]
	public string Name { get; set; }
	[Required(ErrorMessage = "Нет описания коллекции")]
	public string Description { get; set; }
	[Required(ErrorMessage = "Нет темы коллекции")]
	public Theme Theme { get; set; }
}
