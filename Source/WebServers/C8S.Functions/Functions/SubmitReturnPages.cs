using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using Azure.Storage.Blobs;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.FullSlate.Abstractions;
using C8S.FullSlate.Abstractions.Interactions;
using C8S.FullSlate.Abstractions.Models;
using C8S.FullSlate.Services;
using C8S.Functions.Extensions;
using HttpMultipartParser;
using LZStringCSharp;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common;
using SC.Common.Extensions;
using SC.Common.Interfaces;
using Exception = System.Exception;

namespace C8S.Functions.Functions;

public class SubmitReturnPages(
    ILoggerFactory loggerFactory,
    IDateTimeHelper dateTimeHelper,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    BlobServiceClient blobServiceClient)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger _logger = loggerFactory.CreateLogger<SubmitReturnPages>();

    private readonly Dictionary<string, string> _formLookup = new Dictionary<string, string>()
    {
        { "Email", "wpforms[fields][66]" },
        { "ClubsString", "wpforms[fields][66]" },
        { "Comments", "wpforms[fields][73]" }
    };
    #endregion

    #region Function Methods
    [Function("Submit-Return01")]
    public async Task<HttpResponseData> RunPage01(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "return-app/page/1")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Return01 triggered");

            // setup for each page request
            const int pageNumber = 1;
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // the first time through we start by creating the unfinished, which adds the guid
            var unfinished = new UnfinishedDb() { EndPart01On = dateTimeHelper.UtcNow };
            await dbContext.Unfinisheds.AddAsync(unfinished);
            await dbContext.SaveChangesAsync();

            // read the form data
            var formData = await MultipartFormDataParser.ParseAsync(req.Body);
            var email = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Email"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, unfinished.Code, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(email)) throw new Exception("Could not read email.");

            // check for an existing email
            var existing = await dbContext.Persons
                .Include(p => p.Place)
                .Include(p => p.ClubPersons)
                .AsSingleQuery()
                .FirstOrDefaultAsync(c => c.Email == email);

            // a returning coach must have at least one club and an associated place
            if (existing == null ||
                !existing.ClubPersons.Any() || existing.Place == null)
            {
                // redirect to the 'missing' page
                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", "https://crazy8sclub.org/returning-application-email-missing?email=" + HttpUtility.UrlEncode(email));
            }
            else
            {
                // now we can update the unfinished entry, using the proper coach and place
                unfinished.PersonType = ApplicantType.Returning;

                unfinished.PersonId = existing.PersonId;
                unfinished.PersonFirstName = existing.FirstName;
                unfinished.PersonLastName = existing.LastName;
                unfinished.PersonEmail = existing.Email;
                unfinished.PersonPhone = existing.Phone;
                unfinished.PersonTimeZone = existing.TimeZone;
                unfinished.HasHostedBefore = true;

                unfinished.PlaceId = existing.Place.PlaceId;
                unfinished.PlaceName = existing.Place.Name;
                unfinished.PlaceAddress1 = existing.Place.Line1;
                unfinished.PlaceAddress2 = existing.Place.Line2;
                unfinished.PlaceCity = existing.Place.City;
                unfinished.PlaceState = existing.Place.State;
                unfinished.PlacePostalCode = existing.Place.ZIPCode;
                unfinished.PlaceType = existing.Place.Type;
                unfinished.PlaceTypeOther = existing.Place.TypeOther;
                unfinished.PlaceTaxIdentifier = existing.Place.TaxIdentifier;


                unfinished.EndPart01On = dateTimeHelper.UtcNow;

                await dbContext.SaveChangesAsync();

                httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
                httpResponse.Headers.Add("location", $"https://crazy8sclub.org/returning-application-2/?code={unfinished.Code:N}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/returning-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-Return02")]
    public async Task<HttpResponseData> RunPage02(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "return-app/page/2")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Return02 triggered");

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
            var clubsString = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["ClubsString"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            // check for errors (after saving to storage)
            if (String.IsNullOrEmpty(clubsString)) throw new Exception("Could not read clubs string.");

            // update the unfinished data
            unfinished.ClubsString = clubsString;
            unfinished.EndPart02On = dateTimeHelper.UtcNow;

            await dbContext.SaveChangesAsync();

            // set up the address for return in the query string
            var dataJson = JsonSerializer.Serialize(new
            {
                Clubs = clubsString.Split(' '),
                Address = new {
                    Name = unfinished.PlaceName,
                    Line1 = unfinished.PlaceAddress1,
                    Line2 = unfinished.PlaceAddress2,
                    City = unfinished.PlaceCity,
                    State = unfinished.PlaceState,
                    ZIPCode = unfinished.PlacePostalCode
                }
            });
            var compressed = LZString.CompressToEncodedURIComponent(dataJson);

            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/returning-application-3/?code={guidCode:N}&data={compressed}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/returning-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }

    [Function("Submit-Return03")]
    public async Task<HttpResponseData> RunPage03(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "return-app/page/3")] HttpRequestData req)
    {
        HttpResponseData httpResponse;
        try
        {
            _logger.LogInformation("Submit-Return03 triggered");

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
            var comments = formData.Parameters.FirstOrDefault(p => p.Name == _formLookup["Comments"])?.Data;

            // save to storage just in case
            await SaveFormDataToBlob(formData, guidCode, pageNumber);

            /*** DATABASE ***/
            // update the unfinished data
            unfinished.Comments = comments;
            unfinished.SubmittedOn = dateTimeHelper.UtcNow;

            // create the application & clubs
            var request = unfinished.ToRequest(dateTimeHelper);
            await dbContext.Requests.AddAsync(request);
            await dbContext.SaveChangesAsync();

            // return our redirect with the code
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", $"https://crazy8sclub.org/returning-application-complete");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception raised");
            httpResponse = req.CreateResponse(HttpStatusCode.Redirect);
            httpResponse.Headers.Add("location", "https://crazy8sclub.org/returning-application-error/?error=" +
                                                 HttpUtility.UrlEncode(ex.Message));
        }

        return httpResponse;
    }
    #endregion

    #region Private Methods
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