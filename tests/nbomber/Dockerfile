FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:10.0-preview

WORKDIR /app
COPY --from=build /app/out .


ENTRYPOINT ["dotnet", "NBomberTests.dll"]
