using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers;

public class HomeController : Controller
{
	[HttpPost]
	public IActionResult SetLanguage(string language, string returnUrl)
	{
		Response.Cookies.Append(
		CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

		return LocalRedirect(returnUrl);
	}
}
