# مرحله ۱: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PersonalWebSite/PersonalWebSite.csproj .
RUN dotnet restore

COPY PersonalWebSite/ .
RUN dotnet publish -c Release -o /app/publish

# مرحله ۲: Runtime (image سبک‌تر برای اجرا)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PersonalWebSite.dll"]