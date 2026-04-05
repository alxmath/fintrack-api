namespace FinTrack.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess;
    public string ErrorCode { get; init; } = string.Empty;

    public Dictionary<string, string[]> Errors { get; init; } = [];

    public Result() { }

    protected Result(bool isSuccess, string errorCode, Dictionary<string, string[]>? errors = null)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        Errors = errors ?? [];
    }

    public static Result Success()
        => new(true, string.Empty);

    public static Result Failure(Dictionary<string, string[]> errors, string errorCode)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("Errors não pode ser vazio.", nameof(errors));

        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Código de erro não pode ser vazio.", nameof(errorCode));   

        return new(false, errorCode, errors);
    }
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public Result() { }

    protected Result(
        T? value, 
        bool isSuccess, 
        string errorCode,
        Dictionary<string, string[]>? errors = null)
        : base(isSuccess, errorCode, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return new(value, true, string.Empty);
    }

    public static new Result<T> Failure(Dictionary<string, string[]> errors, string errorCode)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("Erro não pode ser vazio.", nameof(errors));

        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Código de erro não pode ser vazio.", nameof(errorCode));

        return new(default, false, errorCode, errors);
    }
}
