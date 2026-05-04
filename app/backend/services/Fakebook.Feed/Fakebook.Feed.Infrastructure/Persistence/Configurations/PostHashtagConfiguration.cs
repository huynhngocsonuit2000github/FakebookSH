using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Feed.Infrastructure.Persistence.Configurations;

public sealed class PostHashtagConfiguration : IEntityTypeConfiguration<PostHashtag>
{
    public void Configure(EntityTypeBuilder<PostHashtag> builder)
    {
        builder.ToTable("post_hashtags");

        builder.HasKey(hashtag => hashtag.Id);

        builder.Property(hashtag => hashtag.Id).HasColumnName("id");
        builder.Property(hashtag => hashtag.PostId).HasColumnName("post_id").IsRequired();

        builder.Property(hashtag => hashtag.Tag)
            .HasColumnName("tag")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(hashtag => hashtag.Post)
            .WithMany(post => post.Hashtags)
            .HasForeignKey(hashtag => hashtag.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(hashtag => new { hashtag.PostId, hashtag.Tag }).IsUnique();
    }
}
