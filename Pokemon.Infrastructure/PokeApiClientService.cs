using System.Text.Json;
using Pokemon.Application.Contracts;
using Pokemon.Domain.DAOs.Responses;

namespace Pokemon.Infrastructure;

public class PokeApiClientService : IPokeApiClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PokeApiClientService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<PokemonApiResponse> GetPokemonDescriptionByCharacterName(string name)
    {
        var httpClient = _httpClientFactory.CreateClient("PokeApi");
        var httpResponseMessage = await httpClient.GetAsync($"api/v2/pokemon-species/{name}");

        httpResponseMessage.EnsureSuccessStatusCode();
        
        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            
        return await JsonSerializer.DeserializeAsync<PokemonApiResponse>(contentStream);
    }
}