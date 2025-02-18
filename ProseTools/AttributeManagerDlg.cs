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
    public partial class AttributeManagerDlg : Form
    {
        // This property holds the key/value pairs for attributes.
        public Dictionary<string, string> Attributes { get; private set; }

        public AttributeManagerDlg(Dictionary<string, string> currentAttributes)
        {
            InitializeComponent();
            // Make a copy of the current attributes so that changes are only committed if the user clicks OK.
            Attributes = new Dictionary<string, string>(currentAttributes);
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            lstAttributes.Items.Clear();
            foreach (var kvp in Attributes)
            {
                lstAttributes.Items.Add($"{kvp.Key}: {kvp.Value}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text.Trim();
            string value = txtValue.Text.Trim();
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Please enter an attribute name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prevent adding the "ProseType" attribute.
            if (key.Equals("ProseType", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The attribute 'ProseType' cannot be added or edited.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add or update the attribute.
            Attributes[key] = value;
            LoadAttributes();
            txtKey.Clear();
            txtValue.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstAttributes.SelectedIndex >= 0)
            {
                string item = lstAttributes.SelectedItem.ToString();
                int colonIndex = item.IndexOf(":");
                if (colonIndex > 0)
                {
                    string key = item.Substring(0, colonIndex).Trim();

                    // Prevent editing of "ProseType".
                    if (key.Equals("ProseType", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("The attribute 'ProseType' cannot be edited.", "Edit Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string value = item.Substring(colonIndex + 1).Trim();
                    txtKey.Text = key;
                    txtValue.Text = value;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAttributes.SelectedIndex >= 0)
            {
                string item = lstAttributes.SelectedItem.ToString();
                int colonIndex = item.IndexOf(":");
                if (colonIndex > 0)
                {
                    string key = item.Substring(0, colonIndex).Trim();

                    // Prevent deleting "ProseType".
                    if (key.Equals("ProseType", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("The attribute 'ProseType' cannot be deleted.", "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (Attributes.ContainsKey(key))
                    {
                        Attributes.Remove(key);
                        LoadAttributes();
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lstAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }

        private void btnSaveValueChange_Click(object sender, EventArgs e)
        {
            if (lstAttributes.SelectedIndex >= 0)
            {
                string item = lstAttributes.SelectedItem.ToString();
                int colonIndex = item.IndexOf(":");
                if (colonIndex > 0)
                {
                    string key = item.Substring(0, colonIndex).Trim();

                    // Prevent editing of "ProseType".
                    if (key.Equals("ProseType", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("The attribute 'ProseType' cannot be edited.", "Edit Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (Attributes.ContainsKey(key))
                    {
                        // The key exists.
                        Attributes[key] = txtValue.Text;
                        LoadAttributes();
                    }
                    else
                    {
                        // The key does not exist.
                        MessageBox.Show("The selected attribute does not exist.  You must click 'Add' to create it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnResetValueChange_Click(object sender, EventArgs e)
        {
            string key = txtKey.Text;
            if (Attributes.ContainsKey(key))
            {
                txtValue.Text = Attributes[key];
            }
            else
            {
                txtValue.Text = "";
            }
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            if(Attributes.ContainsKey(txtKey.Text))
            {
                btnAdd.Text = "Update";
            }
            else
            {
                btnAdd.Text = "Add";
            }
        }
    }
}
