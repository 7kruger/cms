using System.Threading.Tasks;
using CourseWork.Service.Models;
using Microsoft.AspNetCore.Http;

namespace CourseWork.Service.Interfaces
{
	public interface IProfileService
	{
		Task<ProfileModel> Get(string name);
		Task<bool> Update(ProfileModel model, IFormFile image);
		Task Create(string username);
	}
}
