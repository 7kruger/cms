using CourseWork.DAL.Entities;
using CourseWork.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
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

		builder.ToTable("Users").HasKey(x => x.Id);

		builder.HasData(admin);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Password).IsRequired();
		builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
		builder.Property(x => x.Role).IsRequired();
		builder.Property(x => x.RegistrationDate).IsRequired();

		builder.HasOne(x => x.Profile)
			.WithOne(x => x.User)
			.HasPrincipalKey<User>(x => x.Id)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(x => x.UpvotedComments)
			.WithMany(x => x.UpvotedUsers);
	}
}
