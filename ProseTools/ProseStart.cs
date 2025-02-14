using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProseTools
{
    internal class ProseStart
    {
        public static void InsertSection(Document doc, WdCollapseDirection wdCollapse, WdBreakType wdBreakType)
        {
            // Insert the appropriate section break
            var breakRange = doc.Content;
            breakRange.Collapse(wdCollapse);
            breakRange.InsertBreak(wdBreakType);

            // Configure the newly created section
            var newSection = doc.Sections[doc.Sections.Count];

            // Set the vertical alignment to top
            newSection.PageSetup.VerticalAlignment = WdVerticalAlignment.wdAlignVerticalTop;

            // Set the horizontal alignment to left
            var newSectionRange = newSection.Range;
            newSectionRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
        }

        public static void InsertSection(Section section, WdCollapseDirection wdCollapse, WdBreakType wdBreakType)
        {
            // Insert the appropriate section break
            section.Range.Collapse(wdCollapse);
            section.Range.InsertBreak(wdBreakType);

            // Configure the newly created section
            var newSection = section;

            // Set the vertical alignment to top
            newSection.PageSetup.VerticalAlignment = WdVerticalAlignment.wdAlignVerticalTop;

            // Set the horizontal alignment to left
            var newSectionRange = newSection.Range;
            newSectionRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
        }
        public static void ConfigureTOCForCustomStyle(Document doc, string styleName, int tocLevel)
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

        public static void UpdateDocumentTOC(Document doc)
        {
                for (int i = 0; i < doc.TablesOfContents.Count; ++i)
                {
                    doc.TablesOfContents[i + 1].Update();
                }
            }
    }
}
