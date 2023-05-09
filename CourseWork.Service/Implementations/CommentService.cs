using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Enum;
using CourseWork.Domain.Models;
using CourseWork.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

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

		public async Task<List<CommentModel>> LoadComments(string srcId, string username, bool isAdmin)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == username);
				var all = await _commentRepository.GetAll()
					.Where(c => c.SrcId == srcId)
					.ToListAsync();

				var comments = all.Select(x => new CommentModel()
				{
					Id = x.Id,
					SrcId = srcId,
					Parent = x.Parent,
					Created = x.Created,
					Modified = x.Modified,
					Content = x.Content,
					Creator = x.Creator.Id,
					Fullname = x.Creator.Name,
					ProfilePictureUrl = x.Creator?.Profile?.ImgUrl,
					UpvoteCount = x.UpvoteCount,
					UserHasUpvoted = x.UpvotedUsers.Contains(user),
					CreatedByAdmin = x.Creator.Role == Role.Admin ? true : false,
					CreatedByCurrentUser = x.Creator.Name == username ? true : false
				}).ToList();

				return comments;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<CommentModel> AddComment(CommentModel model, string username)
		{
			try
			{
				var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == username);

				var comment = new Comment()
				{
					SrcId = model.SrcId,
					Parent = model.Parent,
					Created = model.Created,
					Modified = model.Modified,
					Content = model.Content,
					UpvoteCount = model.UpvoteCount,
					Creator = user
				};

				await _commentRepository.Create(comment);
				comment = (await _commentRepository.GetAll().ToListAsync()).LastOrDefault();

				model.Id = comment.Id;
				model.Created = comment.Created;
				model.Creator = comment.Creator.Id;

				return model;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<bool> UpdateComment(CommentModel model)
		{
			try
			{
				var comment = await _commentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);

				if (comment == null)
				{
					return false;
				}

				comment.Content = model.Content;
				comment.Modified = DateTime.Now;

				await _commentRepository.Update(comment);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> DeleteComment(long id)
		{
			try
			{
				var parent = await _commentRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
				var comments = await _commentRepository.GetAll().Where(x => x.Parent == parent.Id).ToListAsync();

				if (parent == null)
				{
					return false;
				}

				await _commentRepository.DeleteRange(comments);
				await _commentRepository.Delete(parent);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<bool> Upvote(CommentModel model, string username)
		{
			try
			{
				var comment = await _commentRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);

				comment.UpvoteCount = model.UpvoteCount;

				var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Name == username);

				if (comment.UpvotedUsers.Contains(user))
				{
					comment.UpvotedUsers.Remove(user);
				}
				else
				{
					comment.UpvotedUsers.Add(user);
				}

				await _commentRepository.Update(comment);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
