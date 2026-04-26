param(
    [Parameter(Mandatory = $true)]
    [string]$ServiceName
)

$solutionName = "Fakebook.$ServiceName"
$serviceRoot = "services/$solutionName"

dotnet new sln -n $solutionName -o $serviceRoot

dotnet new webapi -n "$solutionName.Api" -o "$serviceRoot/$solutionName.Api"
dotnet new classlib -n "$solutionName.Application" -o "$serviceRoot/$solutionName.Application"
dotnet new classlib -n "$solutionName.Domain" -o "$serviceRoot/$solutionName.Domain"
dotnet new classlib -n "$solutionName.Infrastructure" -o "$serviceRoot/$solutionName.Infrastructure"

dotnet sln "$serviceRoot/$solutionName.slnx" add `
  "$serviceRoot/$solutionName.Api/$solutionName.Api.csproj" `
  "$serviceRoot/$solutionName.Application/$solutionName.Application.csproj" `
  "$serviceRoot/$solutionName.Domain/$solutionName.Domain.csproj" `
  "$serviceRoot/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Api/Fakebook.BuildingBlocks.Api.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Application/Fakebook.BuildingBlocks.Application.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Domain/Fakebook.BuildingBlocks.Domain.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Infrastructure/Fakebook.BuildingBlocks.Infrastructure.csproj"

dotnet add "$serviceRoot/$solutionName.Application/$solutionName.Application.csproj" reference `
  "$serviceRoot/$solutionName.Domain/$solutionName.Domain.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Application/Fakebook.BuildingBlocks.Application.csproj"

dotnet add "$serviceRoot/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" reference `
  "$serviceRoot/$solutionName.Domain/$solutionName.Domain.csproj" `
  "$serviceRoot/$solutionName.Application/$solutionName.Application.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Infrastructure/Fakebook.BuildingBlocks.Infrastructure.csproj"

dotnet add "$serviceRoot/$solutionName.Api/$solutionName.Api.csproj" reference `
  "$serviceRoot/$solutionName.Application/$solutionName.Application.csproj" `
  "$serviceRoot/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" `
  "building-blocks/Fakebook.BuildingBlocks.Api/Fakebook.BuildingBlocks.Api.csproj"

Write-Host "Created $solutionName service successfully."