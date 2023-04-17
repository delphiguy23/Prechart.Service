using System;
using System.Collections.Generic;

namespace Prechart.Service.Core.FluentResults.Builder;

public class FluentResultsStatusBuilder<T> : ValueResults<T>
{
    public FluentResultsStatusBuilder(FluentResultsStatus status) : base(status)
    {
    }

    public FluentResultsStatusBuilder<T> PrependMessage(string message)
    {
        Messages.Insert(0, message);
        return this;
    }

    public FluentResultsStatusBuilder<T> WithMessageFormat(string message, params object[] paramList)
    {
        message = string.Format(message, paramList);

        Messages.Add(message);
        return this;
    }

    public FluentResultsStatusBuilder<T> WithMessage(string message)
    {
        Messages.Add(message);
        return this;
    }

    public FluentResultsStatusBuilder<T> WithMessage(IEnumerable<string> messages)
    {
        if (messages is not null)
        {
            return this;
        }

        Messages.AddRange(messages);
        return this;
    }

    public FluentResultsStatusBuilder<T> WithMessagesFrom(IFluentResults outcome)
    {
        WithMessage(outcome.Messages);
        return this;
    }

    public FluentResultsStatusBuilder<T> WithValue(T value)
    {
        Value = value;
        return this;
    }

    public FluentResultsStatusBuilder<T> WithKeysFrom(IFluentResults outcome)
    {
        foreach (var key in outcome.Keys.Keys)
        {
            Keys[key] = outcome.Keys[key];
        }

        return this;
    }

    public FluentResultsStatusBuilder<T> WithKey(string key, object value)
    {
        Keys[key] = value;
        return this;
    }

    public FluentResultsStatusBuilder<T> FromResults(IFluentResults outcome)
    {
        WithMessage(outcome.Messages);
        WithKeysFrom(outcome);

        if (outcome.GetType().IsGenericType)
        {
            var value = outcome.GetType()
                .GetProperty("Value")
                ?.GetValue(outcome, null);

            if (value is T)
            {
                WithValue((T) value);
            }
        }

        return this;
    }

    public FluentResultsStatusBuilder<T> FromException(Exception exception)
    {
        Messages.Add(exception.ToString());
        return this;
    }
}