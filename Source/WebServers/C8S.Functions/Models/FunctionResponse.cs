using SC.Common.Extensions;
using SC.Common.Models;

namespace C8S.Functions.Models;

public class FunctionResponse
{
    public static FunctionResponse CreateSuccessResponse() =>
        new FunctionResponse() { Exception = null };

    public static FunctionResponse CreateFailureResponse(Exception exception) =>
        new FunctionResponse() { Exception = exception.ToSerializableException() };

    public static FunctionResponse CreateFailureResponse(SerializableException exception) =>
        new FunctionResponse() { Exception = exception };

    public SerializableException? Exception { get; set; }
}

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