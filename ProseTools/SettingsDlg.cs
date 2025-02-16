using ProseTools;
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
    public partial class SettingsDlg : Form
    {
        public ProseToolsSettings Settings { get; set; }

        public SettingsDlg(ProseToolsSettings settings)
        {
            InitializeComponent();
            // Copy or reference settings (depending on whether you want to work on a clone).
            Settings = settings;

            LoadSettings();
        }

        public SettingsDlg()
        {
            InitializeComponent();
            btnOK.Enabled = false;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void LoadSettings()
        {
        }
    }
}

