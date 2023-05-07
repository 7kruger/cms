using CourseWork.Domain.Enum;
using System.Collections.Generic;
using System;

namespace CourseWork.DAL.Entities;

public class Collection
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Author { get; set; }
	public string Description { get; set; }
	public Theme Theme { get; set; }
	public string ImgRef { get; set; }
	public DateTime Date { get; set; }
	public List<Item> Items { get; set; } = new();
	public List<Tag> Tags { get; set; } = new();
}
