using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly IProfileService _profileService;

		public ProfileController(IProfileService profileService)
		{
			_profileService = profileService;
		}

		public async Task<IActionResult> Index()
		{
			var response = await _profileService.Get(User.Identity.Name);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}
	}
}
