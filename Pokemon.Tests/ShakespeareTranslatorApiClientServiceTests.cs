using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Pokemon.Domain.DAOs.Requests;
using Pokemon.Infrastructure;
using Xunit;

namespace Pokemon.Tests;

public class ShakespeareTranslatorApiClientServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly ShakespeareTranslatorApiClientService _sut;
        
    public ShakespeareTranslatorApiClientServiceTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _sut = new ShakespeareTranslatorApiClientService(_mockHttpClientFactory.Object);
    }
    
    [Fact]
    public async Task When_the_request_is_successful_then_it_should_return_the_expected()
    {
        // Arrange
        const string textToBeTranslated = "Spits fire that is hot enough to melt boulders.";
        
        const string jsonResponse = @"
               {
                   ""success"": {
                   ""total"": 1
                   },
                   ""contents"": {
                        ""translated"": ""Spits fire yond is hot enow to melt boulders."",
                        ""text"": ""Spits fire that is hot enough to melt boulders."",
                        ""translation"": ""shakespeare""
                    }
                }";
        
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
        httpClient.BaseAddress = new Uri("https://api.funtranslations.com");
        
        _mockHttpClientFactory.Setup(client => client.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        
        // Act
        var result = await _sut.GetTranslatedShakespeareText(new ShakeSpearApiRequest{ Text = textToBeTranslated});

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Spits fire yond is hot enow to melt boulders.", result.Content.TranslatedText);
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
        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.GetTranslatedShakespeareText(new ShakeSpearApiRequest{ Text = "textToBeTranslated"}));
    }
}