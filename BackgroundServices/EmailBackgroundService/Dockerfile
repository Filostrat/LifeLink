FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BackgroundServices/EmailBackgroundService/EmailBackgroundService.csproj", "BackgroundServices/EmailBackgroundService/"]
RUN dotnet restore "BackgroundServices/EmailBackgroundService/EmailBackgroundService.csproj"

COPY . .  
WORKDIR "/src/BackgroundServices/EmailBackgroundService"
RUN dotnet publish "EmailBackgroundService.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "EmailBackgroundService.dll"]
