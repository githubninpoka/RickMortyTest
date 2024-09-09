using System.Text.Json.Serialization;

namespace RickMorty.ExternalData.DTOs;

public class Location
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}