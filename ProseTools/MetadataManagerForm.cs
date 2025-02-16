using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace ProseTools
{
    public partial class MetadataManagerForm : Form
    {
        public MetadataManagerForm()
        {
            InitializeComponent();
            LoadMetadataTree();
        }

        /// <summary>
        /// Loads the current metadata (if any) into the TreeView.
        /// </summary>
        private void LoadMetadataTree()
        {
            treeViewMetadata.Nodes.Clear();

            if (Globals.ThisAddIn._ProseMetaData != null)
            {
                // Convert current metadata to XML.
                XElement xml = Globals.ThisAddIn._ProseMetaData.ToXML();
                TreeNode root = new TreeNode("Metadata");
                PopulateTreeNode(root, xml);
                treeViewMetadata.Nodes.Add(root);
                treeViewMetadata.ExpandAll();
            }
            else
            {
                treeViewMetadata.Nodes.Add(new TreeNode("No metadata found in this document."));
            }
        }

        /// <summary>
        /// Recursively populates a TreeNode from an XElement.
        /// </summary>
        private void PopulateTreeNode(TreeNode parent, XElement element)
        {
            TreeNode node = new TreeNode(element.Name.LocalName);
            if (!element.HasElements)
            {
                node.Text += ": " + element.Value;
            }
            parent.Nodes.Add(node);
            foreach (XElement child in element.Elements())
            {
                PopulateTreeNode(node, child);
            }
        }

        /// <summary>
        /// Export current metadata to a file (XML or JSON).
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Globals.ThisAddIn._ProseMetaData == null)
            {
                MessageBox.Show("No metadata found in the current document.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
                dlg.Title = "Export Metadata";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        XElement xml = Globals.ThisAddIn._ProseMetaData.ToXML();
                        xml.Save(dlg.FileName);
                    }
                    else if (dlg.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        XElement xml = Globals.ThisAddIn._ProseMetaData.ToXML();
                        // Using Newtonsoft.Json to convert XML to JSON.
                        string json = Newtonsoft.Json.JsonConvert.SerializeXNode(xml, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(dlg.FileName, json);
                    }
                    MessageBox.Show("Metadata exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Import metadata from a file (XML or JSON) and update the global metadata.
        /// </summary>
        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json";
                dlg.Title = "Import Metadata";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    XElement xml = null;
                    if (dlg.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        xml = XElement.Load(dlg.FileName);
                    }
                    else if (dlg.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        string json = File.ReadAllText(dlg.FileName);
                        // Assuming the root element is "ProseMetaData"
                        xml = Newtonsoft.Json.JsonConvert.DeserializeXNode(json, "ProseMetaData").Root;
                    }
                    // Assuming you have implemented a method like this:
                    Globals.ThisAddIn._ProseMetaData = ProseMetaData.LoadFromXml(xml);
                    MessageBox.Show("Metadata imported successfully.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMetadataTree();
                }
            }
        }

        /// <summary>
        /// Extracts metadata from another Word document and displays it in the TreeView.
        /// </summary>
        private void btnExtract_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Word Documents (*.docx)|*.docx";
                dlg.Title = "Select Document to Extract Metadata";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //try
                    //{
                    //    // Use a helper function to extract metadata from a given Word document.
                    //    XElement xml = MetadataHelper.ExtractMetadataFromDocument(dlg.FileName);
                    //    if (xml != null)
                    //    {
                    //        treeViewMetadata.Nodes.Clear();
                    //        TreeNode root = new TreeNode("Extracted Metadata");
                    //        PopulateTreeNode(root, xml);
                    //        treeViewMetadata.Nodes.Add(root);
                    //        treeViewMetadata.ExpandAll();
                    //        MessageBox.Show("Metadata extracted successfully.", "Extract Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("No metadata found in the selected document.", "Extract", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show("Error extracting metadata: " + ex.Message, "Extract Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
            }
        }

        private void btnClearMetadata_Click(object sender, EventArgs e)
        {
            // Ask for confirmation.
            var result = MessageBox.Show("Are you sure you want to clear all metadata? This operation is irreversible.",
                                         "Clear Metadata", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Set the global metadata to a new NullMetaData.
                    Globals.ThisAddIn._ProseMetaData = new NullMetaData();

                    // Write the (null) metadata to the active document.
                    Globals.ThisAddIn._ProseMetaData.WriteToActiveDocument();

                    // Optionally, refresh the metadata tree in your manager form.
                    LoadMetadataTree();

                    MessageBox.Show("Metadata cleared.", "Clear Metadata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while clearing metadata: " + ex.Message, "Clear Metadata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

