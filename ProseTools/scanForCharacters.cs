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
    public partial class ScanForCharacters : Form
    {
        public ScanForCharacters()
        {
            InitializeComponent();
            InitializeListView();
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

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void doScanButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure there's an active Word document
                var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
                if (wordDoc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Initialize the CharacterScanner
                var scanner = new CharacterScanner();
                var detectedNames = scanner.ScanForNames(wordDoc, chkBoxIgnoreFirstWord.Checked);

                // Clear existing items in the ListView
                listViewCharacters.Items.Clear();

                // Populate ListView with detected names
                foreach (var name in detectedNames)
                {
                    var listViewItem = new ListViewItem(name); // "Name Found" column
                    listViewItem.SubItems.Add("");            // "Character Represented" column (initially empty)
                    listViewCharacters.Items.Add(listViewItem);
                }

                SetCharacterCountParsingText(listViewCharacters.Items.Count, wordDoc.Words.Count);
                // Show a message with the count of detected names
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
    }
}
