using System.Text.Json.Serialization;

namespace RickMorty.ExternalData.DTOs;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

public class RickMortyApiPageRootDTO
{
    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("results")]
    public List<CharacterDTO> Results { get; set; }

    public RickMortyApiPageRootDTO()
    {
        Results = new List<CharacterDTO>();
        Info = new Info();
    }
}

