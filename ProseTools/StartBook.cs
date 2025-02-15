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
            ApplyStyle(tocTitleParagraph.Range, doc.Styles[sProseChapter]);
            tocTitleParagraph.Range.ListFormat.RemoveNumbers();
            tocTitleParagraph.Range.Text = "Table of Contents";

            // Collapse the range to the end of the TOC title paragraph.
            Word.Range tocRange = tocTitleParagraph.Range;
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Optionally insert a blank paragraph if needed.
            tocRange.InsertParagraphAfter();
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            doc.Sections[2].Range.ListFormat.RemoveNumbers();

            // Set the font for the TOC title.
            tocRange = doc.Sections[2].Range.Paragraphs[2].Range;
            tocRange.Bold = 0;
            tocRange.Italic = 0;
            tocRange.Underline = WdUnderline.wdUnderlineNone;
            tocRange.Font.Name = fontName;
            tocRange.Font.Size = 12;
            tocRange.Font.Color = WdColor.wdColorBlack;
            tocRange.Font.StrikeThrough = 0;
            tocRange.Font.Hidden = 0;
            tocRange.ListFormat.RemoveNumbers();

            // Now add the Table of Contents.
            doc.TablesOfContents.Add(tocRange, UseHeadingStyles: true, UpperHeadingLevel: 1, LowerHeadingLevel: 3,
                UseFields: true, TableID: Type.Missing, RightAlignPageNumbers: true, IncludePageNumbers: true,
                AddedStyles: Type.Missing, UseHyperlinks: true, HidePageNumbersInWeb: false, UseOutlineLevels: true);

            ProseStart.ConfigureTOCForCustomStyle(doc, sProseChapter, 1);
            ProseStart.ConfigureTOCForCustomStyle(doc, sProseSubchapter, 2);

            doc.Sections[2].Range.Paragraphs.First.Range.set_Style(sProseChapter);

            doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;


            if (!bHasText)
            {
                // Insert Foreword Section
                AddSectionWithHeading(doc, "Foreword", sProseChapter, "Write a prose...");
                // Insert Chapter 1
                AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
            }
            else
            {
                var endSection = doc.Sections[2].Range;
                endSection.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                endSection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                InsertTitleAndSampleTextInSection(doc.Sections[3], "Foreword", "Write a prose...");

                endSection = doc.Sections[3].Range;
                endSection.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                endSection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
                InsertTitleAndSampleTextInSection(doc.Sections[4], "Chapter 1", "Below this page should begin your original text...");
            }
        }

        //    if(!bHasText)
        //    {
        //        // Insert Foreword Section
        //        AddSectionWithHeading(doc, "Foreword", sProseChapter, "Write a prose...");
        //        // Insert Chapter 1
        //        AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
        //    }
        //    else
        //    {
        //        tocRange.InsertParagraphAfter();
        //        tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //        tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
        //        tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //        // Insert Foreword Section
        //        InsertTitleAndSampleTextInSection(doc.Sections[3], "Foreword", "Write a prose...");
        //       //tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //       // tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
        //       // tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //       // tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
        //        //var forewordSection = doc.Sections[3];
        //        //var forewordRange = forewordSection.Range;
        //        //forewordRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);


        //        //forewordSection.Range.Paragraphs.Add();
        //        //forewordSection.Range.Paragraphs.Add();
        //        //forewordSection.Range.Paragraphs.Add();

        //        //var forewordParagraph = forewordSection.Range.Paragraphs[1].Range;
        //        //forewordParagraph.Text = "Foreword";
        //        //forewordParagraph.set_Style(sProseChapter);
        //        //forewordParagraph.InsertParagraphAfter();
        //        //var forewordSampleParagraph = doc.Sections[3].Range.Paragraphs[2].Range;
        //        //forewordSampleParagraph.Text = "Write a prose...";
        //        //forewordSampleParagraph.set_Style(sProseText);
        //        //forewordSampleParagraph.InsertParagraphAfter();

        //        //InsertTitleAndSampleTextInSection(doc.Sections[3], "Foreword", "Write a prose...");

        //        //var forewordRange = doc.Sections[3].Range;
        //        //forewordRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //        //forewordRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
        //        //var chapter1Section = doc.Sections[4];
        //        //chapter1Section.Range.Paragraphs.Add();
        //        //chapter1Section.Range.Paragraphs.Add();
        //        //chapter1Section.Range.Paragraphs.Add();

        //        //var chapter1Paragraph = chapter1Section.Range.Paragraphs[1];
        //        //chapter1Paragraph.Range.Text = "Chapter 1: ";
        //        //chapter1Paragraph.Range.set_Style(sProseChapter);
        //        //chapter1Paragraph.Range.InsertParagraphAfter();
        //        //var chapter1Range = doc.Sections[4].Range.Paragraphs[2].Range;
        //        //chapter1Range.Text = "Below this line should begin your original text...";
        //        //chapter1Range.set_Style(sProseText);

        //        //// Insert Chapter 1
        //        //AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
        //    }
        //}

        private void InsertTitleAndSampleTextInSection(Word.Section section, string chapterTitle, string sampleText)
        {
            // Collapse the section's range to the start.
            Word.Range sectionRange = section.Range;
            sectionRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);

            // Add three new paragraphs to the section.
            // (This ensures we have at least three paragraphs in the section.)
            section.Range.InsertParagraphBefore();
            section.Range.InsertParagraphBefore();
            section.Range.InsertParagraphBefore();

            // The first paragraph will hold the chapter title.
            Word.Paragraph titleParagraph = section.Range.Paragraphs[1];
            titleParagraph.Range.Text = chapterTitle;
            titleParagraph.Range.set_Style(sProseChapter);  // Assuming sProseChapter is a defined style
            titleParagraph.Range.InsertParagraphAfter();

            // The second paragraph will hold the sample text.
            Word.Paragraph sampleParagraph = section.Range.Paragraphs[2];
            sampleParagraph.Range.Text = sampleText;
            sampleParagraph.Range.set_Style(sProseText);  // Assuming sProseText is a defined style
            sampleParagraph.Range.InsertParagraphAfter();

            section.Range.ListFormat.RemoveNumbers();
        }


        /*
         * 
         * // ... [Code for inserting title, subtitle, spacing, "by", and author paragraphs]

        // --- 2. Insert a Section Break After the Title Block ---
        // This moves all content following the title block (if any) into a new section.
        currentRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
        // Collapse currentRange to the start of the new section (Section 2).
        Word.Section tocSection = doc.Sections[2];
        Word.Range tocRange = tocSection.Range;
        tocRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);

        // (Optional) Set Section 2’s vertical alignment to top.
        tocSection.PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalTop;

        // --- 3. Insert the Table of Contents ---
        // Insert a TOC title paragraph at the start of Section 2.
        Word.Paragraph tocTitleParagraph = doc.Paragraphs.Add(tocRange);
        tocTitleParagraph.Range.Text = "Table of Contents";
        // Apply your custom style (for example, sProseChapter) to the TOC title.
        ApplyStyle(tocTitleParagraph.Range, doc.Styles[sProseChapter]);
        tocTitleParagraph.Range.ListFormat.RemoveNumbers();
        // End the TOC title paragraph.
        tocTitleParagraph.Range.InsertParagraphAfter();

        // Set tocRange to immediately after the TOC title.
        tocRange = tocTitleParagraph.Range;
        tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

        // (Optional) Insert an extra blank paragraph if you want some spacing.
        tocRange.InsertParagraphAfter();
        tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

        // Now add the Table of Contents.
        doc.TablesOfContents.Add(
            tocRange, 
            UseHeadingStyles: true, 
            UpperHeadingLevel: 1, 
            LowerHeadingLevel: 3, 
            UseFields: true, 
            TableID: Type.Missing, 
            RightAlignPageNumbers: true, 
            IncludePageNumbers: true, 
            AddedStyles: Type.Missing, 
            UseHyperlinks: true, 
            HidePageNumbersInWeb: false, 
            UseOutlineLevels: true);

        // --- 4. Adjust Page Setup for Section 1 (Title Block) ---
        // If desired, keep Section 1 vertically centered.
        doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;


                */
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
