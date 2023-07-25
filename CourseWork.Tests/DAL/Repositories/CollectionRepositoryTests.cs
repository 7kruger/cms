using CourseWork.DAL;
using CourseWork.DAL.Entities;
using CourseWork.DAL.Interfaces;
using CourseWork.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseWork.Tests.DAL.Repositories;

public class CollectionRepositoryTests
{
	private readonly IRepository<Collection> _repository;

	public CollectionRepositoryTests()
	{
		_repository = GetInMemoryCollectionRepository();
		FillDatabase();
	}

	[Fact]
	public async Task GetAllCollections()
	{
		var collections = await _repository.GetAll().ToListAsync();

		Assert.NotNull(collections);
		Assert.True(collections.Any());
	}

	[Fact]
	public async Task CreateCollection()
	{
		var collection = new Collection()
		{
			Id = Guid.NewGuid().ToString(),
			Title = "Title",
			Author = "Author",
			Description = "Description",
		};

		await _repository.Create(collection);

		Assert.NotNull(_repository.GetAll().FirstOrDefault(x => x.Id == collection.Id));
	}

	[Fact]
	public async Task UpdateCollection()
	{
		var collection = await _repository.GetAll().FirstOrDefaultAsync();
		collection.Title = "edited";

		await _repository.Update(collection);

		Assert.Equal(collection.Title, _repository.GetAll().FirstOrDefault()?.Title);
	}

	[Fact]
	public async Task DeleteCollection()
	{
		var collection = await _repository.GetAll().FirstOrDefaultAsync();

		await _repository.Delete(collection);

		Assert.Null(_repository.GetAll().FirstOrDefault(x => x.Id == collection.Id));
	}

	private IRepository<Collection> GetInMemoryCollectionRepository()
	{
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseInMemoryDatabase("UnitTestsDatabase");
		var dbcontext = new ApplicationDbContext(builder.Options);
		dbcontext.Database.EnsureCreated();
		return new CollectionRepository(dbcontext);
	}

	private void FillDatabase()
	{
		var collections = new List<Collection>()
		{
			new Collection { Id = Guid.NewGuid().ToString(), Title = "Title", Author = "Author", Description = "Description", },
			new Collection { Id = Guid.NewGuid().ToString(), Title = "Title1", Author = "Author1", Description = "Description1", },
			new Collection { Id = Guid.NewGuid().ToString(), Title = "Title2", Author = "Author2", Description = "Description2", },
		};

		collections.ForEach(x => _repository.Create(x));
	}
}
