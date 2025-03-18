using System.Net;
using C8S.Functions.Models;
using Microsoft.Azure.Functions.Worker.Http;
using SC.Common.Models;

namespace C8S.Functions.Extensions;

public static class HttpRequestDataEx
{
    public static async Task<HttpResponseData> CreateSuccessResponse(this HttpRequestData request,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var result = FunctionResponse.CreateSuccessResponse();
        var response = request.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(result);
        
        return response;
    }
    public static async Task<HttpResponseData> CreateSuccessResponse<TData>(this HttpRequestData request,
        TData payload, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var result = FunctionResponse<TData>.CreateSuccessResponse(payload);
        var response = request.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(result);
        
        return response;
    }
    public static async Task<HttpResponseData>  CreateFailureResponse(this HttpRequestData request,
        Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var result = FunctionResponse.CreateFailureResponse(exception);
        var response = request.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(result);
        
        return response;
    }
    public static async Task<HttpResponseData>  CreateFailureResponse(this HttpRequestData request,
        SerializableException exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        var result = FunctionResponse.CreateFailureResponse(exception);
        var response = request.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(result);
        
        return response;
    }
}