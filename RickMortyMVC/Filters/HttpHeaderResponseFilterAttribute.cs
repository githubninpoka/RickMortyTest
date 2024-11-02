using Microsoft.AspNetCore.Mvc.Filters;

namespace RickMortyMVC.Filters;

public class HttpHeaderResponseFilterAttribute : ResultFilterAttribute
{

    private readonly string _name;
    private readonly string _value;

    public HttpHeaderResponseFilterAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var etag = $"\"{Guid.NewGuid():n}\"";
        context.HttpContext.Response.Headers.ETag = etag; // this helps to send the client a HTTP 304 when being served from the OutputCache

        Console.WriteLine("We are in the Response Filter");
        Console.WriteLine("This response filter is triggered when the action is starting," +
            "but it has not yet finished executing");
        Console.WriteLine($"I am now setting a header {_name} to {_value}");

        context.HttpContext.Response.Headers[_name] = _value;
        base.OnResultExecuting(context);
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        base.OnResultExecuted(context);
    }

}
