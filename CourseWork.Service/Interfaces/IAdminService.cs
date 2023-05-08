using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWork.Domain.Enum;
using CourseWork.Service.Models;

namespace CourseWork.Service.Interfaces
{
	public interface IAdminService
	{
		Task<IEnumerable<UserModel>> GetUsers();
		Task<bool> Do(ActionType type, int[] selectedUsers);
	}
}
