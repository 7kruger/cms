using CourseWork.Domain.ViewModels.Account;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _accountService.Register(model);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					await Authenticate(response.Data);

					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError(string.Empty, response.Description);
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _accountService.Login(model);
				if (response.StatusCode == Domain.Enum.StatusCode.OK)
				{
					await Authenticate(response.Data);

					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError(string.Empty, response.Description);
			}
			return View(model);
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
