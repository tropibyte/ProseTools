using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace ProseTools
{
    public class SettingsManager
    {
        private static string GetUserSettingsFilePath()
        {
            string userName = GetCurrentUserName();
            string sanitizedUserName = userName.Replace(" ", "_").ToLower();
            return $"settings_{sanitizedUserName}.json";
        }

        private static string GetCurrentUserName()
        {
            try
            {
                string userName = Globals.ThisAddIn.Application.UserName;
                return string.IsNullOrWhiteSpace(userName) ? "default_user" : userName;
            }
            catch
            {
                return "default_user";
            }
        }

        public static User CurrentUser { get; private set; }

        static SettingsManager()
        {
            LoadSettings();
        }

        public static void LoadSettings()
        {
            string settingsFilePath = GetUserSettingsFilePath();
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    string json = File.ReadAllText(settingsFilePath);
                    CurrentUser = JsonConvert.DeserializeObject<User>(json) ?? new User(GetCurrentUserName());
                }
                catch (Exception)
                {
                    CurrentUser = new User(GetCurrentUserName());
                }
            }
            else
            {
                CurrentUser = new User(GetCurrentUserName());
            }
        }

        public static void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(CurrentUser, Formatting.Indented);
                File.WriteAllText(GetUserSettingsFilePath(), json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class User
    {
        public string UserName { get; set; }
        public AppSettings Settings { get; set; } = new AppSettings();

        public User(string userName)
        {
            UserName = userName;
        }
    }

    public class AppSettings
    {
        public string OpenAIKey { get; set; } = "";
        public string GeminiKey { get; set; } = "";
        public string CoPilotKey { get; set; } = "";
        public bool EnableLogging { get; set; } = true;
    }
}

