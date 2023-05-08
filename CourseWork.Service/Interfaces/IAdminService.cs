using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;

namespace CourseWork.Service.Interfaces
{
	public interface IAdminService
	{
		Task<IEnumerable<User>> GetUsers();
		Task<bool> Do(ActionType type, int[] selectedUsers);
	}
}
