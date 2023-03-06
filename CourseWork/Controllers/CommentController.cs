using CourseWork.Domain.Models;
using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
			var response = await _commentService.LoadComments(id, GetCurrentUsername(), IsAdmin());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok(response.Data);
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("AddComment")]
		public async Task<IActionResult> AddComment(CommentModel model)
		{
			var response = await _commentService.AddComment(model, GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok(response.Data);
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("UpdateComment")]
		public async Task<IActionResult> UpdateComment(CommentModel model)
		{
			var response = await _commentService.UpdateComment(model);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("DeleteComment")]
		public async Task<IActionResult> DeleteComment(long id)
		{
			var response = await _commentService.DeleteComment(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("Upvote")]
		public async Task<IActionResult> Upvote(CommentModel model)
		{
			var response = await _commentService.Upvote(model, GetCurrentUsername());
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		private string GetCurrentUsername() => User.Identity.Name;
		private bool IsAdmin() => User.IsInRole("admin");
	}
}
