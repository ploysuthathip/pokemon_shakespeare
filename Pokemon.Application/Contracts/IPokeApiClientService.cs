using Pokemon.Domain.Dao;

namespace Pokemon.Application.Interfaces;

public interface IPokeApiClientService
{
    Task<PokemonCharacter> GetPokemonDescriptionByCharacterName(string name);
}