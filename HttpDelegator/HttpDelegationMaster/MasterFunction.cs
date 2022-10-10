using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpDelegationMaster
{
    public class MasterFunction
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MasterFunction(IHttpClientFactory httpClientFactory)
        {
            ServiceRegistry.serviceRegistry.Enqueue("http://localhost:5123");
            ServiceRegistry.serviceRegistry.Enqueue("http://localhost:5246");
            _httpClientFactory = httpClientFactory;
        }

        [FunctionName("MasterFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            var downstreamService = (ServiceRegistry.serviceRegistry.Dequeue()).ToString();

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{downstreamService}/echo");

            return new OkObjectResult(new { From = downstreamService, Response = await response.Content.ReadAsStringAsync() });
        }
    }
}
