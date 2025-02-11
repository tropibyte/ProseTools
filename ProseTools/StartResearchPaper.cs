using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class StartResearchPaper : Form
    {
        WordStyleInfo wsiPaperFont, wsiPaperTitleFont;
        public const string sResearchPaper = "Research Paper";
        public const string sResearchPaperTitle = "Research Paper Title";

        public StartResearchPaper()
        {
            InitializeComponent();
            InitializeStyles();
            InitializeControls();
            SetPaperFontText();
        }

        private void InitializeControls()
        {
            this.comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxFormat.Items.Clear();
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Items.AddRange(new object[] {
            "APA",
            "MLA",
            "Chicago",
            "IEEE",
            "AMA",
            "Harvard",
            "Vancouver",
            "ACS",
            "CSE"});
            this.comboBoxFormat.SelectedIndex = 0; //Set default to APA
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text;
            string author = textBoxAuthor.Text;
            string format = comboBoxFormat.SelectedItem?.ToString();

            try
            {
                // Get the active Word application and document
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(format))
                {
                    MessageBox.Show("Please fill in all fields before proceeding.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add logic to proceed with research paper creation
                CreateResearchPaperStart(doc);
                // Close the form
                this.Close();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetPaperFontButton_Click(object sender, EventArgs e)
        {
            if (WordStyleInfo.ShowFontDialogForWSI(ref wsiPaperFont) == DialogResult.OK)
            {
                // Update the font display
                SetPaperFontText();
                WordStyleInfo.ApplyStyleToLabel(labelPaperFont, wsiPaperFont);
            }
        }

        private void InitializeStyles()
        {
            try
            {
                // Get the active Word application and document
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Initialize the WordStyleInfo object for the paper font
                // Check if "Research Paper" style exists
                if (WordStyleInfo.StyleExists(doc, sResearchPaper))
                {
                    wsiPaperFont = WordStyleInfo.GetStyleInfo(doc, sResearchPaper);
                }
                else
                {
                    // Create a new style if it doesn't exist
                    string fontName = GetResearchPaperFontName(doc);
                    wsiPaperFont = new WordStyleInfo(sResearchPaper, fontName, 12, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }

                // Check if "Research Paper Title" style exists
                if (WordStyleInfo.StyleExists(doc, sResearchPaperTitle))
                {
                    wsiPaperTitleFont = WordStyleInfo.GetStyleInfo(doc, sResearchPaperTitle);
                }
                else
                {
                    // Create a new style if it doesn't exist
                    string fontName = GetResearchPaperFontName(doc);
                    wsiPaperTitleFont = new WordStyleInfo(sResearchPaper, fontName, 12, true, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                    wsiPaperTitleFont.Underline = true;
                }
                WordStyleInfo.ApplyStyleToLabel(labelPaperFont, wsiPaperFont);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing styles: {ex.Message}", "Error");
            }
        }

        private string GetResearchPaperFontName(Document doc)
        {
            var availableFonts = new List<string>();

            // Enumerate all available font names
            foreach (var fontName in doc.Application.FontNames)
            {
                availableFonts.Add((string)fontName);
            }

            // Prioritized font selection
            string selectedFont = availableFonts.FirstOrDefault(f => f.Equals("Times New Roman", StringComparison.OrdinalIgnoreCase))
                ?? availableFonts.FirstOrDefault(f => f.Equals("Times", StringComparison.OrdinalIgnoreCase))
                ?? availableFonts.FirstOrDefault(f => f.Equals("MS Serif", StringComparison.OrdinalIgnoreCase))
                ?? availableFonts.FirstOrDefault(f => f.IndexOf("Times", StringComparison.OrdinalIgnoreCase) >= 0);
            // Default fallback if no match found
            return selectedFont ?? "Arial"; // Return Arial as a generic fallback
        }

        private void SetPaperFontText()
        {
            if (wsiPaperFont == null)
                return;

            labelPaperFont.Text = $"{wsiPaperFont.FontName} {wsiPaperFont.FontSize} pt";
        }

        private void CreateResearchPaperStart(Microsoft.Office.Interop.Word.Document doc)
        {
            // If there is no metadata or it is a NullMetaData, initialize a new ResearchPaperMetaData
            if (Globals.ThisAddIn._ProseMetaData is null || Globals.ThisAddIn._ProseMetaData is NullMetaData)
            {
                Globals.ThisAddIn._ProseMetaData = new ResearchPaperMetaData();
            }
            // If ProseMetaData already exists as ResearchPaperMetaData, fail because the research paper is already started
            else if (Globals.ThisAddIn._ProseMetaData is ResearchPaperMetaData)
            {
                MessageBox.Show("A research paper has already been started in this document.", "Error");
                return;
            }
            // If ProseMetaData is another type (NovelMetaData, ScreenplayMetaData), fail
            else
            {
                MessageBox.Show("This document contains non-research metadata. The research paper creation feature is only available for research papers.", "Error");
                return;
            }

            // Now we are guaranteed that _ProseMetaData is a valid ResearchPaperMetaData
            var researchMetaData = (ResearchPaperMetaData)Globals.ThisAddIn._ProseMetaData;

            // Add styles to the document
            AddStylesToDocument(doc);

            // Initialize the outline node structure
            var outline = researchMetaData.TheOutline;

            // Insert page and section break if the document is not empty
            if (!string.IsNullOrWhiteSpace(doc.Content.Text.Trim()))
            {
                var firstPageRange = doc.Content;
                firstPageRange.Collapse(WdCollapseDirection.wdCollapseStart);
                firstPageRange.InsertBreak(WdBreakType.wdSectionBreakNextPage);
            }

            // Determine the selected paper format and call the appropriate method
            string selectedFormat = comboBoxFormat.SelectedItem?.ToString() ?? "APA";
            switch (selectedFormat)
            {
                case "APA":
                    CreateAPAPaper(doc);
                    break;
                case "MLA":
                    CreateMLAPaper(doc);
                    break;
                case "Chicago":
                    CreateChicagoPaper(doc);
                    break;
                case "IEEE":
                    CreateIEEEPaper(doc);
                    break;
                case "AMA":
                    CreateAMAPaper(doc);
                    break;
                case "Harvard":
                    CreateHarvardPaper(doc);
                    break;
                case "Vancouver":
                    CreateVancouverPaper(doc);
                    break;
                case "ACS":
                    CreateACSPaper(doc);
                    break;
                case "CSE":
                    CreateCSEPaper(doc);
                    break;
                default:
                    MessageBox.Show("Unsupported format selected. Defaulting to APA.");
                    CreateAPAPaper(doc);
                    break;
            }

            // Add Table of Contents if the user has checked the option
            if (checkBoxAddTOC.Checked)
            {
                ProseStart.UpdateDocumentTOC(doc);
            }

            // Create and structure the research paper outline
            CreateResearchPaperOutline(doc, outline);
        }

        private void CreateResearchPaperOutline(Document doc, Outline outline)
        {
            // Create the root node for the research paper outline
            var rootNode = new OutlineNode
            {
                Title = "Research Paper Metadata",
                Details = "Metadata for the research paper",
                Attributes = new Dictionary<string, string>
        {
            { "ProseType", "Research Paper" },
            { "Format", comboBoxFormat.SelectedItem?.ToString() ?? "APA" }, // Format dynamically set
            { "Institution", textBoxInstitution.Text.Trim() },
            { "Course", textBoxCourse.Text.Trim() },
            { "Professor", textBoxProfessor.Text.Trim() }
        }
            };
            outline.AddNode(rootNode);

            // Add child nodes for each major section of the research paper
            var titlePageNode = new OutlineNode
            {
                Title = "Title Page",
                Details = $"Title: {textBoxTitle.Text.Trim()}, Author: {textBoxAuthor.Text.Trim()}"
            };
            rootNode.AddChild(titlePageNode);

            var abstractNode = new OutlineNode
            {
                Title = "Abstract",
                Details = "The abstract provides a brief summary of the paper."
            };
            rootNode.AddChild(abstractNode);

            var introductionNode = new OutlineNode
            {
                Title = "Introduction",
                Details = "The introduction provides an overview of the paper."
            };
            rootNode.AddChild(introductionNode);

            var bodyNode = new OutlineNode
            {
                Title = "Body",
                Details = "The body of the paper contains the main content."
            };
            rootNode.AddChild(bodyNode);

            var conclusionNode = new OutlineNode
            {
                Title = "Conclusion",
                Details = "The conclusion summarizes the paper."
            };
            rootNode.AddChild(conclusionNode);

            var referencesNode = new OutlineNode
            {
                Title = "References",
                Details = "The references section lists all sources cited in the paper."
            };
            rootNode.AddChild(referencesNode);
        }

        private void CreateAPAPaper(Document doc)
        {
            AddStylesToDocument(doc);

            // Set APA format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);
            
            // Insert page and section break if the document is not empty
            if (!string.IsNullOrWhiteSpace(doc.Content.Text.Trim()))
            {
                var firstPageRange = doc.Content;
                firstPageRange.Collapse(WdCollapseDirection.wdCollapseStart);
                firstPageRange.InsertBreak(WdBreakType.wdSectionBreakNextPage);
            }

            var titleRange = doc.Content;

            // Apply the research paper style
            titleRange.set_Style(sResearchPaper);

            // Vertically center all content on the page
            doc.Sections[1].PageSetup.VerticalAlignment = WdVerticalAlignment.wdAlignVerticalCenter;

            // Center-align the content
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            }

            // Insert the title (bold and centered)
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = textBoxTitle.Text;
            titleParagraph.Range.Font.Bold = 1; // Bold
            titleParagraph.Range.InsertParagraphAfter();

            // Insert author name (not bold)
            var authorParagraph = doc.Content.Paragraphs.Add();
            authorParagraph.Range.Text = textBoxAuthor.Text;
            authorParagraph.Range.Font.Bold = 0; // Not bold
            authorParagraph.Range.InsertParagraphAfter();

            // Insert institution name, if provided
            if (!string.IsNullOrWhiteSpace(textBoxInstitution.Text))
            {
                var institutionParagraph = doc.Content.Paragraphs.Add();
                institutionParagraph.Range.Text = textBoxInstitution.Text;
                institutionParagraph.Range.Font.Bold = 0; // Not bold
                institutionParagraph.Range.InsertParagraphAfter();
            }

            // Insert course name, if provided
            if (!string.IsNullOrWhiteSpace(textBoxCourse.Text))
            {
                var courseParagraph = doc.Content.Paragraphs.Add();
                courseParagraph.Range.Text = textBoxCourse.Text;
                courseParagraph.Range.Font.Bold = 0; // Not bold
                courseParagraph.Range.InsertParagraphAfter();
            }

            // Insert professor name, if provided
            if (!string.IsNullOrWhiteSpace(textBoxProfessor.Text))
            {
                var professorParagraph = doc.Content.Paragraphs.Add();
                professorParagraph.Range.Text = $"Professor {textBoxProfessor.Text}";
                professorParagraph.Range.Font.Bold = 0; // Not bold
                professorParagraph.Range.InsertParagraphAfter();
            }

            // Insert the date (system date in "Month Day, Year" format)
            var dateParagraph = doc.Content.Paragraphs.Add();
            dateParagraph.Range.Text = DateTime.Now.ToString("MMMM d, yyyy");
            dateParagraph.Range.Font.Bold = 0; // Not bold
            dateParagraph.Range.InsertParagraphAfter();

            // Set the entire document to double spacing
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }

            // Add a new page and section break
            ProseStart.InsertSection(doc, WdCollapseDirection.wdCollapseEnd, WdBreakType.wdSectionBreakNextPage);

            // Add a Table of Contents if the checkbox is checked
            if (checkBoxAddTOC.Checked)
            {
                // Insert "Table of Contents" title (centered, bold, and underlined)
                var tocTitleParagraph = doc.Content.Paragraphs.Add();
                tocTitleParagraph.Range.Text = "Table of Contents";
                tocTitleParagraph.Range.Font.Bold = 1; // Bold
                tocTitleParagraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle; // Underline
                tocTitleParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                tocTitleParagraph.Range.InsertParagraphAfter();

                // Add spacing and set the next paragraphs to left-align
                var tocParagraph = doc.Content.Paragraphs.Add();
                tocParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                // Add the Table of Contents
                doc.TablesOfContents.Add(tocParagraph.Range, true, 1, 3);
                ProseStart.ConfigureTOCForCustomStyle(doc, sResearchPaperTitle, 1);

                // Add a new page and section break
                ProseStart.InsertSection(doc, WdCollapseDirection.wdCollapseEnd, WdBreakType.wdSectionBreakNextPage);
            }

            // Add the title (centered, bold, and underlined) to the next section
            var newTitleParagraph = doc.Content.Paragraphs.Add();
            newTitleParagraph.Range.Text = textBoxTitle.Text;
            newTitleParagraph.Range.Font.Bold = 1; // Bold
            newTitleParagraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle; // Underline
            newTitleParagraph.Range.set_Style(sResearchPaperTitle); // Add to TOC using sResearchPaperTitle style
            newTitleParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            newTitleParagraph.Range.InsertParagraphAfter();

            // Add spacing and set the next paragraphs to left-align
            var contentParagraph = doc.Content.Paragraphs.Add();
            contentParagraph.Range.Text = "And here begins your paper....";
            newTitleParagraph.Range.set_Style(sResearchPaper); // Add using sResearchPaper style
            contentParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            contentParagraph.Range.Font.Bold = 0;
            contentParagraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;

            // Add a new page and section break
            ProseStart.InsertSection(doc, WdCollapseDirection.wdCollapseEnd, WdBreakType.wdSectionBreakNextPage);

            // Add the "References" section
            var referencesTitleParagraph = doc.Content.Paragraphs.Add();
            referencesTitleParagraph.Range.Text = "References";
            referencesTitleParagraph.Range.Font.Bold = 1; // Bold
            referencesTitleParagraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle; // Underline
            referencesTitleParagraph.Range.set_Style(sResearchPaperTitle); // Add to TOC using sResearchPaperTitle style
            referencesTitleParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            referencesTitleParagraph.Range.InsertParagraphAfter();

            // Add spacing and add the text for references
            var referencesContentParagraph = doc.Content.Paragraphs.Add();
            referencesContentParagraph.Range.Text = "Enter your references here.";
            referencesContentParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            referencesContentParagraph.Range.set_Style(sResearchPaper); // Add using sResearchPaper style
            referencesContentParagraph.Range.Font.Bold = 0;
            referencesContentParagraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;
        }

        private void CreateMLAPaper(Document doc)
        {
            // Set MLA format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // MLA specific content formatting (e.g., author name, page headers, etc.)
            var headerRange = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = $"{textBoxAuthor.Text} - Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

            // Insert MLA-specific title page and content
            var titleRange = doc.Content.Paragraphs.Add();
            titleRange.Range.Text = $"{textBoxAuthor.Text}\n{textBoxProfessor.Text}\n{textBoxCourse.Text}\n{DateTime.Now.ToString("d MMMM yyyy")}\n{textBoxTitle.Text}";
            titleRange.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            titleRange.Range.Font.Bold = 0; // Not bold
            titleRange.Range.InsertParagraphAfter();

            // Double space the document
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateChicagoPaper(Document doc)
        {
            // Set Chicago format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // Chicago-style title page
            var titlePage = doc.Content.Paragraphs.Add();
            titlePage.Range.Text = $"{textBoxTitle.Text}\n\n\n{textBoxAuthor.Text}\n\n{textBoxCourse.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titlePage.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Center-align
            titlePage.Range.Font.Bold = 0; // Not bold
            titlePage.Range.Font.Size = 12; // Standard font size
            titlePage.Range.InsertParagraphAfter();

            // Add a page break
            titlePage.Range.InsertBreak(WdBreakType.wdPageBreak);

            // Insert header with page numbers starting on the second page
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-align
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field

            // Double-space the document
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateIEEEPaper(Document doc)
        {
            // Set IEEE format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // Title
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = textBoxTitle.Text;
            titleParagraph.Range.Font.Size = 14; // Slightly larger font for title
            titleParagraph.Range.Font.Bold = 1; // Bold
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            titleParagraph.Range.InsertParagraphAfter();

            // Author(s) and affiliation(s)
            var authorParagraph = doc.Content.Paragraphs.Add();
            authorParagraph.Range.Text = $"{textBoxAuthor.Text}\n{textBoxInstitution.Text}";
            authorParagraph.Range.Font.Size = 12; // Standard font size
            authorParagraph.Range.Font.Bold = 0; // Not bold
            authorParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            authorParagraph.Range.InsertParagraphAfter();

            // Add an abstract section
            var abstractTitleParagraph = doc.Content.Paragraphs.Add();
            abstractTitleParagraph.Range.Text = "Abstract";
            abstractTitleParagraph.Range.Font.Size = 12; // Standard font size
            abstractTitleParagraph.Range.Font.Bold = 1; // Bold
            abstractTitleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            abstractTitleParagraph.Range.InsertParagraphAfter();

            var abstractContentParagraph = doc.Content.Paragraphs.Add();
            abstractContentParagraph.Range.Text = "Enter your abstract here. The abstract should summarize the key points of your paper in 150-250 words.";
            abstractContentParagraph.Range.Font.Size = 12; // Standard font size
            abstractContentParagraph.Range.Font.Bold = 0; // Not bold
            abstractContentParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            abstractContentParagraph.Range.ParagraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle; // Single-spaced
            abstractContentParagraph.Range.InsertParagraphAfter();

            // Add spacing before the next section
            abstractContentParagraph.Range.InsertParagraphAfter();

            // Double-space remaining sections
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                if (paragraph.Range.Text.Contains("Abstract"))
                {
                    // Keep the abstract single-spaced
                    paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                }
                else
                {
                    paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
                }
            }
        }

        private void CreateAMAPaper(Document doc)
        {
            // Set AMA format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // Title page
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = $"{textBoxTitle.Text}\n\n{textBoxAuthor.Text}\n\n{textBoxInstitution.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Centered
            titleParagraph.Range.Font.Bold = 0; // Not bold
            titleParagraph.Range.Font.Size = 12; // Standard font size
            titleParagraph.Range.InsertParagraphAfter();

            // Add a page break
            titleParagraph.Range.InsertBreak(WdBreakType.wdPageBreak);

            // Add header with page numbers
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = "Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-aligned

            // Set double-spacing throughout
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateHarvardPaper(Document doc)
        {
            // Set Harvard format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);
            
            // Title page
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = $"{textBoxTitle.Text}\n\n{textBoxAuthor.Text}\n\n{textBoxInstitution.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Centered
            titleParagraph.Range.Font.Bold = 0; // Not bold
            titleParagraph.Range.Font.Size = 12; // Standard font size
            titleParagraph.Range.InsertParagraphAfter();
            // Add a page break
            titleParagraph.Range.InsertBreak(WdBreakType.wdPageBreak);
            // Add header with page numbers
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = "Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-aligned
            // Set double-spacing throughout
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateVancouverPaper(Document doc)
        {
            // Set Vancouver format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // Title page
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = $"{textBoxTitle.Text}\n\n{textBoxAuthor.Text}\n\n{textBoxInstitution.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Centered
            titleParagraph.Range.Font.Bold = 0; // Not bold
            titleParagraph.Range.Font.Size = 12; // Standard font size
            titleParagraph.Range.InsertParagraphAfter();
            // Add a page break
            titleParagraph.Range.InsertBreak(WdBreakType.wdPageBreak);
            // Add header with page numbers
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = "Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-aligned
            // Set double-spacing throughout
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateACSPaper(Document doc)
        {
            // Set ACS format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);
            
            // Title page
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = $"{textBoxTitle.Text}\n\n{textBoxAuthor.Text}\n\n{textBoxInstitution.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Centered
            titleParagraph.Range.Font.Bold = 0; // Not bold
            titleParagraph.Range.Font.Size = 12; // Standard font size
            titleParagraph.Range.InsertParagraphAfter();
            // Add a page break
            titleParagraph.Range.InsertBreak(WdBreakType.wdPageBreak);
            // Add header with page numbers
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = "Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-aligned
            // Set double-spacing throughout
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void CreateCSEPaper(Document doc)
        {
            // Set CSE format margins (1-inch margins on all sides)
            WordHelper.SetOneInchMargins(doc);

            // Title page
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = $"{textBoxTitle.Text}\n\n{textBoxAuthor.Text}\n\n{textBoxInstitution.Text}\n\n{DateTime.Now.ToString("MMMM d, yyyy")}";
            titleParagraph.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Centered
            titleParagraph.Range.Font.Bold = 0; // Not bold
            titleParagraph.Range.Font.Size = 12; // Standard font size
            titleParagraph.Range.InsertParagraphAfter();
            // Add a page break
            titleParagraph.Range.InsertBreak(WdBreakType.wdPageBreak);
            // Add header with page numbers
            var headerRange = doc.Sections[2].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.Text = "Page ";
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage); // Page number field
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; // Right-aligned
            // Set double-spacing throughout
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        private void AddStylesToDocument(Document doc)
        {
            try
            {
                // Add or update ResearchPaper style
                if (WordStyleInfo.StyleExists(doc, sResearchPaper))
                {
                    var researchPaperStyle = doc.Styles[sResearchPaper];
                    researchPaperStyle.Font.Name = wsiPaperFont.FontName;
                    researchPaperStyle.Font.Size = wsiPaperFont.FontSize;
                    researchPaperStyle.Font.Bold = wsiPaperFont.Bold ? 1 : 0;
                    researchPaperStyle.Font.Italic = wsiPaperFont.Italic ? 1 : 0;
                    researchPaperStyle.Font.Underline = wsiPaperFont.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    researchPaperStyle.Font.StrikeThrough = wsiPaperFont.Strikethrough ? 1 : 0;
                    researchPaperStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiPaperFont.FontColor);
                    researchPaperStyle.ParagraphFormat.Alignment = wsiPaperFont.Alignment;
                }
                else
                {
                    var researchPaperStyle = doc.Styles.Add(sResearchPaper, WdStyleType.wdStyleTypeParagraph);
                    researchPaperStyle.Font.Name = wsiPaperFont.FontName;
                    researchPaperStyle.Font.Size = wsiPaperFont.FontSize;
                    researchPaperStyle.Font.Bold = wsiPaperFont.Bold ? 1 : 0;
                    researchPaperStyle.Font.Italic = wsiPaperFont.Italic ? 1 : 0;
                    researchPaperStyle.Font.Underline = wsiPaperFont.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    researchPaperStyle.Font.StrikeThrough = wsiPaperFont.Strikethrough ? 1 : 0;
                    researchPaperStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiPaperFont.FontColor);
                    researchPaperStyle.ParagraphFormat.Alignment = wsiPaperFont.Alignment;
                }

                // Add or update ResearchPaperTitle style
                if (WordStyleInfo.StyleExists(doc, sResearchPaperTitle))
                {
                    var researchPaperTitleStyle = doc.Styles[sResearchPaperTitle];
                    researchPaperTitleStyle.Font.Name = wsiPaperTitleFont.FontName;
                    researchPaperTitleStyle.Font.Size = wsiPaperTitleFont.FontSize;
                    researchPaperTitleStyle.Font.Bold = wsiPaperTitleFont.Bold ? 1 : 0;
                    researchPaperTitleStyle.Font.Italic = wsiPaperTitleFont.Italic ? 1 : 0;
                    researchPaperTitleStyle.Font.Underline = wsiPaperTitleFont.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    researchPaperTitleStyle.Font.StrikeThrough = wsiPaperTitleFont.Strikethrough ? 1 : 0;
                    researchPaperTitleStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiPaperFont.FontColor);
                    researchPaperTitleStyle.ParagraphFormat.Alignment = wsiPaperTitleFont.Alignment;
                }
                else
                {
                    var researchPaperTitleStyle = doc.Styles.Add(sResearchPaperTitle, WdStyleType.wdStyleTypeParagraph);
                    researchPaperTitleStyle.Font.Name = wsiPaperTitleFont.FontName;
                    researchPaperTitleStyle.Font.Size = wsiPaperTitleFont.FontSize;
                    researchPaperTitleStyle.Font.Bold = wsiPaperTitleFont.Bold ? 1 : 0;
                    researchPaperTitleStyle.Font.Italic = wsiPaperTitleFont.Italic ? 1 : 0;
                    researchPaperTitleStyle.Font.Underline = wsiPaperTitleFont.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    researchPaperTitleStyle.Font.StrikeThrough = wsiPaperTitleFont.Strikethrough ? 1 : 0;
                    researchPaperTitleStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiPaperTitleFont.FontColor);
                    researchPaperTitleStyle.ParagraphFormat.Alignment = wsiPaperTitleFont.Alignment;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding styles to the document: {ex.Message}", "Error");
            }
        }
    }
}
