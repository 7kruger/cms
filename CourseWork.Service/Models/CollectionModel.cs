using System;
using System.Collections.Generic;
using CourseWork.Domain.Enum;

namespace CourseWork.Service.Models;

public class CollectionModel
{
	public string Id { get; set; }
	public string Title { get; set; }
	public Theme Theme { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
	public List<ItemModel> Items { get; set; } = new();
	public List<TagModel> Tags { get; set; } = new();
}
