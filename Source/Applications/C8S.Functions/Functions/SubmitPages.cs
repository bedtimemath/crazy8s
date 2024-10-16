using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Azure.Storage.Blobs;
using C8S.Common;
using C8S.Common.Extensions;
using C8S.Common.Interfaces;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Enumerations;
using C8S.Database.Repository.Repositories;
using C8S.FullSlate.Abstractions;
using C8S.FullSlate.Abstractions.Models;
using C8S.FullSlate.Services;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions;

public class SubmitForm(
    ILoggerFactory loggerFactory,
    IDateTimeHelper dateTimeHelper,
    BlobServiceClient blobServiceClient,
    C8SRepository repository,
    FullSlateService fullSlateService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SubmitForm>();

    private const string CoachResponse = "I'm a coach";
    private const string HostedBeforeResponse = "We've hosted before";
    private const string NoWorkshopCodeResponse = "No";
    private readonly Dictionary<string, string> _formLookup = new Dictionary<string, string>()
    {
        { "FirstName", "wpforms[fields][7][first]" },
        { "LastName", "wpforms[fields][7][last]" },
        { "Email", "wpforms[fields][9]" },
        { "Phone", "wpforms[fields][10]" },
        { "IsCoach", "wpforms[fields][6]" },
        { "HostedBefore", "wpforms[fields][11]" },
        { "OrganizationName", "wpforms[fields][19]" },
        { "Address1", "wpforms[fields][20][address1]" },
        { "Address2", "wpforms[fields][20][address2]" },
        { "City", "wpforms[fields][20][city]" },
        { "State", "wpforms[fields][20][state]" },
        { "ZIPCode", "wpforms[fields][20][postal]" },
        { "OrganizationType", "wpforms[fields][21]" },
        { "OrganizationTypeOther", "wpforms[fields][22]" },
        { "TaxId", "wpforms[fields][24]" },
        { "ClubsString", "wpforms[fields][66]" },
        { "HasWorkshopCodeString", "wpforms[fields][34]" },
        { "TimeZone", "wpforms[fields][66]" },
        { "TimeSlotString", "wpforms[fields][64]" },
        { "WorkshopCode", "wpforms[fields][23]" },
        { "ReferenceSource", "wpforms[fields][38]" },
        { "ReferenceSourceOther", "wpforms[fields][46]" },
        { "Comments", "wpforms[fields][39]" }
    };
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
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-2/?code={code:N}");
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

    [Function("Submit-Page02")]
    public async Task<HttpResponseData> RunPage02(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/2")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page02 triggered");

            // find the unfinished using the code in the request
            var unfinished = await GetUnfinishedFromRequest(req);
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var firstName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["FirstName"])?.Data;
            var lastName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["LastName"])?.Data;
            var email = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Email"])?.Data;
            var phone = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Phone"])?.Data;
            var isCoachString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["IsCoach"])?.Data;
            var isCoach = isCoachString == CoachResponse;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 2);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(lastName)) throw new Exception("Could not read last name.");
            if (String.IsNullOrEmpty(email)) throw new Exception("Could not read email.");
            if (String.IsNullOrEmpty(phone)) throw new Exception("Could not read phone.");
            if (String.IsNullOrEmpty(isCoachString)) throw new Exception("Could not read applicant type.");

            // check for an existing email
            var existing = await repository.GetCoachByEmail(email);
            if (existing != null)
            {
                // redirect to the 'existing' page
                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-email-exists?email=" + HttpUtility.UrlEncode(email));
            }
            else
            {
                // update the unfinished data
                unfinished.ApplicantFirstName = firstName;
                unfinished.ApplicantLastName = lastName;
                unfinished.ApplicantEmail = email;
                unfinished.ApplicantPhone = phone;
                unfinished.ApplicantType = isCoach ? ApplicantType.Coach : ApplicantType.Supervisor;
                unfinished.EndPart02On = dateTimeHelper.UtcNow;
                await repository.UpdateUnfinished(unfinished);

                // return our redirect with the code
                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-3/?code={code:N}");
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

    [Function("Submit-Page03")]
    public async Task<HttpResponseData> RunPage03(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/3")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page03 triggered");

            // find the unfinished using the code in the request
            var unfinished = await GetUnfinishedFromRequest(req);
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var hostedBeforeString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["HostedBefore"])?.Data;
            var organizationName = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["OrganizationName"])?.Data;
            var address1 = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Address1"])?.Data;
            var address2 = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Address2"])?.Data;
            var city = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["City"])?.Data;
            var state = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["State"])?.Data;
            var zipCode = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ZIPCode"])?.Data;
            var organizationType = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["OrganizationType"])?.Data;
            var organizationTypeOther = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["OrganizationTypeOther"])?.Data;
            var taxId = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TaxId"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 3);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(hostedBeforeString)) throw new Exception("Could not read has done before.");
            if (String.IsNullOrEmpty(organizationName)) throw new Exception("Could not read organization name.");
            if (String.IsNullOrEmpty(address1)) throw new Exception("Could not read address line 1.");
            if (String.IsNullOrEmpty(city)) throw new Exception("Could not read city.");
            if (String.IsNullOrEmpty(state)) throw new Exception("Could not read state.");
            if (String.IsNullOrEmpty(zipCode)) throw new Exception("Could not read ZIP code.");
            if (String.IsNullOrEmpty(organizationType)) throw new Exception("Could not read organization type.");

            // update the unfinished data
            unfinished.HasHostedBefore = hostedBeforeString == HostedBeforeResponse;
            unfinished.OrganizationName = organizationName;
            unfinished.OrganizationAddress1 = address1;
            unfinished.OrganizationAddress2 = address2;
            unfinished.OrganizationCity = city;
            unfinished.OrganizationState = state;
            unfinished.OrganizationPostalCode = zipCode;
            unfinished.OrganizationType = organizationType switch
            {
                "School" => OrganizationType.School,
                "Library" => OrganizationType.Library,
                "Home School Co-Op" => OrganizationType.HomeSchool,
                "Boys & Girls Club" => OrganizationType.BoysGirlsClub,
                "YMCA" => OrganizationType.YMCA,
                "Other" => OrganizationType.Other,
                _ => throw new ArgumentOutOfRangeException(nameof(organizationType))
            };
            unfinished.OrganizationTypeOther = organizationTypeOther;
            unfinished.OrganizationTaxIdentifier = taxId;
            unfinished.EndPart03On = dateTimeHelper.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-4/?code={code:N}");
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

    [Function("Submit-Page04")]
    public async Task<HttpResponseData> RunPage04(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/4")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page04 triggered");

            // find the unfinished using the code in the request
            var unfinished = await GetUnfinishedFromRequest(req);
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var clubsString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ClubsString"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 4);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(clubsString)) throw new Exception("Could not read clubs string.");

            // update the unfinished data
            unfinished.ClubsString = clubsString;
            unfinished.EndPart04On = dateTimeHelper.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-5/?code={code:N}");
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

    [Function("Submit-Page05")]
    public async Task<HttpResponseData> RunPage05(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/5")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page05 triggered");

            // find the unfinished using the code in the request
            var unfinished = await GetUnfinishedFromRequest(req);
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var hasWorkshopCodeString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["HasWorkshopCodeString"])?.Data;
            var timeSlotString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TimeSlotString"])?.Data;
            var timeZone = ConvertFromJavascriptTimeZone(
                formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["TimeZone"])?.Data);
            var workshopCode = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["WorkshopCode"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 5);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(hasWorkshopCodeString)) throw new Exception("Could not read has workshop code.");
            var hasWorkshopCode = hasWorkshopCodeString != NoWorkshopCodeResponse;
            var chosenTimeSlot = (DateTimeOffset?)null;
            if (hasWorkshopCode)
            {
                if (String.IsNullOrEmpty(workshopCode)) throw new Exception("Could not read workshop code.");
                var found = await repository.GetWorkshopCodeByKey(workshopCode);
                if (found == null ||
                    (found.StartsOn != null && found.StartsOn > dateTimeHelper.UtcNow) ||
                    (found.EndsOn != null && found.EndsOn <= dateTimeHelper.UtcNow))
                {
                    return GetPage5ErrorMessageResponse(req, code,
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

            // these should be unnecessary
            if (chosenTimeSlot == null) throw new Exception("Could not read chosen timeslot.");
            if (String.IsNullOrEmpty(unfinished.ApplicantLastName)) throw new Exception("Applicant missing last name.");
            if (String.IsNullOrEmpty(unfinished.ApplicantEmail)) throw new Exception("Applicant missing email.");
            if (String.IsNullOrEmpty(unfinished.ApplicantPhone)) throw new Exception("Applicant missing phone.");

            // check on the new timeslot
            var appointmentCreation = new FullSlateAppointmentCreation()
            {
                At = chosenTimeSlot!.Value,
                Services = [FullSlateConstants.Offerings.CoachCall],
                Client = new FullSlateAppointmentCreationClient()
                {
                    FirstName = unfinished.ApplicantFirstName ?? SharedConstants.Display.None,
                    LastName = unfinished.ApplicantLastName,
                    Email = unfinished.ApplicantEmail,
                    PhoneNumber = new FullSlatePhoneNumber() { Number = unfinished.ApplicantPhone }
                },
                UserTypeString = FullSlateConstants.UserTypes.Client
            };
            var appointmentResponse = await fullSlateService.AddAppointment(appointmentCreation);
            if (!appointmentResponse.Success)
            {
                if (appointmentResponse.Errors?
                        .Any(e => e.ErrorCode is
                            FullSlateConstants.ErrorCodes.StatusBooked or
                            FullSlateConstants.ErrorCodes.NoOpening)
                    ?? false)
                {
                    return GetPage5ErrorMessageResponse(req, code,
                        "Unfortunately, that time slot has been booked. Please try another.");
                }

                var errorMessages = appointmentResponse.Errors?
                    .Select(e => RemoveSupportTeamMessage(e.ErrorMessage)) ?? ["Unknown error"];
                return GetPage5ErrorMessageResponse(req, code,
                    $"ERROR: {String.Join("; ", errorMessages)} Please try again later.");
            }

            // update the unfinished data
            unfinished.WorkshopCode = workshopCode;
            unfinished.ApplicantTimeZone = timeZone;
            unfinished.ChosenTimeSlot = chosenTimeSlot;
            unfinished.EndPart05On = dateTimeHelper.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // create the application
            var application = await repository.AddApplication(
                new ApplicationDTO()
                {
                    Status = ApplicationStatus.Received,
                    ApplicantType = unfinished.ApplicantType,
                    ApplicantFirstName = unfinished.ApplicantFirstName,
                    ApplicantLastName = unfinished.ApplicantLastName,
                    ApplicantEmail = unfinished.ApplicantEmail,
                    ApplicantPhone = unfinished.ApplicantPhone,
                    ApplicantTimeZone = unfinished.ApplicantTimeZone,
                    OrganizationName = unfinished.OrganizationName,
                    OrganizationType = unfinished.OrganizationType,
                    OrganizationTypeOther = unfinished.OrganizationTypeOther,
                    OrganizationTaxIdentifier = unfinished.OrganizationTaxIdentifier,
                    WorkshopCode = unfinished.WorkshopCode,
                    SubmittedOn = dateTimeHelper.UtcNow
                });

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-6/?code={code:N}&appid={application.ApplicationId}");
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

    [Function("Submit-Page06")]
    public async Task<HttpResponseData> RunPage06(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "coach-app/page/6")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Page06 triggered");

            // find the unfinished using the code in the request
            var unfinished = await GetUnfinishedFromRequest(req);
            var code = unfinished.Code ?? throw new Exception("Request missing code.");

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var referenceSource = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ReferenceSource"])?.Data;
            var referenceSourceOther = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ReferenceSourceOther"])?.Data;
            var comments = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Comments"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 6);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(referenceSource)) throw new Exception("Could not read has reference source.");

            // update the unfinished data
            unfinished.ReferenceSource = referenceSource;
            unfinished.ReferenceSourceOther = referenceSourceOther;
            unfinished.Comments = comments;
            unfinished.SubmittedOn = dateTimeHelper.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-complete");
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

    protected async Task<UnfinishedDTO> GetUnfinishedFromRequest(HttpRequestData req)
    {
        var code = req.Query["code"] ??
                   throw new Exception($"Could not read code from query string: {req.Url.AbsoluteUri}");
        if (!Guid.TryParse(code, out var guid))
            throw new Exception($"Badly formatted code: {code}");
        return await repository.GetUnfinishedByCode(guid) ??
                         throw new Exception($"Unrecognized code: {guid:N}");
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

    private static string ConvertFromJavascriptTimeZone(string? jsTimeZone) =>
        jsTimeZone switch
        {
            "America/New_York" => "Eastern Time",
            "America/Chicago" => "Central Time",
            "America/Denver" => "Mountain Time",
            "America/Los_Angeles" => "Pacific Time",
            "America/Anchorage" => "Alaskan Time",
            "Pacific/Honolulu" => "Hawaiian Time",
            _ => SharedConstants.Display.NotSet
        };
    #endregion
}