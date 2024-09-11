using RickMorty.ExternalData.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebApiNamespace;
using System.Text.Json;
using RickMorty.ExternalData.DTOs;
using System.Xml;

namespace RickMorty.ExternalData;

public class RickMortyData
{
    private IWebApiReader webApiReader;

    List<CharacterDTO> externalRickMortyData = new(); // have to initialize the List<Result>, because otherwise I can't iterate and call
                                                      // .Add on a non-existent List<Result> I knew this.
    public RickMortyData(IWebApiReader webApiReader)
    {
        this.webApiReader = webApiReader;
    }
    public async Task CreateFullRickMortyCharacterDataAsync()
    {
        int totalNumberOfPages = 10;
        int currentlyRequestedPage = 1;

        List<Task<RickMortyApiPageRootDTO>> tasks = new();

        //var containsTotalNumberOfPages = await ReturnOnePageOfExternalRickMortyCharactersInARoot_Async(1);
        //totalNumberOfPages = containsTotalNumberOfPages.Info.Pages;

        while (currentlyRequestedPage <= totalNumberOfPages)
        {
            // will help prevent throttling while multithreading
            await SlowDownWhenTooManySimultenousTasks(tasks);

            Console.WriteLine($"Calling the method with argument {currentlyRequestedPage}");
            //tasks.Add(ReturnOnePageOfExternalRickMortyCharactersInARoot_Async(currentlyRequestedPage)); // this runs synchronously....
            // https://stackoverflow.com/questions/30225476/task-run-with-parameters
            // answer 10, remark 6! => declare a local variable so that each task has its own nonchanging variable.
            // otherwise when the tasks start, they can randomly update the variable currentlyRequestedPage, leaving us with
            // multiple calls with gaps and overlapping pages.
            var thisPage = currentlyRequestedPage;
            tasks.Add(Task.Run(() => ReturnOnePageOfExternalRickMortyCharactersInARoot_Async(thisPage))); // and this fucks with the argument
            Console.WriteLine($"Called the method with argument {currentlyRequestedPage}");
            currentlyRequestedPage++;
        }
        Console.WriteLine();

        //Task.WaitAll(tasks.ToArray()); // this is probably blocking main thread
        await Task.WhenAll(tasks);
        Console.WriteLine("Continue onto displaying?");
        //Console.ReadLine();

        for (int i = 0; i < totalNumberOfPages; i++)
        {
            externalRickMortyData.AddRange(tasks[i].Result.Results);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Will now start with index {i}");
            Console.WriteLine($"Next page is: {tasks[i].Result.Info.Next}");
            int numberOfCharactersInThisPage = tasks[i].Result.Results.Count();
            for (int j = 0; j < numberOfCharactersInThisPage; j++)
            {
                CharacterDTO currentCharacter = tasks[i].Result.Results[j];
                //externalRickMortyData.Results.Add(currentCharacter);
                Console.WriteLine($"The current character is: {currentCharacter.ExternalCharacterId} {currentCharacter.Name} with status {currentCharacter.Status}");
            }
        }
        Console.WriteLine("================================================================");
        var alives = externalRickMortyData.Where(character => character.Status.ToLower() == "alive");
        foreach (var alive in alives)
        {
            Console.WriteLine($"The current character is: {alive.ExternalCharacterId} {alive.Name} with status {alive.Status}");
        }

        Console.ReadLine();
    }


    public async Task<RickMortyApiPageRootDTO> ReturnOnePageOfExternalRickMortyCharactersInARoot_Async(int pageNumber)
    {
        string requestParameter = $"?page={pageNumber}";
        Console.WriteLine($"Calling the api with {requestParameter} from thread with id: {Thread.CurrentThread.ManagedThreadId}");
        string data = await webApiReader.ReadStringAsync(
            RickMortyConstants.BASEURI,
            RickMortyConstants.RELATIVEURL,
            requestParameter
            );

        // could I deserialize a string asynchronously? I don't have a 'stream'
        return JsonSerializer.Deserialize<RickMortyApiPageRootDTO>(data);
    }

    private async Task SlowDownWhenTooManySimultenousTasks(List<Task<RickMortyApiPageRootDTO>> tasks)
    {
        // there are probably better ways to limit the amount of simultaneous threads,
        // but hey, this works without relying on just the threadpool
        int maxConcurrentRunningRequests = 3;

        Console.WriteLine($"Tasks running: {tasks.Count(task => task.Status == TaskStatus.Running)}");
        Console.WriteLine($"Tasks waiting for activation: {tasks.Count(task => task.Status == TaskStatus.WaitingForActivation)}");
        Console.WriteLine($"Tasks waiting to run: {tasks.Count(task => task.Status == TaskStatus.WaitingToRun)}");
        Console.WriteLine($"Tasks created: {tasks.Count(task => task.Status == TaskStatus.Created)}");
        while (tasks.Count(
            task => task.Status == TaskStatus.Running
        || task.Status == TaskStatus.WaitingForActivation
        || task.Status == TaskStatus.Created
        ) > maxConcurrentRunningRequests)
        {
            Console.WriteLine("Throttling...");
            await Task.Delay(500);
        }
    }

}
