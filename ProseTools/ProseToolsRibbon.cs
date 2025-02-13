using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
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
            UpdateRibbonVisibility();
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
            dropDownProseType.Items.Add(CreateDropDownItem("Research Paper"));
            dropDownProseType.Items.Add(CreateDropDownItem("Technical Paper"));
            dropDownProseType.Items.Add(CreateDropDownItem("Screenplay"));

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

            if(string.IsNullOrEmpty(proseType))
            {
                MessageBox.Show("Please select a prose type from the dropdown.", "Error");
                return;
            }

            // Get the active Word application and document
            var wordApp = Globals.ThisAddIn.Application;
            var doc = wordApp.ActiveDocument;

            if (doc == null)
            {
                MessageBox.Show("No active document found.", "Error");
                return;
            }

            if (doc.Content.Words.Count > 1 || doc.Content.Text.Trim().Length != 0)
            {
                AddProseToolsToExistingDocument(doc, proseType);
                return;
            }

            // Check the selected prose type and open the corresponding dialog
            if (proseType == "Novel" || proseType == "Book")
            {
                if (ShouldOpenStartBook())
                {
                    // Open StartBook.cs
                    StartBook startBook = new StartBook();
                    if(startBook.ShowDialog() == DialogResult.OK)
                    {
                        if(!(Globals.ThisAddIn._ProseMetaData is NovelMetaData))
                        {
                            MessageBox.Show("Error: Metadata is not for a Novel. Please start a new Novel and try again.", "Error");
                            return;
                        }
                        // Update the ribbon visibility after the dialog closes
                        UpdateRibbonVisibility();
                    }
                }
            }
            else if (proseType == "Research Paper")
            {
                if (ShouldOpenStartResearchPaper())
                {
                    // Open StartResearchPaper.cs
                    StartResearchPaper startResearchPaper = new StartResearchPaper();
                    if(startResearchPaper.ShowDialog() == DialogResult.OK)
                    {
                        // Update the ribbon visibility after the dialog closes
                        UpdateRibbonVisibility();
                    }
                }
            }
            else if (proseType == "Technical Paper")
            {
                if (ShouldOpenStartTechnicalPaper())
                {
                    // Open StartTechnicalPaper.cs
                    StartTechnicalPaper startTechnicalPaper = new StartTechnicalPaper();
                    if(startTechnicalPaper.ShowDialog() == DialogResult.OK)
                    {
                        // Update the ribbon visibility after the dialog closes
                        UpdateRibbonVisibility();
                    }
                }
            }
            else if (proseType == "Screenplay")
            {
                if (ShouldOpenStartScreenplay())
                {
                    // Open StartTechnicalPaper.cs
                    StartScreenplay startScreenplay = new StartScreenplay();
                    if(startScreenplay.ShowDialog() == DialogResult.OK)
                    {
                        // Update the ribbon visibility after the dialog closes
                        UpdateRibbonVisibility();
                    }
                }
            }        
        }

        private void AddProseToolsToExistingDocument(Document doc, string proseType)
        {
            // Ask the user if they want to add metadata for the selected prose type.
            DialogResult result = MessageBox.Show(
                $"The document already contains text. Would you like to add {proseType} metadata to the document?",
                "Add Metadata",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Create and assign the appropriate metadata based on the selected prose type.
                    if (proseType.Equals("Novel", StringComparison.OrdinalIgnoreCase))
                    {
                        NovelMetaData novelMeta = new NovelMetaData();
                        Globals.ThisAddIn._ProseMetaData = novelMeta;
                        novelMeta.WriteToActiveDocument();
                        MessageBox.Show("Novel metadata has been added.", "Metadata Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (proseType.Equals("Research Paper", StringComparison.OrdinalIgnoreCase))
                    {
                        // Assuming ResearchPaperMetaData is implemented.
                        ResearchPaperMetaData researchMeta = new ResearchPaperMetaData();
                        Globals.ThisAddIn._ProseMetaData = researchMeta;
                        researchMeta.WriteToActiveDocument();
                        MessageBox.Show("Research Paper metadata has been added.", "Metadata Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (proseType.Equals("Technical Paper", StringComparison.OrdinalIgnoreCase))
                    {
                        // Assuming TechnicalPaperMetaData is implemented.
                        TechnicalPaperMetaData techMeta = new TechnicalPaperMetaData();
                        Globals.ThisAddIn._ProseMetaData = techMeta;
                        techMeta.WriteToActiveDocument();
                        MessageBox.Show("Technical Paper metadata has been added.", "Metadata Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (proseType.Equals("Screenplay", StringComparison.OrdinalIgnoreCase))
                    {
                        // Assuming ScreenplayMetaData is implemented.
                        ScreenplayMetaData screenplayMeta = new ScreenplayMetaData();
                        Globals.ThisAddIn._ProseMetaData = screenplayMeta;
                        screenplayMeta.WriteToActiveDocument();
                        MessageBox.Show("Screenplay metadata has been added.", "Metadata Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unrecognized prose type. No metadata was added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Refresh the ribbon to reflect the new state.
                    UpdateRibbonVisibility();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while adding metadata: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No metadata was added to the document.", "Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ShouldOpenStartResearchPaper()
        {
            return CanStartNewProse();
        }

        private bool ShouldOpenStartTechnicalPaper()
        {
            return CanStartNewProse();
        }

        private bool ShouldOpenStartScreenplay()
        {
            return CanStartNewProse();
        }

        private bool CanStartNewProse()
        {
            // Check for an active document.
            var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            if (wordDoc == null)
            {
                MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                return false;
            }

            // Check if a prose type has already been started.
            if (Globals.ThisAddIn._ProseMetaData != null && !(Globals.ThisAddIn._ProseMetaData is NullMetaData))
            {
                MessageBox.Show("A prose type has already been started in this document.", "Information");
                return false;
            }

            // Check if the document already has a Table of Contents.
            if (wordDoc.TablesOfContents.Count > 0)
            {
                MessageBox.Show("The document already contains a Table of Contents.", "Information");
                return false;
            }

            // All checks passed; the document is valid for starting a new prose.
            return true;
        }

        //private bool IsActiveDocumentOpen()
        //{
        //    var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
        //    if (wordDoc == null)
        //    {
        //        MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
        //        return false;
        //    }

        //    return true;
        //}

        private bool ShouldOpenStartBook()
        {
            if (!CanStartNewProse())
            {
                return false;
            }

            var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;

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

        private void aiSettingsButton_Click(object sender, RibbonControlEventArgs e)
        {
            string selectedText = genAIDropDown.SelectedItem != null
                ? genAIDropDown.SelectedItem.Label
                : genAIDropDown.Items[0].Label;
            aiSettingsDlg aiSettingsDlg = new aiSettingsDlg(selectedText);
            if (aiSettingsDlg.ShowDialog() == DialogResult.OK)
            {
                ;
            }
        }        
        
        private void Outline_Click(object sender, RibbonControlEventArgs e)
        {
            if (Globals.ThisAddIn.Application.ActiveDocument == null)
            {
                MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                return;
            }

            if (Globals.ThisAddIn._ProseMetaData == null)
            {
                MessageBox.Show("ProseMetaData not found. Please open a document with ProseMetaData and try again.", "Error");
                return;
            }

            // Check if the metadata is specifically for a Novel
            if (!(Globals.ThisAddIn._ProseMetaData is NovelMetaData novelMetaData))
            {
                MessageBox.Show("This document does not contain novel metadata. The Outline feature is only available for novels.", "Error");
                return;
            }

            // Proceed with the Outline
            OutlineForm outlineForm = new OutlineForm();
            outlineForm.InitializeOutline(novelMetaData.TheOutline);  // Now safe to access TheOutline
            outlineForm.ShowDialog();
        }

        private void GenAI_login_Click(object sender, RibbonControlEventArgs e)
        {
            GenAI_Login genAI_Login = new GenAI_Login();
            genAI_Login.ShowDialog();
        }

        internal void UpdateRibbonVisibility()
        {
            // Get the current metadata from the global add-in.
            //var meta = Globals.ThisAddIn._ProseMetaData;

            //// Default: If no metadata (or NullMetaData), only show the proseGroup.
            //if (meta == null || meta is NullMetaData)
            //{
            //    proseGroup.Visible = true;
            //    novel.Visible = false;
            //    researchPaper.Visible = false;
            //    techPaper.Visible = false;
            //    screenPlay.Visible = false;
            //}
            //// If the metadata is for a Novel, show the novel group only.
            //else if (meta is NovelMetaData)
            //{
            //    proseGroup.Visible = false;
            //    novel.Visible = true;
            //    researchPaper.Visible = false;
            //    techPaper.Visible = false;
            //    screenPlay.Visible = false;
            //}
            //// If it's a Research Paper, show the researchPaper group only.
            //else if (meta is ResearchPaperMetaData)
            //{
            //    proseGroup.Visible = false;
            //    novel.Visible = false;
            //    researchPaper.Visible = true;
            //    techPaper.Visible = false;
            //    screenPlay.Visible = false;
            //}
            //// If it's a Technical Paper, show the techPaper group only.
            //else if (meta is TechnicalPaperMetaData)
            //{
            //    proseGroup.Visible = false;
            //    novel.Visible = false;
            //    researchPaper.Visible = false;
            //    techPaper.Visible = true;
            //    screenPlay.Visible = false;
            //}
            //// If it's a Screenplay, show the screenPlay group only.
            //else if (meta is ScreenplayMetaData)
            //{
            //    proseGroup.Visible = false;
            //    novel.Visible = false;
            //    researchPaper.Visible = false;
            //    techPaper.Visible = false;
            //    screenPlay.Visible = true;
            //}

            proseGroup.Visible = true;
            novel.Visible = true;
            researchPaper.Visible = true;
            techPaper.Visible = true;
            screenPlay.Visible = true;

            // Update the Start Prose button label and enable/disable state.
            UpdateStartProseButton();

            // Force the ribbon to refresh so the changes appear immediately.
            this.RibbonUI.Invalidate();
        }

        internal void UpdateStartProseButton()
        {
            // Check for an active document.
            var wordDoc = Globals.ThisAddIn.Application.ActiveDocument;
            if (wordDoc == null ||
                ((Globals.ThisAddIn._ProseMetaData == null || Globals.ThisAddIn._ProseMetaData is NullMetaData) &&
                 wordDoc.Content.Text.Trim().Length == 0)) // Use Trim() here
            {
                startProse.Label = "Start Prose";
                startProse.Enabled = true;
            }
            else
            {
                if (!(Globals.ThisAddIn._ProseMetaData == null || Globals.ThisAddIn._ProseMetaData is NullMetaData))
                {
                    startProse.Label = "Start Prose";
                    startProse.Enabled = false;
                }
                else
                {
                    startProse.Label = "Add Metadata";
                    startProse.Enabled = true;
                }
            }
        }

    }
}