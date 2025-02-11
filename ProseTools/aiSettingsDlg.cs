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
        public aiSettingsDlg(string genAIName)
        {
            InitializeComponent();
            m_genAIName = genAIName;
        }

        private void aiSettingsDlg_Load(object sender, EventArgs e)
        {
            labelGenAI.Text = m_genAIName;
        }
    }
}
