using CourseWork.Domain.Entities;
using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Profile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface IProfileService
	{
		Task<IBaseResponse<IEnumerable<ProfileViewModel>>> GetAll();
		Task<IBaseResponse<ProfileViewModel>> Get(string name);
		Task<IBaseResponse<bool>> Create(Profile profile);
		Task<IBaseResponse<bool>> Update(Profile profile);
		Task<IBaseResponse<bool>> Delete(int id);
	}
}
