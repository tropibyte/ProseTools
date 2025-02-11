using System;
using System.Collections.Generic;
using System.Text;
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
            // Ensure a node is selected
            if (e.Node?.Tag is OutlineNode selectedNode)
            {
                // Populate the richTextBoxDetails with the node's details
                StringBuilder details = new StringBuilder();
                details.AppendLine($"Title: {selectedNode.Title}");
                details.AppendLine($"Details: {selectedNode.Details}");

                if (selectedNode.Attributes != null && selectedNode.Attributes.Count > 0)
                {
                    details.AppendLine("\nAttributes:");
                    foreach (var attribute in selectedNode.Attributes)
                    {
                        details.AppendLine($"{attribute.Key}: {attribute.Value}");
                    }
                }

                richTextBoxDetails.Text = details.ToString();
            }
            else
            {
                // Clear the richTextBoxDetails if no valid node is selected
                richTextBoxDetails.Clear();
            }
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

            if (_outline == null || _outline.Nodes.Count == 0)
                return;

            foreach (var node in _outline.Nodes)
            {
                var treeNode = CreateTreeNode(node);
                treeView1.Nodes.Add(treeNode);
            }

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
            var newNodeTitle = PromptForInput("Enter node title:", "Add Node");
            if (string.IsNullOrWhiteSpace(newNodeTitle))
                return;

            var selectedNode = treeView1.SelectedNode?.Tag as OutlineNode;
            var newNode = new OutlineNode { Title = newNodeTitle };

            if (selectedNode != null)
            {
                selectedNode.AddChild(newNode);
            }
            else
            {
                _outline.AddNode(newNode);
            }

            PopulateTreeView();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (!(treeView1.SelectedNode?.Tag is OutlineNode selectedNode))
                return;

            var updatedTitle = PromptForInput("Edit node title:", "Edit Node", selectedNode.Title);
            if (string.IsNullOrWhiteSpace(updatedTitle))
                return;

            selectedNode.Title = updatedTitle;
            PopulateTreeView();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!(treeView1.SelectedNode?.Tag is OutlineNode selectedNode))
                return;

            var parentNode = FindParentNode(treeView1.SelectedNode);

            if (parentNode?.Tag is OutlineNode parentOutlineNode)
            {
                parentOutlineNode.DeleteChild(selectedNode.Title);
            }
            else
            {
                _outline.DeleteNode(selectedNode.Title);
            }

            PopulateTreeView();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Scanning functionality not yet implemented.");
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

        private string PromptForInput(string prompt, string title, string defaultValue = "")
        {
            return string.Empty;
        }
    }
}
