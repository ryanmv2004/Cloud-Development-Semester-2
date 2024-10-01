using System.Net;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace st10365052FunctionApp
{
    public class FunctionTransaction
    {
        private readonly ILogger _logger;

        public FunctionTransaction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionTransaction>();
        }

        [Function("FunctionTransaction")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            string queueName = "st10365052queue";
            string message = "Order has been Processed";

            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Table name, partition key, row key, and data must be provided.");
                return response;
            }

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10365052demostorage;AccountKey=5fTakvNfnJG3Gm7ndmyFD1gno7X9MsKJbKiuSLZNYxdz6VPzTzH28MpyK/H8u/mZbRQ74C1OOAyO+AStQ/yUkg==;EndpointSuffix=core.windows.net";

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteString("Message added to queue");
            return okResponse;
        }
    }
}
