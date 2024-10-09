using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Azure.Storage.Blobs;
using C8S.Common.Extensions;
using C8S.Common.Interfaces;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Enumerations;
using C8S.Database.Repository.Repositories;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions;

public class SubmitForm(
    ILoggerFactory loggerFactory,
    IDateTimeHelper dateTimeHelper,
    BlobServiceClient blobServiceClient,
    C8SRepository repository)
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
        { "TimeSlotString", "wpforms[fields][64]" },
        { "WorkshopCode", "wpforms[fields][23]" }
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
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" + HttpUtility.UrlEncode(ex.Message));
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
                unfinished.EndPart02On = DateTimeOffset.UtcNow;
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
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" + HttpUtility.UrlEncode(ex.Message));
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
            unfinished.EndPart03On = DateTimeOffset.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-4/?code={code:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" + HttpUtility.UrlEncode(ex.Message));
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
            unfinished.EndPart04On = DateTimeOffset.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-5/?code={code:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" + HttpUtility.UrlEncode(ex.Message));
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
            var workshopCode = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["WorkshopCode"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, code, 5);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(hasWorkshopCodeString)) throw new Exception("Could not read has workshop code.");
            var hasWorkshopCode = hasWorkshopCodeString != NoWorkshopCodeResponse;
            var chosenTimeSlot = (DateTimeOffset?)null;
            if (hasWorkshopCode) {
                if (String.IsNullOrEmpty(workshopCode)) throw new Exception("Could not read workshop code.");
                var found = await repository.GetWorkshopCodeByKey(workshopCode);
                if (found == null || 
                    (found.StartsOn != null && found.StartsOn > dateTimeHelper.UtcNow) || 
                    (found.EndsOn != null && found.EndsOn <= dateTimeHelper.UtcNow))
                {
                    var message = "Unrecognized workshop code. Please try again or choose \"No\".";
                    httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                    httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-5/?code={code:N}&message={message}");
                    return httpResponse;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(timeSlotString)) throw new Exception("Could not read time slot string.");
                if (!DateTimeOffset.TryParse(timeSlotString, out var timeSlot))
                    throw new Exception("Could not parse time slot string.");
                chosenTimeSlot = timeSlot;
            }

            // update the unfinished data
            unfinished.WorkshopCode = workshopCode;
            unfinished.ChosenTimeSlot = chosenTimeSlot;
            unfinished.EndPart05On = DateTimeOffset.UtcNow;
            await repository.UpdateUnfinished(unfinished);

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/coach-application-6/?code={code:N}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/coach-application-error/?error=" + HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }
    #endregion

    #region Private Methods
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
    #endregion
}