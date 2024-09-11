using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RickMorty.Data.Models;

public class Character
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Species { get; set; }
    public string Origin { get; set; }
    public string Location { get; set; }

    public int ExternalId { get; set; }
    public DateTime externalCreated { get; set; }

    public static explicit operator Character(RickMorty.ExternalData.DTOs.CharacterDTO characterDTO)
    {
        var name = characterDTO.Name;
        var status = characterDTO.Status;
        var species = characterDTO.Species;
        var origin = characterDTO.Origin.Name;
        var location = characterDTO.Location.Name;
        
        var externalId = characterDTO.Id;
        var externalCreated = characterDTO.Created;
        return new Character()
        {
            Name = name,
            Status = status,
            Species = species,
            Location = location,
            Origin = origin,
            Created = DateTime.Now,

            ExternalId = externalId,
            externalCreated = externalCreated
        };

    }
}
