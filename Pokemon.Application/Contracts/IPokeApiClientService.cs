using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Application.Interfaces;

public interface IPokeApiClientService
{
    Task<PokemonApiResponse> GetPokemonDescriptionByCharacterName(string name);
}