using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registre os serviços no DI

// Registra o repositório GitHubRepository para ser injetado onde for necessário
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>();

// Registra o serviço RepositoryService
builder.Services.AddScoped<RepositoryService>();

// Adiciona suporte para controllers e views (caso precise de views no futuro)
builder.Services.AddControllersWithViews();

// Adiciona suporte para controllers (para APIs)
builder.Services.AddControllers();

// Configuração do Swagger (para documentar a API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline de requisição HTTP
    app.UseSwagger();
    app.UseSwaggerUI();

// Middleware para redirecionamento HTTPS (garante que todas as requisições vão para HTTPS)
app.UseHttpsRedirection();

// Habilita o uso de autorização, mas no seu caso não é necessário se não estiver usando autenticação
app.UseAuthorization();

// Mapeia os controllers, isso é necessário para APIs
app.MapControllers();

// Inicia a aplicação
app.Run();
