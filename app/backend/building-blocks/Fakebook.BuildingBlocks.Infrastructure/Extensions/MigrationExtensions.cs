using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fakebook.BuildingBlocks.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyDatabaseMigrationAsync<TDbContext>(
        this WebApplication app)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
