using System;
using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
	public void Configure(EntityTypeBuilder<Like> builder)
	{
		builder.ToTable("Likes");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.SrcId).IsRequired();
		builder.Property(x => x.UserName).IsRequired();
	}
}
