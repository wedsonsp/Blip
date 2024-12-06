using Blip.Models;
using Blip.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blip.Repositories
{
    public class GitHubRepository : IGitHubRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GitHubRepository> _logger;

        public GitHubRepository(HttpClient httpClient, ILogger<GitHubRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Repositorio>> GetRepositoriesByUserAsync(string userName)
        {
            var repositories = new List<Repositorio>();
            int page = 1;
            bool hasNextPage = true;

            // Definir o User-Agent (necessário para chamadas à API do GitHub)
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Blip");

            while (hasNextPage)
            {
                var url = $"https://api.github.com/users/{userName}/repos?page={page}&per_page=100";

                try
                {
                    // Realizando a requisição HTTP para buscar os repositórios
                    var response = await _httpClient.GetAsync(url);

                    // Log para ver o código de status da resposta
                    _logger.LogInformation($"Consultando URL: {url}");
                    _logger.LogInformation($"Status da resposta: {response.StatusCode}");

                    // Se o status não for OK (200), logue o conteúdo da resposta
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Erro ao obter dados do GitHub: {errorContent}");
                        throw new Exception($"Erro ao acessar repositórios. Status: {response.StatusCode}, Detalhes: {errorContent}");
                    }

                    // Lê a resposta em string e tenta desserializar o JSON
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var pageRepositories = JsonSerializer.Deserialize<List<Repositorio>>(responseBody);

                    if (pageRepositories != null)
                    {
                        repositories.AddRange(pageRepositories);
                    }

                    // Verifica se existe uma próxima página
                    var linkHeader = response.Headers.Contains("Link")
                        ? response.Headers.GetValues("Link").FirstOrDefault()
                        : null;

                    if (linkHeader != null && linkHeader.Contains("rel=\"next\""))
                    {
                        page++;
                    }
                    else
                    {
                        hasNextPage = false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, $"Erro ao tentar fazer a requisição para a URL: {url}");
                    throw new Exception("Erro ao processar repositórios do GitHub.", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro inesperado ao acessar os repositórios do GitHub para o usuário {userName}.");
                    throw new Exception("Erro inesperado ao processar repositórios.", ex);
                }
            }

            return repositories;
        }


    }
}
