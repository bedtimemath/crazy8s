using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using C8S.Applications.Models;
using C8S.Domain;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using SC.Common.Models;

namespace C8S.Applications.Services;

public class ApplicationService(
    ILogger<ApplicationService> logger,
    BlobServiceClient blobServiceClient)
{
    public async Task<ProcessApplicationsResponse> ProcessApplications(
        Func<SubmittedApplication, Task<SerializableException?>> processApplication)
    {
        var sourceContainer = blobServiceClient.GetBlobContainerClient(C8SConstants.BlobContainers.ApplicationsUnread);
        if (!await sourceContainer.ExistsAsync())
            await blobServiceClient.CreateBlobContainerAsync(C8SConstants.BlobContainers.ApplicationsUnread, PublicAccessType.None);

        var processedContainer = blobServiceClient.GetBlobContainerClient(C8SConstants.BlobContainers.ApplicationsProcessed);
        if (!await processedContainer.ExistsAsync())
            await blobServiceClient.CreateBlobContainerAsync(C8SConstants.BlobContainers.ApplicationsProcessed, PublicAccessType.None);

        var errorContainer = blobServiceClient.GetBlobContainerClient(C8SConstants.BlobContainers.ApplicationsError);
        if (!await errorContainer.ExistsAsync())
            await blobServiceClient.CreateBlobContainerAsync(C8SConstants.BlobContainers.ApplicationsError, PublicAccessType.None);

        var response = new ProcessApplicationsResponse();

        await foreach (var blobItem in sourceContainer.GetBlobsAsync())
        {
            response.TotalProcessed++;

            SerializableException? processException = null;
            try
            {
                logger.LogInformation("Reading blob: {BlobName}", blobItem.Name);
                var blobClient = sourceContainer.GetBlobClient(blobItem.Name);

                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                var createdOn = blobProperties.CreatedOn;

                BlobDownloadResult blobResponse = await blobClient.DownloadContentAsync();

                var blobJson = blobResponse.Content.ToString();
                var parsed = JsonDocument.Parse(blobJson);
                var bodyJson = parsed.RootElement.GetProperty("Body").GetString() ??
                               throw new Exception("Missing Body JSON");

                var application = JsonSerializer.Deserialize<SubmittedApplication>(bodyJson) ??
                                  throw new Exception("Could not deserialize application.");
                application.CreatedOn = createdOn;

                processException = await processApplication(application);
            }
            catch (Exception ex)
            {
                processException = ex.ToSerializableException();
            }

            if (processException != null)
            {
                response.Errors.Add(processException);
                await MoveBlob(sourceContainer, errorContainer, blobItem.Name).ConfigureAwait(false);
            }
            else
            {
                response.TotalSuccessful++;
                await MoveBlob(sourceContainer, processedContainer, blobItem.Name).ConfigureAwait(false);
            }
        }

        return response;
    }
    
    private async Task<string> MoveBlob(
        BlobContainerClient srcContainer,
        BlobContainerClient dstContainer,
        string blobName)
    {
        var srcBlob = srcContainer.GetBlockBlobClient(blobName);
        var destBlob = dstContainer.GetBlockBlobClient(blobName);
        await destBlob.StartCopyFromUriAsync(srcBlob.Uri);
        await srcBlob.DeleteIfExistsAsync();
        return destBlob.Uri.ToString();
    }
}