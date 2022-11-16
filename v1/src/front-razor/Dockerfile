#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["front-razor/front-razor.csproj", "front-razor/"]
RUN dotnet restore "front-razor/front-razor.csproj"
COPY . .
WORKDIR "/src/front-razor"
RUN dotnet build "front-razor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "front-razor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "front-razor.dll"]