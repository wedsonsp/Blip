using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registre os serviços no DI

// Registra o repositório GitHubRepository para ser injetado onde for necessário
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>();

// Registra o serviço RepositoryService
builder.Services.AddScoped<RepositoryService>();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
