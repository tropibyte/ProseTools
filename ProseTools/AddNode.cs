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
    public partial class AddNodeDlg : Form
    {
        public string NodeTitle { get; private set; }
        public string NodeDetails { get; private set; }
        public string NodeNotes { get; private set; }
        public Dictionary<string, string> NodeAttributes { get; set; }

        public AddNodeDlg(string title, string details, string notes, Dictionary<string, string> attributes, string parentName)

        {
            InitializeComponent();
            txtTitle.Text = title;
            txtDetails.Text = details;
            txtNotes.Text = notes;
            Text = "Modify Node";
            labelParentNode.Text = "ParentNode: " + parentName;
            NodeAttributes = attributes;
        }

        public AddNodeDlg(string parentName)
        {
            InitializeComponent();
            Text = "Add Node";
            labelParentNode.Text = "ParentNode: " + parentName;
            NodeAttributes = new Dictionary<string, string>();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            NodeTitle = txtTitle.Text;
            NodeDetails = txtDetails.Text;
            NodeNotes = txtNotes.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnManageAttributes_Click(object sender, EventArgs e)
        {
            // Open the AttributeManagerDlg with current attributes.
            using (var dlg = new AttributeManagerDlg(this.NodeAttributes))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Update the NodeAttributes with changes from the manager.
                    this.NodeAttributes = dlg.Attributes;
                }
            }
        }
    }
}
