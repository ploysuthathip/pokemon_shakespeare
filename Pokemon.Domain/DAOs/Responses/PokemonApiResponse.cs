using System.Text.Json.Serialization;

namespace Pokemon.Domain.DAOs.Responses;

public class PokemonApiResponse
{
    [JsonPropertyName("flavor_text_entries")]
    public IEnumerable<FlavorTextEntries> FlavorTextEntries { get; set; }
}
public class FlavorTextEntries
{
    [JsonPropertyName("flavor_text")]
    public string FlavorText { get; set; }
    
    [JsonPropertyName("language")]
    public Language Language { get; set; }
}
public class Language
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}