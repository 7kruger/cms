using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Models;
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
		private readonly IRepository<User> _userRepository;

		public CommentService(ICommentRepository commentRepository, IRepository<User> userRepository)
		{
			_commentRepository = commentRepository;
			_userRepository = userRepository;
		}

		public async Task<IBaseResponse<List<CommentModel>>> LoadComments(string id, string username, bool isAdmin)
		{
			try
			{
				var all = await _commentRepository.GetAll()
					.Where(c => c.SrcId == id)
					.OrderByDescending(c => c.Date)
					.ToListAsync();

				var comments = all.Select(x =>
				{
					var canUserDeleteComment = (username == x.User.Name || isAdmin) ? true : false;
					return new CommentModel()
					{
						Id = x.Id,
						UserName = x.User.Name,
						Content = x.Content,
						Date = x.Date,
						CanUserDeleteComment = canUserDeleteComment,
					};
				}).ToList();

				return new BaseResponse<List<CommentModel>>
				{
					StatusCode = StatusCode.OK,
					Data = comments,
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<CommentModel>>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[LoadComments] : {ex.Message}"
				};
			}
		}
		public async Task<IBaseResponse<CommentModel>> AddComment(string id, string username, string content)
		{
			try
			{
				await _commentRepository.Create(new Comment
				{
					Content = content,
					User = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Name == username),
					Date = DateTime.Now,
					SrcId = id,
				});

				var comment = await _commentRepository.GetAll()
									.OrderBy(x => x.Date)
									.LastOrDefaultAsync(x => x.User.Name == username);

				return new BaseResponse<CommentModel>
				{
					StatusCode = StatusCode.OK,
					Data = new CommentModel
					{
						Id = comment.Id,
						Content = comment.Content,
						UserName = comment.User.Name,
						Date = comment.Date,
						CanUserDeleteComment = true,
					}
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<CommentModel>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[AddComment] : {ex.Message}"
				};
			}
		}

		public async Task<IBaseResponse<bool>> DeleteComment(int id)
		{
			try
			{
				var comment = await _commentRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);

				if (comment == null)
				{
					return new BaseResponse<bool>
					{
						StatusCode = StatusCode.NotFound,
						Description = "Не удалось удалить коммент"
					};
				}

				await _commentRepository.Delete(comment);

				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.OK,
					Data = true,
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>
				{
					StatusCode = StatusCode.InternalServerError,
					Description = $"[DeleteComment] : {ex.Message}"
				};
			}
		}
	}
}
