using Fakebook.Auth.Api.Extensions;
using Fakebook.Auth.Infrastructure.DependencyInjection;
using Fakebook.Auth.Infrastructure.Persistence;
using Fakebook.BuildingBlocks.Api.Extensions;
using Fakebook.BuildingBlocks.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom service registrations
builder.Services.AddFakebookCorrelationIdDelegatingHandler();
builder.Services.AddDownStreamApiClientWithCorrelationIdHandler(builder.Configuration);
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddValidators();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // In the higher environments, we should use a proper CI/CD migration strategy instead of applying migrations on startup.
    await app.ApplyDatabaseMigrationAsync<AuthDbContext>();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFakebookCorrelationId();
app.UseFakebookExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();