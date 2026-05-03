FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY app/bff/Fakebook.Bff/Fakebook.Bff.Api/Fakebook.Bff.Api.csproj app/bff/Fakebook.Bff/Fakebook.Bff.Api/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Api/Fakebook.BuildingBlocks.Api.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Api/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Application/Fakebook.BuildingBlocks.Application.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Application/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/Fakebook.BuildingBlocks.Domain.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Domain/
COPY app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/Fakebook.BuildingBlocks.Infrastructure.csproj app/backend/building-blocks/Fakebook.BuildingBlocks.Infrastructure/

RUN dotnet restore app/bff/Fakebook.Bff/Fakebook.Bff.Api/Fakebook.Bff.Api.csproj

COPY app/bff/Fakebook.Bff app/bff/Fakebook.Bff
COPY app/backend/building-blocks app/backend/building-blocks

RUN dotnet publish app/bff/Fakebook.Bff/Fakebook.Bff.Api/Fakebook.Bff.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM runtime AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fakebook.Bff.Api.dll"]
