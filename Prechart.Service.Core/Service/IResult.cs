namespace Prechart.Service.Core.Service;

public interface IResult
{
}

#pragma warning disable S2326
public interface IResult<T> : IResult
{
}
#pragma warning restore S2326
#pragma warning disable CA1715
public interface IsFailure
{
    string Message { get; }
}

public interface IsNone
{
}

public interface IsSuccess
{
}

public interface IsSome<out T>
{
    T Value { get; }
}
#pragma warning restore CA1715

public class Success : IResult, IsSuccess
{
}

public class Success<T> : IResult<T>, IsSuccess
{
}

public class Some<T> : Success<T>, IsSome<T>
{
    public Some(T value)
    {
        Value = value;
    }

    public T Value { get; }
}

public class None<T> : IResult<T>, IsNone
{
}

public class None : IResult, IsNone
{
}

public class Failure<T> : IResult<T>, IsFailure
{
    public Failure(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class Failure : IResult, IsFailure
{
    public Failure(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
