using System.Net;
using System.Text.Json;
using C8S.Common.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions;

public class SubmitClubApplication(
    ILoggerFactory loggerFactory,
    IConfiguration configuration)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SubmitClubApplication>();
    #endregion

    [Function("SubmitClubApplication")]
    public async Task<SubmitClubApplicationResponse> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        SubmitClubApplicationResponse response = new SubmitClubApplicationResponse();
        try
        {
            _logger.LogInformation("SubmitClubApplication triggered");

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

    public class SubmitClubApplicationResponse
    {
        [BlobOutput("c8s-applications/{rand-guid}.json", Connection = "AzureWebJobsStorage")]
        public string? JsonString { get; set; }

        public HttpResponseData HttpResponse { get; set; } = default!;
    }
}