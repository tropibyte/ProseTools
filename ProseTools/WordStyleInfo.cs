using Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Windows.Forms;

namespace ProseTools
{
    public class WordStyleInfo
    {
        public string Name { get; set; }
        public string FontName { get; set; }
        public float FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool Strikethrough { get; set; }
        public Color FontColor { get; set; }
        public WdParagraphAlignment Alignment { get; set; }

        public WordStyleInfo(string name, string fontName, float fontSize, bool bold, bool italic, Color fontColor, WdParagraphAlignment alignment)
        {
            Name = name;
            FontName = fontName;
            FontSize = fontSize;
            Bold = bold;
            Italic = italic;
            FontColor = fontColor;
            Alignment = alignment;
        }

        public void ApplyStyle(Document doc)
        {
            // Check if the style already exists
            Style style;
            try
            {
                style = doc.Styles[Name];
            }
            catch
            {
                // Create a new style if it doesn't exist
                style = doc.Styles.Add(Name, WdStyleType.wdStyleTypeParagraph);
            }

            // Set the style properties
            style.Font.Name = FontName;
            style.Font.Size = FontSize;
            style.Font.Bold = Bold ? 1 : 0;
            style.Font.Italic = Italic ? 1 : 0;
            style.Font.Color = (WdColor)(FontColor.R + 0x100 * FontColor.G + 0x10000 * FontColor.B);
            style.ParagraphFormat.Alignment = Alignment;
        }

        public static DialogResult ShowFontDialogForWSI(ref WordStyleInfo wsi)
        {
            if (wsi == null)
                return DialogResult.Abort;

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

        public static void ApplyStyleToLabel(System.Windows.Forms.Label label, WordStyleInfo styleInfo)
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

        public static bool StyleExists(Document doc, string styleName)
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

        public static WordStyleInfo GetStyleInfo(Document doc, string styleName)
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

    }
}
