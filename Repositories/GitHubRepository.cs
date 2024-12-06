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

            try
            {
                while (hasNextPage)
                {
                    var url = $"https://api.github.com/users/{userName}/repos?page={page}&per_page=100";

                    // Log da URL sendo acessada
                    _logger.LogInformation($"Requisitando repositórios para o usuário {userName} - Página {page} - URL: {url}");

                    var response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Erro ao acessar a API do GitHub: {response.StatusCode} - {response.ReasonPhrase}");
                        throw new Exception($"Erro ao acessar a API do GitHub: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Resposta da API GitHub (primeiros 200 caracteres): {responseContent.Substring(0, Math.Min(200, responseContent.Length))}");

                    var pageRepositories = JsonSerializer.Deserialize<List<Repositorio>>(responseContent);

                    if (pageRepositories != null)
                    {
                        repositories.AddRange(pageRepositories);
                    }

                    var linkHeader = response.Headers.TryGetValues("Link", out var linkValues)
                        ? linkValues.FirstOrDefault()
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar acessar os repositórios do GitHub.");
                throw new Exception("Erro ao tentar acessar os repositórios do GitHub.", ex);
            }

            return repositories;
        }
    }
}
