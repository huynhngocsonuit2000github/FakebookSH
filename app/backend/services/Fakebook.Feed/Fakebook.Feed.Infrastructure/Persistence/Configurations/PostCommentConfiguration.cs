using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Feed.Infrastructure.Persistence.Configurations;

public sealed class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.ToTable("post_comments");

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.Id).HasColumnName("id");
        builder.Property(comment => comment.PostId).HasColumnName("post_id").IsRequired();
        builder.Property(comment => comment.UserId).HasColumnName("user_id").IsRequired();

        builder.Property(comment => comment.Avatar)
            .HasColumnName("avatar")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(comment => comment.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(comment => comment.Username)
            .HasColumnName("username")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(comment => comment.Text)
            .HasColumnName("text")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(comment => comment.Likes).HasColumnName("likes").IsRequired();
        builder.Property(comment => comment.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(comment => comment.UpdatedAtUtc).HasColumnName("updated_at_utc");

        builder.HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(comment => comment.PostId);
        builder.HasIndex(comment => comment.CreatedAtUtc);
    }
}
