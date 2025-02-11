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
                // Get the active Word application and document
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Get the current font of the document content
                string currentFont = doc.Content.Font.Name;

                // Get the list of available fonts in Word and sort alphabetically
                var fontNames = wordApp.FontNames.Cast<string>().OrderBy(font => font).ToList();

                // Populate the dropdown with the sorted font names
                dropDownFont.Items.Clear();
                dropDownFont.Items.AddRange(fontNames.ToArray());

                // Set the current selection to the document's content font
                if (fontNames.Contains(currentFont))
                {
                    dropDownFont.SelectedItem = currentFont;
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
                // Get the active Word application and document
                var wordApp = Globals.ThisAddIn.Application;
                var doc = wordApp.ActiveDocument;

                if (doc == null)
                {
                    MessageBox.Show("No active Word document found. Please open a document and try again.", "Error");
                    return;
                }

                // Check if the document already contains a Table of Contents
                if (doc.TablesOfContents.Count > 0)
                {
                    MessageBox.Show("The document already contains a Table of Contents. Please use a new document.", "Warning");
                    return;
                }

                CreateBookStart(doc);
                // Close the form
                DialogResult = DialogResult.OK;
                this.Close();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();

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
            // If there is no metadata or it is a NullMetaData, initialize a new NovelMetaData
            if (Globals.ThisAddIn._ProseMetaData is null || Globals.ThisAddIn._ProseMetaData is NullMetaData)
            {
                Globals.ThisAddIn._ProseMetaData = new NovelMetaData();
            }
            // If ProseMetaData already exists as NovelMetaData, fail because the book is already started
            else if (Globals.ThisAddIn._ProseMetaData is NovelMetaData)
            {
                MessageBox.Show("A book has already been started in this document.", "Error");
                return;
            }
            // If ProseMetaData is another type (ResearchPaperMetaData, ScreenplayMetaData), fail
            else
            {
                MessageBox.Show("This document contains non-novel metadata. The book creation feature is only available for novels.", "Error");
                return;
            }

            // Now we are guaranteed that _ProseMetaData is a valid NovelMetaData
            var novelMetaData = (NovelMetaData)Globals.ThisAddIn._ProseMetaData;

            // Add styles to the document
            AddStylesToDocument(doc);

            // Initialize the root node for the outline
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

            // Add a section break if the document is not empty
            if (!string.IsNullOrWhiteSpace(doc.Content.Text.Trim()))
            {
                var firstPageRange = doc.Content;
                firstPageRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);
                firstPageRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            }

            // Vertically center the title page
            doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;

            // Insert Title
            string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = title.Text.Trim();
            titleParagraph.Range.Font.Name = fontName;
            titleParagraph.Range.Font.Size = 36;
            titleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleParagraph.Range.InsertParagraphAfter();

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
                var subtitleParagraph = doc.Content.Paragraphs.Add();
                subtitleParagraph.Range.Text = subtitle.Text.Trim();
                subtitleParagraph.Range.Font.Name = fontName;
                subtitleParagraph.Range.Font.Size = 24;
                subtitleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                subtitleParagraph.Range.InsertParagraphAfter();

                // Add subtitle to the outline
                var subtitleNode = new OutlineNode
                {
                    Title = "Subtitle",
                    Details = $"Subtitle: {subtitle.Text.Trim()}"
                };
                rootNode.AddChild(subtitleNode);
            }

            // Insert spacer lines
            for (int i = 0; i < 3; i++)
            {
                var spacerParagraph = doc.Content.Paragraphs.Add();
                spacerParagraph.Range.Text = ""; // Empty line
                spacerParagraph.Range.InsertParagraphAfter();
            }

            // Insert "by" and Author Name
            InsertCenteredText(doc, "by", fontName, 18);
            InsertCenteredText(doc, author.Text.Trim(), fontName, 18);

            // Insert a page break for the Table of Contents
            ProseStart.InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);

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

            // Insert Foreword Section
            AddSectionWithHeading(doc, "Foreword", sProseChapter, "Write a prose...");
            rootNode.AddChild(new OutlineNode
            {
                Title = "Foreword",
                Details = "Foreword section of the book."
            });

            // Insert Chapter 1
            AddSectionWithHeading(doc, "Chapter 1: ", sProseChapter, "Once upon a time, an author began his/her book...");
            rootNode.AddChild(new OutlineNode
            {
                Title = "Chapter 1",
                Details = "This is the first chapter of the book."
            });

            // Update the Table of Contents
            ProseStart.UpdateDocumentTOC(doc);
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
