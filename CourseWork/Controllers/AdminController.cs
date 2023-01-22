using CourseWork.Domain.Enum;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		public async Task<IActionResult> Index()
		{
			var response = await _adminService.GetUsers();
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Data);
		}

		[HttpPost]
		public async Task<IActionResult> UsersForm(ActionType type, int[] selectedUsers)
		{
			if (selectedUsers == null)
				return View();

			await _adminService.Do(type, selectedUsers);

			return RedirectToAction("Index", "Admin");
		}
	}
}
