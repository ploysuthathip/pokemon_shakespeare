using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Application;
using Pokemon.Domain;

namespace Pokemon.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonCharacterDescription _pokemonCharacterDescription;
        
        // TODO: Use ILogger
        public PokemonController(IPokemonCharacterDescription pokemonCharacterDescription)
        {
            _pokemonCharacterDescription = pokemonCharacterDescription;
        }
        
        [AllowAnonymous]
        [Route("{name}")]
        [HttpGet]
        public async Task<PokemonCharacterShake> GetCharacterByName([FromRoute] string name)
        {
            var response = await _pokemonCharacterDescription.Get(name);
            
            return response;
        }
    }
}
