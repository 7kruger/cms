using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Response;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWork.Service.Implementations
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _commentRepository;

		public CommentService(ICommentRepository commentRepository)
		{
			_commentRepository = commentRepository;
		}

		public async Task<IBaseResponse<List<Comment>>> LoadComments(string id)
		{
			try
			{
				var comments = await _commentRepository.GetAll()
					.Where(c => c.SrcId == id)
					.OrderByDescending(c => c.Date)
					.ToListAsync();

				return new BaseResponse<List<Comment>>
				{
					StatusCode = StatusCode.OK,
					Data = comments
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Comment>>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[LoadComments] : {ex.Message}"
				};
			}
		}
		public async Task<IBaseResponse<bool>> AddComment(string id, string username, string content)
		{
			try
			{
				var comment = new Comment
				{
					Content = content,
					UserName = username,
					Date = DateTime.Now,
					SrcId = id,
				};

				await _commentRepository.Create(comment);

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[AddComment] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteComment(string id, string username)
		{
			throw new NotImplementedException();
		}
	}
}
