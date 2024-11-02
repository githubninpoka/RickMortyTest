using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;

namespace RickMortyMVC.Filters;

public class InvalidateCacheFilterAttribute : ActionFilterAttribute
{
    private readonly IOutputCacheStore _outputCacheStore;

    public InvalidateCacheFilterAttribute(IOutputCacheStore outputCacheStore)
    {
        this._outputCacheStore = outputCacheStore;
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _outputCacheStore.EvictByTagAsync("TagHandleForExpire300Policy", new CancellationToken());
        _outputCacheStore.EvictByTagAsync("TagHandleForExpire300ByQueryPolicy", new CancellationToken());
    }
}
