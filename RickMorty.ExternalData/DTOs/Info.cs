using System.Text.Json.Serialization;

namespace RickMorty.ExternalData.DTOs;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class Info
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("next")]
    public string Next { get; set; }

    [JsonPropertyName("prev")]
    public string Prev { get; set; }
}
