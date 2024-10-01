using Azure;
using Azure.Storage.Files.Shares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Web;

namespace st10365052FunctionApp
{
    public static class FunctionPDF
    {
        [Function("UploadPDF")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("FunctionPDF");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string shareName = "st10365052fileshare";
            string fileName = req.Query["fileName"];

            if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("File name must be provided.");
                return response;
            }

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10365052demostorage;AccountKey=5fTakvNfnJG3Gm7ndmyFD1gno7X9MsKJbKiuSLZNYxdz6VPzTzH28MpyK/H8u/mZbRQ74C1OOAyO+AStQ/yUkg==;EndpointSuffix=core.windows.net";
            var shareServiceClient = new ShareServiceClient(connectionString);
            var shareClient = shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            using var stream = req.Body;
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteString("PDF uploaded");
            return okResponse;
        }
    }
}


