#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["tomanota-server/tomanota-server.csproj", "tomanota-server/"]
RUN dotnet restore "tomanota-server/tomanota-server.csproj"
COPY . .
WORKDIR "/src/tomanota-server"
RUN dotnet build "tomanota-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tomanota-server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tomanota-server.dll"]