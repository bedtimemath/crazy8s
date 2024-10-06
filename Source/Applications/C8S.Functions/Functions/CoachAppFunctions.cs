#if false
using C8S.Database.Repository.Repositories;
using System.Net;
using System.Text.Json;
using C8S.Database.Abstractions.DTOs;
using Microsoft.Extensions.Logging;
using C8S.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace C8S.Functions.Functions;

internal class CoachAppFunctions(
    ILoggerFactory loggerFactory,
    C8SRepository repository)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<CoachAppFunctions>();
    #endregion

    #region Function Method
    [Function("CoachAppInitialize")]
    public async Task<HttpResponseData> RunInitialize(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "coach-app/initialize")] HttpRequestData req)
    {
        FunctionResponse<UnfinishedDTO> dataResponse;
        try
        {
            _logger.LogInformation("CoachAppInitialize triggered");

            var unfinished = await repository.AddUnfinished();
            dataResponse = FunctionResponse<UnfinishedDTO>.CreateSuccessResponse(unfinished);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            dataResponse = FunctionResponse<UnfinishedDTO>.CreateFailureResponse(ex);
        }

        var httpResponse = req.CreateResponse(HttpStatusCode.OK);
        await httpResponse.WriteStringAsync(JsonSerializer.Serialize(dataResponse));
        return httpResponse;
    }

    [Function("CoachAppCheckEmail")]
    public async Task<HttpResponseData> RunCheckEmail(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/check-email")] HttpRequestData req)
    {
        FunctionResponse<CoachDTO?> dataResponse;
        try
        {
            _logger.LogInformation("CoachAppCheckEmail triggered");

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var coachAppRequest = JsonSerializer.Deserialize<CoachAppRequest<string>>(body) ??
                                  throw new Exception("Could not deserialize");

            _ = await CheckRequest(coachAppRequest);
            var email = coachAppRequest.Data;
            if (String.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            var coach = await repository.GetCoachByEmail(email);
            dataResponse = FunctionResponse<CoachDTO?>.CreateSuccessResponse(coach);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            dataResponse = FunctionResponse<CoachDTO?>.CreateFailureResponse(ex);
        }

        var httpResponse = req.CreateResponse(HttpStatusCode.OK);
        await httpResponse.WriteStringAsync(JsonSerializer.Serialize(dataResponse));
        return httpResponse;
    }
    #endregion

    #region Private Methods
    protected async Task<UnfinishedDTO> CheckRequest<TData>(CoachAppRequest<TData> coachAppRequest)
    {
        if (!Guid.TryParse(coachAppRequest.Code, out var code))
            throw new Exception($"Could not parse code: {coachAppRequest.Code}");

        return await repository.GetUnfinishedByCode(code) ??
            throw new ArgumentOutOfRangeException(nameof(code));
    }
    #endregion
} 
#endif