# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
RUN apt-get update && apt-get -y upgrade && apt-get -y install apt-utils libgdiplus libc6-dev
RUN apt update && apt install tzdata -y
ENV TZ="America/Bogota"
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 50051

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build Backend.Formas.sln -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "Backend.Formas.API/Backend.Formas.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Backend.Formas.API.dll"]