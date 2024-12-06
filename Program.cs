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

// Configura��o do HttpClient para o reposit�rio
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>()
    .ConfigureHttpClient(client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "BlipApp");
    });

// Registra o servi�o RepositoryService
builder.Services.AddScoped<RepositoryService>();

// Adiciona suporte para controllers
builder.Services.AddControllers();

// Configura��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware para redirecionamento HTTPS
app.UseHttpsRedirection();

// Habilita o CORS
app.UseCors("AllowAll");

// Habilita o uso de autoriza��o
app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

// Ativa o Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
