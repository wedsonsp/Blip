using Blip.Models;

namespace Blip.Repositories.Interfaces
{
    public interface IGitHubRepository
    {
        Task<List<Repositorio>> GetRepositoriesByUserAsync(string userName);


    }
}
