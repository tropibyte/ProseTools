using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{
    /// <summary>
    /// Base class for prose metadata, determining the document type.
    /// </summary>
    internal abstract class ProseMetaData
    {
        protected const string MetadataNamespace = "urn:prosetools:metadata";

        public abstract void ReadFromActiveDocument();
        public abstract void WriteToActiveDocument();

        /// <summary>
        /// Factory method to determine prose type and return the correct metadata class.
        /// </summary>
        public static ProseMetaData LoadFromDocument()
        {
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            if (doc == null) throw new InvalidOperationException("No active document is open.");

            foreach (CustomXMLPart part in doc.CustomXMLParts)
            {
                if (part.NamespaceURI == MetadataNamespace)
                {
                    var metadataXml = XElement.Parse(part.XML);
                    string proseType = metadataXml.Element("ProseType")?.Value ?? "Novel";

                    switch (proseType)
                    {
                        case "Novel":
                            return new NovelMetaData(metadataXml);
                        case "ResearchPaper":
                            return new ResearchPaperMetaData(metadataXml);
                        case "Screenplay":
                            return new ScreenplayMetaData(metadataXml);
                        default:
                            return new NullMetaData(); // Default to NullMetaData if invalid
                    }
                }
            }

            return new NullMetaData(); // No metadata found, return NullMetaData
        }
    }
}
