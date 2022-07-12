#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CWT.Service/CWT.Service.csproj", "CWT.Service/"]
RUN dotnet restore "CWT.Service/CWT.Service.csproj"
COPY . .
WORKDIR "/src/CWT.Service"
RUN dotnet build "CWT.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CWT.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CWT.Service.dll"]