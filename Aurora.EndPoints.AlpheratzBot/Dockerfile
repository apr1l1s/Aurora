FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
USER $APP_UID
WORKDIR /usr/share/dotnet

FROM mcr.microsoft.com/dotnet/sdk:latest AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Aurora.EndPoints.AlpheratzBot/Aurora.EndPoints.AlpheratzBot.csproj", "Aurora.EndPoints.AlpheratzBot/"]
RUN dotnet restore "Aurora.EndPoints.AlpheratzBot/Aurora.EndPoints.AlpheratzBot.csproj"
COPY . .
WORKDIR "/src/Aurora.EndPoints.AlpheratzBot"
RUN dotnet build "Aurora.EndPoints.AlpheratzBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Aurora.EndPoints.AlpheratzBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aurora.EndPoints.AlpheratzBot.dll"]
