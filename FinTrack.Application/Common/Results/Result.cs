namespace FinTrack.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; init; } = string.Empty;

    public Result() { }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new() { IsSuccess = true };

    public static Result Failure(string error)
        => new() { IsSuccess = false, Error = error };
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public Result() { }

    protected Result(T? value, bool isSuccess, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new() { IsSuccess = true, Value = value };

    public static new Result<T> Failure(string error)
        => new() { IsSuccess = false, Error = error };
}
