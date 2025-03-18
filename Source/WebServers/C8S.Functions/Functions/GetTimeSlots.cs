using System.Net;
using System.Text.Json;
using C8S.FullSlate.Services;
using C8S.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

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

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var endDate = startDate.AddDays(28);
            var openings = fullSlateService.GetOpeningsList(startDate, endDate);

            response = await req.CreateSuccessResponse(openings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            response = await req.CreateFailureResponse(ex);
        }
        return response;
    }
} 
