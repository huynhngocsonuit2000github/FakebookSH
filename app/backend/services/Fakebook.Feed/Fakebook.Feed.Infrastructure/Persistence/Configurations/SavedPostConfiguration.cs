using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Feed.Infrastructure.Persistence.Configurations;

public sealed class SavedPostConfiguration : IEntityTypeConfiguration<SavedPost>
{
    public void Configure(EntityTypeBuilder<SavedPost> builder)
    {
        builder.ToTable("saved_posts");

        builder.HasKey(savedPost => savedPost.Id);

        builder.Property(savedPost => savedPost.Id).HasColumnName("id");
        builder.Property(savedPost => savedPost.PostId).HasColumnName("post_id").IsRequired();
        builder.Property(savedPost => savedPost.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(savedPost => savedPost.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(savedPost => savedPost.UpdatedAtUtc).HasColumnName("updated_at_utc");

        builder.HasOne(savedPost => savedPost.Post)
            .WithMany(post => post.SavedBy)
            .HasForeignKey(savedPost => savedPost.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(savedPost => new { savedPost.PostId, savedPost.UserId }).IsUnique();
        builder.HasIndex(savedPost => savedPost.UserId);
    }
}
