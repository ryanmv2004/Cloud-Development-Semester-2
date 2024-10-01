using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using cldv2APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace cldv2APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image != null) 
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                stream.Position = 0;

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(stream), "image", image.FileName);

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync("https://st10365052cldva2.azurewebsites.net/api/FunctionImages?code=WEoP_u6iH26GmVg4T88WN1j2lennP7n_d15iMw3SiqBuAzFumO3MOA%3D%3D" + image.FileName, content);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPDF(IFormFile pdf)
        {
            if (pdf == null || pdf.Length == 0)
            {
                return BadRequest("No PDF file uploaded.");
            }

            var client = _httpClientFactory.CreateClient();
            var requestContent = new MultipartFormDataContent();
            var fileContent = new StreamContent(pdf.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(pdf.ContentType);
            requestContent.Add(fileContent, "pdf", pdf.FileName);

            var functionUrl = "https://st10365052cldva2.azurewebsites.net/api/UploadPDF?code=d3jVepHs_3juv7wVnfky-STCl969xG1GGcGjHNzxvVPEAzFu1S9KSQ%3D%3D"; // Replace with your actual function URL
            var response = await client.PostAsync(functionUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessTransaction()
        {
            string functionUrl = "https://st10365052cldva2.azurewebsites.net/api/FunctionTransaction?code=OrJEW3Ur_FrToUYwAkDmack_veRgaOWuZ2-NFAAHWbW3AzFuzrUMqg%3D%3D";
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(functionUrl, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SubmitUserDetails(string name, string email, string password, string city)
        {
            var functionUrl = "https://st10365052cldva2.azurewebsites.net/api/FunctionTable?code=-2Q97uC-PXbddeFoLQpMEc6CUnTzmrCTEgYeWjYbjQb2AzFuPg-C7A%3D%3D";

            using (var client = new HttpClient())
            {
                var requestBody = new Dictionary<string, string>
        {
            { "name", name },
            { "email", email },
            { "password", password },
            { "city", city }
        };

                var content = new FormUrlEncodedContent(requestBody);
                var response = await client.PostAsync(functionUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
        }

    }
}
