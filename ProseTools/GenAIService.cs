using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ProseTools
{
    public class GenAIService
    {
        // This method sends the prompt to the AI service and returns the response.
        public async Task<string> GetAIResponseAsync(string prompt, GenAIConfig config)
        {
            // You may want to configure the base URL and endpoint elsewhere.
            var options = new RestClientOptions("https://openrouter.ai/api/v1/chat/completions");
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Post);

            // Add headers.
            request.AddHeader("Authorization", $"Bearer {config.ApiKey}");
            request.AddHeader("Content-Type", "application/json");

            // Build the request body.
            var requestBody = new
            {
                model = config.ModelName, // you can use config.ModelName here
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                }
            };

            request.AddJsonBody(requestBody);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                return jsonResponse["choices"]?[0]?["message"]?["content"]?.ToString() ?? "No response.";
            }
            else
            {
                return $"Error: {response.StatusCode}\n{response.Content}\n{response.ErrorMessage}";
            }
        }
    }
}

