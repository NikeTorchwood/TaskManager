using System.Text.Json.Serialization;

namespace WebApi.Responses;

/// <summary>
/// Wrapper for the standard API response, including execution status, data, and error message.
/// Used for unified result return from controller methods.
/// </summary>
/// <typeparam name="T">The type of data that will be returned in case of a successful operation.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates the success of the operation.
    /// </summary>
    /// <value>True if the operation was successful; False if there was an error.</value>
    public bool Success { get; set; }

    /// <summary>
    /// The data returned in case of a successful operation.
    /// </summary>
    /// <value>An object of type <typeparamref name="T"/> containing the operation result.</value>
    public T? Data { get; set; }

    /// <summary>
    /// The error message if the operation was not successful.
    /// </summary>
    /// <value>An error message in the form of a string.</value>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Constructor for a JsonConverter
    /// </summary>
    [JsonConstructor]
    private ApiResponse()
    {

    }

    /// <summary>
    /// Constructor for a successful response with data.
    /// </summary>
    /// <param name="data">The data to be returned in case of a successful operation.</param>
    public ApiResponse(T? data)
    {
        Success = true;
        Data = data;
        ErrorMessage = null;
    }

    /// <summary>
    /// Constructor for a response with an error.
    /// </summary>
    /// <param name="errorMessage">The error message to be returned in case of failure.</param>
    public ApiResponse(string? errorMessage)
    {
        Success = false;
        Data = default;
        ErrorMessage = errorMessage;
    }
}