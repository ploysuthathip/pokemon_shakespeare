using Pokemon.Domain;

namespace Pokemon.Application;

public interface IPokemonCharacterDescription
{
    Task<PokemonCharacterShake> Get(string name);
}