namespace FinTrack.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; init; } = string.Empty;
    public string ErrorCode { get; init; } = string.Empty;

    public Result() { }

    protected Result(bool isSuccess, string error, string errorCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result Success()
        => new(true, string.Empty, string.Empty);

    public static Result Failure(string error, string errorCode)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Erro não pode ser vazio.", nameof(error));

        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Código de erro não pode ser vazio.", nameof(errorCode));   

        return new(false, error, errorCode);
    }
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public Result() { }

    protected Result(T? value, bool isSuccess, string error, string errorCode)
        : base(isSuccess, error, errorCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new(value, true, string.Empty, string.Empty);
    }

    public static new Result<T> Failure(string error, string errorCode)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("Erro não pode ser vazio.", nameof(error));

        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Código de erro não pode ser vazio.", nameof(errorCode));

        return new(default, false, error, errorCode);
    }
}
