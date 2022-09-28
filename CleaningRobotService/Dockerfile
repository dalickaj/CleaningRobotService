#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CleaningRobotService/CleaningRobotService.Api.csproj", "CleaningRobotService/"]
COPY ["CleaningRobotService.Commands/CleaningRobotService.Commands.csproj", "CleaningRobotService.Commands/"]
COPY ["CleaningRobotService.Infrastructure/CleaningRobotService.Infrastructure.csproj", "CleaningRobotService.Infrastructure/"]
COPY ["CleaningRobotService.Model/CleaningRobotService.ApiModel.csproj", "CleaningRobotService.Model/"]
RUN dotnet restore "CleaningRobotService/CleaningRobotService.Api.csproj"
COPY . .
WORKDIR "/src/CleaningRobotService"
RUN dotnet build "CleaningRobotService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleaningRobotService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleaningRobotService.Api.dll"]