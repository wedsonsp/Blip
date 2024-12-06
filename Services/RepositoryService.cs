using Blip.Models;
using Blip.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RepositoryService
{
    private readonly IGitHubRepository _gitHubRepository;

    public RepositoryService(IGitHubRepository gitHubRepository)
    {
        _gitHubRepository = gitHubRepository;
    }

    public async Task<List<Repositorio>> GetSortedRepositoriesAsync(string userName)
    {
        try
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
        catch (Exception ex)
        {
            // Logar o erro completo para obter mais informações
            Console.WriteLine($"Erro ao processar repositórios do GitHub: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }

            // Caso ocorra algum erro, re-raise a exceção original
            throw new Exception("Erro ao processar repositórios do GitHub.", ex);
        }
    }

}
