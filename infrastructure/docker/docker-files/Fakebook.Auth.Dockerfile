FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY app/backend/services/Fakebook.Auth/Directory.Build.props app/backend/services/Fakebook.Auth/
COPY app/backend/services/Fakebook.Auth/Fakebook.Auth.Api/Fakebook.Auth.Api.csproj app/backend/services/Fakebook.Auth/Fakebook.Auth.Api/
COPY app/backend/services/Fakebook.Auth/Fakebook.Auth.Application/Fakebook.Auth.Application.csproj app/backend/services/Fakebook.Auth/Fakebook.Auth.Application/
COPY app/backend/services/Fakebook.Auth/Fakebook.Auth.Domain/Fakebook.Auth.Domain.csproj app/backend/services/Fakebook.Auth/Fakebook.Auth.Domain/
COPY app/backend/services/Fakebook.Auth/Fakebook.Auth.Infrastructure/Fakebook.Auth.Infrastructure.csproj app/backend/services/Fakebook.Auth/Fakebook.Auth.Infrastructure/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Api/Fakebook.BuildingBlocks.Api.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Api/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Application/Fakebook.BuildingBlocks.Application.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Application/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/Fakebook.BuildingBlocks.Domain.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/Fakebook.BuildingBlocks.Infrastructure.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/

RUN dotnet restore app/backend/services/Fakebook.Auth/Fakebook.Auth.Api/Fakebook.Auth.Api.csproj

COPY app/backend/services/Fakebook.Auth app/backend/services/Fakebook.Auth
COPY app/backend/building-blocks app/backend/building-blocks

RUN dotnet publish app/backend/services/Fakebook.Auth/Fakebook.Auth.Api/Fakebook.Auth.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.Auth.Api.dll"]
