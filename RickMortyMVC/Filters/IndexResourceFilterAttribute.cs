using Microsoft.AspNetCore.Mvc.Filters;

namespace RickMortyMVC.Filters;

public class IndexResourceFilterAttribute : IResourceFilter
{
    private readonly string _name;
    private readonly string _value;

    public IndexResourceFilterAttribute(string name = "from-database", string value = "nope")
    {
        _name = name;
        _value = value;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
    }
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
