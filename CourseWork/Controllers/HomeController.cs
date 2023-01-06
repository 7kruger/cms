using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index() => Redirect("/");
	}
}
