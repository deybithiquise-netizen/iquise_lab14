# Build stage: restore and publish the API with the .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY iquise_lab14.Api/iquise_lab14.Api.csproj iquise_lab14.Api/
RUN dotnet restore iquise_lab14.Api/iquise_lab14.Api.csproj

COPY iquise_lab14.Api/ iquise_lab14.Api/
RUN dotnet publish iquise_lab14.Api/iquise_lab14.Api.csproj --configuration Release --no-restore --output /app/publish

# Runtime stage: only the ASP.NET Core runtime and the published files
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Default port when PORT is not set (Render sets PORT automatically)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

USER $APP_UID
ENTRYPOINT ["dotnet", "iquise_lab14.Api.dll"]
