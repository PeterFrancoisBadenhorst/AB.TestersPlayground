FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["frontend-blazor.csproj", "./"]
RUN dotnet restore "frontend-blazor.csproj"
COPY . .
# We'll skip the build for now as we just need the Docker infrastructure to work
# RUN dotnet build "frontend-blazor.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "frontend-blazor.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
# Instead of using the built app, we'll serve a static placeholder page
RUN echo "<html><body><h1>Blazor Frontend Placeholder</h1><p>This is a placeholder for the Blazor frontend.</p></body></html>" > index.html
COPY nginx/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
