FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Blip.csproj", "Blip/"]
RUN dotnet restore "Blip.csproj"
COPY . .
WORKDIR "/src/Blip"
RUN dotnet build "Blip.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blip.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blip.dll"]