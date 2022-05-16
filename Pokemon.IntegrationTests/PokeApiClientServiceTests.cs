using System;
using System.Threading.Tasks;
using Pokemon.Application.Contracts;
using Xunit;

namespace Pokemon.IntegrationTests;

public class PokeApiClientServiceTests
{
    private readonly IPokeApiClientService _pokeApiClientService;
    
    public PokeApiClientServiceTests(IPokeApiClientService pokeApiClientService)
    {
        _pokeApiClientService = pokeApiClientService;
    }
    
    [Fact]
    public async Task Given_the_api_call_is_valid_then_it_should_return_the_correct_response()
    {
        // Arrange
        const string request = "Ditto";

        // Act
        var response = await _pokeApiClientService.GetPokemonDescriptionByCharacterName(request);

        // Assert
        //Assert.Equal();
    }
    
     
    [Fact]
    public async Task Given_the_api_request_is_invalid_then_it_should_throws_an_exception()
    {
        // Arrange
        const string request = "eee";

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _pokeApiClientService.GetPokemonDescriptionByCharacterName(request));
        
        // Assert
        Assert.Equal("expected error message here", exception.Message);
    }
}