using System.Text.Json;
using Pokemon.Application.Interfaces;
using Pokemon.Domain.Dao;

namespace Pokemon.Infrastructure;

public class PokeApiClientService : IPokeApiClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PokeApiClientService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<PokemonCharacter> GetPokemonDescriptionByCharacterName(string name)
    {
        var httpClient = _httpClientFactory.CreateClient("PokeApi");
        var httpResponseMessage = await httpClient.GetAsync($"api/v2/pokemon-species/{name}");

        // TODO: Add better exception and logging
        if (!httpResponseMessage.IsSuccessStatusCode) throw new Exception("Failed to make a request");

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            
        return await JsonSerializer.DeserializeAsync<PokemonCharacter>(contentStream);
    }
}