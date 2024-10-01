using System.Net;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace st10365052FunctionApp
{
    public class FunctionTable
    {
        private readonly ILogger _logger;

        public FunctionTable(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionTable>();
        }
        
        [Function("FunctionTable")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10365052demostorage;AccountKey=5fTakvNfnJG3Gm7ndmyFD1gno7X9MsKJbKiuSLZNYxdz6VPzTzH28MpyK/H8u/mZbRQ74C1OOAyO+AStQ/yUkg==;EndpointSuffix=core.windows.net";
            string tableName = "userTable";

            string name = req.Query["name"];
            string email = req.Query["email"];
            string password = req.Query["password"];
            string city = req.Query["city"];

            if (name.Contains(null) || email.Contains(null) || password.Contains(null) || city.Contains(null))
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.WriteString("Table name, partition key, row key, and data must be provided.");
                return response;
            }

            var serviceClient = new TableServiceClient(connectionString);
            var tableClient = serviceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();

            var customerEntity = new TableEntity("Customer", Guid.NewGuid().ToString())
                {
                    { "Name", name },
                    { "Email", email },
                    { "Password", password },
                    { "City", city }
                };

            await tableClient.AddEntityAsync(customerEntity);

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteString("Data added to table");
            return okResponse;
        }
        
    }
}
