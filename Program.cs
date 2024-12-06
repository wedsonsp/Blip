using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS (se necessário)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Permite qualquer origem
              .AllowAnyMethod()   // Permite qualquer método HTTP
              .AllowAnyHeader();  // Permite qualquer cabeçalho
    });
});

// Registre os serviços no DI

// Registra o repositório GitHubRepository para ser injetado onde for necessário
// Configuração do HttpClient para o repositório
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>()
    .ConfigureHttpClient(client =>
    {
        // Configuração adicional do HttpClient, caso necessário
        client.DefaultRequestHeaders.Add("User-Agent", "BlipApp");  // Exemplo de configuração
    });

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

// Ativa o Swagger para documentar a API
app.UseSwagger();
app.UseSwaggerUI();

// Middleware para redirecionamento HTTPS (garante que todas as requisições vão para HTTPS)
app.UseHttpsRedirection();

// Habilita o CORS (caso necessário para aplicações em front-end separadas)
app.UseCors("AllowAll");

// Habilita o uso de autorização (caso esteja utilizando autenticação, adicione o middleware de autenticação)
app.UseAuthorization();

// Mapeia os controllers, isso é necessário para APIs
app.MapControllers();

// Inicia a aplicação
app.Run();
