using CourseWork.Domain.Response;
using CourseWork.Domain.ViewModels.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseWork.Service.Interfaces
{
    public interface IAccountService
	{
		Task<IBaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);
		Task<IBaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
		Task<IBaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
	}
}
