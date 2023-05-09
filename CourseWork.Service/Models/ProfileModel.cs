namespace CourseWork.Service.Models;

public class ProfileModel
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string ImgUrl { get; set; }
	public int CollectionsCreated { get; set; }
	public int ItemsCreated { get; set; }
	public int LikesCount { get; set; }
}
