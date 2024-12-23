using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class ProseToolsRibbon
    {
        internal void SetGroupControlsEnabled(bool visible)
        {
            
        }

        private void ProseToolsRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            PopulateGenAIDropDown();
            PopulateProseTypeDropDown();
            SetGroupControlsEnabled(Globals.ThisAddIn.IsProseToolsTaskPaneVisible());
            this.RibbonUI.Invalidate(); // Forces the ribbon to refresh
        }

        private void PopulateGenAIDropDown()
        {
            // Clear any existing items in the dropdown
            genAIDropDown.Items.Clear();

            // Add the four GenAI options
            genAIDropDown.Items.Add(CreateDropDownItem("ChatGPT"));
            genAIDropDown.Items.Add(CreateDropDownItem("CoPilot"));
            genAIDropDown.Items.Add(CreateDropDownItem("Claude"));
            genAIDropDown.Items.Add(CreateDropDownItem("Gemini"));

            // Optionally set a default selection
            genAIDropDown.SelectedItemIndex = 0; // Default to ChatGPT
        }

        private void PopulateProseTypeDropDown()
        {
            // Clear any existing items in the dropdown
            dropDownProseType.Items.Clear();

            dropDownProseType.Items.Add(CreateDropDownItem("Novel"));
            // Optionally set a default selection
            dropDownProseType.SelectedItemIndex = 0; // Default to Narrative
        }

        // Helper method to create dropdown items
        private Microsoft.Office.Tools.Ribbon.RibbonDropDownItem CreateDropDownItem(string label)
        {
            var item = Factory.CreateRibbonDropDownItem();
            item.Label = label;
            return item;
        }

        private void scanForCharactersButton_Click(object sender, RibbonControlEventArgs e)
        {
            ScanForCharacters scanForCharacters = new ScanForCharacters();
            scanForCharacters.ShowDialog();
        }

        private void characterManagementButton_Click(object sender, RibbonControlEventArgs e)
        {
            CharacterManagement characterManagement = new CharacterManagement();
            characterManagement.ShowDialog();
        }

        private void startProse_Click(object sender, RibbonControlEventArgs e)
        {
            // Get the selected prose type from the dropdown
            string proseType = dropDownProseType.SelectedItem?.Label;

            if (proseType == "Novel" || proseType == "Book")
            {
                if (ShouldOpenStartBook())
                {
                    // Open StartBook.cs
                    StartBook startBook = new StartBook();
                    startBook.ShowDialog();
                }
            }
        }

        private bool ShouldOpenStartBook()
        {
            var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            if (wordDoc == null)
            {
                MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                return false;
            }

            // Check if the document contains a Table of Contents
            if (wordDoc.TablesOfContents.Count > 0)
            {
                MessageBox.Show("The document already contains a Table of Contents.", "Information");
                return false;
            }

            // No Table of Contents found, proceed to open StartBook.cs
            return true;
        }

        private void NewChapter_Click(object sender, RibbonControlEventArgs e)
        {
            NewChapterDialog newDlg = new NewChapterDialog();
            if (newDlg.ShowDialog() == DialogResult.OK)
            {
                ;
            }
        }

        private void settingsButton_Click(object sender, RibbonControlEventArgs e)
        {
            SettingsDlg settingsDlg = new SettingsDlg();
            if (settingsDlg.ShowDialog() == DialogResult.OK)
            {
                ;
            }
        }
    }
}
