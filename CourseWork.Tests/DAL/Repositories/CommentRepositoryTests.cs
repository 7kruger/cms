using CourseWork.DAL;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class CommentRepositoryTests
{
	private readonly ICommentRepository _repository;

	public CommentRepositoryTests()
	{
		_repository = GetInMemoryCommentRepository();
		FillDatabase();
	}

	[Fact]
	public async Task GetAllComments()
	{
		var comments = await _repository.GetAll().ToListAsync();

		Assert.NotNull(comments);
		Assert.True(comments.Any());
	}

	[Fact]
	public async Task CreateComment()
	{
		var comment = new Comment
		{
			SrcId = Guid.NewGuid().ToString(),
			Content = "new comment",
			Created = DateTime.Now,
			Modified = DateTime.Now,
			UpvoteCount = 0
		};

		await _repository.Create(comment);

		Assert.NotNull(_repository.GetAll().FirstOrDefault(x => x.Id == comment.Id));
	}

	[Fact]
	public async Task UpdateComment()
	{
		var comment = await _repository.GetAll().FirstOrDefaultAsync();
		comment.Content = "edited";

		await _repository.Update(comment);

		Assert.Equal(comment.Content, _repository.GetAll().FirstOrDefault(x => x.Id == comment.Id)?.Content);
	}

	[Fact]
	public async Task DeleteComment()
	{
		var comment = await _repository.GetAll().FirstOrDefaultAsync();

		await _repository.Delete(comment);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == comment.Id));
	}

	private ICommentRepository GetInMemoryCommentRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new CommentRepository(dbcontext);
	}

	private void FillDatabase()
	{
		var collections = new List<Comment>()
		{
			new Comment { SrcId = Guid.NewGuid().ToString(), Content = "comment", Created = DateTime.Now, Modified = DateTime.Now, UpvoteCount = 0 },
			new Comment { SrcId = Guid.NewGuid().ToString(), Content = "comment1", Created = DateTime.Now, Modified = DateTime.Now, UpvoteCount = 0 },
			new Comment { SrcId = Guid.NewGuid().ToString(), Content = "comment2", Created = DateTime.Now, Modified = DateTime.Now, UpvoteCount = 0 },
		};

		collections.ForEach(x => _repository.Create(x));
	}
}
