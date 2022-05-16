using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Pokemon.Infrastructure;
using Xunit;

namespace Pokemon.Tests;

public class PokeApiClientServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly PokeApiClientService _sut;
    private static string PokemonName => "Charizard";
        
    public PokeApiClientServiceTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _sut = new PokeApiClientService(_mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task When_the_request_is_successful_then_it_should_return_the_expected()
    {
        // Arrange

        var jsonResponse = @"{
            ""base_happiness"": 50,
            ""flavor_text_entries"": [
            {
                ""flavor_text"": ""Spits fire that\nis hot enough to\nmelt boulders."",
                ""language"": {
                    ""name"": ""en"",
                    ""url"": ""https://pokeapi.co/api/v2/language/9/""
                },
                ""version"": {
                ""name"": ""red"",
                ""url"": ""https://pokeapi.co/api/v2/version/1/""
                }
            },
            {
                ""flavor_text"": ""The red\fflame at the tip\nof its tail burns\nmore intensely."",
                ""language"": {
                    ""name"": ""en"",
                    ""url"": ""https://pokeapi.co/api/v2/language/9/""
                }
            }]}";
        
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        var httpMessageHandler = new Mock<HttpMessageHandler>();
        
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("https://pokeapi.co");
        
        _mockHttpClientFactory.Setup(client => client.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        
        // Act
        var result = await _sut.GetPokemonDescriptionByCharacterName(PokemonName);

        // Assert
        Assert.NotNull(result);
        
        var firstParagraph = result.FlavorTextEntries.First().FlavorText;
        var secondParagraph = result.FlavorTextEntries.Last().FlavorText;
        
        Assert.Equal("Spits fire that\nis hot enough to\nmelt boulders.", firstParagraph);
        Assert.Equal("The red\fflame at the tip\nof its tail burns\nmore intensely.", secondParagraph);
    }
    
    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task When_the_request_is_not_successful_then_it_should_throw_a_HTTPRequestException(HttpStatusCode statusCode)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode
        };

        var httpMessageHandler = new Mock<HttpMessageHandler>();
        
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        var httpClient = new HttpClient(httpMessageHandler.Object);
        httpClient.BaseAddress = new Uri("https://pokeapi.co");
        
        _mockHttpClientFactory.Setup(client => client.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.GetPokemonDescriptionByCharacterName(PokemonName));
    }
}