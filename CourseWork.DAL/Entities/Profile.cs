namespace CourseWork.DAL.Entities;

public class Profile
{
	public int Id { get; set; }
	public string ImgRef { get; set; }
	public int UserId { get; set; }
	public User User { get; set; }
}
