namespace Prechart.Service.Core.FluentResults.Extension;

public static class FluentResultsExtension
{
    public static bool IsSuccess(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Success;
    }

    public static bool IsNotFound(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.NotFound;
    }

    public static bool IsFailure(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Failure;
    }
        
    public static bool IsBadRequest(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.BadRequest;
    }

    public static bool IsNotFoundOrBadRequest(this IFluentResults result)
    {
        return result is { } fluentResults && (fluentResults.Status == FluentResultsStatus.BadRequest || fluentResults.Status == FluentResultsStatus.NotFound);
    }
}