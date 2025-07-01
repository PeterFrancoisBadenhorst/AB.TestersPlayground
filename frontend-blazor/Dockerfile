# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["frontend-blazor.csproj", "./"]
RUN dotnet restore "frontend-blazor.csproj"
COPY . .
RUN dotnet build "frontend-blazor.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "frontend-blazor.csproj" -c Release -o /app/publish

# Production stage
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
# Remove default nginx static files
RUN rm -rf ./*
# Copy published Blazor app
COPY --from=publish /app/publish/wwwroot .
# Copy nginx configuration
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
