using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.Feed.Infrastructure.Persistence;

public sealed class FeedDbContext : DbContext
{
    public FeedDbContext(DbContextOptions<FeedDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostComment> PostComments => Set<PostComment>();
    public DbSet<PostHashtag> PostHashtags => Set<PostHashtag>();
    public DbSet<PostReaction> PostReactions => Set<PostReaction>();
    public DbSet<SavedPost> SavedPosts => Set<SavedPost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FeedDbContext).Assembly);
    }
}
