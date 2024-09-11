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
        DbContext db = new RickMortyDbContext();

        IWebApiReader webApiReader = new WebApiReader();
        RickMortyData rickMortyData = new RickMortyData(webApiReader);
        Task<IEnumerable<CharacterDTO>> aliveCharacterDTOsTask = rickMortyData.CreateFullRickMortyCharacterDataAsync();

        //do
        //{
        //    Console.WriteLine("Doing some main work while waiting for DTOs to load");
        //    await Task.Delay(500);
        //} while (aliveCharacterDTOsTask.Status == TaskStatus.Running);
        IEnumerable<CharacterDTO> aliveCharacterDTOs = aliveCharacterDTOsTask.Result;

        List<Character> characters= CreateCharacters(aliveCharacterDTOs);

        db.Database.ExecuteSqlRaw("TRUNCATE TABLE[Characters]");
        db.AddRangeAsync(characters);
        db.SaveChanges();

        Console.ReadLine();
    }

    private static List<Character> CreateCharacters(IEnumerable<CharacterDTO> aliveCharacterDTOs)
    {
        // the structure to get this working with different projects is:
        // Program references both data project and external data project
        // then data project references external data project
        // this is to facilitate the conversion from DTO to internal model data through a cast
        List<Character> aliveCharacters = new List<Character>();
        foreach (CharacterDTO characterDTO in aliveCharacterDTOs)
        {
            Character character = (Character)characterDTO;
            aliveCharacters.Add(character);
        }
        return aliveCharacters;
    }
}