using RickMorty.ExternalData;
using RickMorty.ExternalData.Constants;
using MyWebApiNamespace;

namespace ConsoleApp1;

internal class Program
{
    
    static async Task Main(string[] args)
    {
        IWebApiReader webApiReader = new WebApiReader();
        RickMortyData rickMortyData = new RickMortyData(webApiReader);
        Task thistask= rickMortyData.CreateFullRickMortyCharacterDataAsync();

        Console.WriteLine();
        do
        {
            Console.WriteLine("just type something");
            string input = Console.ReadLine();
            Console.WriteLine($"You typed {input}");
        } while (thistask.Status == TaskStatus.Running);
        Console.ReadLine();
    }
}
