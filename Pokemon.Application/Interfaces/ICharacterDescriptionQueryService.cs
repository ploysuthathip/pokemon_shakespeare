using Pokemon.Domain;

namespace Pokemon.Application.Interfaces;

public interface ICharacterDescriptionQueryService
{
    Task<PokemonCharacterShake> GetDescription(string name);
}