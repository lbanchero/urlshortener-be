# Use the official Microsoft ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official Microsoft SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UrlShortener.Http/UrlShortener.Http.csproj", "UrlShortener.Http/"]
RUN dotnet restore "UrlShortener.Http/UrlShortener.Http.csproj"
COPY . .
WORKDIR /src/UrlShortener.Http
RUN dotnet build "UrlShortener.Http.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrlShortener.Http.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.Http.dll"]

HEALTHCHECK CMD curl --fail http://localhost:8080/healthz || exit 1