FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GreenZoneWifiBot/GreenZoneWifiBot.csproj", "GreenZoneWifiBot/"]
RUN dotnet restore "GreenZoneWifiBot/GreenZoneWifiBot.csproj"
COPY . .
WORKDIR "/src/GreenZoneWifiBot"
RUN dotnet build "GreenZoneWifiBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/GreenZoneWifiBot"
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GreenZoneWifiBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenZoneWifiBot.dll"]