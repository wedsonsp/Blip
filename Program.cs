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

// Configuração do HttpClient para o repositório
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "BlipApp");
    });

// Registra o serviço RepositoryService
builder.Services.AddScoped<RepositoryService>();

// Adiciona suporte para controllers
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware para redirecionamento HTTPS
app.UseHttpsRedirection();

// Habilita o CORS
app.UseCors("AllowAll");

// Habilita o uso de autorização
app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

// Ativa o Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
