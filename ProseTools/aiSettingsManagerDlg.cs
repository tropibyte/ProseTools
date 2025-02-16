using System;
using System.Collections.Generic;
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
            // For demonstration, we simulate loading with some dummy data.
            systemGenAIConfigs = new List<GenAIConfig>()
            {
                new GenAIConfig { Name = "System AI 1", Username = "sys1" },
                new GenAIConfig { Name = "System AI 2", Username = "sys2" }
            };

            userGenAIConfigs = new List<GenAIConfig>()
            {
                new GenAIConfig { Name = "User AI A", Username = "userA" },
                new GenAIConfig { Name = "User AI B", Username = "userB" }
            };

            // Bind the lists to the list boxes.
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

        private void RefreshListBoxes()
        {
            // Refresh the bindings to reflect any changes.
            lstSystemGenAI.DataSource = null;
            lstSystemGenAI.DataSource = systemGenAIConfigs;
            lstSystemGenAI.DisplayMember = "Name";

            lstUserGenAI.DataSource = null;
            lstUserGenAI.DataSource = userGenAIConfigs;
            lstUserGenAI.DisplayMember = "Name";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Here you would save the settings (e.g., write to a file or update a config manager).
            // For example: SettingsManager.Save(systemGenAIConfigs, userGenAIConfigs);
            MessageBox.Show("Settings saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
