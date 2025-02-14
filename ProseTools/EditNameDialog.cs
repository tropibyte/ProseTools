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
    public partial class EditNameDialog : Form
    {
        public string EditedName { get; private set; }

        public EditNameDialog(string currentName)
        {
            InitializeComponent();
            txtName.Text = currentName;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            EditedName = txtName.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
