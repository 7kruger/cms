using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface IProfileService
	{
		Task<IBaseResponse<ProfileViewModel>> Get(string name);
		Task<IBaseResponse<bool>> Update(ProfileViewModel model, IFormFile image);
		Task Create(string username);
	}
}
