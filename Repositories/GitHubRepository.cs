using Blip.Models;
using Blip.Repositories.Interfaces;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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
            // Configura o User-Agent uma vez para todas as requisições
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
        }

        public async Task<List<Repositorio>> GetRepositoriesByUserAsync(string userName)
        {
            var repositories = new List<Repositorio>();
            int page = 1;
            bool hasNextPage = true;

            try
            {
                while (hasNextPage)
                {
                    var url = $"https://api.github.com/users/{userName}/repos?page={page}&per_page=100";
                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Erro ao acessar o GitHub: {response.StatusCode} - {response.ReasonPhrase}");
                        throw new Exception($"Erro ao acessar a API do GitHub: {response.StatusCode}");
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserializar a resposta JSON para uma lista de repositórios
                    var pageRepositories = JsonSerializer.Deserialize<List<Repositorio>>(responseContent);

                    if (pageRepositories != null)
                    {
                        repositories.AddRange(pageRepositories);
                    }

                    // Lê o cabeçalho de link para saber se há mais páginas
                    var linkHeader = response.Headers.TryGetValues("Link", out var linkValues)
                        ? linkValues.FirstOrDefault()
                        : null;

                    // Se houver uma próxima página
                    if (linkHeader != null && linkHeader.Contains("rel=\"next\""))
                    {
                        page++;
                    }
                    else
                    {
                        hasNextPage = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar repositórios do GitHub.");
                throw new Exception("Erro ao obter repositórios do GitHub.", ex);
            }

            return repositories;
        }
    }
}
