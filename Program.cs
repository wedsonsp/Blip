using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configura��o de CORS (se necess�rio)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Permite qualquer origem
              .AllowAnyMethod()   // Permite qualquer m�todo HTTP
              .AllowAnyHeader();  // Permite qualquer cabe�alho
    });
});

// Registre os servi�os no DI

// Registra o reposit�rio GitHubRepository para ser injetado onde for necess�rio
// Configura��o do HttpClient para o reposit�rio
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>()
    .ConfigureHttpClient(client =>
    {
        // Configura��o adicional do HttpClient, caso necess�rio
        client.DefaultRequestHeaders.Add("User-Agent", "BlipApp");  // Exemplo de configura��o
    });

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

// Ativa o Swagger para documentar a API
app.UseSwagger();
app.UseSwaggerUI();

// Middleware para redirecionamento HTTPS (garante que todas as requisi��es v�o para HTTPS)
app.UseHttpsRedirection();

// Habilita o CORS (caso necess�rio para aplica��es em front-end separadas)
app.UseCors("AllowAll");

// Habilita o uso de autoriza��o (caso esteja utilizando autentica��o, adicione o middleware de autentica��o)
app.UseAuthorization();

// Mapeia os controllers, isso � necess�rio para APIs
app.MapControllers();

// Inicia a aplica��o
app.Run();
