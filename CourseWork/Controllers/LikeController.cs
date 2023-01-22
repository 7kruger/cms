using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseWork.Controllers
{
	[Authorize]
	[Route("api")]
	public class LikeController : Controller
	{
		private readonly ILikeService _likeService;

		public LikeController(ILikeService likeService)
		{
			_likeService = likeService;
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("LoadLikes")]
		public async Task<IActionResult> LoadLikes(string id)
		{
			var response = await _likeService.LoadLikes(id, GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok(response.Data);
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("AddLike")]
		public async Task<IActionResult> AddLike(string id)
		{
			var response = await _likeService.AddLike(id, GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return Ok(response.Description);
		}

		[HttpPost]
		[Route("RemoveLike")]
		public async Task<IActionResult> RemoveLike(string id)
		{
			var response = await _likeService.RemoveLike(id, GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
