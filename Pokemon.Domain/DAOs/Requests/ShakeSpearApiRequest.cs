using System.Text.Json.Serialization;

namespace Pokemon.Domain.DAOs.Requests;

public class ShakeSpearApiRequest
{ 
    [JsonPropertyName("text")]
    public string Text { get; set; }
}