using Microsoft.AspNetCore.Mvc;

namespace Prechart.Service.Core.Service;

public static class ResultExtensions
{
    public static IResult<T> Some<T>(this T value)
    {
        if (value == null)
        {
            return new None<T>();
        }

        return new Some<T>(value);
    }

    public static IResult<T> Failure<T>(this string value)
    {
        return new Failure<T>(value);
    }

    public static IResult Failure(this string value)
    {
        return new Failure(value);
    }

    public static IResult Success(this object value)
    {
        return new Success();
    }

    public static ActionResult ToActionResult<T>(this IResult<T> result)
    {
        return result switch
        {
            IsSome<T> someResult => new OkObjectResult(someResult.Value),
            IsNone _ => new NotFoundResult(),
            _ => new StatusCodeResult(500),
        };
    }

    public static ActionResult ToActionResult(this IResult result)
    {
        return result switch
        {
            IsSuccess _ => new OkResult(),
            IsNone _ => new NotFoundResult(),
            _ => new StatusCodeResult(500),
        };
    }
}
