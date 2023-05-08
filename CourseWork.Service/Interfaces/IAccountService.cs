using System.Threading.Tasks;
using CourseWork.Service.Models;

namespace CourseWork.Service.Interfaces
{
	public interface IAccountService
	{
		Task<IdentityResult> Register(UserModel model);
		Task<IdentityResult> Login(UserModel model);
		Task<bool> ChangePassword(string username, string newPassword);
	}
}
