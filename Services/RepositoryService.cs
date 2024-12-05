using Blip.Models;
using Blip.Repositories.Interfaces;

public class RepositoryService
{
    private readonly IGitHubRepository _gitHubRepository;

    public RepositoryService(IGitHubRepository gitHubRepository)
    {
        _gitHubRepository = gitHubRepository;
    }

    public async Task<List<Repositorio>> GetSortedRepositoriesAsync(string userName)
    {
        // Obtendo todos os repositórios, sem limite de página
        var repositories = await _gitHubRepository.GetRepositoriesByUserAsync(userName);

        if (repositories == null || repositories.Count == 0)
        {
            return new List<Repositorio>(); // Nenhum repositório encontrado
        }

        // Filtrando repositórios que são do tipo "C#" e ordenando pela data de criação (do mais antigo para o mais novo)
        var filteredAndSortedRepositories = repositories
            .Where(r => r.Language != null && r.Language.Equals("C#", StringComparison.OrdinalIgnoreCase))  // Filtra pelo tipo "C#"
            .OrderBy(r => r.CreatedAt)  // Ordena pela data de criação (do mais antigo para o mais novo)
            .Take(5)  // Pega apenas os primeiros 5
            .ToList();

        return filteredAndSortedRepositories;
    }
}
