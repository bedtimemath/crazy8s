using C8S.Common.Models;
using C8S.FullSlate.Abstractions.Interactions;
using C8S.FullSlate.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace C8S.FullSlate.Services;

public class FullSlateService(
    ILogger<FullSlateService> logger,
    IHttpClientFactory httpClientFactory)
{
    #region Constants & ReadOnlys
    public static string HttpAuthName = "FullSlate";

    public const int Crazy8sCallId = 1;

    public const string AppointmentsEndpoint = "appointments";
    public const string OpeningsEndpoint = "openings";
    #endregion

    #region Public Methods
    public async Task<FullSlateResponse<FullSlateOpeningsList>> GetOpeningsList(
        DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = OpeningsEndpoint;
            var qsParams = new Dictionary<string, string?>()
                { { "services", Crazy8sCallId.ToString() } };
            if (fromDate != null) qsParams.Add("from", fromDate.Value.ToString("yyyy-MM-dd"));
            if (toDate != null) qsParams.Add("to", toDate.Value.ToString("yyyy-MM-dd"));
            if (qsParams.Any())
                url = QueryHelpers.AddQueryString(url, qsParams);

            // make the call
            var retrieved = await httpClient.GetAsync(url);

            // get the results; used for success or failure
            var success = retrieved.IsSuccessStatusCode;
            var responseBody = await retrieved.Content.ReadAsStringAsync();

            // *** FAILURE ***
            // on failure, we should have a BiginResponseResult as the response body
            if (!success)
            {
                var failureResponse = FullSlateResponse<FullSlateOpeningsList>
                    .CreateFailure(retrieved.StatusCode, responseBody);
                return failureResponse;
            }

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return FullSlateResponse<FullSlateOpeningsList>
                .CreateSuccess(retrieved.StatusCode, responseBody);

        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return FullSlateResponse<FullSlateOpeningsList>
                .CreateFailure(HttpStatusCode.InternalServerError,
                    new FullSlateErrorResponse()
                    {
                        Failure = true,
                        ErrorMessage = ex.Message,
                        Details = JsonSerializer.SerializeToElement(
                            new SerializableException(ex))
                    });
        }
    }

    public async Task<FullSlateResponse<List<FullSlateAppointment>>> GetAppointments(
        DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = AppointmentsEndpoint;
            var qsParams = new Dictionary<string, string?>();
            if (fromDate != null) qsParams.Add("from", fromDate.Value.ToString("yyyy-MM-dd"));
            if (toDate != null) qsParams.Add("to", toDate.Value.ToString("yyyy-MM-dd"));
            if (qsParams.Any())
                url = QueryHelpers.AddQueryString(url, qsParams);

            // make the call
            var retrieved = await httpClient.GetAsync(url);

            // get the results; used for success or failure
            var success = retrieved.IsSuccessStatusCode;
            var responseBody = await retrieved.Content.ReadAsStringAsync();

            // *** FAILURE ***
            // on failure, we should have a BiginResponseResult as the response body
            if (!success)
            {
                var failureResponse = FullSlateResponse<List<FullSlateAppointment>>
                    .CreateFailure(retrieved.StatusCode, responseBody);
                return failureResponse;
            }

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return FullSlateResponse<List<FullSlateAppointment>>
                .CreateSuccess(retrieved.StatusCode, responseBody);

        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return FullSlateResponse<List<FullSlateAppointment>>
                .CreateFailure(HttpStatusCode.InternalServerError,
                    new FullSlateErrorResponse()
                    {
                        Failure = true,
                        ErrorMessage = ex.Message,
                        Details = JsonSerializer.SerializeToElement(
                            new SerializableException(ex))
                    });
        }
    }
    #endregion
}