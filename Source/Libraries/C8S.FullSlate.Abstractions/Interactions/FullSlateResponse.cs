using System.Net;
using System.Text.Json;

namespace C8S.FullSlate.Abstractions.Interactions;

public class FullSlateResponse<TData>
    where TData : class, new()
{
    #region Static Creation Methods
    // responseBody can be empty when no records are found, so we have to adjust for that
    public static FullSlateResponse<TData> CreateSuccess(HttpStatusCode statusCode, string? responseBody) =>
        CreateSuccess(statusCode, string.IsNullOrEmpty(responseBody) ? new TData() :
            JsonSerializer.Deserialize<TData>(responseBody) ??
            throw new Exception($"Could not deserialize FullSlateErrorResponse: {responseBody}"));
    public static FullSlateResponse<TData> CreateSuccess(HttpStatusCode statusCode, TData data) =>
        new()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data
        };

    public static FullSlateResponse<TData> CreateFailure(HttpStatusCode statusCode, string responseBody) =>
        CreateFailure(statusCode, JsonSerializer.Deserialize<FullSlateErrorResponse>(responseBody) ??
                                  throw new Exception($"Could not deserialize BiginResponseResult: {responseBody}"));
    public static FullSlateResponse<TData> CreateFailure(HttpStatusCode statusCode, 
        FullSlateErrorResponse errorResponse) => new() { Success = false, StatusCode = statusCode, Error = errorResponse };
    #endregion

    #region Public Properties
    public HttpStatusCode StatusCode { get; set; }

    public bool Success { get; set; }

    public FullSlateErrorResponse? Error { get; set; } = null;

    public TData? Data { get; set; } = null;
    #endregion
}