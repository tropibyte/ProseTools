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
        public const string MetadataNamespace = "urn:prosetools:metadata";
        public Outline TheOutline { get; set; }
        public abstract string _name { get; }
        // New property for document-specific settings.
        public DocumentSettings DocumentSettings { get; set; }


        public abstract void ReadFromActiveDocument();
        public abstract void WriteToActiveDocument();

        public ProseMetaData()
        {
            TheOutline = new Outline();
            DocumentSettings = new DocumentSettings();
        }
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
                    XNamespace ns = MetadataNamespace;
                    string proseType = metadataXml.Element(ns + "ProseType")?.Value ?? "Novel";

                    if(proseType == "Novel" || proseType == NovelMetaData.Name)
                    {
                        return new NovelMetaData(metadataXml);
                    }
                    else if (proseType == "ResearchPaper" || proseType == ResearchPaperMetaData.Name)
                    {
                        return new ResearchPaperMetaData(metadataXml);
                    }
                    else if (proseType == "TechnicalPaper" || proseType == TechnicalPaperMetaData.Name)
                    {
                        return new TechnicalPaperMetaData(metadataXml);
                    }
                    else if (proseType == "Screenplay" || proseType == ScreenplayMetaData.Name)
                    {
                        return new ScreenplayMetaData(metadataXml);
                    }
                    else
                    {
                        return new NullMetaData(); // Default to NullMetaData if invalid
                    }
                }
            }

            return new NullMetaData(); // No metadata found, return NullMetaData
        }

        /// <summary>
        /// Converts the metadata to an XML element.
        /// </summary>
        /// <returns>An XElement representing the metadata.</returns>
        public virtual XElement ToXML()
        {
            XNamespace ns = MetadataNamespace;
            // A base version that outputs a minimal structure.
            // Derived classes should override this to output the complete details.
            return new XElement("ProseMetaData",
                new XAttribute(XNamespace.Xmlns + "ns", MetadataNamespace),
                new XElement("ProseType", "NullMetaData"),
            new XElement("DocumentSettings", DocumentSettings.ToXML().Elements()) // flatten inner DocumentSettings elements
            );
        }

        public static ProseMetaData LoadFromXml(XElement xml)
        {
            string proseType = xml.Element(XNamespace.Get(MetadataNamespace) + "ProseType")?.Value ?? "Novel";
            switch (proseType)
            {
                case "Novel":
                    return new NovelMetaData(xml);
                case "ResearchPaper":
                    return new ResearchPaperMetaData(xml);
                case "TechnicalPaper":
                    return new TechnicalPaperMetaData(xml);
                case "Screenplay":
                    return new ScreenplayMetaData(xml);
                default:
                    return new NullMetaData();
            }
        }
    }
}
