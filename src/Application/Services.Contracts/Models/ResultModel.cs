namespace Services.Contracts.Models;

public class ResultModel<T>
{
    public bool Success { get; }
    public T Value { get; }
    public string Error { get; }

    private ResultModel(T value)
    {
        Success = true;
        Value = value;
    }

    private ResultModel(string error)
    {
        Success = false;
        Error = error;
    }

    public static ResultModel<T> SuccessResult(T value) => new ResultModel<T>(value);
    public static ResultModel<T> FailureResult(string error) => new ResultModel<T>(error);
}