using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Application.Contracts;

public interface IPokeApiClientService
{
    Task<PokemonApiResponse> GetPokemonDescriptionByCharacterName(string name);
}