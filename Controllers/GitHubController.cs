using Blip.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly RepositoryService _repositoryService;

        // Injeção de dependência no construtor
        public GitHubController(RepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet("repos")]
        [SwaggerOperation(Summary = "Obter repositórios de um usuário do GitHub",
                          Description = "Retorna repositórios de um usuário ordenados pela data de criação (do mais antigo para o mais novo), filtrados por linguagem C#.")]
        public async Task<IActionResult> Index([FromQuery] string userName = "takenet")
        {
            // Usando "takenet" como o nome do usuário para buscar os repositórios
            var repositories = await _repositoryService.GetSortedRepositoriesAsync(userName);
            return Ok(repositories);  // Retorna os repositórios com status 200
        }
    }
}
