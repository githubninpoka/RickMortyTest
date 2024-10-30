using Microsoft.AspNetCore.Mvc.Filters;

namespace RickMortyMVC.Filters;

public class IndexPreActionFilterAttribute : ActionFilterAttribute
{
    private readonly string _name;
    private readonly string _value;

    public IndexPreActionFilterAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }
}
