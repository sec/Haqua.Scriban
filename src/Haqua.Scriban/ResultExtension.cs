using Microsoft.AspNetCore.Http;

namespace Haqua.Scriban;

public static class ResultExtension
{
    public static IResult ScribanView(this IResultExtensions extensions, string views)
    {
        return new ScribanView(views);
    }

    public static IResult ScribanView(this IResultExtensions extensions, string views, object model)
    {
        return new ScribanView(views, model);
    }

    public static IResult ScribanView(this IResultExtensions extensions, string views, object model, int statusCode)
    {
        return new ScribanView(views, model, statusCode);
    }

    public static IResult ScribanView(this IResultExtensions extensions, string views, int statusCode)
    {
        return new ScribanView(views, statusCode);
    }
}