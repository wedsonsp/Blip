using Blip.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Blip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly RepositoryService _repositoryService;
        private readonly ILogger<GitHubController> _logger;

        // Injeção de dependência no construtor
        public GitHubController(RepositoryService repositoryService, ILogger<GitHubController> logger)
        {
            _repositoryService = repositoryService;
            _logger = logger;
        }

        [HttpGet("repos")]
        [SwaggerOperation(Summary = "Obter repositórios de um usuário do GitHub",
                          Description = "Retorna repositórios de um usuário ordenados pela data de criação (do mais antigo para o mais novo), filtrados por linguagem C#.")]
        public async Task<IActionResult> Index([FromQuery] string userName = "takenet")
        {
            try
            {
                var repositories = await _repositoryService.GetSortedRepositoriesAsync(userName);
                return Ok(repositories);  // Retorna os repositórios com status 200
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar obter os repositórios do GitHub.");
                return StatusCode(500, new { Message = "Erro ao tentar acessar os repositórios do GitHub", Detail = ex.Message });
            }
        }
    }
}
