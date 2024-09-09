using System.Text.Json.Serialization;

namespace RickMorty.ExternalData.DTOs;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

public class Root
{
    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("results")]
    public List<Result> Results { get; set; }

    public Root()
    {
        Results = new List<Result>();
        Info = new Info();
    }
}

