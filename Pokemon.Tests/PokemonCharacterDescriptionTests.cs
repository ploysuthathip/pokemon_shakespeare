using System.Threading.Tasks;
using Moq;
using Pokemon.Application;
using Pokemon.Application.Interfaces;
using Pokemon.Domain.Dao;
using Xunit;

namespace Pokemon.Tests;

public class PokemonCharacterDescriptionTests
{
    private readonly Mock<IPokeApiClientService> _mockPokeApiClientService;
    private PokemonCharacterDescription _pokemonCharacterDescription;

    public PokemonCharacterDescriptionTests()
    {
        _mockPokeApiClientService = new Mock<IPokeApiClientService>();
        _pokemonCharacterDescription = new PokemonCharacterDescription(_mockPokeApiClientService.Object);
    }
    
    [Fact]
    public async Task When_getting_character_description_by_name_then_it_should_return_the_expected()
    {
        // Arrange
        const string name = "Charizard";

        _mockPokeApiClientService
            .Setup(service => service.GetPokemonDescriptionByCharacterName(It.Is<string>(s => s == name)))
            .ReturnsAsync(new PokemonCharacter{ Description = ""});

        // Act
        _pokemonCharacterDescription = new PokemonCharacterDescription(_mockPokeApiClientService.Object);
        var response = await _pokemonCharacterDescription.Get(name);

        // Assert
        Assert.Equal(name, response.Name);
    }
}