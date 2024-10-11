using System.Net;
using System.Text.Json;

namespace C8S.FullSlate.Abstractions.Interactions;

public class ServiceResponse<TData>
    where TData : class, new()
{
    #region Static Creation Methods
    // responseBody can be empty when no records are found, so we have to adjust for that
    public static ServiceResponse<TData> CreateSuccess(HttpStatusCode statusCode, string? responseBody) =>
        CreateSuccess(statusCode, string.IsNullOrEmpty(responseBody) ? new TData() :
            JsonSerializer.Deserialize<TData>(responseBody) ??
            throw new Exception($"Could not deserialize {typeof(TData)}: {responseBody}"));
    public static ServiceResponse<TData> CreateSuccess(HttpStatusCode statusCode, TData data) =>
        new() { Success = true, StatusCode = statusCode, Data = data };

    public static ServiceResponse<TData> CreateFailure(HttpStatusCode statusCode, string responseBody) =>
        CreateFailure(statusCode, JsonSerializer.Deserialize<List<ServiceError>?>(responseBody) ??
                                  throw new Exception($"Could not deserialize ServiceError: {responseBody}"));
    public static ServiceResponse<TData> CreateFailure(HttpStatusCode statusCode, 
        List<ServiceError>? errors) => new() { Success = false, StatusCode = statusCode, Errors = errors };
    #endregion

    #region Public Properties
    public HttpStatusCode StatusCode { get; set; }

    public bool Success { get; set; }

    public List<ServiceError>? Errors { get; set; } = null;

    public TData? Data { get; set; } = null;
    #endregion
}