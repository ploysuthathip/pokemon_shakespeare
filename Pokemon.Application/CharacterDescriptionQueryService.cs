using Pokemon.Application.Contracts;
using Pokemon.Application.Helpers;
using Pokemon.Application.Interfaces;
using Pokemon.Domain;
using Pokemon.Domain.DAOs.Requests;

namespace Pokemon.Application;

public class CharacterDescriptionQueryService : ICharacterDescriptionQueryService
{
    private readonly IPokeApiClientService _pokeApiClientService;
    private readonly IShakespeareTranslatorApiClientService _shakespeareClientService;

    public CharacterDescriptionQueryService(IPokeApiClientService pokeApiClientService, IShakespeareTranslatorApiClientService shakespeareClientService)
    {
        _pokeApiClientService = pokeApiClientService;
        _shakespeareClientService = shakespeareClientService;
    }

    public async Task<PokemonCharacterShake> GetDescription(string name)
    {
        var characterDescription = await _pokeApiClientService.GetPokemonDescriptionByCharacterName(name);
        
        var fullDescription = PokemonDescriptionStringConcatenator.ConcatenateString(characterDescription.FlavorTextEntries);

        var translatedShakespeareText = await _shakespeareClientService.GetTranslatedShakespeareText(
                new ShakeSpearApiRequest
                {
                    Text = fullDescription
                });

        return new PokemonCharacterShake
        {
            Name = name,
            Description = translatedShakespeareText.Content.TranslatedText
        };
    }
}