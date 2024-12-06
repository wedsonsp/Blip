using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registre os servi�os no DI

// Registra o reposit�rio GitHubRepository para ser injetado onde for necess�rio
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>();

// Registra o servi�o RepositoryService
builder.Services.AddScoped<RepositoryService>();

// Adiciona suporte para controllers e views (caso precise de views no futuro)
builder.Services.AddControllersWithViews();

// Adiciona suporte para controllers (para APIs)
builder.Services.AddControllers();

// Configura��o do Swagger (para documentar a API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline de requisi��o HTTP
    app.UseSwagger();
    app.UseSwaggerUI();

// Middleware para redirecionamento HTTPS (garante que todas as requisi��es v�o para HTTPS)
app.UseHttpsRedirection();

// Habilita o uso de autoriza��o, mas no seu caso n�o � necess�rio se n�o estiver usando autentica��o
app.UseAuthorization();

// Mapeia os controllers, isso � necess�rio para APIs
app.MapControllers();

// Inicia a aplica��o
app.Run();
