using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class aiSettingsManagerDlg : Form
    {
        // Collections for system and user GenAI configurations.
        private List<GenAIConfig> systemGenAIConfigs;
        private List<GenAIConfig> userGenAIConfigs;

        public aiSettingsManagerDlg()
        {
            InitializeComponent();
            // Load the configurations.
            LoadGenAIConfigs();
        }

        private void LoadGenAIConfigs()
        {
            // Assume Settings.UserSettingsList is a List<UserSettings>
            // "SYSTEM" is reserved for system settings.

            // Find the system settings (username "SYSTEM")
            var systemSetting = Globals.ThisAddIn.Settings.UserSettingsList
                                .FirstOrDefault(us => us.Username.Equals("SYSTEM", StringComparison.OrdinalIgnoreCase));
            if (systemSetting == null)
            {
                // If not found, create and insert at the beginning.
                systemSetting = new UserSettings() { Username = "SYSTEM" };
                Globals.ThisAddIn.Settings.UserSettingsList.Insert(0, systemSetting);
            }
            systemGenAIConfigs = systemSetting.GenAIConfigs;

            // Get the current Windows username.
            string currentUsername = Environment.UserName;

            // Find user settings for the current user.
            var userSetting = Globals.ThisAddIn.Settings.UserSettingsList
                              .FirstOrDefault(us => us.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase));
            if (userSetting == null)
            {
                // If not found, create new settings for this user.
                userSetting = new UserSettings() { Username = currentUsername };
                Globals.ThisAddIn.Settings.UserSettingsList.Add(userSetting);
            }
            userGenAIConfigs = userSetting.GenAIConfigs;

            RefreshListBoxes();
        }

        private void RefreshListBoxes()
        {
            lstSystemGenAI.DataSource = null;
            lstSystemGenAI.DataSource = systemGenAIConfigs;
            lstSystemGenAI.DisplayMember = "Name";

            lstUserGenAI.DataSource = null;
            lstUserGenAI.DataSource = userGenAIConfigs;
            lstUserGenAI.DisplayMember = "Name";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Determine which tab is active and get the selected config.
            GenAIConfig selectedConfig = null;
            if (tabControlGenAI.SelectedTab == tabSystem)
            {
                selectedConfig = lstSystemGenAI.SelectedItem as GenAIConfig;
            }
            else if (tabControlGenAI.SelectedTab == tabUser)
            {
                selectedConfig = lstUserGenAI.SelectedItem as GenAIConfig;
            }

            if (selectedConfig == null)
            {
                MessageBox.Show("Please select a GenAI configuration to edit.",
                                "Edit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open the individual configuration dialog.
            // Assume you have a GenAISettingsDlg that accepts a GenAIConfig.
            using (var dlg = new GenAISettingsDlg(selectedConfig))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // The dialog updates the config; refresh the list boxes.
                    RefreshListBoxes();
                }
            }
        }

        // Button to add a new blank GenAI configuration.
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            // Create a new, blank GenAIConfig.
            GenAIConfig newConfig = new GenAIConfig { Name = "New AI" };

            // For example, add new configs to the user settings.
            userGenAIConfigs.Add(newConfig);
            RefreshListBoxes();

            // Optionally, open the configuration dialog immediately for the new config.
            using (var dlg = new GenAISettingsDlg(newConfig))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshListBoxes();
                }
            }
        }

        // Button to delete the selected GenAI configuration.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            GenAIConfig selectedConfig = null;
            List<GenAIConfig> list = null;
            if (tabControlGenAI.SelectedTab == tabSystem)
            {
                selectedConfig = lstSystemGenAI.SelectedItem as GenAIConfig;
                list = systemGenAIConfigs;
            }
            else if (tabControlGenAI.SelectedTab == tabUser)
            {
                selectedConfig = lstUserGenAI.SelectedItem as GenAIConfig;
                list = userGenAIConfigs;
            }

            if (selectedConfig == null)
            {
                MessageBox.Show("Please select a GenAI configuration to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected configuration?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                list.Remove(selectedConfig);
                RefreshListBoxes();
            }
        }

        private void AddSystemStandardConfig(GenAIConfig standardConfig, bool bConfigure)
        {
            // Check if configuration with the same name already exists in the user list.
            if (systemGenAIConfigs.Any(cfg => cfg.Name.Equals(standardConfig.Name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Configuration for {standardConfig.Name} already exists.", "Add Standard Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add the standard configuration to the system list.
            systemGenAIConfigs.Add(standardConfig);
            RefreshListBoxes();

            if(bConfigure)
            {
                // Optionally open the configuration dialog immediately to allow the user to fill in details.
                using (var dlg = new GenAISettingsDlg(standardConfig))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshListBoxes();
                    }
                }
            }
        }

        // Adds a standard configuration to the user list (if it doesn't already exist).
        private void AddStandardConfigToUser(GenAIConfig standardConfig, bool bConfigure)
        {
            // Check if configuration with the same name already exists in the user list.
            if (userGenAIConfigs.Any(cfg => cfg.Name.Equals(standardConfig.Name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Configuration for {standardConfig.Name} already exists.", "Add Standard Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            userGenAIConfigs.Add(standardConfig);
            RefreshListBoxes();

            if(bConfigure)
            {
                // Optionally open the configuration dialog immediately to allow the user to fill in details.
                using (var dlg = new GenAISettingsDlg(standardConfig))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshListBoxes();
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Update the system GenAI configurations.
            var systemSettings = Globals.ThisAddIn.Settings.UserSettingsList
                                    .FirstOrDefault(us => us.Username.Equals("SYSTEM", StringComparison.OrdinalIgnoreCase));
            if (systemSettings != null)
            {
                systemSettings.GenAIConfigs = systemGenAIConfigs;
            }
            else
            {
                // Should not occur if load was done correctly.
                systemSettings = new UserSettings { Username = "SYSTEM", GenAIConfigs = systemGenAIConfigs };
                Globals.ThisAddIn.Settings.UserSettingsList.Insert(0, systemSettings);
            }

            // Update the current user's GenAI configurations.
            string currentUsername = Environment.UserName;
            var userSettings = Globals.ThisAddIn.Settings.UserSettingsList
                                  .FirstOrDefault(us => us.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase));
            if (userSettings != null)
            {
                userSettings.GenAIConfigs = userGenAIConfigs;
            }
            else
            {
                // If the user settings don't exist, add a new entry.
                userSettings = new UserSettings { Username = currentUsername, GenAIConfigs = userGenAIConfigs };
                Globals.ThisAddIn.Settings.UserSettingsList.Add(userSettings);
            }

            // Save the updated settings.
            Globals.ThisAddIn.SaveSettings();

            MessageBox.Show("Settings saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnStdFour_Click(object sender, EventArgs e)
        {
            using (var Std4Dlg = new AddStandard4GenAIDlg())
            {
                if (Std4Dlg.ShowDialog() == DialogResult.OK)
                {
                    bool bConfigure = Std4Dlg.ConfigureSetting();
                    if (tabControlGenAI.SelectedTab == tabSystem)
                    {
                        // System tab is selected.
                        // Add the standard configurations here.
                        if (Std4Dlg.WantsChatGPT())
                            AddSystemStandardConfig(new GenAIConfig
                            {
                                Name = "ChatGPT",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.openai.com/v1/engines/davinci-codex/completions",
                                ModelName = "davinci-codex",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsCopilot())
                            AddSystemStandardConfig(new GenAIConfig
                            {
                                Name = "Co-Pilot",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.copilot.com/auth",
                                ModelName = "default",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsGemini())
                            AddSystemStandardConfig(new GenAIConfig
                            {
                                Name = "Gemini",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.gemini.com/auth",
                                ModelName = "default",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsClaude())
                            AddSystemStandardConfig(new GenAIConfig
                            {
                                Name = "Claude",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.anthropic.com/claude",
                                ModelName = "claude-v1",
                                UseApiKey = true
                            }, bConfigure);
                    }
                    else if (tabControlGenAI.SelectedTab == tabUser)
                    {
                        // User tab is selected.
                        // Add the standard configurations here.
                        if (Std4Dlg.WantsChatGPT())
                            AddStandardConfigToUser(new GenAIConfig
                            {
                                Name = "ChatGPT",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.openai.com/v1/engines/davinci-codex/completions",
                                ModelName = "davinci-codex",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsCopilot())
                            AddStandardConfigToUser(new GenAIConfig
                            {
                                Name = "Co-Pilot",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.copilot.com/auth",
                                ModelName = "default",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsGemini())
                            AddStandardConfigToUser(new GenAIConfig
                            {
                                Name = "Gemini",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.gemini.com/auth",
                                ModelName = "default",
                                UseApiKey = true
                            }, bConfigure);
                        if (Std4Dlg.WantsClaude())
                            AddStandardConfigToUser(new GenAIConfig
                            {
                                Name = "Claude",
                                // Set default values here.
                                ApiKey = "",
                                AuthUrl = "https://api.anthropic.com/claude",
                                ModelName = "claude-v1",
                                UseApiKey = true
                            }, bConfigure);
                    }
                    RefreshListBoxes();
                }
            }
        }
    }
}
