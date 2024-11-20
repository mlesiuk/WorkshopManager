FROM mcr.microsoft.com/dotnet/sdk:8.0.403-alpine3.20 AS build
WORKDIR /src
COPY ["./src/", "./"]
WORKDIR /src/api
RUN dotnet restore "workshopManager.Api.csproj"
RUN dotnet build "workshopManager.Api.csproj" -c Release -o /app/build
RUN dotnet publish "workshopManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10-alpine3.20 AS publish
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "workshopManager.Api.dll"]