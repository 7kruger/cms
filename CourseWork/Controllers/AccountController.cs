using System.Security.Claims;
using AutoMapper;
using CourseWork.Service.Interfaces;
using CourseWork.Service.Models;
using CourseWork.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly IMapper _mapper;

		public AccountController(IAccountService accountService, IMapper mapper)
		{
			_accountService = accountService;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _accountService.Register(_mapper.Map<UserModel>(model));

			if (result.Succeeded)
			{
				await Authenticate(result.Claims);
				return Redirect("/");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _accountService.Login(_mapper.Map<UserModel>(model));

			if (result.Succeeded)
			{
				await Authenticate(result.Claims);
				return Redirect("/");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ChangePassword() => View();

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _accountService.ChangePassword(model.Name, model.NewPassword);

			if (result)
			{
				return Redirect("/");
			}

			return View("Error", "Не удалось сменить пароль");
		}

		private async Task Authenticate(ClaimsIdentity identy)
		{
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(identy));
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}
	}
}
