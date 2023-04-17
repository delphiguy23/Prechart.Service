using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Builder;

namespace Prechart.Service.Core.Outcomes;

public static class ResultsTo
{
    //Success
    public static FluentResultsStatusBuilder<object> Success()
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.Success);
    }

    public static FluentResultsStatusBuilder<TValue> Success<TValue>()
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Success);
    }

    public static FluentResultsStatusBuilder<object> Success(object value)
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.Success).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> Success<TValue>(TValue value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Success).WithValue(value);
    }

    //Not Found
    public static FluentResultsStatusBuilder<object> NotFound()
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.NotFound);
    }

    public static FluentResultsStatusBuilder<TValue> NotFound<TValue>()
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound);
    }

    public static FluentResultsStatusBuilder<object> NotFound(object value)
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.NotFound).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> NotFound<TValue>(TValue value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> NotFound<TValue>(string value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound).WithMessage(value);
    }

    //Failure
    public static FluentResultsStatusBuilder<object> Failure()
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.Failure);
    }

    public static FluentResultsStatusBuilder<TValue> Failure<TValue>()
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Failure);
    }

    public static FluentResultsStatusBuilder<object> Failure(object value)
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.Failure).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> Failure<TValue>(TValue value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Failure).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> Failure<TValue>(string value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Failure).WithMessage(value);
    }

    //BadRequest
    public static FluentResultsStatusBuilder<object> BadRequest()
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.BadRequest);
    }

    public static FluentResultsStatusBuilder<TValue> BadRequest<TValue>()
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.BadRequest);
    }

    public static FluentResultsStatusBuilder<object> BadRequest(object value)
    {
        return new FluentResultsStatusBuilder<object>(FluentResultsStatus.BadRequest).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> BadRequest<TValue>(TValue value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.BadRequest).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> BadRequest<TValue>(string value)
    {
        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.BadRequest).WithMessage(value);
    }

    ////

    public static FluentResultsStatusBuilder<TValue> Something<TValue>(TValue value)
    {
        if (value == null)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound);
        }

        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Success).WithValue(value);
    }

    public static FluentResultsStatusBuilder<TValue> Something<TValue>(IFluentResults<TValue> value)
    {
        if (value == null)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound);
        }

        if (value.Status == FluentResultsStatus.Success)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Success).FromResults(value);
        }

        if (value.Status == FluentResultsStatus.NotFound)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.NotFound).FromResults(value);
        }

        if (value.Status == FluentResultsStatus.BadRequest)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.BadRequest).FromResults(value);
        }

        if (value.Status == FluentResultsStatus.Failure)
        {
            return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Failure).FromResults(value);
        }

        return new FluentResultsStatusBuilder<TValue>(FluentResultsStatus.Success).FromResults(value);
    }
}
