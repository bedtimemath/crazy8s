using System.Net;
using System.Text.Json;
using System.Web;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Models;
using C8S.Database.Repository.Repositories;
using C8S.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions;

public class SubmitForm(
    ILoggerFactory loggerFactory,
    C8SRepository repository)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SubmitForm>();
    #endregion

    #region Function Methods
    [Function("Submit-Page01")]
    public async Task<HttpResponseData> RunPage01(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/1")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page01 triggered");

            var unfinished = await repository.AddUnfinished();
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-2/?code=" + unfinished.Code!.Value.ToString("N"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-2/?error=" + ex.Message);
        }

        return httpResponse;
    }

    [Function("Submit-Page02")]
    public async Task<HttpResponseData> RunPage02(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/2")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page02 triggered");

            var code = HttpUtility.ParseQueryString(req.Url.AbsoluteUri)["code"] ??
                       throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
            var unfinished = await repository.GetUnfinishedByCode(Guid.Parse(code));

            unfinished.EndPart02On = DateTimeOffset.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-3/?code=" + code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-3/?error=" + ex.Message);
        }

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