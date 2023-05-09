using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
			var result = await _likeService.LoadLikes(id, GetCurrentUsername());
			if (!string.IsNullOrWhiteSpace(result))
			{
				return Ok(result);
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("AddLike")]
		public async Task<IActionResult> AddLike(string id)
		{
			var added = await _likeService.AddLike(id, GetCurrentUsername());
			if (added)
			{
				return Ok();
			}
			return Ok(404);
		}

		[HttpPost]
		[Route("RemoveLike")]
		public async Task<IActionResult> RemoveLike(string id)
		{
			var removed = await _likeService.RemoveLike(id, GetCurrentUsername());
			if (removed)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
