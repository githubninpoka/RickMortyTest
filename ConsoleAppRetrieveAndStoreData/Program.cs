using RickMorty.ExternalData;
using MyWebApiNamespace;
using RickMorty.Domain.Models;
using RickMorty.ExternalData.DTOs;
using Microsoft.EntityFrameworkCore;
using RickMorty.Data;


namespace RickMorty.ConsoleAppRetrieveAndStoreData;

internal class Program
{

    static async Task Main(string[] args)
    {

        IWebApiReader webApiReader = new WebApiReader();
        RickMortyData rickMortyData = new RickMortyData(webApiReader);
        Task<IEnumerable<CharacterDTO>> aliveCharacterDTOsTask = rickMortyData.CreateFullRickMortyCharacterDataAsync();

        using RickMortyDbContext db = new RickMortyDbContext();
        
        var chars = db.Characters;

        IEnumerable<CharacterDTO> aliveCharacterDTOs = aliveCharacterDTOsTask.Result; // this is a blocking operation, which is ok here.

        List<Character> characterList = CreateCharacters(aliveCharacterDTOs);

        if (characterList.Count != 0)
        {
            //db.Database.ExecuteSqlRaw("TRUNCATE TABLE[Characters]");
            db.Characters.RemoveRange(chars);
            db.SaveChanges();
            await db.AddRangeAsync(characterList); // i know it's not useful to do async here.
            db.SaveChanges();
        }

        Console.WriteLine("I have completed my work in Main");
        Console.ReadLine();
    }

    private static List<Character> CreateCharacters(IEnumerable<CharacterDTO> aliveCharacterDTOs)
    {
        // the structure to get this working with different projects is:
        // Program references both data project and external data project
        // then data project references external data project
        // this is to facilitate the conversion from DTO to internal model data through a cast
        // I think I'm not doing it correctly here, but at least it works.
        List<Character> aliveCharacters = new List<Character>();
        foreach (CharacterDTO characterDTO in aliveCharacterDTOs)
        {
            Character character = (Character)characterDTO;
            aliveCharacters.Add(character);
        }
        return aliveCharacters;
    }

}
