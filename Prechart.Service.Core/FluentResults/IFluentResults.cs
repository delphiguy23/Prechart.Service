using System.Collections.Generic;

namespace Prechart.Service.Core.FluentResults;

public interface IFluentResults<T> : IFluentResults
{
    T Value { get; }
}

public interface IFluentResults
{
    FluentResultsStatus Status { get; }
    List<string> Messages { get; }
    Dictionary<string, object> Keys { get; }
    string ToMultiLine(string delimiter = null);
    string ToString();
}