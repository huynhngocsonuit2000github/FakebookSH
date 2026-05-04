using Fakebook.Feed.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.Feed.Infrastructure.Persistence;

public static class FeedDbSeeder
{
    public static async Task SeedFeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<FeedDbContext>();

        if (await dbContext.Posts.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;
        var authors = new[]
        {
            new SeedAuthor("Jordan Park", "jordanp", "https://i.pravatar.cc/100?img=12"),
            new SeedAuthor("Mia Reynolds", "miareynolds", "https://i.pravatar.cc/100?img=25"),
            new SeedAuthor("Sofia Almeida", "sofiaa", "https://i.pravatar.cc/100?img=56"),
            new SeedAuthor("Theo Nakamura", "theon", "https://i.pravatar.cc/100?img=33"),
            new SeedAuthor("Maya Chen", "mayac", "https://i.pravatar.cc/100?img=47")
        };

        var data = new[]
        {
            ("Sunrise from the cabin. No notifications, no dashboards - just coffee and a stubborn fire.", "#weekend #outdoors", "/assets/design/5.post-1-background.jpg", 4, 1284, 31),
            ("Wrapped up a day hike and the sunset from the ridge was unreal. This place is a dream.", "#hiking #sunset #nature", "/assets/design/6.post-2-background.jpg", 24, 2264, 13),
            ("Coffee, a warm campfire, and a fresh morning breeze. These are the small moments that matter.", "#camping #coffeetime", "/assets/design/7.post-3-background.jpg", 72, 984, 19),
            ("Testing the new home office layout. Fewer cables, better light, same amount of unfinished notes.", "#workspace #focus", null, 5, 342, 6),
            ("A tiny launch checklist that finally fits on one page. Progress counts.", "#buildinpublic #product", null, 7, 421, 8),
            ("Late lunch with the team after a long planning session. Good food fixes most diagrams.", "#teamday #food", "/assets/design/6.post-2-background.jpg", 8, 512, 4),
            ("Morning playlist, clean inbox, and a plan that might actually survive the day.", "#monday #routine", null, 12, 209, 2),
            ("Found a quiet corner to read before the city woke up.", "#books #citylife", "/assets/design/5.post-1-background.jpg", 48, 774, 10),
            ("Weekend market haul. The strawberries did not survive the walk home.", "#weekend #market", null, 50, 651, 5),
            ("Small refactor, big relief. Naming things was still the hardest part.", "#engineering #devlife", "/assets/design/7.post-3-background.jpg", 96, 1107, 22)
        };

        var posts = data.Select((item, index) =>
        {
            var author = authors[index % authors.Length];
            var createdAtUtc = now.AddHours(-item.Item4);
            return new Post
            {
                AuthorId = Guid.Parse($"30000000-0000-0000-0000-{index + 1:000000000000}"),
                Author = author.Name,
                Username = author.UserName,
                Avatar = author.Avatar,
                Visibility = index % 3 == 0 ? "friends" : "public",
                Content = item.Item1,
                Image = item.Item3,
                BaseLikeCount = item.Item5,
                ShareCount = item.Item6,
                CreatedAtUtc = createdAtUtc,
                UpdatedAtUtc = createdAtUtc,
                Hashtags = item.Item2
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select((tag, tagIndex) => new PostHashtag
                    {
                        Tag = tag
                    })
                    .ToList(),
                Comments = SeedComments(index, createdAtUtc)
            };
        }).ToList();

        dbContext.Posts.AddRange(posts);
        await dbContext.SaveChangesAsync();
    }

    private static List<PostComment> SeedComments(int postIndex, DateTime postCreatedAtUtc)
    {
        var firstCommentIndex = postIndex + 20;

        return
        [
            new PostComment
            {
                UserId = Guid.Parse($"50000000-0000-0000-0000-{firstCommentIndex:000000000000}"),
                Avatar = $"https://i.pravatar.cc/100?img={10 + postIndex}",
                Name = postIndex % 2 == 0 ? "Emma Carter" : "Liam Brooks",
                Username = postIndex % 2 == 0 ? "emmacarter" : "liambrooks",
                Text = postIndex % 2 == 0 ? "This is exactly the kind of reset I need." : "Need the details for this one.",
                Likes = 2 + postIndex,
                CreatedAtUtc = postCreatedAtUtc.AddMinutes(15),
                UpdatedAtUtc = postCreatedAtUtc.AddMinutes(15)
            },
            new PostComment
            {
                UserId = Guid.Parse($"50000000-0000-0000-0000-{firstCommentIndex + 100:000000000000}"),
                Avatar = $"https://i.pravatar.cc/100?img={30 + postIndex}",
                Name = postIndex % 2 == 0 ? "Noah Ellis" : "Ava Brooks",
                Username = postIndex % 2 == 0 ? "noahellis" : "avabrooks",
                Text = postIndex % 2 == 0 ? "Saving this idea for later." : "Love the calm energy here.",
                Likes = postIndex,
                CreatedAtUtc = postCreatedAtUtc.AddMinutes(60),
                UpdatedAtUtc = postCreatedAtUtc.AddMinutes(60)
            }
        ];
    }

    private sealed record SeedAuthor(string Name, string UserName, string Avatar);
}
