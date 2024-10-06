#if false
using System.Net;
using System.Text.Json;
using System.Web;
using C8S.Database.Repository.Repositories;
using C8S.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions.Deprecated;

public class CheckEmail(
    ILoggerFactory loggerFactory,
    C8SRepository repository)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<CheckEmail>();
    #endregion

    [Function("CheckEmail")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        HttpResponseData httpResponseData;
        FunctionResponse<CheckEmailResult> functionResponse;
        try
        {
            _logger.LogInformation("CheckEmail triggered");

            var email = HttpUtility.ParseQueryString(req.Url.Query)["email"];
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            httpResponseData = req.CreateResponse(HttpStatusCode.OK);

            var found = await repository.GetCoachByEmail(email);
            var result = new CheckEmailResult() { Found = found != null, Coach = found };

            functionResponse = FunctionResponse<CheckEmailResult>.CreateSuccessResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponseData = req.CreateResponse(HttpStatusCode.InternalServerError);
            // can't use WriteAsJson, see: https://github.com/Azure/azure-functions-dotnet-worker/issues/344
            // await response.WriteAsJsonAsync(ex.ToSerializableException());
            functionResponse = FunctionResponse<CheckEmailResult>.CreateFailureResponse(ex);
        }

        await httpResponseData.WriteStringAsync(JsonSerializer.Serialize(functionResponse));
        return httpResponseData;
    }
} 
#endif