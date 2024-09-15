using System.Net;
using System.Text.Json;
using C8S.Common.Extensions;
using C8S.FullSlate.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions;

public class GetOpenTimeSlots(
    ILoggerFactory loggerFactory,
    FullSlateService fullSlateService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetOpenTimeSlots>();
    #endregion

    [Function("GetOpenTimeSlots")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        HttpResponseData response;
        try
        {
            _logger.LogInformation("GetOpenTimeSlots triggered");

            response = req.CreateResponse(HttpStatusCode.OK);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var endDate = startDate.AddDays(28);
            var openings = fullSlateService.GetOpeningsList(startDate, endDate);

            await response.WriteStringAsync(JsonSerializer.Serialize(openings));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            // can't use WriteAsJson, see: https://github.com/Azure/azure-functions-dotnet-worker/issues/344
            // await response.WriteAsJsonAsync(ex.ToSerializableException());
            await response.WriteStringAsync(JsonSerializer.Serialize(ex.ToSerializableException()));
        }
        return response;
    }
}