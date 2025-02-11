using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Drawing;

namespace ProseTools
{
    public static class WordHelper
    {
        /// <summary>
        /// Sets one-inch margins on all sides of the given document.
        /// </summary>
        public static void SetOneInchMargins(Document doc)
        {
            doc.PageSetup.TopMargin = doc.Application.InchesToPoints(1);
            doc.PageSetup.BottomMargin = doc.Application.InchesToPoints(1);
            doc.PageSetup.LeftMargin = doc.Application.InchesToPoints(1);
            doc.PageSetup.RightMargin = doc.Application.InchesToPoints(1);
        }

        public static void SetIdenticalMargins(Document doc, float marginInches)
        {
            SetMargins(doc, marginInches, marginInches, marginInches, marginInches);
        }

        /// <summary>
        /// Sets document margins in inches, with the following constraints:
        /// 1) Each margin >= 0.4 inches.
        /// 2) For left/right: <= floor(pageWidth/2) - 1.
        ///    For top/bottom: <= floor(pageHeight/2) - 1.
        /// </summary>
        /// <param name="doc">The Word document to modify.</param>
        /// <param name="left">Left margin in inches.</param>
        /// <param name="top">Top margin in inches.</param>
        /// <param name="right">Right margin in inches.</param>
        /// <param name="bottom">Bottom margin in inches.</param>
        public static void SetMargins(Document doc, float left, float top, float right, float bottom)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc), "Document cannot be null.");

            // Minimum allowed margin in inches
            const float MIN_MARGIN = 0.4F;

            // Convert page dimensions from points to inches (1 inch = 72 points)
            float pageWidthInches = doc.Application.PointsToInches(doc.PageSetup.PageWidth);
            float pageHeightInches = doc.Application.PointsToInches(doc.PageSetup.PageHeight);

            // Maximum allowed margins based on user-specified formula
            float maxMarginHorizontal = (float)Math.Floor(pageWidthInches / 2) - 1F;
            float maxMarginVertical = (float)Math.Floor(pageHeightInches / 2) - 1F;

            // Validate left margin
            if (left < MIN_MARGIN || left > maxMarginHorizontal)
            {
                throw new ArgumentOutOfRangeException(nameof(left),
                    $"Left margin must be between {MIN_MARGIN:0.0} and {maxMarginHorizontal:0.0} inches.");
            }

            // Validate right margin
            if (right < MIN_MARGIN || right > maxMarginHorizontal)
            {
                throw new ArgumentOutOfRangeException(nameof(right),
                    $"Right margin must be between {MIN_MARGIN:0.0} and {maxMarginHorizontal:0.0} inches.");
            }

            // Validate top margin
            if (top < MIN_MARGIN || top > maxMarginVertical)
            {
                throw new ArgumentOutOfRangeException(nameof(top),
                    $"Top margin must be between {MIN_MARGIN:0.0} and {maxMarginVertical:0.0} inches.");
            }

            // Validate bottom margin
            if (bottom < MIN_MARGIN || bottom > maxMarginVertical)
            {
                throw new ArgumentOutOfRangeException(nameof(bottom),
                    $"Bottom margin must be between {MIN_MARGIN:0.0} and {maxMarginVertical:0.0} inches.");
            }

            // Everything is valid—convert inches to points and assign
            doc.PageSetup.LeftMargin = doc.Application.InchesToPoints(left);
            doc.PageSetup.RightMargin = doc.Application.InchesToPoints(right);
            doc.PageSetup.TopMargin = doc.Application.InchesToPoints(top);
            doc.PageSetup.BottomMargin = doc.Application.InchesToPoints(bottom);
        }

        public static void SetFontColor(Document doc, Color color)
        {
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Range.Font.Color = (WdColor)color.ToArgb();
            }
        }

        public static void SetFont(Document doc, string fontName, float size)
        {
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Range.Font.Name = fontName;
                paragraph.Range.Font.Size = size;
            }
        }
        /// <summary>
        /// Inserts a section break only if the document isn't empty.
        /// </summary>
        public static void InsertSectionIfDocNotEmpty(
            Document doc,
            WdCollapseDirection collapseDirection,
            WdBreakType breakType)
        {
            if (!string.IsNullOrWhiteSpace(doc.Content.Text.Trim()))
            {
                var range = doc.Content;
                range.Collapse(collapseDirection);
                range.InsertBreak(breakType);
            }
        }

        /// <summary>
        /// Sets double line spacing for all paragraphs.
        /// </summary>
        public static void SetDoubleSpacing(Document doc)
        {
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceDouble;
            }
        }

        /// <summary>
        /// Center-aligns all paragraphs in the given document.
        /// </summary>
        public static void CenterAllParagraphs(Document doc)
        {
            foreach (Paragraph paragraph in doc.Paragraphs)
            {
                paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            }
        }

        /// <summary>
        /// Inserts a right-aligned page number in the specified section's header.
        /// If `extraText` is not empty, it will precede the page number.
        /// </summary>
        public static void InsertPageNumberRightHeader(Document doc, int sectionIndex = 1, string extraText = "")
        {
            // Ensure the document actually has enough sections
            if (doc.Sections.Count < sectionIndex) return;

            var headerRange = doc.Sections[sectionIndex].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

            // If you need text before the page number (e.g., "Smith ")
            if (!string.IsNullOrEmpty(extraText))
            {
                headerRange.Text = extraText + " ";
                headerRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            }

            // Insert the page field
            headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
        }

        /// <summary>
        /// Inserts text as a new paragraph if it's not empty.
        /// </summary>
        public static void InsertIfNotEmpty(Document doc, string text, bool bold = false)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var paragraph = doc.Content.Paragraphs.Add();
                paragraph.Range.Text = text;
                paragraph.Range.Font.Bold = bold ? 1 : 0;
                paragraph.Range.InsertParagraphAfter();
            }
        }
    }
}
