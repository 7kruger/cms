using CourseWork.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseWork.Domain.ViewModels.Collection
{
	public class CreateCollectionViewModel
	{
		[Required(ErrorMessage = "Не указано названия коллекции")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Нет описания коллекции")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Нет темы коллекции")]
		public Theme Theme { get; set; }
	}
}
