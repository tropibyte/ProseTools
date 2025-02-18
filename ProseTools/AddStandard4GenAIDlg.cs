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
    public partial class AddStandard4GenAIDlg : Form
    {
        bool bChatGPT = false;
        bool bCopilot = false;
        bool bGemini = false;
        bool bClaude = false;

        public AddStandard4GenAIDlg()
        {
            InitializeComponent();
        }

        internal bool WantsChatGPT()
        {
            return bChatGPT;
        }

        internal bool WantsCopilot()
        {
            return bCopilot;
        }

        internal bool WantsGemini()
        {
            return bGemini;
        }

        internal bool WantsClaude()
        {
            return bClaude;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            bChatGPT = chkboxChatGPT.Checked;
            bCopilot = chkBoxCoPilot.Checked;
            bGemini = chkboxGemini.Checked;
            bClaude = chkBoxClaude.Checked;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            bChatGPT = false;
            bCopilot = false;
            bGemini = false;
            bClaude = false;
            this.Close();
        }

        internal bool ConfigureSetting()
        {
            return false;
        }
    }
}
