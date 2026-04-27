using Fakebook.Bff.Api.Extensions;
using Fakebook.BuildingBlocks.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBffServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseFakebookCorrelationId();
app.UseFakebookExceptionHandling();

app.UseHttpsRedirection();

app.UseBffCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "Fakebook.Bff" }));

app.Run();
