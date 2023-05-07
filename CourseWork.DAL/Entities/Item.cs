using System.Collections.Generic;
using System;

namespace CourseWork.DAL.Entities;

public class Item
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Author { get; set; }
	public string Description { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
	public string? CollectionId { get; set; }
	public Collection? Collection { get; set; }
	public List<Tag> Tags { get; set; } = new();
}
