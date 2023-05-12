using CourseWork.Domain.Models;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
	[Route("api")]
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;

		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[HttpGet]
		[Route("LoadComments")]
		public async Task<IActionResult> LoadComments(string id)
		{
			var comments = await _commentService.LoadComments(id, GetCurrentUsername(), IsAdmin());

            return Ok(comments);
        }

		[HttpPost]
		[Route("AddComment")]
		public async Task<IActionResult> AddComment(CommentModel model)
		{
			var comment = await _commentService.AddComment(model, GetCurrentUsername());
			if (comment != null)
			{
				return Ok(comment);
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("UpdateComment")]
		public async Task<IActionResult> UpdateComment(CommentModel model)
		{
			var updated = await _commentService.UpdateComment(model);
			if (updated)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("DeleteComment")]
		public async Task<IActionResult> DeleteComment(long id)
		{
			var deleted = await _commentService.DeleteComment(id);
			if (deleted)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("Upvote")]
		public async Task<IActionResult> Upvote(CommentModel model)
		{
			var upvoted = await _commentService.Upvote(model, GetCurrentUsername());
			if (upvoted)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		private string GetCurrentUsername() => User.Identity.Name;
		private bool IsAdmin() => User.IsInRole("admin");
	}
}
