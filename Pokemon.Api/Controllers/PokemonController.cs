using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application.Interfaces;
using Pokemon.Domain;

namespace Pokemon.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ICharacterDescriptionQuery _characterDescriptionQuery;
        private readonly ILogger _logger;

        public PokemonController(ICharacterDescriptionQuery characterDescriptionQuery, ILogger<PokemonController> logger)
        {
            _characterDescriptionQuery = characterDescriptionQuery;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("{name}")]
        [HttpGet]
        public async Task<ActionResult> GetCharacterByName([FromRoute] string name)
        {
            PokemonCharacterShakespeare response;

            try
            {
                _logger.LogInformation($"Request received with parameter: {name}");
                
                response = await _characterDescriptionQuery.GetDescription(name);
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = "";
                
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    errorMessage = "Unable to find record. Please ensure that the correct character name is provided.";
                    
                    _logger.LogError(ex, $"Not found response from the API. Status code: HTTP {ex.StatusCode}");
                }
                else
                {
                    errorMessage = "Unable to complete the request at this time. Please try again later.";
                    
                    _logger.LogError(ex, $"Unsuccessful response from the API. Status code: HTTP {ex.StatusCode}");
                }
                    
                                
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = errorMessage });
            }

            return Ok(response);
        }
    }
}