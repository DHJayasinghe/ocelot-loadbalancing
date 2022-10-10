using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections;

namespace HttpDelegationMaster
{
    public static class ServiceRegistry
    {
        public static readonly Queue serviceRegistry = new();

        [FunctionName("RegisterService")]
        public static async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "service-registry")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string fqdn = data?.FQDN ?? "";

            log.LogInformation("Service Registry function processed a request from Slave {0}", fqdn);

            if (!serviceRegistry.Contains(fqdn))
            {
                serviceRegistry.Enqueue(fqdn);
            };

            return new OkResult();
        }

        [FunctionName("RegisteredService")]
        public static IActionResult List([HttpTrigger(AuthorizationLevel.Function, "get", Route = "service-registry")] HttpRequest req)
        {
            return new OkObjectResult(serviceRegistry.ToArray());
        }
    }
}
