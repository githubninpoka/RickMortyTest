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
    public string Location { get; set; }
    public int ExternalId { get; set; }
    public DateTime externalCreated { get; set; }

}
