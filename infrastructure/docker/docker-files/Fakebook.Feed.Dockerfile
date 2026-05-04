FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY app/backend/services/Fakebook.Feed/Fakebook.Feed.Api/Fakebook.Feed.Api.csproj app/backend/services/Fakebook.Feed/Fakebook.Feed.Api/
COPY app/backend/services/Fakebook.Feed/Fakebook.Feed.Application/Fakebook.Feed.Application.csproj app/backend/services/Fakebook.Feed/Fakebook.Feed.Application/
COPY app/backend/services/Fakebook.Feed/Fakebook.Feed.Domain/Fakebook.Feed.Domain.csproj app/backend/services/Fakebook.Feed/Fakebook.Feed.Domain/
COPY app/backend/services/Fakebook.Feed/Fakebook.Feed.Infrastructure/Fakebook.Feed.Infrastructure.csproj app/backend/services/Fakebook.Feed/Fakebook.Feed.Infrastructure/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Api/Fakebook.BuildingBlocks.Api.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Api/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Application/Fakebook.BuildingBlocks.Application.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Application/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/Fakebook.BuildingBlocks.Domain.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/Fakebook.BuildingBlocks.Infrastructure.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Messaging/Fakebook.BuildingBlocks.Messaging.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Messaging/

RUN dotnet restore app/backend/services/Fakebook.Feed/Fakebook.Feed.Api/Fakebook.Feed.Api.csproj

COPY app/backend/services/Fakebook.Feed app/backend/services/Fakebook.Feed
COPY app/backend/building-blocks app/backend/building-blocks

RUN dotnet publish app/backend/services/Fakebook.Feed/Fakebook.Feed.Api/Fakebook.Feed.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM runtime AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.Feed.Api.dll"]
