using CourseWork.Domain.Entities;
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
		public async Task<IActionResult> AddComment(string id, string content)
		{
			var response = await _commentService.AddComment(id, GetCurrentUsername(), content);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok();
			}
			return StatusCode(404);
		}

		[HttpPost]
		[Route("DeleteComment")]
		public async Task<IActionResult> DeleteComment(int id)
		{
			var response = await _commentService.DeleteComment(id);
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
