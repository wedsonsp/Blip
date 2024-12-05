# Etapa de construção
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiar o arquivo .csproj para o contêiner
COPY Blip.csproj ./

# Restaurar as dependências
RUN dotnet restore

# Copiar o restante dos arquivos do projeto
COPY . ./

# Construir o projeto
RUN dotnet build -c Release

# Publicar o projeto
RUN dotnet publish -c Release -o /app/publish

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "/app/publish/Blip.dll"]
