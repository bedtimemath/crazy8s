using System.Text.Json.Serialization;
using SC.Common.Extensions;
using SC.Common.Models;

namespace SC.Common.Responses;

[Serializable]
public class WrappedListResponse<TModel>
    where TModel: class?
{
    public static WrappedListResponse<TModel> CreateSuccessResponse(IList<TModel> response, int total) =>
        new() { Result = response, Total = total, Exception = null };
    public static WrappedListResponse<TModel> CreateSuccessResponse(TModel response) =>
        new() { Result = [response], Total = 1, Exception = null };
    public static WrappedListResponse<TModel> CreateFailureResponse(Exception exception) =>
        new() { Result = null, Total = 0, Exception = exception.ToSerializableException() };
    public static WrappedListResponse<TModel> CreateFailureResponse(SerializableException exception) =>
        new() { Result = null, Total = 0, Exception = exception };

    public IList<TModel>? Result { get; set; }
    public int Total { get; set; }
    public SerializableException? Exception { get; set; }

    [JsonIgnore]
    public bool Success => Result != null;
}
