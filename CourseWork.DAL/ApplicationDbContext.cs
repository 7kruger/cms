using CourseWork.Domain.Entities;
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
		var admin = new User
		{
			Id = 1,
			Name = "admin",
			Password = HashPasswordHelper.HashPassword("admin"),
			Role = Domain.Enum.Role.Admin,
			RegistrationDate = System.DateTime.Now,
			IsBlocked = false,
		};

		var profile = new Profile
		{
			Id = 1,
			ImgRef = "/images/person.svg",
			UserId = admin.Id,
		};

		modelBuilder.Entity<User>(builder =>
		{
			builder.ToTable("Users").HasKey(x => x.Id);

			builder.HasData(admin);

			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.Property(x => x.Password).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

			builder.HasOne(x => x.Profile)
				.WithOne(x => x.User)
				.HasPrincipalKey<User>(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<Profile>(builder =>
		{
			builder.ToTable("Profiles").HasKey(x => x.Id);

			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.ImgRef).IsRequired(false);

			builder.HasData(profile);
		});

		modelBuilder.Entity<Comment>(builder =>
		{
			builder.HasOne(x => x.Creator);
			builder.HasMany(x => x.UpvotedUsers)
				.WithMany(x => x.UpvotedComments);
		});

		base.OnModelCreating(modelBuilder);
	}
}
