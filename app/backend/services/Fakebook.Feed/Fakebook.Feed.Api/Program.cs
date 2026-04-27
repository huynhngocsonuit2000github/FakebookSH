using Fakebook.BuildingBlocks.Api.Extensions;
using Fakebook.Feed.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

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

app.UseFakebookCorrelationId();
app.UseFakebookExceptionHandling();

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();