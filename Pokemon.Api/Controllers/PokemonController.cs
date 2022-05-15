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
        private readonly ICharacterDescriptionQueryService _characterDescriptionQueryService;
        private readonly ILogger _logger;

        public PokemonController(ICharacterDescriptionQueryService characterDescriptionQueryService, ILogger<PokemonController> logger)
        {
            _characterDescriptionQueryService = characterDescriptionQueryService;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("{name}")]
        [HttpGet]
        public async Task<ActionResult> GetCharacterByName([FromRoute] string name)
        {
            PokemonCharacterShake response;

            try
            {
                response = await _characterDescriptionQueryService.GetDescription(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured, see the log for more details");

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message});
            }

            return Ok(response);
        }
    }
}