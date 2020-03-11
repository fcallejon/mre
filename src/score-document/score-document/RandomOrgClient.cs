using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Function
{
    public class RandomOrgClient
    {
        private const string invokePath = "/json-rpc/2/invoke";
        private const string requestMediaType = "application/json";
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public RandomOrgClient(HttpClient client, string apiKey)
        {
            _apiKey = apiKey;
            _client = client;
        }

        public async Task<double> GetRandom()
        {
            // just for speed of coding
            var body = "{" +
            "    \"jsonrpc\": \"2.0\"," +
            "    \"method\": \"generateGaussians\"," +
            "    \"params\": " +
            "    {" +
            $"        \"apiKey\": \"{_apiKey}\"," +
            "        \"n\": 1," +
            "        \"mean\": 1," +
            "        \"standardDeviation\": 1," +
            "        \"significantDigits\": 8" +
            "    }," +
            $"    \"id\": {DateTime.UtcNow.Ticks}" +
            "}";

            var httpResponse = await _client.PostAsync(invokePath, new StringContent(body, Encoding.UTF8, requestMediaType));
            // we might want to add a retry functionality here
            // maybe polly policies in startup
            httpResponse.EnsureSuccessStatusCode();

            var raw = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RandomOrgResponse<double>>(raw);
            return response.Result.Random.Data.First();
        }

        public async Task<string> GetUUID()
        {
            var body = "{" +
            "    \"jsonrpc\": \"2.0\"," +
            "    \"method\": \"generateUUIDs\"," +
            "    \"params\": " +
            "    {" +
            $"        \"apiKey\": \"{_apiKey}\"," +
            "        \"n\": 1" +
            "    }," +
            $"    \"id\": {DateTime.UtcNow.Ticks}" +
            "}";

            var httpResponse = await _client.PostAsync(invokePath, new StringContent(body, Encoding.UTF8, requestMediaType));
            // we might want to add a retry functionality here
            // maybe polly policies
            httpResponse.EnsureSuccessStatusCode();

            var raw = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RandomOrgResponse<string>>(raw);
            return response.Result.Random.Data.First();
        }
    }
}
