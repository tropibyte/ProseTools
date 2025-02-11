using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProseTools
{
    public class GenAIConfig
    {
        public string Name { get; set; }       // e.g., "OpenAI", "Gemini", "Co-Pilot", "Claude"
        public string Username { get; set; }
        public string Password { get; set; }   // or Secret Key / API Key
        public string AuthUrl { get; set; }    // endpoint URL, if relevant
        public string OAuthKey { get; set; }   // or any other token/credential needed
        public string ModelName { get; set; }   // version of GenAI to use

        // Optional: add any other service-specific fields or methods
    }
}
