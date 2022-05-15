using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Pokemon.Application;
using Pokemon.Application.Contracts;
using Pokemon.Application.Interfaces;
using Pokemon.Domain.DAOs.Requests;
using Pokemon.Domain.DAOs.Responses;
using Xunit;

namespace Pokemon.Tests;

public class CharacterDescriptionQueryServiceTests
{
    private readonly Mock<IPokeApiClientService> _mockPokeApiClientService;
    private readonly Mock<IShakespeareTranslatorApiClientService> _mockShakespeareTranslatorApiClientService;
    
    private CharacterDescriptionQueryService _characterDescriptionQueryService;

    public CharacterDescriptionQueryServiceTests()
    {
        _mockPokeApiClientService = new Mock<IPokeApiClientService>();
        _mockShakespeareTranslatorApiClientService = new Mock<IShakespeareTranslatorApiClientService>();
        
        _characterDescriptionQueryService = new CharacterDescriptionQueryService(
            _mockPokeApiClientService.Object, 
            _mockShakespeareTranslatorApiClientService.Object);
    }
    
    [Fact]
    public async Task When_getting_character_description_by_name_then_it_should_return_the_expected()
    {
        // Arrange
        const string name = "Charizard";
        const string description = "When the time comes, describe something";
        const string translatedDescription = "At which hour the time cometh, describe something";

        _mockPokeApiClientService
            .Setup(service => service.GetPokemonDescriptionByCharacterName(It.Is<string>(s => s == name)))
            .ReturnsAsync(new PokemonApiResponse
            {
                FlavorTextEntries = new List<FlavorTextEntries>
                {
                    new()
                    {
                        FlavorText = description, 
                        Language = new Language { Name = "en" }
                    }
                }
            });

        _mockShakespeareTranslatorApiClientService
            .Setup(service => service.GetTranslatedShakespeareText(It.IsAny<ShakeSpearApiRequest>()))
            .ReturnsAsync(new ShakespeareApiResponse
            {
                Content = new Content
                {
                    TranslatedText = translatedDescription
                }
            });

        // Act
        var response = await _characterDescriptionQueryService.GetDescription(name);

        // Assert
        Assert.Equal(name, response.Name);
        Assert.Equal(translatedDescription, response.Description);
    }
}