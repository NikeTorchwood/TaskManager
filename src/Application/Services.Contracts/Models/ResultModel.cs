using System.Text.Json.Serialization;

namespace Services.Contracts.Models;

/// <summary>
/// Represents a result model for operations, encapsulating success, value, and error details.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
public class ResultModel<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets the value returned by the operation, if successful.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets the error message if the operation failed.
    /// </summary>
    public string Error { get; set; }


    /// <summary>
    /// Constructor for JsonConverter
    /// </summary>
    [JsonConstructor]
    private ResultModel()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultModel{T}"/> class.
    /// </summary>
    /// <param name="success">Indicates whether the operation was successful.</param>
    /// <param name="error">The error message (if any).</param>
    /// <param name="value">The value returned by the operation (if any).</param>
    public ResultModel(bool success, string? error = null, T? value = default)
    {
        Success = success;
        Error = error;
        Value = value;
    }

    /// <summary>
    /// Creates a successful result model with a specified value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A new instance of <see cref="ResultModel{T}"/> representing a successful result.</returns>
    public static ResultModel<T> SuccessResult(T? value)
        => new(true, value: value);

    /// <summary>
    /// Creates a failure result model with a specified error message.
    /// </summary>
    /// <param name="error">The error message of the failed result.</param>
    /// <returns>A new instance of <see cref="ResultModel{T}"/> representing a failed result.</returns>
    public static ResultModel<T> FailureResult(string error)
        => new(false, error: error);
}