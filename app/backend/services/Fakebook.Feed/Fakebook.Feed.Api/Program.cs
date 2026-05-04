using Fakebook.BuildingBlocks.Api.Extensions;
using Fakebook.BuildingBlocks.Infrastructure.Extensions;
using Fakebook.Feed.Infrastructure.DependencyInjection;
using Fakebook.Feed.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Custom service registrations
builder.Services.AddFakebookCorrelationIdProvider();
builder.Services.AddFakebookCorrelationIdDelegatingHandler();
//builder.Services.AddDownStreamApiClientWithCorrelationIdHandler(builder.Configuration);
builder.Services.AddFeedInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    // In higher environments, run migrations from CI/CD instead of service startup.
    await app.ApplyDatabaseMigrationAsync<FeedDbContext>();
    await app.Services.SeedFeedDatabaseAsync();
}

app.UseFakebookCorrelationId();
app.UseFakebookExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();