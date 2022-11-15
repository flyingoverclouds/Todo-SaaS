#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["todo-service/todo-service.csproj", "todo-service/"]
RUN dotnet restore "todo-service/todo-service.csproj"
COPY . .
WORKDIR "/src/todo-service"
RUN dotnet build "todo-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "todo-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "todo-service.dll"]