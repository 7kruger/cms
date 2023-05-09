using System;

namespace CourseWork.Service.Models; 

public class ItemModel 
{
	public string Id { get; set; }
	public string CollectionId { get; set; }
	public string CollectionTitle { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Author { get; set; }
	public DateTime Date { get; set; }
	public string ImgUrl { get; set; }
	public long LikesCount { get; set; }
	public long CommentsCount { get; set; }
}
