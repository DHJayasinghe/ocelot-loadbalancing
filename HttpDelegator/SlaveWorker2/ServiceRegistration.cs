using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace SlaveWorker1;

public class ServiceRegistration
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _serviceRegistrarUrl;
    private readonly string _myFQDN;

    public ServiceRegistration(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _serviceRegistrarUrl = configuration.GetSection("ServiceRegistrarUrl").Value;
        _myFQDN = configuration.GetSection("MyFQDN").Value;
    }

    [FunctionName("ServiceRegistration")]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        var httpClient = _httpClientFactory.CreateClient();
        _ = await httpClient.PostAsJsonAsync(_serviceRegistrarUrl, new { FQDN = _myFQDN });
    }
}