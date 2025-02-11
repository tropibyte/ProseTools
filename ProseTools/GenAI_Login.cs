using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class GenAI_Login : Form
    {
        private List<GenAIConfig> _genAIConfigs;

        public GenAI_Login()
        {
            InitializeComponent();
            LoadGenAIConfigs();
        }

        private void LoadGenAIConfigs()
        {
            // In a real scenario, you might load this from a JSON file, 
            // user settings, or a database. For now, let's hard-code.
            _genAIConfigs = new List<GenAIConfig>
            {
                new GenAIConfig
                {
                    Name = "OpenAI",
                    Username = "",
                    Password = "",
                    AuthUrl = "https://api.openai.com/v1/chat/completions",
                    OAuthKey = "sk-XXXXXX"
                },
                new GenAIConfig
                {
                    Name = "Gemini",
                    Username = "",
                    Password = "",
                    AuthUrl = "https://api.gemini.com",
                    OAuthKey = ""
                },
                new GenAIConfig
                {
                    Name = "Co-Pilot",
                    Username = "",
                    Password = "",
                    AuthUrl = "https://github.com/features/copilot",
                    OAuthKey = ""
                },
                new GenAIConfig
                {
                    Name = "Claude",
                    Username = "",
                    Password = "",
                    AuthUrl = "https://api.anthropic.com",
                    OAuthKey = ""
                }
            };

            // Bind them to our combo box
            cbGenAI.DataSource = _genAIConfigs;
            cbGenAI.DisplayMember = "Name";
        }

        private void cbGenAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Fill the text boxes with the info from the selected GenAI
            var selected = cbGenAI.SelectedItem as GenAIConfig;
            if (selected != null)
            {
                txtUsername.Text = selected.Username;
                txtPassword.Text = selected.Password;
                txtAuthUrl.Text = selected.AuthUrl;
                txtOAuthKey.Text = selected.OAuthKey;
            }
        }

        private void btnAddNewAI_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Retrieve user input
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            var selectedConfig = cbGenAI.SelectedItem as GenAIConfig;
            if (selectedConfig == null)
            {
                MessageBox.Show("Please select a valid AI provider.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update selected configuration with current UI values.
            selectedConfig.Username = username;
            selectedConfig.Password = password;
            selectedConfig.AuthUrl = txtAuthUrl.Text.Trim();
            // Note: txtOAuthKey.Text will later be updated if authentication returns a new token.

            bool success = false;

            try
            {
                // Ensure TLS 1.2 is used (this helps resolve the SSL/TLS error you encountered)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    // Build the JSON payload. The exact structure depends on the API.
                    var authData = new
                    {
                        username = selectedConfig.Username,
                        password = selectedConfig.Password
                    };

                    string jsonBody = JsonConvert.SerializeObject(authData);
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Send the authentication request.
                    HttpResponseMessage response = await client.PostAsync(selectedConfig.AuthUrl, content);

                    // If we get 401 or a specific error, try to handle token expiration or invalid credentials.
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Optionally, attempt to refresh the token if your API provides a refresh token flow.
                        bool tokenRefreshed = await RefreshTokenAsync(selectedConfig);
                        if (tokenRefreshed)
                        {
                            // Retry the request with the new token (this is a simplified example).
                            content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(selectedConfig.AuthUrl, content);
                        }
                        else
                        {
                            MessageBox.Show("Authentication failed: Unauthorized.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        // Parse the response. We assume the JSON contains a field named "token".
                        dynamic result = JsonConvert.DeserializeObject(responseContent);

                        if (result != null && result.token != null)
                        {
                            string token = result.token;
                            selectedConfig.OAuthKey = token;
                            txtOAuthKey.Text = token;
                            success = true;
                        }
                        else
                        {
                            MessageBox.Show("Authentication succeeded but no token was returned.",
                                            "Warning",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Login failed: {response.ReasonPhrase}\nDetails: {errorDetails}",
                                        "Login Failed",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"A network error occurred during login: {httpEx.Message}",
                                "Network Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            if (success)
            {
                MessageBox.Show("Login successful! You can now access AI features.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// This stub represents a potential method for refreshing a token if it is expired.
        /// You would adjust the implementation based on your API's refresh token flow.
        /// </summary>
        /// <param name="config">The current GenAI configuration</param>
        /// <returns>true if the token was successfully refreshed; otherwise, false.</returns>
        private async Task<bool> RefreshTokenAsync(GenAIConfig config)
        {
            try
            {
                // For demonstration, assume there's an endpoint to refresh the token
                // and that the API requires the current OAuthKey (or a dedicated refresh token)
                // along with username/password, etc.
                // The code below is highly simplified and must be adapted to your API.

                string refreshUrl = config.AuthUrl + "/refresh";  // Adjust the URL as needed

                var refreshData = new
                {
                    username = config.Username,
                    password = config.Password,
                    token = config.OAuthKey   // This might be a separate refresh token in your API.
                };

                string jsonBody = JsonConvert.SerializeObject(refreshData);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    // Ensure TLS 1.2 for the refresh call as well.
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = await client.PostAsync(refreshUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseContent);

                        if (result != null && result.token != null)
                        {
                            string newToken = result.token;
                            config.OAuthKey = newToken;
                            txtOAuthKey.Text = newToken;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception as needed.
                MessageBox.Show($"Token refresh failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
    }
}
