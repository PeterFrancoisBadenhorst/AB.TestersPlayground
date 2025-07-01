FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["api/Api.csproj", "api/"]
COPY ["Testers.ServiceDefaults/Testers.ServiceDefaults.csproj", "Testers.ServiceDefaults/"]

# Restore as distinct layers
RUN dotnet restore "api/Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
