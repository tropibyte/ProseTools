using edu.stanford.nlp.ling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

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

        private float treeToRichWidthRatio;

        private int gapTreeViewToRichTextBox;
        private int buttonBottomMargin;

        private int addButtonLeftOffset;
        private int editButtonLeftOffset;
        private int deleteButtonLeftOffset;
        private int scanButtonLeftOffset;

        private int treeViewBottomMargin;
        private int richTextBoxBottomMargin;

        private int okButtonRightMargin;
        private int cancelButtonRightMargin;

        private int addButtonWidth;

        private Outline _outline;

        public OutlineForm()
        {
            InitializeComponent();
            richTextBoxDetails.ReadOnly = true;
            richTextBoxDetails.ShortcutsEnabled = false;
            richTextBoxDetails.BackColor = treeView1.BackColor;//System.Drawing.Color.White;
        }

        private void OutlineForm_Load(object sender, EventArgs e)
        {
            CaptureInitialSizes();
            if (_outline != null)
                PopulateTreeView();

            // Subscribe to the AfterSelect event
            treeView1.AfterSelect += TreeView1_AfterSelect;
        }

        private void CaptureInitialSizes()
        {
            initialFormWidth = this.Width;
            initialFormHeight = this.Height;

            treeViewInitialWidth = treeView1.Width;
            richTextBoxInitialWidth = richTextBoxDetails.Width;
            treeViewInitialHeight = treeView1.Height;
            richTextBoxInitialHeight = richTextBoxDetails.Height;

            float combinedWidth = treeView1.Width + (float)richTextBoxDetails.Width;
            treeToRichWidthRatio = treeView1.Width / combinedWidth;

            gapTreeViewToRichTextBox = richTextBoxDetails.Left - (treeView1.Left + treeView1.Width);

            buttonBottomMargin = this.ClientSize.Height - buttonAdd.Bottom;

            addButtonLeftOffset = buttonAdd.Left - richTextBoxDetails.Left;
            editButtonLeftOffset = buttonEdit.Left - richTextBoxDetails.Left;
            deleteButtonLeftOffset = buttonDelete.Left - richTextBoxDetails.Left;
            scanButtonLeftOffset = buttonScan.Left - richTextBoxDetails.Left;

            treeViewBottomMargin = this.ClientSize.Height - (treeView1.Top + treeView1.Height);
            richTextBoxBottomMargin = this.ClientSize.Height - (richTextBoxDetails.Top + richTextBoxDetails.Height);

            okButtonRightMargin = this.ClientSize.Width - Ok.Right;
            cancelButtonRightMargin = this.ClientSize.Width - Cancel.Right;

            addButtonWidth = buttonAdd.Width;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is OutlineNode selectedNode)
            {
                richTextBoxDetails.Text = BuildNodeDetailsText(selectedNode);
            }
            else
            {
                richTextBoxDetails.Clear();
            }
        }

        private string BuildNodeDetailsText(OutlineNode node)
        {
            if (node == null)
                return string.Empty;

            StringBuilder details = new StringBuilder();
            details.AppendLine($"Title: {node.Title}");
            details.AppendLine($"Details: {node.Details}");
            details.AppendLine($"Notes: {node.Notes}");

            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                details.AppendLine("\nAttributes:");
                foreach (var attribute in node.Attributes)
                {
                    details.AppendLine($"{attribute.Key}: {attribute.Value}");
                }
            }

            return details.ToString();
        }

        private void OutlineForm_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            SuspendLayout();

            int clientW = this.ClientSize.Width;
            int clientH = this.ClientSize.Height;

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

            treeView1.Height = clientH - treeView1.Top - treeViewBottomMargin;
            richTextBoxDetails.Height = clientH - richTextBoxDetails.Top - richTextBoxBottomMargin;

            int newButtonTop = clientH - buttonBottomMargin - buttonAdd.Height;
            buttonAdd.Top = newButtonTop;
            buttonEdit.Top = newButtonTop;
            buttonDelete.Top = newButtonTop;
            buttonScan.Top = newButtonTop;

            buttonAdd.Left = richTextBoxDetails.Left + addButtonLeftOffset;
            buttonEdit.Left = richTextBoxDetails.Left + editButtonLeftOffset;
            buttonDelete.Left = richTextBoxDetails.Left + deleteButtonLeftOffset;
            buttonScan.Left = richTextBoxDetails.Left + scanButtonLeftOffset;

            Ok.Left = clientW - okButtonRightMargin - Ok.Width;
            Cancel.Left = clientW - cancelButtonRightMargin - Cancel.Width;

            buttonAdd.Width = addButtonWidth;
            buttonEdit.Width = addButtonWidth;
            buttonDelete.Width = addButtonWidth;
            buttonScan.Width = addButtonWidth;

            label2.Left = richTextBoxDetails.Left;

            ResumeLayout();
        }

        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();

            // If using the new design, we expect a single root node.
            if (_outline == null || _outline.RootNode == null)
                return;

            var rootTreeNode = CreateTreeNode(_outline.RootNode);
            treeView1.Nodes.Add(rootTreeNode);

            treeView1.ExpandAll();
        }

        private TreeNode CreateTreeNode(OutlineNode outlineNode)
        {
            var treeNode = new TreeNode(outlineNode.Title)
            {
                Tag = outlineNode
            };

            foreach (var child in outlineNode.Children)
            {
                treeNode.Nodes.Add(CreateTreeNode(child));
            }

            return treeNode;
        }

        internal void InitializeOutline(Outline theOutline)
        {
            _outline = theOutline;
            PopulateTreeView();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // Check if a node is selected.
            string parentName = string.Empty;
            if (treeView1.SelectedNode?.Tag is OutlineNode selNode)
            {
                // Use the title of the selected node as the parent.
                parentName = selNode.Title;
            }
            else if (_outline != null && _outline.RootNode != null)
            {
                // If no node is selected, use the root node's title.
                parentName = _outline.RootNode.Title;
            }
            else
            {
                parentName = "None"; // Or handle this case as needed.
            }
            // Open the dialog with empty values.
            using (var dialog = new AddNodeDlg(parentName))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Create a new node using all fields from the dialog.
                    var newNode = new OutlineNode
                    {
                        Title = dialog.NodeTitle,
                        Details = dialog.NodeDetails,
                        Notes = dialog.NodeNotes,
                        Attributes = dialog.NodeAttributes
                    };

                    // If a node is selected, add as a child of that node.
                    if (treeView1.SelectedNode?.Tag is OutlineNode selectedNode)
                    {
                        // Optionally check for duplicates under the selected node.
                        if (selectedNode.FindChild(newNode.Title) == null)
                            selectedNode.AddChild(newNode);
                        else
                            MessageBox.Show("A node with that title already exists under the selected node.");
                    }
                    else
                    {
                        // No node is selected; add the new node as a child of the root.
                        if (_outline != null && _outline.RootNode != null)
                        {
                            if (_outline.RootNode.FindChild(newNode.Title) == null)
                                _outline.RootNode.AddChild(newNode);
                            else
                                MessageBox.Show("A node with that title already exists under the root node.");
                        }
                        else
                        {
                            MessageBox.Show("The outline has no root node. Please initialize the outline first.");
                            return;
                        }
                    }

                    PopulateTreeView();
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // First, ensure a node is selected.
            if (!(treeView1.SelectedNode?.Tag is OutlineNode selectedNode))
                return;

            // Determine the parent node's title.
            string parentName = "None";
            TreeNode parentTreeNode = FindParentNode(treeView1.SelectedNode);
            if (parentTreeNode?.Tag is OutlineNode parentNode)
            {
                parentName = parentNode.Title;
            }

            // Open the dialog for editing, passing the parent node name.
            using (var dialog = new AddNodeDlg(selectedNode.Title, selectedNode.Details, selectedNode.Notes, selectedNode.Attributes, parentName))
            {
                dialog.Text = "Modify Node"; // Optional: set a specific title for editing.
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Update the selected node with the new values.
                    selectedNode.Title = dialog.NodeTitle;
                    selectedNode.Details = dialog.NodeDetails;
                    selectedNode.Notes = dialog.NodeNotes;
                    selectedNode.Attributes = dialog.NodeAttributes;
                    PopulateTreeView();
                    richTextBoxDetails.Text = BuildNodeDetailsText(selectedNode);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!(treeView1.SelectedNode?.Tag is OutlineNode selectedNode))
                return;

            // Prevent deletion of the root node.
            if (_outline != null && _outline.RootNode == selectedNode)
            {
                MessageBox.Show("Cannot delete the root node.");
                return;
            }

            // Find the parent node using the tree's structure.
            var parentNode = FindParentNode(treeView1.SelectedNode);
            if (parentNode?.Tag is OutlineNode parentOutlineNode)
            {
                parentOutlineNode.DeleteChild(selectedNode.Title);
            }
            else
            {
                MessageBox.Show("Parent node not found.");
            }

            PopulateTreeView();
        }


        private void buttonScan_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the active document from Word.
                var activeDoc = Globals.ThisAddIn.Application.ActiveDocument;
                if (activeDoc == null)
                {
                    MessageBox.Show("No active document found.");
                    return;
                }

                // Ensure _outline exists and has a root. If not, create a default root.
                if (_outline == null)
                    _outline = new Outline();

                if (_outline.RootNode == null)
                {
                    // Create a default root node if one doesn't exist.
                    var defaultRoot = new OutlineNode
                    {
                        Title = "Book Metadata",
                        Details = "Default Book Metadata"
                    };
                    _outline.InitializeRoot(defaultRoot);
                }

                int addedCount = 0; // Count new chapters added.

                // Loop through paragraphs and detect chapters based on the desired styles.
                foreach (Word.Paragraph para in activeDoc.Paragraphs)
                {
                    var style = para.get_Style() as Word.Style;
                    if (style != null &&
                        (style.NameLocal.Equals("Prose Chapter", StringComparison.CurrentCultureIgnoreCase) ||
                         style.NameLocal.Equals("Heading 1", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var chapterTitle = para.Range.Text.Trim();

                        // Exclude Table of Contents entries.
                        if (chapterTitle.StartsWith("Table of Contents", StringComparison.CurrentCultureIgnoreCase))
                            continue;

                        if (!string.IsNullOrWhiteSpace(chapterTitle))
                        {
                            // Check for duplicates in the root's children.
                            if (_outline.RootNode.FindChild(chapterTitle) == null)
                            {
                                var chapterNode = new OutlineNode
                                {
                                    Title = chapterTitle,
                                    Details = "Double-click to edit details or add notes.",
                                    Notes = string.Empty
                                };

                                _outline.RootNode.AddChild(chapterNode);
                                addedCount++;
                            }
                        }
                    }
                }

                if (addedCount == 0)
                {
                    MessageBox.Show("No new chapters found to add.");
                }
                else
                {
                    PopulateTreeView();
                    MessageBox.Show($"{addedCount} chapter(s) added to the outline.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while scanning: " + ex.Message);
            }
        }

        private TreeNode FindParentNode(TreeNode childNode)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Nodes.Contains(childNode))
                    return node;
                var parent = FindParentNodeRecursive(node, childNode);
                if (parent != null)
                    return parent;
            }
            return null;
        }

        private TreeNode FindParentNodeRecursive(TreeNode currentNode, TreeNode childNode)
        {
            foreach (TreeNode node in currentNode.Nodes)
            {
                if (node == childNode)
                    return currentNode;
                var parent = FindParentNodeRecursive(node, childNode);
                if (parent != null)
                    return parent;
            }
            return null;
        }

        private string PromptForInput(string prompt, string title, string defaultValue = "", bool multiline = false)
        {
            using (var dialog = new InputDialog(prompt, title, defaultValue, multiline))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    return dialog.InputValue;
                }
            }
            return defaultValue; // or return string.Empty if canceled
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            SaveOutline();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Trigger editing on double-click.
        private void richTextBoxDetails_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedNode();
        }

        // Trigger editing on Ctrl+Enter.
        private void richTextBoxDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control)
            {
                EditSelectedNode();
                // Prevent the beep sound that may occur on Enter.
                e.SuppressKeyPress = true;
            }
        }

        // Consolidate the editing functionality into one method.
        private void EditSelectedNode()
        {
            if (treeView1.SelectedNode?.Tag is OutlineNode selectedNode)
            {
                // Allow editing details.
                var updatedDetails = PromptForInput("Edit details:", "Edit Node", selectedNode.Details);
                if (string.IsNullOrWhiteSpace(updatedDetails))
                    return;
                selectedNode.Details = updatedDetails;

                // Allow editing notes.
                var updatedNotes = PromptForInput("Edit notes:", "Edit Node", selectedNode.Notes);
                if (string.IsNullOrWhiteSpace(updatedNotes))
                    return;
                selectedNode.Notes = updatedNotes;

                PopulateTreeView();
            }
        }

        private void SaveOutline()
        {
            // Implement saving logic here
        }
    }
}
