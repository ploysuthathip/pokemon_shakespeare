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
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError(ex, $"Not found response from the API. Status code: HTTP {ex.StatusCode}");
                    return NotFound(ErrorMessages.NotFoundMessage);
                }

                _logger.LogError(ex, $"Unsuccessful response from the API. Status code: HTTP {ex.StatusCode}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new {message = ErrorMessages.GenericErrorMessage});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occured, see the log for more details");
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new {message = ErrorMessages.GenericErrorMessage});
            }

            return Ok(response);
        }
    }
}