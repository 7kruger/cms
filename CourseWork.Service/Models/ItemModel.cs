using System;

namespace CourseWork.Service.Models; 

public class ItemModel 
{
	public int Id { get; set; }
	public string CollectionId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
}
