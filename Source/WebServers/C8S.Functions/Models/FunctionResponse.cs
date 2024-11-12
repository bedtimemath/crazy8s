using SC.Common.Extensions;
using SC.Common.Models;

namespace C8S.Functions.Models;

public class FunctionResponse<TData>
{
    public static FunctionResponse<TData> CreateSuccessResponse(TData result) =>
        new FunctionResponse<TData>() { Result = result, Exception = null };

    public static FunctionResponse<TData> CreateFailureResponse(Exception exception) =>
        new FunctionResponse<TData>() { Result = default(TData), Exception = exception.ToSerializableException() };

    public static FunctionResponse<TData> CreateFailureResponse(SerializableException exception) =>
        new FunctionResponse<TData>() { Result = default(TData), Exception = exception };

    public TData? Result { get; set; } = default(TData);
    public SerializableException? Exception { get; set; }
}