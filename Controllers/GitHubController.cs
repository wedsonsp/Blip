using Blip.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Http;

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

        /// <summary>
        /// Obtém repositórios de um usuário do GitHub, filtrados por linguagem C#.
        /// </summary>
        /// <param name="userName">Nome de usuário do GitHub para consulta.</param>
        /// <returns>Lista de repositórios filtrados por linguagem C# e ordenados pela data de criação.</returns>
        /// <response code="200">Retorna os repositórios encontrados.</response>
        /// <response code="500">Erro ao tentar acessar os repositórios do GitHub.</response>
        [HttpGet("repos")]
        [SwaggerOperation(Summary = "Obter repositórios de um usuário do GitHub",
                          Description = "Retorna repositórios de um usuário ordenados pela data de criação (do mais antigo para o mais novo), filtrados por linguagem C#.")]
        [ProducesResponseType(typeof(List<Repositorio>), 200)]
        [ProducesResponseType(typeof(object), 500)]  // Especifica que a resposta 500 será um objeto de erro
        public async Task<IActionResult> Index([FromQuery] string userName = "takenet")
        {
            try
            {
                var repositories = await _repositoryService.GetSortedRepositoriesAsync(userName);
                return Ok(repositories);  // Retorna os repositórios com status 200
            }
            catch (Exception ex)
            {
                // Log de erro com o detalhe da exceção
                _logger.LogError(ex, "Erro ao tentar obter os repositórios do GitHub.");

                // Retorna o erro real da exceção para o cliente (não há sobreposição da mensagem)
                return StatusCode(500, new
                {
                    // Aqui não estamos modificando a mensagem. Apenas retornamos a exceção como ela é
                    Message = ex.Message,  // Mensagem real da exceção
                    StackTrace = ex.StackTrace  // Inclui a pilha de erro para depuração
                });
            }
        }
    }
}
