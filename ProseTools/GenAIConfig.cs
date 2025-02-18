using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{
    public class GenAIConfig
    {
        // Service name: e.g., "ChatGPT", "Gemini", "Co-Pilot", "Claude"
        public string Name { get; set; }

        // Credentials for username/password authentication
        public string Username { get; set; }
        public string Password { get; set; }

        // API key for authentication
        public string ApiKey { get; set; }

        // If true, use the ApiKey; if false, use username/password
        public bool UseApiKey { get; set; }

        public string OAuthKey { get; set; }   // or any other token/credential needed

        // The authentication endpoint URL for the service
        public string AuthUrl { get; set; }

        // The model (version) to use (if applicable)
        public string ModelName { get; set; }

        /// <summary>
        /// Connects to the GenAI service using either API key or username/password,
        /// and returns an access token.
        /// </summary>
        /// <returns>The access token returned by the GenAI service.</returns>
        public async Task<string> ConnectAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                object requestData;
                if (UseApiKey)
                {
                    // If using API key authentication.
                    requestData = new
                    {
                        apiKey = this.ApiKey,
                        model = this.ModelName
                    };
                }
                else
                {
                    // Use username and password for authentication.
                    requestData = new
                    {
                        username = this.Username,
                        password = this.Password,
                        model = this.ModelName
                    };
                }

                // Serialize the request data to JSON.
                string jsonData = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the POST request to the authentication endpoint.
                HttpResponseMessage response = await client.PostAsync(this.AuthUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to connect to {Name}: {response.ReasonPhrase}");
                }

                // Read and deserialize the response.
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseJson);

                // Assume the response JSON has a "token" property.
                string token = result.token;
                return token;
            }
        }

        /// <summary>
        /// Serializes this GenAIConfig to an XML element.
        /// </summary>
        public XElement ToXML()
        {
            XNamespace ns = "urn:prosetools:prosetoolssettings"; // Adjust as needed
            return new XElement(ns + "GenAIConfig",
                new XElement(ns + "Name", Name),
                new XElement(ns + "Username", Username),
                new XElement(ns + "Password", Password),
                new XElement(ns + "ApiKey", ApiKey),
                new XElement(ns + "UseApiKey", UseApiKey),
                new XElement(ns + "OAuthKey", OAuthKey),
                new XElement(ns + "AuthUrl", AuthUrl),
                new XElement(ns + "ModelName", ModelName)
            );
        }

        /// <summary>
        /// Deserializes a GenAIConfig from the provided XML element.
        /// </summary>
        public static GenAIConfig FromXML(XElement element)
        {
            XNamespace ns = "urn:prosetools:prosetoolssettings";
            return new GenAIConfig
            {
                Name = element.Element(ns + "Name")?.Value,
                Username = element.Element(ns + "Username")?.Value,
                Password = element.Element(ns + "Password")?.Value,
                ApiKey = element.Element(ns + "ApiKey")?.Value,
                UseApiKey = bool.Parse(element.Element(ns + "UseApiKey")?.Value ?? "false"),
                OAuthKey = element.Element(ns + "OAuthKey")?.Value,
                AuthUrl = element.Element(ns + "AuthUrl")?.Value,
                ModelName = element.Element(ns + "ModelName")?.Value
            };
        }
    }
}
