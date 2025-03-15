FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Aurora.EndPoints.SerpensBot/Aurora.EndPoints.SerpensBot.csproj", "Aurora.EndPoints.SerpensBot/"]
RUN dotnet restore "Aurora.EndPoints.SerpensBot/Aurora.EndPoints.SerpensBot.csproj"
COPY . .
WORKDIR "/src/Aurora.EndPoints.SerpensBot"
RUN dotnet build "Aurora.EndPoints.SerpensBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Aurora.EndPoints.SerpensBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aurora.EndPoints.SerpensBot.dll"]
