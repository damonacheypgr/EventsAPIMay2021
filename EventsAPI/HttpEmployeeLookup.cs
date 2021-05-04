using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventsAPI
{
    internal class HttpEmployeeLookup : ILookupEmployees
    {
        private readonly HttpClient _client;
        public HttpEmployeeLookup(HttpClient client, IOptions<ApiOptions> config)
        {
            _client = client;
            var url = config.Value.EmployeeApiUrl;
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", .05));
        }

        public async Task<bool> CheckEmployeeIsActive(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, "employees/" + id);
            var response = await _client.SendAsync(request);

            return response.IsSuccessStatusCode; // 404 means that isn't an employee anymore, a 200 is.
        }
    }
}