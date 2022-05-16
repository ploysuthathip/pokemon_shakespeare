using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokemon.Api;
using Pokemon.Api.Controllers;
using Pokemon.Application.Interfaces;
using Pokemon.Domain;
using Xunit;

namespace Pokemon.Tests.Controller;

public class PokemonControllerTests
{
    private readonly PokemonController _sut;
    private readonly Mock<ICharacterDescriptionQuery> _characterDescriptionQuery;
    private static string Name => "Charizard";
        
    public PokemonControllerTests()
    {
        _characterDescriptionQuery = new Mock<ICharacterDescriptionQuery>();
        var logger = new Mock<ILogger<PokemonController>>();
        
        _sut = new PokemonController(_characterDescriptionQuery.Object, logger.Object);
    }

    [Fact]
    public async Task When_a_valid_request_is_made_and_there_are_no_exception_then_it_should_return_successful_with_correct_response()
    {
        // Arrange
        const string description = "Charizard description";

        _characterDescriptionQuery.Setup(query => query.GetDescription(It.Is<string>(parameter => parameter == Name)))
            .ReturnsAsync(new PokemonCharacterShakespeare
            {
                Name = Name,
                Description = description
            });
        
        // Act
        var contentResult = await _sut.GetCharacterByName(Name);
        
        // Assert
        var result = ((OkObjectResult) contentResult).Value;
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<PokemonCharacterShakespeare>(result);
        Assert.Equal(Name, ((PokemonCharacterShakespeare)result).Name);
        Assert.Equal(description, ((PokemonCharacterShakespeare)result).Description);
    }
    
    [Fact]
    public async Task When_an_InvalidOperation_is_thrown_then_it_should_return_InternelServerError_with_expected_error_message()
    {
        // Arrange
        _characterDescriptionQuery.Setup(query => query.GetDescription(It.Is<string>(parameter => parameter == Name)))
            .ThrowsAsync(new InvalidOperationException());
        
        // Act
        var contentResult = await _sut.GetCharacterByName(Name);
        
        // Assert
        var result = (ObjectResult) contentResult;
        
        Assert.Equal(500, result.StatusCode);
        Assert.Contains(ErrorMessages.GenericErrorMessage, result.Value.ToString());
    }
    
    [Fact]
    public async Task When_a_HttpRequestException_for_not_found_is_thrown_then_it_should_return_NotFound_with_expected_error_message()
    {
        // Arrange
        _characterDescriptionQuery.Setup(query => query.GetDescription(It.Is<string>(parameter => parameter == Name)))
            .ThrowsAsync(new HttpRequestException(message: "Not found", null, HttpStatusCode.NotFound));
        
        // Act
        var contentResult = await _sut.GetCharacterByName(Name);
        
        // Assert
        var result = (ObjectResult) contentResult;
        
        Assert.Equal(404, result.StatusCode);
        Assert.Contains(ErrorMessages.NotFoundMessage, result.Value.ToString());
    }
    
    [Fact]
    public async Task When_a_HttpRequestException_is_thrown_then_it_should_return_InternelServerError_with_expected_error_message()
    {
        // Arrange
        _characterDescriptionQuery.Setup(query => query.GetDescription(It.Is<string>(parameter => parameter == Name)))
            .ThrowsAsync(new HttpRequestException("Internal error", null, HttpStatusCode.InternalServerError));
        
        // Act
        var contentResult = await _sut.GetCharacterByName(Name);
        
        // Assert
        var result = (ObjectResult) contentResult;
        
        Assert.Equal(500, result.StatusCode);
        Assert.Contains(ErrorMessages.GenericErrorMessage, result.Value.ToString());
    }
}