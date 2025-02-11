using System.Xml.Linq;
using System;

namespace ProseTools
{
    internal class Note
    {
        // Serialize to XML
        public XElement ToXML()
        {
            return new XElement("Note");
            //    // Add serialized properties here when they are defined
            //);
        }

        // Deserialize from XML
        public static Note FromXML(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

            // Create and return a new Scene object
            var note = new Note();

            // Populate properties from XML when defined

            return note;
        }
    }
}