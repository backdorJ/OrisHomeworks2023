FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyHttpServer/MyHttpServer.csproj", "MyHttpServer/"]
RUN dotnet restore "MyHttpServer/MyHttpServer.csproj"
COPY . .
WORKDIR "/src/MyHttpServer"
RUN dotnet build "MyHttpServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHttpServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHttpServer.dll"]
