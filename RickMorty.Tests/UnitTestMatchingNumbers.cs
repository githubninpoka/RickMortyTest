namespace RickMorty.Tests;
using RickMorty.Data;
using RickMorty.ExternalData;
using MyWebApiNamespace;
using RickMorty.ExternalData.DTOs;
using RickMorty.Domain.Models;
using RickMorty.ConsoleAppRetrieveAndStoreData;

public class Tests
{
    // for a more difficult test I would probably load some manual data with a NULL externalId into the database
    // then remove all entries except that manual entry
    // then verify the numbers (external api vs entries in db with externalIds) are the same
    // why?
    // the developer exercise now asks to 'empty' the database through the console app
    // but in a more realistic scenario we would possibly leave the manual entries alone when deleting data, depending on business logic.

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void WhenDownloadingDataAndFillingTheDatabaseFetchingFromDatabaseShallGiveSameNumberOfEntities()
    {
        // Arrange
        using RickMortyDbContext db = new RickMortyDbContext();
        var characters = db.Characters;
        IWebApiReader webApiReader = new WebApiReader();
        RickMortyData rickmortyData = new RickMortyData(webApiReader);
        IEnumerable<CharacterDTO> aliveCharacterDtos = rickmortyData.CreateFullRickMortyCharacterDataAsync().Result;
        List<Character> characterList = RickMorty.ConsoleAppRetrieveAndStoreData.Program.CreateCharacters(aliveCharacterDtos);

        if (characterList.Count != 0)
        {
            db.Characters.RemoveRange(characters);
            db.SaveChanges();
            db.AddRange(characterList);
            db.SaveChanges();
        }

        // Act
        var amountInDatabaseAfterLoadingFromExternalApi = db.Characters.Where(x => x.ExternalId != null).Count();
        Console.WriteLine(amountInDatabaseAfterLoadingFromExternalApi);
        int amountExpectedToBeStoredInDatabase = characterList.Count;

        // Assert
        Assert.True(amountInDatabaseAfterLoadingFromExternalApi == amountExpectedToBeStoredInDatabase);
        
    }
}