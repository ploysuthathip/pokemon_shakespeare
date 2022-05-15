using System.Net;
using System.Text;
using System.Text.Json;
using Pokemon.Application.Contracts;
using Pokemon.Application.Interfaces;
using Pokemon.Domain.DAOs.Requests;
using Pokemon.Domain.DAOs.Responses;


namespace Pokemon.Infrastructure;

public class ShakespeareTranslatorApiClientService : IShakespeareTranslatorApiClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ShakespeareTranslatorApiClientService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;
    
    public async Task<ShakespeareApiResponse> GetTranslatedShakespeareText(ShakeSpearApiRequest shakeSpearApiRequest)
    {
        var httpClient = _httpClientFactory.CreateClient("ShakespeareTranslatorApi");
        
        var textToTranslateJson = new StringContent(
            JsonSerializer.Serialize(shakeSpearApiRequest),
            Encoding.UTF8,
            "application/json");
        
        var httpResponseMessage = await httpClient.PostAsync("translate/shakespeare.json", textToTranslateJson);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            var errorMessage = 
                httpResponseMessage.StatusCode == HttpStatusCode.NotFound ? "Unable to " : "Failed to make a request, please try again";
            
            throw new Exception(errorMessage);
        }

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            
        return await JsonSerializer.DeserializeAsync<ShakespeareApiResponse>(contentStream);
    }
}