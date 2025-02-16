using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace ProseTools
{
    public partial class InputDialog : Form
    {
        public string InputValue { get; private set; }

        public InputDialog(string prompt, string title, string defaultValue = "", bool multiline = false)
        {
            InitializeComponent();
            this.Text = title;
            lblPrompt.Text = prompt;

            txtInput.Text = defaultValue;

            if (multiline)
            {
                // Allow resizing and set appropriate anchors
                this.FormBorderStyle = FormBorderStyle.Sizable;
                txtInput.Multiline = true;
                txtInput.Height = 100; // Adjust as needed

                // Anchor the input textbox to all four sides
                txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else
            {
                // Fixed size dialog: no resizing allowed
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                txtInput.Multiline = false;
                txtInput.Height = 25;

                // Anchor the textbox to left, top, and right (but not bottom)
                txtInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }

            // Set anchoring for OK and Cancel buttons to maintain margins.
            // OK button remains at bottom-left.
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            // Cancel button remains at bottom-right.
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InputValue = txtInput.Text;
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
