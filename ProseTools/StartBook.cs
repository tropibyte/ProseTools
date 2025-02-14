using com.sun.tools.@internal.xjc.outline;
using com.sun.tools.@internal.xjc.reader.gbind;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace ProseTools
{
    public partial class StartBook : Form
    {
        WordStyleInfo wsiProseChapter, wsiProseSubchapter, wsiProseText;
        public const string sProseChapter = "Prose Chapter";
        public const string sProseSubchapter = "Prose Subchapter";
        public const string sProseText = "Prose Text";

        public StartBook()
        {
            InitializeComponent();
            InitializeFontDropDown();
            InitializeStyles();
            WordStyleInfo.ApplyStyleToLabel(proseChapter, wsiProseChapter);
            WordStyleInfo.ApplyStyleToLabel(proseSubchapter, wsiProseSubchapter);
            WordStyleInfo.ApplyStyleToLabel(proseText, wsiProseText);
        }

        private void InitializeFontDropDown()
        {
            try
            {
                // Get the active Word application and document.
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Get the current font of the document content.
                string currentFont = doc.Content.Font.Name;
                // If empty, fallback to a default.
                if (string.IsNullOrWhiteSpace(currentFont))
                {
                    currentFont = "Times New Roman";
                }

                // Get the list of available fonts in Word and sort alphabetically.
                var fontNames = wordApp.FontNames.Cast<string>().OrderBy(font => font).ToList();

                // Populate the dropdown with the sorted font names.
                dropDownFont.Items.Clear();
                dropDownFont.Items.AddRange(fontNames.ToArray());

                // Set the current selection to the document's content font if available.
                if (fontNames.Contains(currentFont))
                {
                    dropDownFont.SelectedItem = currentFont;
                }
                else
                {
                    // Optionally, set a default if the currentFont isn't in the list.
                    dropDownFont.SelectedItem = "Times New Roman";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the font dropdown: {ex.Message}", "Error");
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

                // Determine the font name from the dropdown or default to "Times New Roman"
                string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";

                // Check if "Prose Chapter" style exists
                if (WordStyleInfo.StyleExists(doc, sProseChapter))
                {
                    wsiProseChapter = WordStyleInfo.GetStyleInfo(doc, sProseChapter);
                }
                else
                {
                    wsiProseChapter = new WordStyleInfo(sProseChapter, fontName, 16, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
                // Check if "Prose Subchapter" style exists
                if (WordStyleInfo.StyleExists(doc, sProseSubchapter))
                {
                    wsiProseSubchapter = WordStyleInfo.GetStyleInfo(doc, sProseSubchapter);
                }
                else
                {
                    wsiProseSubchapter = new WordStyleInfo(sProseSubchapter, fontName, 14, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
                // Check if "Prose Text" style exists
                if (WordStyleInfo.StyleExists(doc, sProseText))
                {
                    wsiProseText = WordStyleInfo.GetStyleInfo(doc, sProseText);
                }
                else
                {
                    wsiProseText = new WordStyleInfo(sProseText, fontName, 12, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing styles: {ex.Message}", "Error");
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the active Word application and document.
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;
                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Determine if the document is empty (ignoring whitespace).
                bool isDocumentEmpty = doc.Content.Words.Count < 3 && string.IsNullOrWhiteSpace(doc.Content.Text.Trim());
                bool hasTOC = (doc.TablesOfContents.Count > 0);

                // Case 1: Empty document – use current scheme.
                // Case 2: Nonempty document, without TOC – use current scheme.
                if (isDocumentEmpty || !hasTOC)
                {
                    CreateBookStart(doc);
                }
                else
                {
                    // Case 3: TOC already exists.
                    MessageBox.Show("A Table of Contents already exists. Updating styles and metadata only.",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // In either nonempty case, update the styles.
                    AddStylesToDocument(doc);

                    // Set metadata if not already present or if it’s the default.
                    if (Globals.ThisAddIn._ProseMetaData is null || Globals.ThisAddIn._ProseMetaData is NullMetaData)
                    {
                        Globals.ThisAddIn._ProseMetaData = new NovelMetaData();
                        EstablishNewOutline((NovelMetaData)Globals.ThisAddIn._ProseMetaData);
                    }
                }

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CreateBookStart(Word.Document doc)
        {
            // Determine if the document already has non‐whitespace text.
            bool bHasText = doc.Words.Count > 2 ||
                !string.IsNullOrWhiteSpace(doc.Content.Text.Trim());

            if (!SetupMetadata())
            {
                return;
            }
            // _ProseMetaData is now a valid NovelMetaData.
            var novelMetaData = (NovelMetaData)Globals.ThisAddIn._ProseMetaData;

            // Add document styles and initialize the outline.
            AddStylesToDocument(doc);
            EstablishNewOutline(novelMetaData);

            // Get an insertion point at the very beginning of the document.
            Word.Range startRange = doc.Range(0, 0);
            string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";

            int snippetLength = 100;

            // Ensure we don't exceed the document's length.
            int rangeEnd = Math.Min(doc.Content.End, snippetLength);

            // Get a range from the start of the document up to 'rangeEnd'
            var range = doc.Range(0, rangeEnd);



            // Retrieve the text of that range
            string textSnippet = range.Text;
            string trimmed = textSnippet.TrimStart();
            Paragraph firstParagraph = null;
            if (trimmed.Length > 0)
            {
                var docRange = doc.Content;
                docRange.Collapse(WdCollapseDirection.wdCollapseStart);
                docRange.InsertBreak(WdBreakType.wdSectionBreakNextPage);
                doc.Sections[1].Range.ListFormat.RemoveNumbers();
                startRange = doc.Range(0, 0);
                startRange.InsertParagraphBefore();
                startRange = doc.Range(0, 0);
                startRange.ListFormat.RemoveNumbers();
            }

            // Get the index of the paragraph that contains startRange.
            firstParagraph = startRange.Paragraphs.First;
            firstParagraph.Reset();
            firstParagraph.Range.Bold = 0;
            firstParagraph.Range.Italic = 0;
            firstParagraph.Range.Font.Name = fontName;
            firstParagraph.Range.Underline = WdUnderline.wdUnderlineNone;
            firstParagraph.Range.ListFormat.RemoveNumbers();

            // --- 1. Insert the Title Block at the Beginning ---

            // Insert Title (font size 36, centered)
            Word.Paragraph titleParagraph = doc.Paragraphs.Add(startRange);
            titleParagraph.Range.Text = title.Text.Trim();
            titleParagraph.Range.Font.Name = fontName;
            titleParagraph.Range.Font.Size = 36;
            titleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleParagraph.Range.InsertParagraphAfter();

            // Set insertion point after the title.
            Word.Range currentRange = titleParagraph.Range;
            currentRange.ListFormat.RemoveNumbers();
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Insert Subtitle if present (font size 24, centered)
            if (!string.IsNullOrWhiteSpace(subtitle.Text.Trim()))
            {
                Word.Paragraph subtitleParagraph = doc.Paragraphs.Add(currentRange);
                subtitleParagraph.Range.Text = subtitle.Text.Trim();
                subtitleParagraph.Range.Font.Name = fontName;
                subtitleParagraph.Range.Font.Size = 24;
                subtitleParagraph.Range.Bold = 0;
                subtitleParagraph.Range.Italic = 0;
                subtitleParagraph.Range.Underline = WdUnderline.wdUnderlineNone;
                subtitleParagraph.Range.ListFormat.RemoveNumbers();
                subtitleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                subtitleParagraph.Range.InsertParagraphAfter();
                currentRange = subtitleParagraph.Range;
                currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            }

            // Insert 3 blank paragraphs (for spacing)
            for (int i = 0; i < 3; i++)
            {
                Word.Paragraph blankPara = doc.Paragraphs.Add(currentRange);
                blankPara.Range.Text = "";
                blankPara.Range.Bold = 0;
                blankPara.Range.Italic = 0;
                blankPara.Range.Underline = WdUnderline.wdUnderlineNone;
                blankPara.Range.ListFormat.RemoveNumbers();
                blankPara.Range.InsertParagraphAfter();
                currentRange = blankPara.Range;
                currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                currentRange.ListFormat.RemoveNumbers();
            }

            // Insert the "by" paragraph (font size 18, centered)
            Word.Paragraph byParagraph = doc.Paragraphs.Add(currentRange);
            byParagraph.Range.Text = "by";
            byParagraph.Range.Font.Name = fontName;
            byParagraph.Range.Font.Size = 18;
            byParagraph.Range.Bold = 0;
            byParagraph.Range.Italic = 0;
            byParagraph.Range.Underline = WdUnderline.wdUnderlineNone;
            byParagraph.Range.ListFormat.RemoveNumbers();
            byParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            byParagraph.Range.InsertParagraphAfter();
            currentRange = byParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            currentRange.ListFormat.RemoveNumbers();

            // Insert the Author Name paragraph (font size 18, centered)
            Word.Paragraph authorParagraph = doc.Paragraphs.Add(currentRange);
            authorParagraph.Range.Text = author.Text.Trim();
            authorParagraph.Range.Font.Name = fontName;
            authorParagraph.Range.Font.Size = 18;
            authorParagraph.Range.Bold = 0;
            authorParagraph.Range.Italic = 0;
            authorParagraph.Range.Underline = WdUnderline.wdUnderlineNone;
            authorParagraph.Range.ListFormat.RemoveNumbers();
            authorParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            authorParagraph.Range.InsertParagraphAfter();
            currentRange = authorParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            currentRange.ListFormat.RemoveNumbers();

            // Insert a page break for the Table of Contents
            currentRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            currentRange.ListFormat.RemoveNumbers();

            // Add Table of Contents Title
            var tocTitleParagraph = doc.Content.Paragraphs.Add(currentRange);
            tocTitleParagraph.Range.Text = "Table of Contents";
            ApplyStyle(tocTitleParagraph.Range, doc.Styles[sProseChapter]);
            tocTitleParagraph.Range.ListFormat.RemoveNumbers();

            // Collapse the range to the end of the TOC title paragraph.
            Word.Range tocRange = tocTitleParagraph.Range;
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Insert a new paragraph after the TOC title (optional blank line).
            tocRange.InsertParagraphAfter();
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            tocRange.Font.Size = 12;
            tocRange.Bold = 0;
            tocRange.Italic = 0;
            tocRange.Underline = WdUnderline.wdUnderlineNone;
            tocRange.ListFormat.RemoveNumbers();

            // Now add the Table of Contents. Adjust the parameters as needed.
            doc.TablesOfContents.Add(tocRange, UseHeadingStyles: true, UpperHeadingLevel: 1, LowerHeadingLevel: 3,
                UseFields: true, TableID: Type.Missing, RightAlignPageNumbers: true, IncludePageNumbers: true,
                AddedStyles: Type.Missing, UseHyperlinks: true, HidePageNumbersInWeb: false, UseOutlineLevels: true);

            ProseStart.ConfigureTOCForCustomStyle(doc, sProseChapter, 1);
            ProseStart.ConfigureTOCForCustomStyle(doc, sProseSubchapter, 2);

            doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;

            if(!bHasText)
            {
                // Insert Foreword Section
                AddSectionWithHeading(doc, "Foreword", sProseChapter, "Write a prose...");
                // Insert Chapter 1
                AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
            }
            else
            {
                tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                // Insert Chapter 1
                AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
                tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);

            }
        }
        
        /*

            // Add Table of Contents Title
            var tocTitleParagraph = doc.Content.Paragraphs.Add(currentRange);
            tocTitleParagraph.Range.Text = "Table of Contents";
            currentRange = tocTitleParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            currentRange.ListFormat.RemoveNumbers();

            // Apply ProseChapter style
            ApplyStyle(tocTitleParagraph.Range, doc.Styles[sProseChapter]);
   //         currentRange.InsertParagraphAfter();

            // Add Table of Contents to the document
                        //var tocRange = doc.Content;
                        //tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                        //tocRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        //tocRange.Font.Size = 12;
                        //doc.TablesOfContents.Add(tocRange, true, 1, 3);
                        //ProseStart.ConfigureTOCForCustomStyle(doc, sProseChapter, 1);
                        //ProseStart.ConfigureTOCForCustomStyle(doc, sProseSubchapter, 2);
            // --- 3. Adjust Page Setup ---
            // Set Section 1’s vertical alignment to center so the title block is centered on page 1.
            
        }

        */

        private void CreateBookStartXXXXX(Word.Document doc)
        {
            // Determine if the document already has non‐whitespace text.
            bool bHasText = doc.Words.Count > 2 ||
                !string.IsNullOrWhiteSpace(doc.Content.Text.Trim());

            if (!SetupMetadata())
            {
                return;
            }
            // _ProseMetaData is now a valid NovelMetaData.
            var novelMetaData = (NovelMetaData)Globals.ThisAddIn._ProseMetaData;

            // Add document styles and initialize the outline.
            AddStylesToDocument(doc);
            EstablishNewOutline(novelMetaData);

            // We want to insert our title block at the very beginning,
            // regardless of whether there’s existing text.
            Word.Range startRange = doc.Range(0, 0);
            string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";

            // --- 1. Insert the Title Block at the Beginning ---

            // Insert Title (font size 36, centered)
            Word.Paragraph titleParagraph = doc.Paragraphs.Add(startRange);
            titleParagraph.Range.Text = title.Text.Trim();
            titleParagraph.Range.Font.Name = fontName;
            titleParagraph.Range.Font.Size = 36;
            titleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleParagraph.Range.InsertParagraphAfter();

            // Get insertion point after the title
            Word.Range currentRange = titleParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Insert 3 blank paragraphs (for spacing)
            for (int i = 0; i < 3; i++)
            {
                Word.Paragraph blankPara = doc.Paragraphs.Add(currentRange);
                blankPara.Range.Text = "";
                blankPara.Range.InsertParagraphAfter();
                currentRange = blankPara.Range;
                currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            }

            // Insert the "by" paragraph (font size 18, centered)
            Word.Paragraph byParagraph = doc.Paragraphs.Add(currentRange);
            byParagraph.Range.Text = "by";
            byParagraph.Range.Font.Name = fontName;
            byParagraph.Range.Font.Size = 18;
            byParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            byParagraph.Range.InsertParagraphAfter();
            currentRange = byParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Insert the Author Name paragraph (font size 18, centered)
            Word.Paragraph authorParagraph = doc.Paragraphs.Add(currentRange);
            authorParagraph.Range.Text = author.Text.Trim();
            authorParagraph.Range.Font.Name = fontName;
            authorParagraph.Range.Font.Size = 18;
            authorParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            authorParagraph.Range.InsertParagraphAfter();
            currentRange = authorParagraph.Range;
            currentRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            currentRange.InsertBreak(Word.WdBreakType.wdPageBreak);

            // Add Table of Contents Title
            var tocTitleParagraph = doc.Content.Paragraphs.Add();
            tocTitleParagraph.Range.Text = "Table of Contents";

            // Apply ProseChapter style
            ApplyStyle(tocTitleParagraph.Range, doc.Styles[sProseChapter]);
            tocTitleParagraph.Range.InsertParagraphAfter();

            // Add Table of Contents to the document
            var tocRange = doc.Content;
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            tocRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            tocRange.Font.Size = 12;
            doc.TablesOfContents.Add(tocRange, true, 1, 3);
            ProseStart.ConfigureTOCForCustomStyle(doc, sProseChapter, 1);
            ProseStart.ConfigureTOCForCustomStyle(doc, sProseSubchapter, 2);

            // --- 2. Insert a Section Break After the Title Block ---
            // This moves all content following the title block (if any) into Section 2.
            // That way, Section 1 (page 1) contains only the title block.
            if (!bHasText)
            {
                // If the document is empty, we need to insert a page break
                // so that any future content starts on a new page.
//                
            }
            else
            {
                // If the document has text, we need to insert a section break
                // so that the text moves to Section 2.
                currentRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            }

            // --- 3. Adjust Page Setup ---
            // Set Section 1’s vertical alignment to center (so the title block is centered on page 1).
            doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;

            // At this point:
            // • If the document originally had text, that content (along with its formatting)
            //   has been pushed to Section 2 (page 2).
            // • If the document was blank, Section 2 will be empty.
        }

        private void CreateBookStartForExistingDocuments(Word.Document doc)
        {
            bool bHasText = doc.Words.Count > 2 || !string.IsNullOrWhiteSpace(doc.Content.Text.Trim());

            if (!SetupMetadata())
            {
                return;
            }
            // Now we are guaranteed that _ProseMetaData is a valid NovelMetaData
            var novelMetaData = (NovelMetaData)Globals.ThisAddIn._ProseMetaData;

            // Add styles to the document
            AddStylesToDocument(doc);

            // Initialize the Outline for the outline
            EstablishNewOutline(novelMetaData);

            // If there is original text, insert a section break at the very beginning.
            // This pushes the original text into Section 2 while Section 1 becomes our title page.
            ProseStart.InsertSection(doc, Word.WdCollapseDirection.wdCollapseStart, Word.WdBreakType.wdSectionBreakNextPage);

            // Create the title page
            CreateBookTitlePage(doc);

            // In the case of an originally empty document, there’s no Section 2.
            // Therefore, add a page break at the end of the title block so that any future content starts on a new page.
            if (!bHasText)
            {
                doc.Content.InsertBreak(Word.WdBreakType.wdPageBreak);
            }
        }

        private void CreateBookTitlePage(Document doc)
        {
            // Work with Section 1 (the title page)
            Word.Section titleSection = doc.Sections[1];
            // Vertically center the title page
            titleSection.PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;

            // Get a range for Section 1 and collapse it to the start
            Word.Range titleRange = titleSection.Range;
            titleRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);

            string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";

            // Insert Title Paragraph (font size 36, centered)
            int nParagraphCount = titleSection.Range.Paragraphs.Count;
            Word.Paragraph titleParagraph = titleSection.Range.Paragraphs.Add(titleRange);
            nParagraphCount = titleSection.Range.Paragraphs.Count;
            titleParagraph.Range.Text = title.Text.Trim();
            titleParagraph.Range.Font.Name = fontName;
            titleParagraph.Range.Font.Size = 36;
            titleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            nParagraphCount = titleSection.Range.Paragraphs.Count;
        }

        private void EstablishNewOutline(NovelMetaData novelMetaData)
        {
            var outline = novelMetaData.TheOutline;
            var rootNode = new OutlineNode
            {
                Title = "Book Metadata",
                Details = "Metadata for the book",
                Attributes = new Dictionary<string, string>
        {
            { "ProseType", "Book" },
            { "SubType", "Fiction" },
            { "Genre", "Mystery" } // Example genre, can be dynamic
        }
            };
            outline.AddNode(rootNode);

            // Add title to the outline as a child of the root node
            var titleNode = new OutlineNode
            {
                Title = "Title",
                Details = $"Book title: {title.Text.Trim()}"
            };
            rootNode.AddChild(titleNode);

            // Insert Subtitle if present
            if (!string.IsNullOrWhiteSpace(subtitle.Text.Trim()))
            {
                // Add subtitle to the outline
                var subtitleNode = new OutlineNode
                {
                    Title = "Subtitle",
                    Details = $"Subtitle: {subtitle.Text.Trim()}"
                };
                rootNode.AddChild(subtitleNode);
            }

            rootNode.AddChild(new OutlineNode
            {
                Title = "Foreword",
                Details = "Foreword section of the book."
            });

            rootNode.AddChild(new OutlineNode
            {
                Title = "Chapter 1",
                Details = "This is the first chapter of the book."
            });

        }

        private bool SetupMetadata()
        {
            // If there is no metadata or it is a NullMetaData, initialize a new NovelMetaData
            if (Globals.ThisAddIn._ProseMetaData is null || Globals.ThisAddIn._ProseMetaData is NullMetaData)
            {
                Globals.ThisAddIn._ProseMetaData = new NovelMetaData();
                return true;
            }
            // If ProseMetaData already exists as NovelMetaData, fail because the book is already started
            else if (Globals.ThisAddIn._ProseMetaData is NovelMetaData)
            {
                MessageBox.Show("A book has already been started in this document.", "Error");
                return false;
            }
            // If ProseMetaData is another type (ResearchPaperMetaData, ScreenplayMetaData), fail
            else
            {
                MessageBox.Show("This document contains non-novel metadata. The book creation feature is only available for novels.", "Error");
                return false;
            }
        }

        private void InsertCenteredText(Word.Document doc, string text, string fontName, int fontSize)
        {
            var paragraph = doc.Content.Paragraphs.Add();
            paragraph.Range.Text = text;
            paragraph.Range.Font.Name = fontName;
            paragraph.Range.Font.Size = fontSize;
            paragraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();
        }

        private void InsertCenteredText(Word.Range range, string text, string fontName, int fontSize)
        {
            int nCount = range.Paragraphs.Count;
            Word.Paragraph paragraph = range.Paragraphs.Add();
            paragraph.Range.Text = text;
            paragraph.Range.Font.Name = fontName;
            paragraph.Range.Font.Size = fontSize;
            paragraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            nCount = range.Paragraphs.Count;
            paragraph.Range.InsertParagraphAfter();
            nCount = range.Paragraphs.Count;
        }

        private void ApplyStyle(Word.Range range, Word.Style style)
        {
            range.Font.Name = style.Font.Name;
            range.Font.Size = style.Font.Size;
            range.Font.Bold = style.Font.Bold;
            range.Font.Italic = style.Font.Italic;
            range.Font.Underline = style.Font.Underline;
            range.Font.Color = style.Font.Color;
            range.ParagraphFormat.Alignment = style.ParagraphFormat.Alignment;
        }

        private void AddSectionWithHeading(Word.Document doc, string heading, string style, string content)
        {
            ProseStart.InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);
            var range = doc.Content;
            range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            var headingParagraph = range.Paragraphs.Add();
            headingParagraph.Range.Text = heading;
            headingParagraph.Range.set_Style(style);
            headingParagraph.Range.InsertParagraphAfter();

            var bodyParagraph = doc.Content.Paragraphs.Add();
            bodyParagraph.Range.Text = content;
            bodyParagraph.set_Style(sProseText);
            bodyParagraph.Range.InsertParagraphAfter();
        }


        private void SetProseChapterFontInfoButton_Click(object sender, EventArgs e)
        {
            if (WordStyleInfo.ShowFontDialogForWSI(ref wsiProseChapter) == DialogResult.OK)
            {
                WordStyleInfo.ApplyStyleToLabel(proseChapter, wsiProseChapter);
            }
        }

        private void SetProseSubchapterFontInfoButton_Click(object sender, EventArgs e)
        {
            if (WordStyleInfo.ShowFontDialogForWSI(ref wsiProseSubchapter) == DialogResult.OK)
            {
                WordStyleInfo.ApplyStyleToLabel(proseSubchapter, wsiProseSubchapter);
            }
        }

        private void SetProseTextFontInfoButton_Click(object sender, EventArgs e)
        {
            if (WordStyleInfo.ShowFontDialogForWSI(ref wsiProseText) == DialogResult.OK)
            {
                WordStyleInfo.ApplyStyleToLabel(proseText, wsiProseText);
            }
        }

        private void AddStylesToDocument(Document doc)
        {
            try
            {
                // Add or update ProseChapter style
                if (WordStyleInfo.StyleExists(doc, sProseChapter))
                {
                    var proseChapterStyle = doc.Styles[sProseChapter];
                    proseChapterStyle.Font.Name = wsiProseChapter.FontName;
                    proseChapterStyle.Font.Size = wsiProseChapter.FontSize;
                    proseChapterStyle.Font.Bold = wsiProseChapter.Bold ? 1 : 0;
                    proseChapterStyle.Font.Italic = wsiProseChapter.Italic ? 1 : 0;
                    proseChapterStyle.Font.Underline = wsiProseText.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseChapterStyle.Font.StrikeThrough = wsiProseText.Strikethrough ? 1 : 0;
                    proseChapterStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseChapter.FontColor);
                    proseChapterStyle.ParagraphFormat.Alignment = wsiProseChapter.Alignment;
                }
                else
                {
                    var proseChapterStyle = doc.Styles.Add(sProseChapter, WdStyleType.wdStyleTypeParagraph);
                    proseChapterStyle.Font.Name = wsiProseChapter.FontName;
                    proseChapterStyle.Font.Size = wsiProseChapter.FontSize;
                    proseChapterStyle.Font.Bold = wsiProseChapter.Bold ? 1 : 0;
                    proseChapterStyle.Font.Italic = wsiProseChapter.Italic ? 1 : 0;
                    proseChapterStyle.Font.Underline = wsiProseChapter.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseChapterStyle.Font.StrikeThrough = wsiProseChapter.Strikethrough ? 1 : 0;
                    proseChapterStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseChapter.FontColor);
                    proseChapterStyle.ParagraphFormat.Alignment = wsiProseChapter.Alignment;
                }

                // Add or update ProseChapter style
                if (WordStyleInfo.StyleExists(doc, sProseSubchapter))
                {
                    var proseSubchapterStyle = doc.Styles[sProseSubchapter];
                    proseSubchapterStyle.Font.Name = wsiProseSubchapter.FontName;
                    proseSubchapterStyle.Font.Size = wsiProseSubchapter.FontSize;
                    proseSubchapterStyle.Font.Bold = wsiProseSubchapter.Bold ? 1 : 0;
                    proseSubchapterStyle.Font.Italic = wsiProseSubchapter.Italic ? 1 : 0;
                    proseSubchapterStyle.Font.Underline = wsiProseSubchapter.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseSubchapterStyle.Font.StrikeThrough = wsiProseSubchapter.Strikethrough ? 1 : 0;
                    proseSubchapterStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseSubchapter.FontColor);
                    proseSubchapterStyle.ParagraphFormat.Alignment = wsiProseSubchapter.Alignment;
                }
                else
                {
                    var proseSubchapterStyle = doc.Styles.Add(sProseSubchapter, WdStyleType.wdStyleTypeParagraph);
                    proseSubchapterStyle.Font.Name = wsiProseSubchapter.FontName;
                    proseSubchapterStyle.Font.Size = wsiProseSubchapter.FontSize;
                    proseSubchapterStyle.Font.Bold = wsiProseSubchapter.Bold ? 1 : 0;
                    proseSubchapterStyle.Font.Italic = wsiProseSubchapter.Italic ? 1 : 0;
                    proseSubchapterStyle.Font.Underline = wsiProseSubchapter.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseSubchapterStyle.Font.StrikeThrough = wsiProseSubchapter.Strikethrough ? 1 : 0;
                    proseSubchapterStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseSubchapter.FontColor);
                    proseSubchapterStyle.ParagraphFormat.Alignment = wsiProseSubchapter.Alignment;
                }
                // Add or update "Prose Text" style
                if (WordStyleInfo.StyleExists(doc, sProseText))
                {
                    var proseTextStyle = doc.Styles[sProseText];
                    proseTextStyle.Font.Name = wsiProseText.FontName;
                    proseTextStyle.Font.Size = wsiProseText.FontSize;
                    proseTextStyle.Font.Bold = wsiProseText.Bold ? 1 : 0;
                    proseTextStyle.Font.Italic = wsiProseText.Italic ? 1 : 0;
                    proseTextStyle.Font.Underline = wsiProseText.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseTextStyle.Font.StrikeThrough = wsiProseText.Strikethrough ? 1 : 0;
                    proseTextStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseText.FontColor);
                    proseTextStyle.ParagraphFormat.Alignment = wsiProseText.Alignment;
                }
                else
                {
                    var proseTextStyle = doc.Styles.Add(sProseText, WdStyleType.wdStyleTypeParagraph);
                    proseTextStyle.Font.Name = wsiProseText.FontName;
                    proseTextStyle.Font.Size = wsiProseText.FontSize;
                    proseTextStyle.Font.Bold = wsiProseText.Bold ? 1 : 0;
                    proseTextStyle.Font.Italic = wsiProseText.Italic ? 1 : 0;
                    proseTextStyle.Font.Underline = wsiProseText.Underline ? WdUnderline.wdUnderlineSingle : WdUnderline.wdUnderlineNone;
                    proseTextStyle.Font.StrikeThrough = wsiProseText.Strikethrough ? 1 : 0;
                    proseTextStyle.Font.Color = (WdColor)ColorTranslator.ToOle(wsiProseText.FontColor);
                    proseTextStyle.ParagraphFormat.Alignment = wsiProseText.Alignment;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding styles to the document: {ex.Message}", "Error");
            }
        }

    }


}
