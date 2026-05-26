namespace HRMS.Application.Common.Models;

/// <summary>
/// A uniform API response wrapper for standardizing all API outputs.
/// </summary>
/// <typeparam name="T">Type of the data payload returned.</typeparam>
public class ApiResponse<T>
{
    // Indicates if the API request was successful or not
    public bool Success { get; set; }
    
    // A user-friendly message describing the result
    public string Message { get; set; } = string.Empty;
    
    // The actual payload data returned by the API
    public T? Data { get; set; }
    
    // An optional list of error details (useful for validation errors)
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Factory method for creating a generic success response.
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Factory method for creating a generic failure response.
    /// </summary>
    public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}