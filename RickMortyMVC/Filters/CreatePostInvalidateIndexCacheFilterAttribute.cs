using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OutputCaching;

namespace RickMortyMVC.Filters;

public class CreatePostInvalidateIndexCacheFilterAttribute : ActionFilterAttribute
{
    private readonly IOutputCacheStore _outputCacheStore;

    public CreatePostInvalidateIndexCacheFilterAttribute(IOutputCacheStore outputCacheStore)
    {
        this._outputCacheStore = outputCacheStore;
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _outputCacheStore.EvictByTagAsync("TagHandleForExpire300Policy", new CancellationToken());
    }
}
