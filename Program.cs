using Blip.Repositories.Interfaces;
using Blip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Registre os servi�os no DI

// Registra o reposit�rio GitHubRepository para ser injetado onde for necess�rio
builder.Services.AddHttpClient<IGitHubRepository, GitHubRepository>();

// Registra o servi�o RepositoryService
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
