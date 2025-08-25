using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SimpleBank.Client.Repository.Interface;

namespace SimpleBank.Client.Repository.Implementation
{
    public class GenericHttpClients : IGenericHttpClients
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public GenericHttpClients(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _client.BaseAddress = new Uri(_configuration["ApiClientConfiguration:ClientUrl"] ?? throw new ArgumentNullException("ApiBaseUrl is not configured."));
        }

        // Method to retrieve token from API
        private async Task<string> GetToken()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Token/GetToken");

            Dictionary<string, string> dynamicRequest = new Dictionary<string, string>();
            //dynamicRequest.Add("username", _configuration["ApiClientConfiguration:Username"] ?? throw new ArgumentNullException("Username is not configured."));
            dynamicRequest.Add("username", "admin@gmail.com");
            dynamicRequest.Add("password", "admin@123");

            var content = new StringContent(JsonConvert.SerializeObject(dynamicRequest));
            request.Content = content;
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            
            var response = await _client.SendAsync(request);
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve token from API.");
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                return result.ToString();
            }

            throw new ApplicationException($"Token fetch failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
        }

        // Generic methods for HTTP operations

        // Sends a GET request to the specified address and returns the response deserialized to type T
        public async Task<T> GetAsync<T>(string address)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, address);

            string token = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result) ?? throw new ApplicationException("Deserialization failed. Result is null.");
        }

        // Sends a POST request to the specified address with the dynamic request body and returns the response deserialized to type T
        public async Task<T> PostAsync<T>(string address, dynamic dynamicRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, address);
            var content = new StringContent(JsonConvert.SerializeObject(dynamicRequest));
            request.Content = content;
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            string token = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result) ?? throw new ApplicationException("Deserialization failed. Result is null.");

        }

        // Sends a PUT request to the specified address with the dynamic request body and returns the response deserialized to type T
        public async Task<T> PutAsync<T>(string address, dynamic dynamicRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, address);
            var content = new StringContent(JsonConvert.SerializeObject(dynamicRequest));
            request.Content = content;
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            string token = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
            }
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result) ?? throw new ApplicationException("Deserialization failed. Result is null.");
        }

        // Sends a DELETE request to the specified address and returns the response deserialized to type T
        public async Task<T> DeleteAsync<T>(string address)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, address);

            string token = await GetToken();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Request failed. Status: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result) ?? throw new ApplicationException("Deserialization failed. Result is null.");
        }
    }
}
