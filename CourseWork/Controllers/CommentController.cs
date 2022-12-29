using CourseWork.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
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

		[Route("LoadComments")]
		public async Task<IActionResult> LoadComments(string id)
		{
			var response = await _commentService.LoadComments(id);
			if (response.StatusCode == Domain.Enum.StatusCode.OK)
			{
				return Ok(response.Data);
			}
			return Ok(StatusCode(404));
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
			return Ok(StatusCode(404));
		}

		private string GetCurrentUsername() => User.Identity.Name;
	}
}
