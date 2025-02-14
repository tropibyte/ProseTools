using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{
    internal class ResearchPaperMetaData : ProseMetaData
    {
        public ResearchPaperMetaData() { }

        public ResearchPaperMetaData(XElement metadataXml) { }

        public override void ReadFromActiveDocument() { }

        public override void WriteToActiveDocument()
        {
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            if (doc == null) throw new InvalidOperationException("No active document is open.");

            var metadataXml = ToXML();

            CustomXMLPart existingPart = doc.CustomXMLParts
                .OfType<CustomXMLPart>()
                .FirstOrDefault(part => part.NamespaceURI == MetadataNamespace);

            existingPart?.Delete();
            doc.CustomXMLParts.Add(metadataXml.ToString());
        }

        public override XElement ToXML()
        {
            return new XElement("ProseMetaData",
                new XAttribute(XNamespace.Xmlns + "ns", MetadataNamespace),
                new XElement("ProseType", "ResearchPaper")
            );
        }
    }
}
