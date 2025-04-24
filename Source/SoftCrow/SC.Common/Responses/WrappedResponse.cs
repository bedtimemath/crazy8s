using System.Text.Json.Serialization;
using SC.Common.Extensions;
using SC.Common.Models;

namespace SC.Common.Responses;

[Serializable]
public class WrappedResponse<TModel>
    where TModel: class?
{
    public static WrappedResponse<TModel> CreateSuccessResponse(TModel? response) =>
        new() { Result = response, Exception = null };
    public static WrappedResponse<TModel> CreateFailureResponse(Exception exception) =>
        new() { Result = null, Exception = exception.ToSerializableException() };
    public static WrappedResponse<TModel> CreateFailureResponse(SerializableException exception) =>
        new() { Result = null, Exception = exception };

    public TModel? Result { get; set; }
    public SerializableException? Exception { get; set; }

    [JsonIgnore]
    public bool Success => Exception == null;
}

[Serializable]
public class WrappedResponse
{
    public static WrappedResponse CreateSuccessResponse() =>
        new() { Exception = null };
    public static WrappedResponse CreateFailureResponse(Exception exception) =>
        new() { Exception = exception.ToSerializableException() };

    public SerializableException? Exception { get; set; }

    [JsonIgnore]
    public bool Success => Exception == null;
}