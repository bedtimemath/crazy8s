using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using Azure.Storage.Blobs;
using C8S.Domain.AppConfigs;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.FullSlate.Abstractions;
using C8S.FullSlate.Abstractions.Interactions;
using C8S.FullSlate.Abstractions.Models;
using C8S.FullSlate.Services;
using C8S.Functions.Extensions;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Models;
using SC.Common;
using SC.Common.Extensions;
using SC.Common.Interfaces;
using Exception = System.Exception;

namespace C8S.Functions.Functions;

public class SubmitNewPages(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory,
    IDateTimeHelper dateTimeHelper,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    BlobServiceClient blobServiceClient,
    FullSlateService fullSlateService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SubmitNewPages>();

    private const string CoachResponse = "I'm a coach";
    private const string HostedBeforeResponse = "We've hosted before";
    private const string NoWorkshopCodeResponse = "No";
    private readonly Dictionary<string, string> _formLookup = new Dictionary<string, string>()
    {
        { "FirstName", "wpforms[fields][7][first]" },
        { "LastName", "wpforms[fields][7][last]" },
        { "Email", "wpforms[fields][9]" },
        { "IsCoach", "wpforms[fields][6]" },
        { "HostedBefore", "wpforms[fields][77]" },
        { "PlaceName", "wpforms[fields][19]" },
        { "Address1", "wpforms[fields][65]" },
        { "Address2", "wpforms[fields][70]" },
        { "City", "wpforms[fields][67]" },
        { "State", "wpforms[fields][68]" },
        { "ZIPCode", "wpforms[fields][69]" },
        { "PlaceType", "wpforms[fields][73]" },
        { "PlaceTypeOther", "wpforms[fields][75]" },
        { "TaxId", "wpforms[fields][76]" },
        { "ClubsString", "wpforms[fields][66]" },
        { "HasWorkshopCodeString", "wpforms[fields][34]" },
        { "TimeZone", "wpforms[fields][66]" },
        { "TimeSlotString", "wpforms[fields][64]" },
        { "WorkshopCode", "wpforms[fields][23]" },
        { "Phone", "wpforms[fields][74]" },
        { "ReferenceSource", "wpforms[fields][67]" },
        { "ReferenceSourceOther", "wpforms[fields][71]" },
        { "Comments", "wpforms[fields][73]" }
    };

    private readonly Regex _parseStateFull = new Regex(@".*\((?<code>[A-Z][A-Z])\)");
    #endregion

    #region Function Methods
    [Function("Submit-New01")]
    public async Task<HttpResponseData> RunPage01(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/1")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-New01 triggered");
            
            // setup for each page request
            const int pageNumber = 1;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // the first time through we start by creating the unfinished, which adds the guid
            var unfinished = new UnfinishedDb() { EndPart01On = dateTimeHelper.UtcNow };
            await dbContext.Unfinisheds.AddAsync(unfinished);
            await dbContext.SaveChangesAsync();

            // save to storage just in case
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            await SaveFormDataToBlob(formData, unfinished.Code, pageNumber);

            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-2/?code={unfinished.Code:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-New02")]
    public async Task<HttpResponseData> RunPage02(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/2")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-New02 triggered");
            
            // setup for each page request
            const int pageNumber = 2;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var queryCode = req.Query["code"] ??
                       throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
            if (!Guid.TryParse(queryCode, out var guidCode))
                throw new Exception($"Badly formatted code: {queryCode}");

            // find the unfinished using the code in the request
            var unfinished = await dbContext.Unfinisheds // clubs included automatically
                .FirstOrDefaultAsync(a => a.Code == guidCode) ??
                   throw new Exception($"Unrecognized code: {guidCode:N}");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var firstName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["FirstName"])?.Data;
            var lastName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["LastName"])?.Data;
            var email = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Email"])?.Data;
            var isCoachString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["IsCoach"])?.Data;
            var isCoach = isCoachString == CoachResponse;

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(lastName)) throw new Exception("Could not read last name.");
            if (String.IsNullOrEmpty(email)) throw new Exception("Could not read email.");
            if (String.IsNullOrEmpty(isCoachString)) throw new Exception("Could not read applicant type.");

            // check for an existing email
            var existing = await dbContext.Persons
                .Include(p => p.Place)
                .Include(p => p.ClubPersons)
                .AsSingleQuery()
                .FirstOrDefaultAsync(c => c.Email == email);

            // if the person doesn't have a club or place, then it's okay
            if (existing != null &&
                existing.ClubPersons.Any() && existing.Place != null)
            {
                // redirect to the 'existing' page
                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-email-exists?email=" + HttpUtility.UrlEncode(email));
            }
            else
            {
                // update the unfinished data
                unfinished.PersonFirstName = firstName;
                unfinished.PersonLastName = lastName;
                unfinished.PersonEmail = email;
                unfinished.PersonType = isCoach ? ApplicantType.Coach : ApplicantType.Supervisor;
                unfinished.EndPart02On = dateTimeHelper.UtcNow;

                // in case we matched, but there weren't any clubs
                unfinished.PersonId = existing?.PersonId;
                unfinished.PlaceId = existing?.PlaceId;

                await dbContext.SaveChangesAsync();

                // return our redirect with the code
                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-3/?code={guidCode:N}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-New03")]
    public async Task<HttpResponseData> RunPage03(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/3")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-New03 triggered");
            
            // setup for each page request
            const int pageNumber = 3;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // find the unfinished using the code in the request
            var queryCode = req.Query["code"] ??
                            throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
            if (!Guid.TryParse(queryCode, out var guidCode))
                throw new Exception($"Badly formatted code: {queryCode}");

            // find the unfinished using the code in the request
            var unfinished = await dbContext.Unfinisheds // clubs included automatically
                                 .FirstOrDefaultAsync(a => a.Code == guidCode) ??
                             throw new Exception($"Unrecognized code: {guidCode:N}");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var hostedBeforeString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["HostedBefore"])?.Data;
            var placeName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["PlaceName"])?.Data;
            var address1 = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Address1"])?.Data;
            var address2 = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Address2"])?.Data;
            var city = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["City"])?.Data;
            var stateFull = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["State"])?.Data;
            var zipCode = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ZIPCode"])?.Data;
            var placeType = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["PlaceType"])?.Data;
            var placeTypeOther = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["PlaceTypeOther"])?.Data;
            var taxId = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TaxId"])?.Data;

            // the full state name is too long, but in case the values get changed, we'll use the best we can
            var stateMatch = _parseStateFull.Match(stateFull ?? String.Empty);
            var state = stateMatch.Success ? stateMatch.Groups["code"].Value : 
                (stateFull ?? String.Empty).LimitTo(SoftCrowConstants.MaxLengths.Tiny);

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(hostedBeforeString)) throw new Exception("Could not read has done before.");
            if (String.IsNullOrEmpty(placeName)) throw new Exception("Could not read place name.");
            if (String.IsNullOrEmpty(address1)) throw new Exception("Could not read address line 1.");
            if (String.IsNullOrEmpty(city)) throw new Exception("Could not read city.");
            if (String.IsNullOrEmpty(state)) throw new Exception("Could not read state.");
            if (String.IsNullOrEmpty(zipCode)) throw new Exception("Could not read ZIP code.");
            if (String.IsNullOrEmpty(placeType)) throw new Exception("Could not read place type.");

            // update the unfinished data
            unfinished.HasHostedBefore = hostedBeforeString == HostedBeforeResponse;
            unfinished.PlaceName = placeName;
            unfinished.PlaceAddress1 = address1;
            unfinished.PlaceAddress2 = address2;
            unfinished.PlaceCity = city;
            unfinished.PlaceState = state;
            unfinished.PlacePostalCode = zipCode;
            unfinished.PlaceType = placeType switch
            {
                "School" => PlaceType.School,
                "Library" => PlaceType.Library,
                "Home School Co-Op" => PlaceType.HomeSchool,
                "Boys & Girls Club" => PlaceType.BoysGirlsClub,
                "YMCA" => PlaceType.YMCA,
                "Other" => PlaceType.Other,
                _ => throw new ArgumentOutOfRangeException(nameof(placeType))
            };
            unfinished.PlaceTypeOther = placeTypeOther;
            unfinished.PlaceTaxIdentifier = taxId;
            unfinished.EndPart03On = dateTimeHelper.UtcNow;
            
            await dbContext.SaveChangesAsync();

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-4/?code={guidCode:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-New04")]
    public async Task<HttpResponseData> RunPage04(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/4")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-New04 triggered");
            
            // setup for each page request
            const int pageNumber = 4;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // find the unfinished using the code in the request
            var queryCode = req.Query["code"] ??
                            throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
            if (!Guid.TryParse(queryCode, out var guidCode))
                throw new Exception($"Badly formatted code: {queryCode}");

            // find the unfinished using the code in the request
            var unfinished = await dbContext.Unfinisheds // clubs included automatically
                                 .FirstOrDefaultAsync(a => a.Code == guidCode) ??
                             throw new Exception($"Unrecognized code: {guidCode:N}");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var clubsString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ClubsString"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(clubsString)) throw new Exception("Could not read clubs string.");

            // update the unfinished data
            unfinished.ClubsString = clubsString;
            unfinished.EndPart04On = dateTimeHelper.UtcNow;

            await dbContext.SaveChangesAsync();

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-5/?code={guidCode:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-New05")]
    public async Task<HttpResponseData> RunPage05(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/5")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-New05 triggered");
            
            // setup for each page request
            const int pageNumber = 5;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // find the unfinished using the code in the request
            var queryCode = req.Query["code"] ??
                            throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
            if (!Guid.TryParse(queryCode, out var guidCode))
                throw new Exception($"Badly formatted code: {queryCode}");

            // find the unfinished using the code in the request
            var unfinished = await dbContext.Unfinisheds // clubs included automatically
                                 .FirstOrDefaultAsync(a => a.Code == guidCode) ??
                             throw new Exception($"Unrecognized code: {guidCode:N}");
            
            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var hasWorkshopCodeString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["HasWorkshopCodeString"])?.Data;
            var timeSlotString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TimeSlotString"])?.Data;
            var timeZone = ConvertFromJavascriptTimeZone(
                formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TimeZone"])?.Data);
            var phone = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Phone"])?.Data;
            var workshopCode = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["WorkshopCode"])?.Data;
            var referenceSource = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ReferenceSource"])?.Data;
            var referenceSourceOther = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ReferenceSourceOther"])?.Data;
            var comments = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Comments"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(phone)) throw new Exception("Could not read phone.");
            if (String.IsNullOrEmpty(hasWorkshopCodeString)) throw new Exception("Could not read has workshop code.");
            var hasWorkshopCode = hasWorkshopCodeString != NoWorkshopCodeResponse;
            var chosenTimeSlot = (DateTimeOffset?)null;
            if (hasWorkshopCode)
            {
                if (String.IsNullOrEmpty(workshopCode)) throw new Exception("Could not read workshop code.");

                var found = await dbContext.WorkshopCodes
                    .FirstOrDefaultAsync(a => a.Key == workshopCode);
                if (found == null ||
                    (found.StartsOn != null && found.StartsOn > dateTimeHelper.UtcNow) ||
                    (found.EndsOn != null && found.EndsOn <= dateTimeHelper.UtcNow))
                {
                    return GetPage5ErrorMessageResponse(req, guidCode,
                        "Unrecognized workshop code. Please try again or choose \"No\".");
                }
            }
            else
            {
                if (String.IsNullOrEmpty(timeSlotString)) throw new Exception("Could not read time slot string.");
                if (!DateTimeOffset.TryParse(timeSlotString, out var timeSlot))
                    throw new Exception("Could not parse time slot string.");
                chosenTimeSlot = timeSlot;
            }

            /*** DATABASE ***/
            // update the unfinished data
            unfinished.PersonPhone = phone;
            unfinished.WorkshopCode = workshopCode;
            unfinished.PersonTimeZone = timeZone;
            unfinished.ChosenTimeSlot = chosenTimeSlot;
            unfinished.ReferenceSource = referenceSource;
            unfinished.ReferenceSourceOther = referenceSourceOther;
            unfinished.Comments = comments;
            unfinished.SubmittedOn = dateTimeHelper.UtcNow;
            
            // these should be unnecessary, but full slate requires them
            if (chosenTimeSlot == null) throw new Exception("Could not read chosen timeslot.");
            if (String.IsNullOrEmpty(unfinished.PersonLastName)) throw new Exception("Person missing last name.");
            if (String.IsNullOrEmpty(unfinished.PersonEmail)) throw new Exception("Person missing email.");
            if (String.IsNullOrEmpty(unfinished.PersonPhone)) throw new Exception("Person missing phone.");

            // create the application & clubs
            var request = unfinished.ToRequest(dateTimeHelper);
            await dbContext.Requests.AddAsync(request);
            unfinished.Request = request;

            await dbContext.SaveChangesAsync();

            /*** FULL SLATE ***/
            // convert timeslot to eastern time
            var easternTimeSlot =
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(chosenTimeSlot.Value, "Eastern Standard Time");

            // check on the new timeslot
            var appointmentCreation = new FullSlateAppointmentCreation()
            {
                At = easternTimeSlot,
                Services = [FullSlateConstants.Offerings.CoachCall],
                Client = new FullSlateAppointmentCreationClient()
                {
                    FirstName = unfinished.PersonFirstName ?? SoftCrowConstants.Display.None,
                    LastName = unfinished.PersonLastName,
                    Email = unfinished.PersonEmail,
                    PhoneNumber = new FullSlatePhoneNumber() { Number = unfinished.PersonPhone }
                },
                UserTypeString = FullSlateConstants.UserTypes.Client
            };
            var appointmentResponse = await fullSlateService.AddAppointment(appointmentCreation);
            if (!appointmentResponse.Success)
            {
                await SaveFullSlateErrorToBlob(appointmentResponse, guidCode, pageNumber);

                // gather the full slate errors
                if (appointmentResponse.Errors?
                        .Any(e => e.ErrorCode is
                            FullSlateConstants.ErrorCodes.StatusBooked or
                            FullSlateConstants.ErrorCodes.NoOpening)
                    ?? false)
                {
                    return GetPage5ErrorMessageResponse(req, guidCode,
                        "Unfortunately, that time slot has been booked. Please try another.");
                }
                var errorMessages = appointmentResponse.Errors?
                    .Select(e => RemoveSupportTeamMessage(e.ErrorMessage))?
                    .ToList() ?? new List<string>();
                
                // back out the application
                try
                {
                    dbContext.Requests.Remove(request);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    errorMessages.Add($"Could not remove application: {exception.Message}");
                }

                // return the error messages to the user
                if (!errorMessages.Any()) errorMessages.Add("Unknown Error");
                return GetPage5ErrorMessageResponse(req, guidCode,
                    $"ERROR: {String.Join("; ", errorMessages)} Please try again later.");
            }

            /*** COMPLETE ***/
            // call the admin app endpoint to let it know
            try
            {
                using var httpClient = httpClientFactory.CreateClient(nameof(Endpoints.C8SAdminApp));
                var options = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } };
                var dataChange = new DataChange()
                {
                    EntityId = request.RequestId,
                    EntityName = nameof(RequestDb),
                    EntityState = EntityState.Added
                };
                var response = await httpClient.PostAsJsonAsync("/api/datachanges", dataChange, options);
                if (!response.IsSuccessStatusCode)
                    _logger.LogWarning("Failure calling C8SAdminApp endpoint: {Message}", response.ReasonPhrase);
            }
            catch (Exception exception)
            {
                _logger.LogWarning("Error calling C8SAdminApp endpoint: {Message}", exception.Message);
            }

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-complete");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" +
                                                 HttpUtility.UrlEncode(RemoveSupportTeamMessage(ex.Message)));
        }

        return httpResponse;
    }
    #endregion

    #region Private Methods
    // Full Slate returns error messages asking the user to send something to the support team. We don't
    //  want our users seeing that.
    private static string RemoveSupportTeamMessage(string? message)
    {
        if (String.IsNullOrEmpty(message)) return String.Empty;
        return message.Contains(FullSlateConstants.ErrorMessages.PleaseSendRequestId)
            ? message.Replace(FullSlateConstants.ErrorMessages.PleaseSendRequestId, String.Empty) : message;
    }

    private static HttpResponseData GetPage5ErrorMessageResponse(HttpRequestData req, Guid code, string message)
    {
        var httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
        httpResponse.Headers.Add("location",
            $"https://crazy8sclub.org/coach-application-5/?code={code:N}&message={Uri.EscapeDataString(message)}");
        return httpResponse;
    }

    private async Task SaveFormDataToBlob(MultipartFormDataParser formData, Guid code, int pageNumber)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient("c8s-applications");
        var blobName = $"{dateTimeHelper.UtcNow:yyyyMMddTHHmmss}-{code:N}-{pageNumber:00}-{String.Empty.AppendRandomAlphaOnly(3)}.json";
        var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(formData.Parameters)));
        var azureResponse = await containerClient.UploadBlobAsync(blobName, jsonStream);
        if (!azureResponse.HasValue)
            throw new Exception($"Could not create blob: {blobName}");
    }

    private async Task SaveFullSlateErrorToBlob(ServiceResponse<FullSlateAppointment> response, Guid code, int pageNumber)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient("c8s-applications");
        var blobName = $"{dateTimeHelper.UtcNow:yyyyMMddTHHmmss}-{code:N}-{pageNumber:00}-FullSlateError-{String.Empty.AppendRandomAlphaOnly(3)}.json";
        var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)));
        var azureResponse = await containerClient.UploadBlobAsync(blobName, jsonStream);
        if (!azureResponse.HasValue)
            throw new Exception($"Could not create blob: {blobName}");
    }

    private static string ConvertFromJavascriptTimeZone(string? jsTimeZone) =>
        jsTimeZone switch
        {
            "America/New_York" => "Eastern Time",
            "America/Chicago" => "Central Time",
            "America/Denver" => "Mountain Time",
            "America/Los_Angeles" => "Pacific Time",
            "America/Anchorage" => "Alaskan Time",
            "Pacific/Honolulu" => "Hawaiian Time",
            _ => SoftCrowConstants.Display.NotSet
        };
    #endregion
}