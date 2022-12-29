using CourseWork.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseWork.Domain.Entities;
using System;

namespace CourseWork.Domain.ViewModels.Collection
{
	public class CollectionViewModel
	{
		[HiddenInput(DisplayValue = false)]
		public string Id { get; set; }

		[Required(ErrorMessage = "Нет названия коллекции")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Не указана тема коллекции")]
		public Theme Theme { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		public DateTime Date { get; set; }

		public string ImgRef { get; set; }

		public List<CourseWork.Domain.Entities.Item> Items { get; set; }

		public List<CourseWork.Domain.Entities.Item> FreeItems { get; set; }
	}
}
