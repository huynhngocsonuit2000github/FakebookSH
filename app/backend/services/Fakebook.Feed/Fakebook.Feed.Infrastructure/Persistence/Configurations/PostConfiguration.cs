using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Feed.Infrastructure.Persistence.Configurations;

public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");

        builder.HasKey(post => post.Id);

        builder.Property(post => post.Id)
            .HasColumnName("id");

        builder.Property(post => post.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();

        builder.Property(post => post.Author)
            .HasColumnName("author")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(post => post.Username)
            .HasColumnName("username")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(post => post.Avatar)
            .HasColumnName("avatar")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(post => post.Visibility)
            .HasColumnName("visibility")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(post => post.Content)
            .HasColumnName("content")
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(post => post.Feeling)
            .HasColumnName("feeling")
            .HasMaxLength(120);

        builder.Property(post => post.Location)
            .HasColumnName("location")
            .HasMaxLength(200);

        builder.Property(post => post.Image)
            .HasColumnName("image")
            .HasMaxLength(512);

        builder.Property(post => post.BaseLikeCount)
            .HasColumnName("base_like_count")
            .IsRequired();

        builder.Property(post => post.ShareCount)
            .HasColumnName("share_count")
            .IsRequired();

        builder.Property(post => post.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(post => post.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        builder.HasIndex(post => post.CreatedAtUtc);
        builder.HasIndex(post => post.AuthorId);
    }
}
