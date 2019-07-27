FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DockerRedisAppNew.csproj", "."]
RUN dotnet restore "DockerRedisAppNew.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "DockerRedisAppNew.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DockerRedisAppNew.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV REDIS_URL 192.168.99.100:6379
ENTRYPOINT ["dotnet", "DockerRedisAppNew.dll"]