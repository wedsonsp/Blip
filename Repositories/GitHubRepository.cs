using Blip.Models;
using Blip.Repositories.Interfaces;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Blip.Repositories
{
    public class GitHubRepository : IGitHubRepository
    {
        private readonly HttpClient _httpClient;

        public GitHubRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Repositorio>> GetRepositoriesByUserAsync(string userName)
        {
            var repositories = new List<Repositorio>();
            int page = 1;
            bool hasNextPage = true;

            // Definir o User-Agent (necessário para chamadas à API do GitHub)
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");

            while (hasNextPage)
            {
                // URL da API pública do GitHub para obter repositórios de um usuário, incluindo a paginação
                var url = $"https://api.github.com/users/{userName}/repos?page={page}&per_page=100"; // 100 repositórios por página

                // Realizando a requisição HTTP para buscar os repositórios
                var response = await _httpClient.GetStringAsync(url);

                // Deserializar a resposta JSON para uma lista de repositórios
                var pageRepositories = JsonSerializer.Deserialize<List<Repositorio>>(response);

                // Adicionando os repositórios da página à lista principal
                if (pageRepositories != null)
                {
                    repositories.AddRange(pageRepositories);
                }

                // Verifica se existe uma próxima página. O GitHub inclui um cabeçalho "Link" com informações de paginação.
                var linkHeader = _httpClient.DefaultRequestHeaders.TryGetValues("Link", out var linkValues)
                    ? linkValues.FirstOrDefault()
                    : null;

                if (linkHeader != null && linkHeader.Contains("rel=\"next\""))
                {
                    // Se o cabeçalho Link contiver "rel=\"next\"", há mais páginas para buscar
                    page++;
                }
                else
                {
                    // Caso contrário, não há mais páginas
                    hasNextPage = false;
                }
            }

            return repositories;
        }
    }
}
