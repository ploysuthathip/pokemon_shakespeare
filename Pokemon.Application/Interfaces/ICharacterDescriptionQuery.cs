using Pokemon.Domain;

namespace Pokemon.Application.Interfaces;

public interface ICharacterDescriptionQuery
{
    Task<PokemonCharacterShakespeare> GetDescription(string name);
}