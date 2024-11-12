using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.Functions.Functions;

public class WebHook(
    ILoggerFactory loggerFactory)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<WebHook>();
    #endregion

    [Function("WebHook")]
    public async Task<WebHookResponse> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        WebHookResponse response = new WebHookResponse();
        try
        {
            _logger.LogInformation("WebHook triggered");

            var reqObject = new
            {
                req.Url,
                req.Method,
                req.Headers,
                req.Cookies,
                Body = await new StreamReader(req.Body).ReadToEndAsync()
            };
            response.JsonString = JsonSerializer.Serialize(reqObject);

            response.HttpResponse = req.CreateResponse(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            response.HttpResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            // can't use WriteAsJson, see: https://github.com/Azure/azure-functions-dotnet-worker/issues/344
            // await response.WriteAsJsonAsync(ex.ToSerializableException());
            await response.HttpResponse.WriteStringAsync(JsonSerializer.Serialize(ex.ToSerializableException()));
        }
        return response;
    }

    public class WebHookResponse
    {
        [BlobOutput("webhook/{rand-guid}.json", Connection = "AzureWebJobsStorage")]
        public string? JsonString { get; set; }

        public HttpResponseData HttpResponse { get; set; } = default!;
    }
}