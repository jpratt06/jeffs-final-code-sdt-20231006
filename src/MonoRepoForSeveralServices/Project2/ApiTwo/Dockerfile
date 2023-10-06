#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Project2/ApiTwo/ApiTwo.csproj", "Project2/ApiTwo/"]
COPY ["Libs/SharedUtils/SharedUtils.csproj", "Libs/SharedUtils/"]
RUN dotnet restore "Project2/ApiTwo/ApiTwo.csproj"
COPY . .
WORKDIR "/src/Project2/ApiTwo"
RUN dotnet build "ApiTwo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiTwo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiTwo.dll"]