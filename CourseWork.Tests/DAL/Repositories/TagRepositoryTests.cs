using CourseWork.DAL;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class TagRepositoryTests
{
	private readonly IRepository<Tag> _repository;

	public TagRepositoryTests()
	{
		_repository = GetInMemoryTagRepository();
		FillDatabase();
	}

	[Fact]
	public async Task GetAllTags()
	{
		var tags = await _repository.GetAll().ToListAsync();

		Assert.NotNull(tags);
		Assert.True(tags.Any());
	}

	[Fact]
	public async Task CreateTag()
	{
		var tag = new Tag { Name = "New Tag" };

		await _repository.Create(tag);

		Assert.NotNull(_repository.GetAll().FirstOrDefault(x => x.Id == tag.Id));
	}

	[Fact]
	public async Task UpdateTag()
	{
		var tag = await _repository.GetAll().FirstOrDefaultAsync();
		tag.Name = "edited";

		await _repository.Update(tag);

		Assert.Equal(tag.Name, _repository.GetAll().FirstOrDefault(x => x.Id == tag.Id)?.Name);
	}

	[Fact]
	public async Task DeleteTag()
	{
		var tag = await _repository.GetAll().FirstOrDefaultAsync();

		await _repository.Delete(tag);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == tag.Id));
	}

	private IRepository<Tag> GetInMemoryTagRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new TagRepository(dbcontext);
	}

	private void FillDatabase()
	{
		var collections = new List<Tag>()
		{
			new Tag { Name = "Tag" },
			new Tag { Name = "Tag1" },
			new Tag { Name = "Tag2" },
		};

		collections.ForEach(x => _repository.Create(x));
	}
}
