#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Project1/ApiOne/ApiOne.csproj", "Project1/ApiOne/"]
COPY ["Libs/SharedUtils/SharedUtils.csproj", "Libs/SharedUtils/"]
RUN dotnet restore "Project1/ApiOne/ApiOne.csproj"
COPY . .
WORKDIR "/src/Project1/ApiOne"
RUN dotnet build "ApiOne.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiOne.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiOne.dll"]