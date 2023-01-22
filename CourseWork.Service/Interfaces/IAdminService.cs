using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
	public interface IAdminService
	{
		Task<IBaseResponse<List<User>>> GetUsers();
		Task<IBaseResponse<bool>> Do(ActionType type, int[] selectedUsers);
	}
}
