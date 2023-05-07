using System;
using CourseWork.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseWork.DAL.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
		builder.ToTable("Comments");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.SrcId).IsRequired();
		builder.Property(x => x.Created).IsRequired();
		builder.Property(x => x.Modified).IsRequired();
		builder.Property(x => x.Content).IsRequired();
		builder.Property(x => x.UpvoteCount).IsRequired();

		builder.HasOne(x => x.Creator);
		builder.HasMany(x => x.UpvotedUsers)
			.WithMany(x => x.UpvotedComments);
	}
}
