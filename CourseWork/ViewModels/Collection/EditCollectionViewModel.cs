using System.ComponentModel.DataAnnotations;
using CourseWork.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.ViewModels.Collection;

public class EditCollectionViewModel
{
	[HiddenInput(DisplayValue = false)]
	public string Id { get; set; }
	[Required(ErrorMessage = "Нет название колекции")]
	public string Title { get; set; }
	[Required(ErrorMessage = "Нет темы колекции")]
	public Theme Theme { get; set; }
	[Required(ErrorMessage = "Нет описания колекции")]
	public string Description { get; set; }
	public string? ImgUrl { get; set; }
	public IEnumerable<string>? Tags { get; set; } = Enumerable.Empty<string>();
}
