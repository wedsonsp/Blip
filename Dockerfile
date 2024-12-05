# Etapa de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Defina o diretório de trabalho dentro do contêiner
WORKDIR /src

# Copie o arquivo .csproj para o contêiner
COPY ["C:/Users/gabri/source/repos/Blip/Blip.csproj", "/src/Blip/"]

# Restaurar as dependências
RUN dotnet restore "/src/Blip/Blip.csproj"

# Copiar o restante dos arquivos do projeto para o contêiner
COPY C:/Users/gabri/source/repos/Blip /src

# Construir o projeto
RUN dotnet build "/src/Blip/Blip.csproj" -c Release -o /app/build

# Publicar o projeto (gerando os arquivos para produção)
RUN dotnet publish "/src/Blip/Blip.csproj" -c Release -o /app/publish

# Expor a porta usada pela aplicação web (se necessário)
EXPOSE 80

# Definir o ponto de entrada para rodar a aplicação web
ENTRYPOINT ["dotnet", "/app/publish/Blip.dll"]
