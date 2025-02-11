using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class aiSettingsDlg : Form
    {
        internal string m_genAIName;
        public GenAIConfig Config { get; private set; }

        public aiSettingsDlg(string genAIName)
        {
            InitializeComponent();
            m_genAIName = genAIName;
        }

        private void aiSettingsDlg_Load(object sender, EventArgs e)
        {
            labelGenAI.Text = m_genAIName;
        }

        // OK button click event
        private void btnOk_Click(object sender, EventArgs e)
        {
            // Build the GenAIConfig using the values from the controls.
            Config = new GenAIConfig
            {
                Name = m_genAIName,
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text, // In a real app, consider secure storage!
                ApiKey = txtApiKey.Text.Trim(),
                AuthUrl = txtAuthUrl.Text.Trim(),
                ModelName = txtModelName.Text.Trim(),
                UseApiKey = chkUseApiKey.Checked
            };

            // Close the dialog with OK result.
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Cancel button click event
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
