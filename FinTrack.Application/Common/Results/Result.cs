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

    public static Result Success()
        => new(true, string.Empty);

    public static Result Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Erro não pode ser vazio.", nameof(error));

        return new(false, error);
    }
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
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new(value, true, string.Empty);
    }

    public static new Result<T> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Erro não pode ser vazio.", nameof(error));

        return new(default, false, error);
    }
}
