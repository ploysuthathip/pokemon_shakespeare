using System.Net.Http;
using System.Threading.Tasks;
using Pokemon.Application.Contracts;
using Pokemon.Domain.DAOs.Requests;
using Pokemon.Infrastructure;
using Xunit;

namespace Pokemon.IntegrationTests;

public class ShakespeareTranslatorApiClientServiceTests
{
    private readonly IShakespeareTranslatorApiClientService _sut;

    public ShakespeareTranslatorApiClientServiceTests(IHttpClientFactory _httpClientFactory)
    {
        _sut = new ShakespeareTranslatorApiClientService(_httpClientFactory);
    }
    
    [Fact]
    public async Task Given_the_api_call_is_valid_then_it_should_return_the_correct_response()
    {
        // Arrange
        var request = new ShakeSpearApiRequest { Text = "Spits fire that is hot enough to melt boulders. Known to cause forest." };

        // Act
        var response = await _sut.GetTranslatedShakespeareText(request);

        // Assert
        Assert.Equal("Spits fire yond is hot enow to melt boulders. Known to cause forest.", response.Content.TranslatedText);
    }
    
     
    [Fact]
    public async Task Given_the_api_request_is_invalid_then_it_should_throws_an_exception()
    {
        // Arrange
        var request = new ShakeSpearApiRequest { Text = "Spits fire that is hot enough to melt boulders. Known to cause forest." };

        // Act
        var response = await _sut.GetTranslatedShakespeareText(request);

        // Assert
        Assert.Equal("Spits fire yond is hot enow to melt boulders. Known to cause forest.", response.Content.TranslatedText);
    }
}