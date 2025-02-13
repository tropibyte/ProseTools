using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using edu.stanford.nlp.pipeline;

namespace ProseTools
{
    public partial class ScanForCharacters : Form
    {
        public static class ScanOptions
        {
            public const string Heuristic = "Heuristic";
            public const string OpenNLP = "OpenNLP";
            public const string SpacyScan = "Spacy Scan";
        }

        public ScanForCharacters()
        {
            InitializeComponent();
            InitializeListView();
            InitializeScanOptions();
            var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            if (wordDoc != null)
            {
                SetCharacterCountParsingText(listViewCharacters.Items.Count, wordDoc.Words.Count);
            }
        }

        private void SetCharacterCountParsingText(int count, int nWordCount)
        {
            labelWordCountParsing.Text = $"Found {count} potential characters in {nWordCount} words...";
        }

        private void InitializeListView()
        {
            // Set ListView properties
            listViewCharacters.View = View.Details;
            listViewCharacters.FullRowSelect = true;
            listViewCharacters.GridLines = true; // Optional: adds grid lines for clarity

            // Add columns
            listViewCharacters.Columns.Add("Name Found", 200); // Column for detected names
            listViewCharacters.Columns.Add("Character Represented", 250); // Column for mapped characters
            listViewCharacters.Columns.Add("Type", 150); // Column for mapped characters

        }

        private void InitializeScanOptions()
        {
            cbWhichScan.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWhichScan.Items.Clear();

            // Add scan options. You can change the names as appropriate.
            cbWhichScan.Items.Add(ScanOptions.Heuristic);
            cbWhichScan.Items.Add(ScanOptions.OpenNLP);
            cbWhichScan.Items.Add(ScanOptions.SpacyScan);

            // Set the default selection (for example, Heuristic)
            cbWhichScan.SelectedIndex = 0;
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool IsNameAlreadyAssigned(string scannedName)
        {
            if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
            {
                // Check if any character has the scanned name as an alias by comparing the string representation of each alias.
                return novel.ListCharacters.Any(c => c.Alias != null &&
                    c.Alias.Any(a => a.ToString().Equals(scannedName, StringComparison.OrdinalIgnoreCase)));
            }
            return false;
        }

        private void DoScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
                if (wordDoc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                var scanner = new CharacterScanner();
                List<string> detectedNames = null;

                string selectedScan = cbWhichScan.SelectedItem?.ToString();
                if (selectedScan == ScanOptions.Heuristic)
                {
                    detectedNames = scanner.ScanForNames(wordDoc, chkBoxIgnoreFirstWord.Checked);
                }
                else if (selectedScan == ScanOptions.OpenNLP)
                {
                    string documentText = wordDoc.Content.Text;
                    detectedNames = scanner.ScanForNamesUsingOpenNLP(documentText);
                }
                else if (selectedScan == ScanOptions.SpacyScan)
                {
                    string documentText = wordDoc.Content.Text;
                    detectedNames = scanner.ScanForNamesUsingSpacy(documentText);
                }
                else
                {
                    MessageBox.Show("Unknown scan option selected.", "Error");
                    return;
                }

                listViewCharacters.Items.Clear();
                foreach (var name in detectedNames)
                {
                    var item = new ListViewItem(name);
                    // Check if name exists already
                    if (IsNameAlreadyAssigned(name))
                    {
                        item.SubItems.Add("Already Assigned");
                        item.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.SubItems.Add("New Alias");
                    }
                    item.SubItems.Add(""); // Reserved for Character Type
                    listViewCharacters.Items.Add(item);
                }
                SetCharacterCountParsingText(listViewCharacters.Items.Count, wordDoc.Words.Count);
                MessageBox.Show($"{detectedNames.Count} potential character names found!", "Scan Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the scan: {ex.Message}", "Error");
            }
        }


        private void DoScanButton_Click_old(object sender, EventArgs e)
        {
            try
            {
                // Ensure there's an active Word document.
                var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
                if (wordDoc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Create a new CharacterScanner instance.
                var scanner = new CharacterScanner();
                List<string> detectedNames = null;

                // Determine which scan method to use.
                string selectedScan = cbWhichScan.SelectedItem?.ToString();
                if (selectedScan == ScanOptions.Heuristic)
                {
                    // Use the existing heuristic scanning method.
                    detectedNames = scanner.ScanForNames(wordDoc, chkBoxIgnoreFirstWord.Checked);
                }
                else if (selectedScan == ScanOptions.OpenNLP)
                {
                    // For OpenNLP scan, extract the document text and process it asynchronously.
                    string documentText = wordDoc.Content.Text;
                    detectedNames = scanner.ScanForNamesUsingOpenNLP(documentText);
                }
                else if (selectedScan == ScanOptions.SpacyScan)
                {
                    // For spaCy scan, extract the document text and process it asynchronously.
                    string documentText = wordDoc.Content.Text;
                    detectedNames = scanner.ScanForNamesUsingSpacy(documentText);
                }
                else
                {
                    MessageBox.Show("Unknown scan option selected.", "Error");
                    return;
                }

                // Clear existing items in the ListView.
                listViewCharacters.Items.Clear();

                // Populate the ListView with the detected names.
                foreach (var name in detectedNames)
                {
                    var listViewItem = new ListViewItem(name);
                    listViewItem.SubItems.Add(""); // "Character Represented" (initially empty)
                    listViewCharacters.Items.Add(listViewItem);
                }

                SetCharacterCountParsingText(listViewCharacters.Items.Count, wordDoc.Words.Count);
                MessageBox.Show($"{detectedNames.Count} potential character names found!", "Scan Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during the scan: {ex.Message}", "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // If there are no items or no items are selected, ignore the click
            if (listViewCharacters.Items.Count == 0 || listViewCharacters.SelectedItems.Count == 0)
            {
                return;
            }

            // Confirm with the user
            var result = MessageBox.Show(
                "Are you sure you want to delete the selected item(s)?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Delete selected items
                foreach (ListViewItem item in listViewCharacters.SelectedItems)
                {
                    listViewCharacters.Items.Remove(item);
                }
            }
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            if (listViewCharacters.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a name from the list.", "No Selection");
                return;
            }

            string scannedName = listViewCharacters.SelectedItems[0].Text;

            // Open the assign dialog, passing the scanned name.
            var assignForm = new AssignCharacterForm(scannedName);
            if (assignForm.ShowDialog() == DialogResult.OK)
            {
                // Optionally update the ListView item to show the linked character.
                listViewCharacters.SelectedItems[0].SubItems[1].Text = assignForm.AssignedCharacterName;
            }
        }
    }
}
