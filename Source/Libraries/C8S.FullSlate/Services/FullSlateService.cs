using C8S.Common.Models;
using C8S.FullSlate.Abstractions.Interactions;
using C8S.FullSlate.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using C8S.FullSlate.Abstractions;
using Microsoft.AspNetCore.WebUtilities;

namespace C8S.FullSlate.Services;

public class FullSlateService(
    ILogger<FullSlateService> logger,
    IHttpClientFactory httpClientFactory)
{
    #region Constants & ReadOnlys
    public static string HttpAuthName = "FullSlate";

    public static JsonSerializerOptions FullSlateRequestJsonOptions =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
    #endregion

    #region Public Methods
    public async Task<ServiceResponse<FullSlateOpeningsList>> GetOpeningsList(
        DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = FullSlateConstants.Endpoints.Openings;
            var qsParams = new Dictionary<string, string?>()
                { { "services", FullSlateConstants.Offerings.CoachCall.ToString() } };
            if (fromDate != null) qsParams.Add("from", fromDate.Value.ToString("yyyy-MM-dd"));
            if (toDate != null) qsParams.Add("to", toDate.Value.ToString("yyyy-MM-dd"));
            if (qsParams.Any())
                url = QueryHelpers.AddQueryString(url, qsParams);

            // make the call
            var retrieved = await httpClient.GetAsync(url);

            // get the results; used for success or failure
            var success = retrieved.IsSuccessStatusCode;
            var responseBody = await retrieved.Content.ReadAsStringAsync();

            // *** FAILURE (Network) ***
            // on failure, we should have a BiginResponseResult as the response body
            if (!success)
                return CreateNetworkErrorResponse<FullSlateOpeningsList>(retrieved, responseBody);

            // *** FAILURE (FullSlate) ***
            var deserialized = JsonSerializer.Deserialize<JsonNode>(responseBody);
            var errorCode = deserialized?["code"]?.GetValue<string?>();
            if (!String.IsNullOrEmpty(errorCode))
                return CreateFullSlateErrorResponse<FullSlateOpeningsList>(responseBody);

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return ServiceResponse<FullSlateOpeningsList>
                .CreateSuccess(retrieved.StatusCode, responseBody);

        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return ServiceResponse<FullSlateOpeningsList>
                .CreateFailure(HttpStatusCode.InternalServerError,
                [new ServiceError()
                {
                    ErrorMessage = ex.Message,
                    Details = JsonSerializer.Serialize(new SerializableException(ex))
                }]);
        }
    }

    public async Task<ServiceResponse<List<FullSlateAppointment>>> GetAppointments(
        DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = FullSlateConstants.Endpoints.Appointments;
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

            // *** FAILURE (Network) ***
            // on failure, we should have a BiginResponseResult as the response body
            if (!success)
                return CreateNetworkErrorResponse<List<FullSlateAppointment>>(retrieved, responseBody);

            // *** FAILURE (FullSlate) ***
            var deserialized = JsonSerializer.Deserialize<JsonNode>(responseBody);
            var errorCode = deserialized?["code"]?.GetValue<string?>();
            if (!String.IsNullOrEmpty(errorCode))
                return CreateFullSlateErrorResponse<List<FullSlateAppointment>>(responseBody);

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return ServiceResponse<List<FullSlateAppointment>>
                .CreateSuccess(retrieved.StatusCode, responseBody);

        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return ServiceResponse<List<FullSlateAppointment>>
                .CreateFailure(HttpStatusCode.InternalServerError,
                [new ServiceError()
                {
                    ErrorMessage = ex.Message,
                    Details = JsonSerializer.Serialize(new SerializableException(ex))
                }]);
        }
    }

    public async Task<ServiceResponse<FullSlateAppointment>> AddAppointment(
        FullSlateAppointmentCreation appointmentCreation)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = FullSlateConstants.Endpoints.Appointments;

            // make the call
            var retrieved = await httpClient.PostAsync(url,
                new StringContent(
                    JsonSerializer.Serialize(appointmentCreation, FullSlateRequestJsonOptions),
                    Encoding.UTF8, "application/json"));

            // get the results; used for success or failure
            var success = retrieved.IsSuccessStatusCode;
            var responseBody = await retrieved.Content.ReadAsStringAsync();

            // *** FAILURE (Network) ***
            if (!success)
            {
                if (retrieved.StatusCode == HttpStatusCode.BadRequest)
                {
                    try
                    {
                        return CreateFullSlateBadRequestResponse<FullSlateAppointment>(responseBody);
                    }
                    catch
                    {
                        // if this fails, the fallthrough is the best choice anyway
                    }
                }
                return CreateNetworkErrorResponse<FullSlateAppointment>(retrieved, responseBody);
            }

            // *** FAILURE (FullSlate) ***
            var deserialized = JsonSerializer.Deserialize<JsonNode>(responseBody);
            var errorCode = deserialized?["code"]?.GetValue<string?>();
            if (!String.IsNullOrEmpty(errorCode))
                return CreateFullSlateErrorResponse<FullSlateAppointment>(responseBody);

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return ServiceResponse<FullSlateAppointment>
                .CreateSuccess(retrieved.StatusCode, responseBody);
        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return ServiceResponse<FullSlateAppointment>
                .CreateFailure(HttpStatusCode.InternalServerError,
                [new ServiceError()
                {
                    ErrorMessage = ex.Message,
                    Details = JsonSerializer.Serialize(new SerializableException(ex))
                }]);
        }
    }

    public async Task<ServiceResponse<FullSlateClient>> AddClient(
        FullSlateClientCreation clientCreation)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(FullSlateService.HttpAuthName);

            // set up the query string
            var url = FullSlateConstants.Endpoints.Clients;

            // make the call
            var retrieved = await httpClient.PostAsync(url,
                new StringContent(
                    JsonSerializer.Serialize(clientCreation, FullSlateRequestJsonOptions),
                    Encoding.UTF8, "application/json"));

            // get the results; used for success or failure
            var success = retrieved.IsSuccessStatusCode;
            var responseBody = await retrieved.Content.ReadAsStringAsync();

            // *** FAILURE (Network) ***
            // on failure, we should have a BiginResponseResult as the response body
            if (!success)
                return CreateNetworkErrorResponse<FullSlateClient>(retrieved, responseBody);

            // *** FAILURE (FullSlate) ***
            var deserialized = JsonSerializer.Deserialize<JsonNode>(responseBody);
            var errorCode = deserialized?["code"]?.GetValue<string?>();
            if (!String.IsNullOrEmpty(errorCode))
                return CreateFullSlateErrorResponse<FullSlateClient>(responseBody);

            // *** SUCCESS ***
            // on success, the response body is of the TData type
            return ServiceResponse<FullSlateClient>
                .CreateSuccess(retrieved.StatusCode, responseBody);
        }
        catch (Exception ex)
        {
            // *** EXCEPTION ***
            // fake a failure, using the serialized exception as the details
            return ServiceResponse<FullSlateClient>
                .CreateFailure(HttpStatusCode.InternalServerError,
                    [new ServiceError()
                    {
                        ErrorMessage = ex.Message,
                        Details = JsonSerializer.Serialize(new SerializableException(ex))
                    }]);
        }
    }

    #endregion

    #region Private Static Methods

    private static ServiceResponse<TData> CreateNetworkErrorResponse<TData>(
        HttpResponseMessage retrieved, string responseBody)
        where TData : class, new()
    {
        var failureResponse = ServiceResponse<TData>
            .CreateFailure(retrieved.StatusCode,
            [
                new ServiceError()
                {
                    ErrorCode = "NETWORK_ERROR",
                    ErrorMessage = "StatusCode Failure: " + (retrieved.ReasonPhrase ?? "Unknown"),
                    Details = responseBody
                }
            ]);
        return failureResponse;
    }

    private static ServiceResponse<TData> CreateFullSlateErrorResponse<TData>(
        string responseBody)
        where TData : class, new()
    {
        var fullSlateErrorResponse =
            JsonSerializer.Deserialize<FullSlateErrorResponse>(responseBody) ??
            throw new Exception($"Could not deserialize as FullSlate error: {responseBody}");
        var failureResponse = ServiceResponse<TData>
            .CreateFailure(HttpStatusCode.OK,
                fullSlateErrorResponse.Errors
                    .Select(fullSlateError =>
                        new ServiceError()
                        {
                            ErrorCode = fullSlateErrorResponse.Code,
                            ErrorMessage = fullSlateError.Message,
                            FieldName = fullSlateError.FieldName,
                            Example = fullSlateError.Example,
                            Details = responseBody
                        })
                    .ToList());
        return failureResponse;
    }

    private static ServiceResponse<TData> CreateFullSlateBadRequestResponse<TData>(
        string responseBody)
        where TData : class, new()
    {
        var badRequestResponse =
            JsonSerializer.Deserialize<FullSlateBadRequestResponse>(responseBody) ??
            throw new Exception($"Could not deserialize as FullSlate error: {responseBody}");
        var failureResponse = ServiceResponse<TData>
            .CreateFailure(HttpStatusCode.OK, [new ServiceError()
            {
                ErrorCode = badRequestResponse.ErrorCode,
                ErrorMessage = badRequestResponse.ErrorMessage,
                Details = responseBody
            }]);
        return failureResponse;
    }

    #endregion
}