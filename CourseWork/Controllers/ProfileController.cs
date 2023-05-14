using AutoMapper;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using CourseWork.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly IProfileService _profileService;
		private readonly IMapper _mapper;

		public ProfileController(IProfileService profileService, IMapper mapper)
		{
			_profileService = profileService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var profile = await _profileService.Get(GetCurrentUsername());
			if (profile != null)
			{
				return View(_mapper.Map<ProfileViewModel>(profile));
			}
			return View("Error", "Error");
		}

		[HttpGet]
		public async Task<IActionResult> Settings()
		{
			var profile = await _profileService.Get(GetCurrentUsername());
			if (profile != null)
			{
				return View(_mapper.Map<ProfileViewModel>(profile));
			}
			return View("Error", "Error");
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

			var updated = await _profileService.Update(_mapper.Map<ProfileModel>(model), file);
			if (updated)
			{
				return RedirectToAction("Index");
			}
			return View("Error", "Error");
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ShowProfile(string name)
		{
			var profile = await _profileService.Get(name);
			if (profile != null)
			{
				return Ok(_mapper.Map<ProfileViewModel>(profile));
			}
			return Ok();
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
