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
        WordStyleInfo wsiProseChapter, wsiProseText;

        public StartBook()
        {
            InitializeComponent();
            InitializeFontDropDown();
            InitializeStyles();
            ApplyStyleToLabel(proseChapter, wsiProseChapter);
            ApplyStyleToLabel(proseText, wsiProseText);
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
                if (StyleExists(doc, "Prose Chapter"))
                {
                    wsiProseChapter = GetStyleInfo(doc, "Prose Chapter");
                }
                else
                {
                    wsiProseChapter = new WordStyleInfo("Prose Chapter", fontName, 16, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }

                // Check if "Prose Text" style exists
                if (StyleExists(doc, "Prose Text"))
                {
                    wsiProseText = GetStyleInfo(doc, "Prose Text");
                }
                else
                {
                    wsiProseText = new WordStyleInfo("Prose Text", fontName, 12, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing styles: {ex.Message}", "Error");
            }
        }

        private bool StyleExists(Document doc, string styleName)
        {
            try
            {
                var style = doc.Styles[styleName];
                return true;
            }
            catch
            {
                return false;
            }
        }

        private WordStyleInfo GetStyleInfo(Document doc, string styleName)
        {
            var style = doc.Styles[styleName];
            return new WordStyleInfo(
                style.NameLocal,
                style.Font.Name,
                style.Font.Size,
                style.Font.Bold == 1,
                style.Font.Italic == 1,
                Color.FromArgb((int)style.Font.Color),
                style.ParagraphFormat.Alignment
            );
        }

        private void ApplyStyleToLabel(System.Windows.Forms.Label label, WordStyleInfo styleInfo)
        {
            if (label == null || styleInfo == null) return;

            // Set font style
            FontStyle fontStyle = FontStyle.Regular;
            if (styleInfo.Bold) fontStyle |= FontStyle.Bold;
            if (styleInfo.Italic) fontStyle |= FontStyle.Italic;

            // Apply font, size, and color to the label
            label.Font = new System.Drawing.Font(styleInfo.FontName, styleInfo.FontSize, fontStyle);
            label.ForeColor = styleInfo.FontColor;

            // Set alignment if needed
            ContentAlignment alignment;
            switch (styleInfo.Alignment)
            {
                case WdParagraphAlignment.wdAlignParagraphCenter:
                    alignment = ContentAlignment.MiddleCenter;
                    break;
                case WdParagraphAlignment.wdAlignParagraphLeft:
                    alignment = ContentAlignment.MiddleLeft;
                    break;
                case WdParagraphAlignment.wdAlignParagraphRight:
                    alignment = ContentAlignment.MiddleRight;
                    break;
                default:
                    alignment = ContentAlignment.MiddleLeft; // Default to left alignment
                    break;
            }

            label.TextAlign = alignment;
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
                this.Close();

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateBookStart(Word.Document doc)
        {
            // Add a new section for the title page
            var titleRange = doc.Content;

            string fontName = dropDownFont.SelectedItem?.ToString() ?? "Times New Roman";

            if (!string.IsNullOrWhiteSpace(titleRange.Text.Trim()))
            {
                var firstPageRange = doc.Content;
                firstPageRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);
                firstPageRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            }

            // Vertically center the title page
            doc.Sections[1].PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalCenter;

            // Insert Title
            var titleParagraph = doc.Content.Paragraphs.Add();
            titleParagraph.Range.Text = title.Text.Trim();
            titleParagraph.Range.Font.Name = fontName;
            titleParagraph.Range.Font.Size = 36;
            titleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleParagraph.Range.InsertParagraphAfter();

            // Insert Subtitle
            if (!string.IsNullOrWhiteSpace(subtitle.Text.Trim()))
            {
                var subtitleParagraph = doc.Content.Paragraphs.Add();
                subtitleParagraph.Range.Text = subtitle.Text.Trim();
                subtitleParagraph.Range.Font.Name = fontName;
                subtitleParagraph.Range.Font.Size = 24;
                subtitleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                subtitleParagraph.Range.InsertParagraphAfter();
            }

            // Insert Spacer
            for (int i = 0; i < 3; i++)
            {
                var spacerParagraph = doc.Content.Paragraphs.Add();
                spacerParagraph.Range.Text = ""; // Empty line
                spacerParagraph.Range.InsertParagraphAfter();
            }

            // Insert "by"
            var byParagraph = doc.Content.Paragraphs.Add();
            byParagraph.Range.Text = "by";
            byParagraph.Range.Font.Name = fontName;
            byParagraph.Range.Font.Size = 18;
            byParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            byParagraph.Range.InsertParagraphAfter();

            // Insert Author Name
            var authorParagraph = doc.Content.Paragraphs.Add();
            authorParagraph.Range.Text = author.Text.Trim();
            authorParagraph.Range.Font.Name = fontName;
            authorParagraph.Range.Font.Size = 18;
            authorParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            authorParagraph.Range.InsertParagraphAfter();

            // Insert page and section break
            InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);

            // Insert "Table of Contents" title
            var tocTitleParagraph = doc.Content.Paragraphs.Add();
            tocTitleParagraph.Range.Text = "Table of Contents";
            tocTitleParagraph.Range.Font.Name = fontName;
            tocTitleParagraph.Range.Font.Size = 14;
            tocTitleParagraph.Range.Font.Bold = 1; // Make it bold
            tocTitleParagraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            tocTitleParagraph.Range.InsertParagraphAfter();
            tocTitleParagraph.Range.InsertParagraphAfter();

            // Insert Table of Contents
            var tocRange = doc.Content;
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            tocRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            tocRange.Font.Size = 12;
            doc.TablesOfContents.Add(tocRange, true, 1, 3);

            // Insert page and section break
            InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);

            // Insert "Foreword" heading
            var forewordRange = doc.Content;
            forewordRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            forewordRange.Text = "Foreword";
            forewordRange.set_Style("Heading 1");
            forewordRange.InsertParagraphAfter();
        }

        //// Insert page and section break
        //var breakRange = doc.Content;
        //breakRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //breakRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);

        //// Insert Table of Contents
        //var tocRange = doc.Content;
        //tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //tocRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //doc.TablesOfContents.Add(tocRange, true, 1, 3);

        //// Insert page and section break
        //tocRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);

        //// Insert "Foreword" heading
        //var forewordRange = doc.Content;
        //forewordRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //forewordRange.Text = "Foreword";
        //forewordRange.set_Style("Heading 1");
        //forewordRange.InsertParagraphAfter();

        //// Insert page and section break after "Foreword"
        //forewordRange.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);

        //// Insert "Chapter 1:" heading
        //var chapterRange = doc.Content;
        //chapterRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
        //chapterRange.Text = "Chapter 1:";
        //chapterRange.set_Style("Heading 1");
        //chapterRange.InsertParagraphAfter();

        //// Update the Table of Contents
        //doc.TablesOfContents[1].Update();
        // }

        private void InsertSection(Document doc, WdCollapseDirection wdCollapse, WdBreakType wdBreakType)
        {
            // Insert the appropriate section break
            var breakRange = doc.Content;
            breakRange.Collapse(wdCollapse);
            breakRange.InsertBreak(wdBreakType);

            // Configure the newly created section
            var newSection = doc.Sections[doc.Sections.Count];

            // Set the vertical alignment to top
            newSection.PageSetup.VerticalAlignment = Word.WdVerticalAlignment.wdAlignVerticalTop;

            // Set the horizontal alignment to left
            var newSectionRange = newSection.Range;
            newSectionRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        }

        private void SetProseChapterFontInfoButton_Click(object sender, EventArgs e)
        {
            var testFont = new System.Drawing.Font("Aptos", 12, FontStyle.Regular);
            MessageBox.Show(testFont.Name); // Check what GDI+ resolves the font to

            // Create a new FontDialog instance
            using (PTFontDialog fontDialog = new PTFontDialog())
            {
                // Set initial properties for the FontDialog based on wsiProseChapter
                fontDialog.ShowColor = true; // Allow the user to select a color
                fontDialog.Font = new System.Drawing.Font(wsiProseChapter.FontName, wsiProseChapter.FontSize,
                    (wsiProseChapter.Bold ? FontStyle.Bold : FontStyle.Regular) |
                    (wsiProseChapter.Italic ? FontStyle.Italic : FontStyle.Regular));
                fontDialog.Color = wsiProseChapter.FontColor; // Set the initial color

                fontDialog.SelectedFontName = wsiProseChapter.FontName;
                fontDialog.SelectedFontSize = wsiProseChapter.FontSize;
                fontDialog.SelectedBold = wsiProseChapter.Bold;
                fontDialog.SelectedItalic = wsiProseChapter.Italic;
                //fontDialog.SelectedUnderline = wsiProseChapter.;
                //fontDialog.SelectedStrike = chkBoxStrike.Checked;
                fontDialog.SelectedFontColor = wsiProseChapter.FontColor;



                // Show the FontDialog and check if the user clicked OK
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    // Retrieve the selected font and color
                    System.Drawing.Font selectedFont = fontDialog.Font;
                    Color selectedColor = fontDialog.Color;

                    // Update wsiProseChapter with the new values
                    wsiProseChapter.FontName = selectedFont.Name;
                    wsiProseChapter.FontSize = selectedFont.Size;
                    wsiProseChapter.Bold = selectedFont.Bold;
                    wsiProseChapter.Italic = selectedFont.Italic;
                    wsiProseChapter.FontColor = selectedColor;

                    // Display the selected font information (for demonstration purposes)
                    MessageBox.Show($"Selected Font: {selectedFont.Name}, Size: {selectedFont.Size}, Color: {selectedColor.Name}", "Font Selected");

                    ApplyStyleToLabel(proseChapter, wsiProseChapter);

                }
            }
        }
    }


}
