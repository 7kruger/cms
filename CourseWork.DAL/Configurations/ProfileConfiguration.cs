using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurationsl;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder)
	{
		var adminProfile = new Profile
		{
			Id = 1,
			ImgUrl = "/images/person.svg",
			UserId = 1,
		};

		builder.ToTable("Profiles").HasKey(x => x.Id);

		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.Property(x => x.ImgUrl).IsRequired(false);

		builder.HasOne(x => x.User)
			.WithOne(x => x.Profile);

		builder.HasData(adminProfile);
	}
}
