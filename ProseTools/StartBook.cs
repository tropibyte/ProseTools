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
            ApplyStyleToLabel(proseChapter, wsiProseChapter);
            ApplyStyleToLabel(proseSubchapter, wsiProseSubchapter);
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
                if (StyleExists(doc, sProseChapter))
                {
                    wsiProseChapter = GetStyleInfo(doc, sProseChapter);
                }
                else
                {
                    wsiProseChapter = new WordStyleInfo(sProseChapter, fontName, 16, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
                // Check if "Prose Subchapter" style exists
                if (StyleExists(doc, sProseSubchapter))
                {
                    wsiProseSubchapter = GetStyleInfo(doc, sProseSubchapter);
                }
                else
                {
                    wsiProseSubchapter = new WordStyleInfo(sProseSubchapter, fontName, 14, false, false, Color.Black, WdParagraphAlignment.wdAlignParagraphLeft);
                }
                // Check if "Prose Text" style exists
                if (StyleExists(doc, sProseText))
                {
                    wsiProseText = GetStyleInfo(doc, sProseText);
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
            if (styleInfo.Underline) fontStyle |= FontStyle.Underline;
            if (styleInfo.Strikethrough) fontStyle |= FontStyle.Strikeout;

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

            AddStylesToDocument(doc);

            // Insert page and section break
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

            // Retrieve the ProseChapter style attributes
            var proseChapterStyle = doc.Styles[sProseChapter];
            tocTitleParagraph.Range.Font.Name = proseChapterStyle.Font.Name;
            tocTitleParagraph.Range.Font.Size = proseChapterStyle.Font.Size;
            tocTitleParagraph.Range.Font.Bold = proseChapterStyle.Font.Bold;
            tocTitleParagraph.Range.Font.Italic = proseChapterStyle.Font.Italic;
            tocTitleParagraph.Range.Font.Underline = proseChapterStyle.Font.Underline;
            tocTitleParagraph.Range.Font.Color = proseChapterStyle.Font.Color;
            tocTitleParagraph.Range.ParagraphFormat.Alignment = proseChapterStyle.ParagraphFormat.Alignment;

            // Insert the paragraph after applying formatting
            tocTitleParagraph.Range.InsertParagraphAfter();
            tocTitleParagraph.Range.InsertParagraphAfter();


            // Insert Table of Contents
            var tocRange = doc.Content;
            tocRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            tocRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            tocRange.Font.Size = 12;
            doc.TablesOfContents.Add(tocRange, true, 1, 3);
            ConfigureTOCForCustomStyle(doc, sProseChapter, 1);
            ConfigureTOCForCustomStyle(doc, sProseSubchapter, 2);

            // Insert page and section break
            InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);

            // Insert "Foreword" heading
            var forewordRange = doc.Content;
            forewordRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

            // Insert "Foreword" text
            forewordRange.Text = "Foreword";
            forewordRange.set_Style(sProseChapter); // Ensure style is applied (optional)

            // Mark the text as a TOC entry
            forewordRange.InsertParagraphAfter(); // Ensure the paragraph is fully inserted
            //var tocEntryRange = forewordRange.Duplicate; // Duplicate range for the TOC field
            //tocEntryRange.Collapse(Word.WdCollapseDirection.wdCollapseStart);

            //// Insert the TOC field
            //var tocField = tocEntryRange.Fields.Add(tocEntryRange, Word.WdFieldType.wdFieldTOCEntry, "Foreword", false);


            //// Add spacing after the entry
            //forewordRange.InsertParagraphAfter();
            var nextParagraph = forewordRange.Paragraphs.Add();
            nextParagraph.Range.set_Style(sProseText);    

            // Insert page and section break
            InsertSection(doc, Word.WdCollapseDirection.wdCollapseEnd, Word.WdBreakType.wdSectionBreakNextPage);

            var chapterRange = doc.Content;
            chapterRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            chapterRange.Text = "Chapter 1:";
            chapterRange.set_Style(sProseChapter);
            chapterRange.InsertParagraphAfter();
            nextParagraph = chapterRange.Paragraphs.Add();
            nextParagraph.Range.set_Style(sProseText);
            // Update the Table of Contents
            doc.TablesOfContents[1].Update();
        }

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
            if (ShowFontDialogForWSI(ref wsiProseChapter) == DialogResult.OK)
            {
                ApplyStyleToLabel(proseChapter, wsiProseChapter);
            }
        }

        private void SetProseSubchapterFontInfoButton_Click(object sender, EventArgs e)
        {
            if (ShowFontDialogForWSI(ref wsiProseSubchapter) == DialogResult.OK)
            {
                ApplyStyleToLabel(proseSubchapter, wsiProseSubchapter);
            }
        }

        private void SetProseTextFontInfoButton_Click(object sender, EventArgs e)
        {
            if (ShowFontDialogForWSI(ref wsiProseText) == DialogResult.OK)
            {
                ApplyStyleToLabel(proseText, wsiProseText);
            }
        }

        private DialogResult ShowFontDialogForWSI(ref WordStyleInfo wsi)
        {
            using (PTFontDialog fontDialog = new PTFontDialog())
            {
                // Set initial properties for the FontDialog based on wsiProseChapter
                fontDialog.ShowColor = true; // Allow the user to select a color

                fontDialog.SelectedFontName = wsi.FontName;
                fontDialog.SelectedFontSize = wsi.FontSize;
                fontDialog.SelectedBold = wsi.Bold;
                fontDialog.SelectedItalic = wsi.Italic;
                fontDialog.SelectedUnderline = wsi.Underline;
                fontDialog.SelectedStrike = wsi.Strikethrough;
                fontDialog.SelectedFontColor = wsi.FontColor;

                // Show the FontDialog and check if the user clicked OK
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    // Retrieve the selected font and color
                    Color selectedColor = fontDialog.Color;

                    // Update wsiProseChapter with the new values
                    wsi.FontName = fontDialog.SelectedFontName;
                    wsi.FontSize = fontDialog.SelectedFontSize;
                    wsi.Bold = fontDialog.SelectedBold;
                    wsi.Italic = fontDialog.SelectedItalic;
                    wsi.Underline = fontDialog.SelectedUnderline;
                    wsi.Strikethrough = fontDialog.SelectedStrike;
                    wsi.FontColor = selectedColor;

                    return DialogResult.OK;
                }

                return DialogResult.Cancel;
            }
        }
        private void AddStylesToDocument(Document doc)
        {
            try
            {
                // Add or update ProseChapter style
                if (StyleExists(doc, sProseChapter))
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
                if (StyleExists(doc, sProseSubchapter))
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
                if (StyleExists(doc, sProseText))
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

        private void ConfigureTOCForCustomStyle(Word.Document doc, string styleName, int tocLevel)
        {
            if (doc.TablesOfContents.Count > 0)
            {
                var toc = doc.TablesOfContents[1];
                var tocEntries = toc.Range.ParagraphFormat.TabStops;
                var tocField = toc.Range.Fields[1];
                var fieldCode = tocField.Code.Text;

                // Add the custom style to the TOC field code
                if (!fieldCode.Contains($@" \t ""{styleName}"""))
                {
                    fieldCode = fieldCode.TrimEnd() + $@" \t ""{styleName},{tocLevel}""";
                    tocField.Code.Text = fieldCode;
                }
            }
        }

    }


}
