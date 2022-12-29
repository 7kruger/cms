using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CourseWork.Domain.ViewModels.Item
{
	public class CreateItemViewModel
	{
		[Required(ErrorMessage = "Нет названия айтема")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Нет содержания")]
		public string Content { get; set; }
		public string CollectionId { get; set; }
	}
}
