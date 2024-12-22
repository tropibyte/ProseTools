using Microsoft.Office.Interop.Word;
using System.Drawing;

namespace ProseTools
{
    public class WordStyleInfo
    {
        public string Name { get; set; }
        public string FontName { get; set; }
        public float FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
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
    }
}
