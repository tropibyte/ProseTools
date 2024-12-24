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
    public partial class OutlineForm : Form
    {
        // Minimum form size
        private int initialFormWidth;
        private int initialFormHeight;

        // Control initial sizes for ratio calculations
        private int treeViewInitialWidth;
        private int richTextBoxInitialWidth;
        private int treeViewInitialHeight;
        private int richTextBoxInitialHeight;

        // Ratio used so TreeView and RichTextBox expand proportionally
        private float treeToRichWidthRatio;

        // Horizontal gap between TreeView and RichTextBox
        private int gapTreeViewToRichTextBox;

        // Distance from bottom edge to bottom-row buttons
        private int buttonBottomMargin;

        // Distances from left edge of the RichTextBox to each button
        private int addButtonLeftOffset;
        private int editButtonLeftOffset;
        private int deleteButtonLeftOffset;
        private int scanButtonLeftOffset;

        // Distances from bottom edge for TreeView and RichTextBox
        private int treeViewBottomMargin;
        private int richTextBoxBottomMargin;

        // Distances from the right edge for Ok/Cancel
        private int okButtonRightMargin;
        private int cancelButtonRightMargin;

        private int addButtonWidth;

        public OutlineForm()
        {
            InitializeComponent();
        }

        private void OutlineForm_Load(object sender, EventArgs e)
        {
            // 1) Capture the initial form dimensions
            initialFormWidth = this.Width;
            initialFormHeight = this.Height;

            // 2) Capture the initial widths and heights of TreeView and RichTextBox
            treeViewInitialWidth = treeView1.Width;
            richTextBoxInitialWidth = richTextBoxDetails.Width;

            treeViewInitialHeight = treeView1.Height;
            richTextBoxInitialHeight = richTextBoxDetails.Height;

            // 3) Calculate the ratio so TreeView & RichTextBox expand proportionally in the horizontal dimension
            float combinedWidth = treeView1.Width + (float)richTextBoxDetails.Width;
            treeToRichWidthRatio = treeView1.Width / combinedWidth;

            // 4) Capture the gap between TreeView and RichTextBox
            gapTreeViewToRichTextBox = richTextBoxDetails.Left - (treeView1.Left + treeView1.Width);

            // 5) Capture the distance from the bottom edge to the bottom row of buttons
            buttonBottomMargin = this.ClientSize.Height - buttonAdd.Bottom;

            // 6) Capture each button’s horizontal offset from the RichTextBox’s left edge
            addButtonLeftOffset = buttonAdd.Left - richTextBoxDetails.Left;
            editButtonLeftOffset = buttonEdit.Left - richTextBoxDetails.Left;
            deleteButtonLeftOffset = buttonDelete.Left - richTextBoxDetails.Left;
            scanButtonLeftOffset = buttonScan.Left - richTextBoxDetails.Left;

            // 7) Capture bottom margins for TreeView and RichTextBox
            treeViewBottomMargin = this.ClientSize.Height - (treeView1.Top + treeView1.Height);
            richTextBoxBottomMargin = this.ClientSize.Height - (richTextBoxDetails.Top + richTextBoxDetails.Height);

            // 8) Capture distances from the right edge for Ok and Cancel
            okButtonRightMargin = this.ClientSize.Width - Ok.Right;
            cancelButtonRightMargin = this.ClientSize.Width - Cancel.Right;

            // 9) Capture the initial width of the Add button
            addButtonWidth = buttonAdd.Width;
        }

        private void OutlineForm_Resize(object sender, EventArgs e)
        {
            // Suspend layout logic for the form and its controls
            this.SuspendLayout();
            // Enforce minimum size
            if (this.Width < initialFormWidth)
            {
                this.Width = initialFormWidth;
                treeView1.Width = treeViewInitialWidth + 4;
                richTextBoxDetails.Width = richTextBoxInitialWidth + 4;
            }

            if (this.Height < initialFormHeight)
            {
                this.Height = initialFormHeight;
                treeView1.Height = treeViewInitialHeight;
                richTextBoxDetails.Height = richTextBoxInitialHeight;
            }

            // Current client dimensions
            int clientW = this.ClientSize.Width;
            int clientH = this.ClientSize.Height;

            // Proportionally size the TreeView and RichTextBox (horizontally)
            int richTextboxRightMargin = clientW - (richTextBoxDetails.Left + richTextBoxDetails.Width);
            int totalHorizontalSpace = clientW
                                       - treeView1.Left
                                       - richTextboxRightMargin
                                       - gapTreeViewToRichTextBox;

            int newTreeWidth = (int)(treeToRichWidthRatio * totalHorizontalSpace);
            int newRichWidth = totalHorizontalSpace - newTreeWidth;

            treeView1.Width = newTreeWidth;
            richTextBoxDetails.Left = treeView1.Right + gapTreeViewToRichTextBox;
            richTextBoxDetails.Width = newRichWidth;

            // Resize TreeView & RichTextBox vertically to maintain bottom margin
            treeView1.Height = clientH - treeView1.Top - treeViewBottomMargin;
            richTextBoxDetails.Height = clientH - richTextBoxDetails.Top - richTextBoxBottomMargin;

            // Position bottom buttons so they keep the same distance from bottom
            // and maintain the same horizontal offset from the RichTextBox’s left edge.
            int newButtonTop = clientH - buttonBottomMargin - buttonAdd.Height;

            buttonAdd.Top = newButtonTop;
            buttonEdit.Top = newButtonTop;
            buttonDelete.Top = newButtonTop;
            buttonScan.Top = newButtonTop;

            // Keep their widths the same (no code changing Width),
            // only reposition horizontally based on the new RichTextBox left.
            buttonAdd.Left = richTextBoxDetails.Left + addButtonLeftOffset;
            buttonEdit.Left = richTextBoxDetails.Left + editButtonLeftOffset;
            buttonDelete.Left = richTextBoxDetails.Left + deleteButtonLeftOffset;
            buttonScan.Left = richTextBoxDetails.Left + scanButtonLeftOffset;

            // Keep OK and Cancel the same distance from the right edge
            Ok.Left = clientW - okButtonRightMargin - Ok.Width;
            Cancel.Left = clientW - cancelButtonRightMargin - Cancel.Width;

            // Keep the Add button the same width
            buttonAdd.Width = addButtonWidth;
            buttonEdit.Width = addButtonWidth;
            buttonDelete.Width = addButtonWidth;
            buttonScan.Width = addButtonWidth;

            // Reposition label2 to have the same left edge as richTextBoxDetails
            label2.Left = richTextBoxDetails.Left;

            // Resume layout logic for the form and its controls
            this.ResumeLayout();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
