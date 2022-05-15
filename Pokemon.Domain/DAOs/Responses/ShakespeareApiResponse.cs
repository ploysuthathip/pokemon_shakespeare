using System.Text.Json.Serialization;

namespace Pokemon.Domain.DAOs.Responses;

public class ShakespeareApiResponse
{
    [JsonPropertyName("contents")]
    public Content Content { get; set;}
}

public class Content
{
    [JsonPropertyName("translated")]
    public string TranslatedText { get; set; }
}