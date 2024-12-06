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
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BlipApp"); // Ajuste o nome da aplicação conforme necessário.

            while (hasNextPage)
            {
                // Corrigindo a URL da API para pegar repositórios do usuário
                var url = $"https://api.github.com/users/{userName}/repos?page={page}&per_page=100";  // 100 repositórios por página

                try
                {
                    // Fazendo a requisição HTTP para a API do GitHub
                    var response = await _httpClient.GetStringAsync(url);

                    // Deserializa a resposta JSON para uma lista de repositórios
                    var pageRepositories = JsonSerializer.Deserialize<List<Repositorio>>(response);

                    if (pageRepositories != null)
                    {
                        repositories.AddRange(pageRepositories);
                    }

                    // Verifica se há mais páginas de repositórios
                    var linkHeader = _httpClient.DefaultRequestHeaders.TryGetValues("Link", out var linkValues)
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
                catch (HttpRequestException ex)
                {
                    // Log detalhado da exceção
                    _logger.LogError(ex, $"Erro na requisição HTTP ao GitHub. URL: {url}");
                    throw new Exception("Erro ao processar repositórios do GitHub.", ex);
                }
                catch (Exception ex)
                {
                    // Log para qualquer outra exceção
                    _logger.LogError(ex, $"Erro inesperado ao acessar os repositórios do GitHub para o usuário {userName}.");
                    throw new Exception("Erro inesperado ao processar repositórios.", ex);
                }
            }

            return repositories;
        }

    }
}
