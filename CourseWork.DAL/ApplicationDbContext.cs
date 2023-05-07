using CourseWork.DAL.Configurations;
using CourseWork.DAL.Configurationsl;
using CourseWork.DAL.Entities;
using CourseWork.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.DAL;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options) { }

	public DbSet<Collection> Collections { get; set; }
	public DbSet<Item> Items { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Like> Likes { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Profile> Profiles { get; set; }
	public DbSet<Tag> Tags { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CollectionConfiguration());
		modelBuilder.ApplyConfiguration(new ItemConfiguration());
		modelBuilder.ApplyConfiguration(new CommentConfiguration());
		modelBuilder.ApplyConfiguration(new LikeConfiguration());
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new ProfileConfiguration());
		modelBuilder.ApplyConfiguration(new TagConfiguration());

		base.OnModelCreating(modelBuilder);
	}
}
