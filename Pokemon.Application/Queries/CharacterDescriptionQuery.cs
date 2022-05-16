using Microsoft.Extensions.Logging;
using Pokemon.Application.Contracts;
using Pokemon.Application.Helpers;
using Pokemon.Application.Interfaces;
using Pokemon.Domain;
using Pokemon.Domain.DAOs.Requests;

namespace Pokemon.Application.Queries;

public class CharacterDescriptionQuery : ICharacterDescriptionQuery
{
    private readonly IPokeApiClientService _pokeApiClientService;
    private readonly IShakespeareTranslatorApiClientService _shakespeareClientService;
    private readonly ILogger<CharacterDescriptionQuery> _logger;

    public CharacterDescriptionQuery(
        IPokeApiClientService pokeApiClientService, 
        IShakespeareTranslatorApiClientService shakespeareClientService,
        ILogger<CharacterDescriptionQuery> logger)
    {
        _pokeApiClientService = pokeApiClientService;
        _shakespeareClientService = shakespeareClientService;
        _logger = logger;
    }

    public async Task<PokemonCharacterShakespeare> GetDescription(string name)
    {
        var characterDescription = await _pokeApiClientService.GetPokemonDescriptionByCharacterName(name);
        
        _logger.LogInformation("Successful call to PokeAPI.");
        
        var fullDescription = PokemonDescriptionStringConcatenator.ConcatenateString(characterDescription.FlavorTextEntries);

        _logger.LogInformation($"Successfully concatenated string from API response: {fullDescription}");
        
        var translatedShakespeareText = await _shakespeareClientService.GetTranslatedShakespeareText(
                new ShakeSpearApiRequest
                {
                    Text = fullDescription
                });

        _logger.LogInformation($"Successful response from Shakespear transalator API. /nResponse: {translatedShakespeareText}");
        
        return new PokemonCharacterShakespeare
        {
            Name = name,
            Description = translatedShakespeareText.Content.TranslatedText
        };
    }
}