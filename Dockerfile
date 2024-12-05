# Usar imagem base do .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj para dentro do contêiner
COPY ["Blip/Blip.csproj", "Blip/"]

# Restaurar as dependências do projeto
RUN dotnet restore "Blip/Blip.csproj"

# Copiar o restante do código
COPY . .

# Construir o projeto
RUN dotnet build "Blip/Blip.csproj" -c Release -o /app/build

# Publicar o projeto
RUN dotnet publish "Blip/Blip.csproj" -c Release -o /app/publish

# Definir ponto de entrada para o contêiner
ENTRYPOINT ["dotnet", "/app/publish/Blip.dll"]
