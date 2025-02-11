using System;
using System.Xml.Linq;

namespace ProseTools
{
    internal class Chapter
    {
        public string Title { get; internal set; }
        public int ChapterNumber { get; internal set; }

        public Chapter() { }

        public Chapter(string title, int chapterNumber)
        {
            Title = title;
            ChapterNumber = chapterNumber;
        }

        // Serialize to XML
        public XElement ToXML()
        {
            return new XElement("Chapter",
                new XElement("Title", Title),
                new XElement("ChapterNumber", ChapterNumber)
            );
        }

        // Deserialize from XML
        public static Chapter FromXML(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

            return new Chapter
            {
                Title = element.Element("Title")?.Value,
                ChapterNumber = int.TryParse(element.Element("ChapterNumber")?.Value, out var number) ? number : 0
            };
        }
    }
}
