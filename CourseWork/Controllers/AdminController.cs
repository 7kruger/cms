using AutoMapper;
using CourseWork.Domain.Enum;
using CourseWork.Service.Interfaces;
using CourseWork.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly IAdminService _adminService;
		private readonly IMapper _mapper;

		public AdminController(IAdminService adminService, IMapper mapper)
		{
			_adminService = adminService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _adminService.GetUsers();

			if (users == null)
			{
				return View("Error", "Ошибка");
			}

			return View(_mapper.Map<IEnumerable<UserViewModel>>(users));
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
