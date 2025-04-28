FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api/Api.csproj", "Api/Api/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Infrastructure/Identity/Identity.csproj", "Infrastructure/Identity/"]
COPY ["Core/Application/Application.csproj", "Core/Application/"]
COPY ["Infrastructure/Infrastructure/Infrastructure.csproj", "Infrastructure/Infrastructure/"]
COPY ["Infrastructure/Kafka/Kafka.csproj", "Infrastructure/Kafka/"]
COPY ["Infrastructure/Persistence/Persistence.csproj", "Infrastructure/Persistence/"]
RUN dotnet restore "./Api/Api/Api.csproj"
COPY . .
WORKDIR "/src/Api/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]