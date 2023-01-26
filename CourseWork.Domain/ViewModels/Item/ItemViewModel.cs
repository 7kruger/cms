﻿using CourseWork.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Domain.ViewModels.Item
{
	public class ItemViewModel
	{
		[HiddenInput(DisplayValue = false)]
		public string Id { get; set; }
		[HiddenInput(DisplayValue = false)]
		public string CollectionId { get; set; }

		[Required(ErrorMessage = "Нет названия айтема")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Нет описания айтема")]
		public string Content { get; set; }

		public string Author { get; set; }
		public DateTime Date { get; set; }
		public string ImgRef { get; set; }
		public List<Tag> Tags { get; set; }
		public string CollectionName { get; set; }
		public long LikesCount { get; set; }
		public long CommentsCount { get; set; }
	}
}
