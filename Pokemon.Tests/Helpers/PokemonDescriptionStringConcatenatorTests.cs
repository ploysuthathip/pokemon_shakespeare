using System;
using System.Collections.Generic;
using Pokemon.Application.Helpers;
using Pokemon.Domain.DAOs.Responses;
using Xunit;

namespace Pokemon.Tests.Helpers;

public class PokemonDescriptionStringConcatenatorTests
{
    [Fact]
    public void When_an_input_is_valid_then_it_should_return_correct_concatnated_string()
    {
        // Arrange
        var entries = new List<FlavorTextEntries>
        {
            new() { FlavorText = "First paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Second paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Third paragraph.\n", Language = new Language { Name = "es"}}
        };
        
        // Act
        var result = PokemonDescriptionStringConcatenator.ConcatenateString(entries);
        
        // Arrange
        Assert.Equal("First paragraph. Second paragraph. ", result);
    }
    
    [Fact]
    public void When_an_input_does_not_contain_english_entry_then_it_should_return_empty()
    {
        // Arrange
        var entries = new List<FlavorTextEntries>
        {
            new() { FlavorText = "Primer párrafo.\n", Language = new Language { Name = "es"}},
            new() { FlavorText = "Segundo párrafo.\n", Language = new Language { Name = "es"}}
        };
        
        // Act
        var result = PokemonDescriptionStringConcatenator.ConcatenateString(entries);
        
        // Arrange
        Assert.Equal("", result);
    }
    
    [Fact]
    public void When_an_input_contains_duplication_then_it_should_be_eliminated()
    {
        // Arrange
        var entries = new List<FlavorTextEntries>
        {
            new() { FlavorText = "First paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Second paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Second paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Third paragraph.\n", Language = new Language { Name = "en"}},
            new() { FlavorText = "Third paragraph.\n", Language = new Language { Name = "en"}}
        };
        
        // Act
        var result = PokemonDescriptionStringConcatenator.ConcatenateString(entries);
        
        // Arrange
        Assert.Equal("First paragraph. Second paragraph. Third paragraph. ", result);
    }
    
    [Fact]
    public void When_an_empty_list_is_given_then_an_InvalidOperationException_should_be_thrown()
    {
        Assert.Throws<InvalidOperationException>(() =>
            PokemonDescriptionStringConcatenator.ConcatenateString(new List<FlavorTextEntries>()));
    }
}