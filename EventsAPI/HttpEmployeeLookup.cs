using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EventsAPI
{
    internal class HttpEmployeeLookup : ILookupEmployees
    {
        private readonly HttpClient _client;
        public HttpEmployeeLookup(HttpClient client, IOptions<ApiOptions> config)
        {
            _client = client;
            var url = config.Value.EmployeeApiUrl;
        }
    }
}