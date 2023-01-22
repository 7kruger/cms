using CourseWork.DAL.Interfaces;
using CourseWork.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseWork.Middlewares
{
	public class CheckUserStatusMiddleware
	{
		private readonly RequestDelegate _next;

		public CheckUserStatusMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IRepository<User> repository)
		{
			var username = context.User.Identity.Name;

			if (!string.IsNullOrWhiteSpace(username))
			{
				var user = await repository.GetAll().FirstOrDefaultAsync(x => x.Name == username);

				if (user == null || user.IsBlocked)
				{
					await context.SignOutAsync();
				}
			}

			await _next.Invoke(context);
		}
	}
}
