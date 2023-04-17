using Prechart.Service.Core.FluentResults.Formats;
using System;
using System.Collections.Generic;

namespace Prechart.Service.Core.FluentResults;

[Serializable]
public class ValueResults<TValue> : IFluentResults<TValue>
{
    internal ValueResults(FluentResultsStatus status)
    {
        Status = status;
        Messages = new List<string>();
        Value = default;
        Keys = new Dictionary<string, object>();
    }

    internal ValueResults(IFluentResults<TValue> results)
    {
        Status = results.Status;
        Messages = results.Messages;
        Value = results.Value;
        Keys = results.Keys;
    }

    internal ValueResults(IFluentResults results)
    {
        Status = results.Status;
        Messages = results.Messages;
        Value = default;
        Keys = results.Keys;
    }

    public bool Success => Status == FluentResultsStatus.Success;
    public bool NotFound => Status == FluentResultsStatus.NotFound;
    public bool BadRequest => Status == FluentResultsStatus.BadRequest;
    public bool Failure => Status == FluentResultsStatus.Failure;

    public List<string> Messages { get; protected set; }
    public FluentResultsStatus Status { get; set; }
    public TValue Value { get; set; }

    public Dictionary<string, object> Keys { get; }

    public override string ToString()
    {
        return ToMultiLine(string.Empty);
    }

    public string ToMultiLine(string delimiter = null)
    {
        return MultiLineFormatter.ToMultiLine(delimiter, Messages);
    }
}
