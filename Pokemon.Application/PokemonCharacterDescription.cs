using Pokemon.Application.Interfaces;
using Pokemon.Domain;

namespace Pokemon.Application;

public class PokemonCharacterDescription : IPokemonCharacterDescription
{
    private readonly IPokeApiClientService _pokeApiClientService;
    
    public PokemonCharacterDescription(IPokeApiClientService pokeApiClientService)
    {
        _pokeApiClientService = pokeApiClientService;
    }

    // TODO: Change name
    public async Task<PokemonCharacterShake> Get(string name)
    {
        var characterDescription = await _pokeApiClientService.GetPokemonDescriptionByCharacterName(name);

        // TODO: Call shakespear api
        
        return new PokemonCharacterShake
        {
            Name = name,
            Description = characterDescription.Description
        };
    }
}