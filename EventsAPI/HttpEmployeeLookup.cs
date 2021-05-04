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
            /// TODO: Look up on how 'EmployeeApiUrl' is a 'member' of value...
            /// public interface IOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> where TOptions : class
        }
}
}