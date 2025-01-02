namespace Services.Contracts.Models;

public class ResultModel<T>
{
    public bool Success { get; }
    public T Value { get; }
    public string Error { get; }

    private ResultModel(bool success, string? error = null, T? value = default)
    {
        Success = success;
        Error = error;
        Value = value;
    }

    public static ResultModel<T> SuccessResult(T? value)
        => new(true, value: value);

    public static ResultModel<T> FailureResult(string error)
        => new(false, error: error);
}