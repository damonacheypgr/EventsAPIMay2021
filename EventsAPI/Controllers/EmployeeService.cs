using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventsAPI.Controllers
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _client;

        public EmployeeService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:1337/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.5));
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, "employees/" + id);
            var response = await _client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
