namespace RickMorty.Tests;
using RickMorty.Data;
using RickMorty.ExternalData;
using MyWebApiNamespace;
using RickMorty.ExternalData.DTOs;
using RickMorty.Domain.Models;
using RickMorty.ConsoleAppRetrieveAndStoreData;

public class Tests
{

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