using System;
using System.Xml.Linq;

namespace ProseTools
{
    internal class Location
    {
        public string Name { get; internal set; }

        public Location() { }

        public Location(string name)
        {
            Name = name;
        }

        // Serialize to XML
        public XElement ToXML()
        {
            return new XElement("Location",
                new XElement("Name", Name)
            );
        }

        // Deserialize from XML
        public static Location FromXML(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

            return new Location
            {
                Name = element.Element("Name")?.Value
            };
        }
    }
}
