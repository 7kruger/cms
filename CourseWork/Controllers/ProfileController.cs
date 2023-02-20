using CourseWork.Domain.ViewModels.Profile;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
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

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _profileService.Get(GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpGet]
		public async Task<IActionResult> Settings()
		{
			var response = await _profileService.Get(GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return View(response.Data);
			}
			return View("Error", response.Description);
		}

		[HttpPost]
		public async Task<IActionResult> Settings(ProfileViewModel model, string image)
		{
			IFormFile file = null;

			if (!string.IsNullOrWhiteSpace(image))
			{
				image = image.Replace("data:image/jpeg;base64,", string.Empty);
				var fileBytes = Convert.FromBase64String(image);
				var ms = new MemoryStream(fileBytes);
				file = new FormFile(ms, 0, fileBytes.Length, GetCurrentUsername(), GetCurrentUsername() + ".jpg");
			}

			var response = await _profileService.Update(model, file);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return RedirectToAction("Index");
			}
			return View("Error", response.Data);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ShowProfile(string name)
		{
			var response = await _profileService.Get(name);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok(response.Data);
			}
			return Ok();
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
