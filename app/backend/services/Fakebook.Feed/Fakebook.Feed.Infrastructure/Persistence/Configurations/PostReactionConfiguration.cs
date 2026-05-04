using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.Feed.Infrastructure.Persistence.Configurations;

public sealed class PostReactionConfiguration : IEntityTypeConfiguration<PostReaction>
{
    public void Configure(EntityTypeBuilder<PostReaction> builder)
    {
        builder.ToTable("post_reactions");

        builder.HasKey(reaction => reaction.Id);

        builder.Property(reaction => reaction.Id).HasColumnName("id");
        builder.Property(reaction => reaction.PostId).HasColumnName("post_id").IsRequired();
        builder.Property(reaction => reaction.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(reaction => reaction.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
        builder.Property(reaction => reaction.UpdatedAtUtc).HasColumnName("updated_at_utc");

        builder.HasOne(reaction => reaction.Post)
            .WithMany(post => post.Reactions)
            .HasForeignKey(reaction => reaction.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(reaction => new { reaction.PostId, reaction.UserId }).IsUnique();
        builder.HasIndex(reaction => reaction.UserId);
    }
}
