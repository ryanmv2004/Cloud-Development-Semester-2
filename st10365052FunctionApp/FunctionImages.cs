using System.Net;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace st10365052FunctionApp
{
    public class FunctionImages
    {
        private readonly ILogger _logger;

        public FunctionImages(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionImages>();
        }

        [Function("FunctionImages")]
        public async Task<HttpResponseData> UploadBlob(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function to upload a blob processed a request.");

            string containerName = "st10365052container";
            string blobName = req.Query["blobName"];
                
            if (string.IsNullOrEmpty(blobName) || string.IsNullOrEmpty(containerName)) 
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Blob name and Container name must be provided.");
                return response;
            }

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=st10365052demostorage;AccountKey=5fTakvNfnJG3Gm7ndmyFD1gno7X9MsKJbKiuSLZNYxdz6VPzTzH28MpyK/H8u/mZbRQ74C1OOAyO+AStQ/yUkg==;EndpointSuffix=core.windows.net";
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            using var stream = req.Body;
            await blobClient.UploadAsync(stream, true);

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteString("Inage Uploaded");
            return okResponse;
        }

    }
}
