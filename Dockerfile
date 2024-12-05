# Etapa de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj para o contêiner
COPY ["Blip.csproj", "Blip/"]

# Restaurar as dependências
RUN dotnet restore "Blip/Blip.csproj"

# Copiar o restante dos arquivos do projeto
COPY . .

# Construir o projeto
RUN dotnet build "Blip/Blip.csproj" -c Release -o /app/build

# Publicar o projeto
RUN dotnet publish "Blip/Blip.csproj" -c Release -o /app/publish

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "/app/publish/Blip.dll"]
