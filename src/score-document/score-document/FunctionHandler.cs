using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Function
{
    public class FunctionHandler
    {
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.random.org/")
            };
            try
            {
                var witheSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

                var reader = new StreamReader(request.Body);
                var requestBody = await reader.ReadToEndAsync();

                Console.WriteLine($"raw: {requestBody}");

                var parsedRequest = JsonConvert.DeserializeObject<RequestBody>(requestBody);

                Console.WriteLine($"parsedRequest = {parsedRequest} | documents.Length = {parsedRequest.Documents.Length}");

                var client = new RandomOrgClient(httpClient, Environment.GetEnvironmentVariable("RANDOM_ORG_API_KEY"));

                var result = new dynamic[parsedRequest.Documents.Length];
                for (var index = 0; index < parsedRequest.Documents.Length; index++)
                {
                    Console.WriteLine($"'{parsedRequest.Documents[index]}' ...");
                    var score = (await client.GetRandom()) * witheSpaceRegex.Replace(parsedRequest.Documents[index], string.Empty).Length;
                    var id = await client.GetUUID();
                    result[index] = new { id, score };
                    Console.WriteLine($"'{parsedRequest.Documents[index]}' = ({id}, {score})");
                }

                return (200, JsonConvert.SerializeObject(result));
            }
            catch(Exception ex)
            {
                return (200, ex.Message + "\r\n" + ex.StackTrace);
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }

    public class RequestBody
    {
        [JsonProperty("documents")]
        public string[] Documents { get; set; }
    }
}
