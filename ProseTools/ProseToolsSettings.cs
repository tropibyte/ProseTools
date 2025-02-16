using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{
    public class ProseToolsSettings
    {
        readonly XNamespace ns = "urn:prosetools:prosetoolssettings"; // or reuse your metadata namespace if you prefer

        public SystemSettings SystemSettings { get; set; }
        public UserSettings UserSettings { get; set; }
        public DocumentSettings DocumentSettings { get; set; }

        public ProseToolsSettings()
        {
            SystemSettings = new SystemSettings();
            UserSettings = new UserSettings();
            DocumentSettings = new DocumentSettings();
        }
        public XElement ToXML()
        {
            return new XElement(ns + "ProseToolsSettings"
            );
        }

        internal static ProseToolsSettings FromXML(XElement xml)
        {
            //throw new NotImplementedException();
            return new ProseToolsSettings();
        }
    }

    public class SystemSettings
    {
        public List<GenAIConfig> GenAIConfigs { get; set; }
        public SystemSettings()
        {
            GenAIConfigs = new List<GenAIConfig>();
        }
    }

    public class UserSettings
    {
        public List<GenAIConfig> GenAIConfigs { get; set; }
        public UserSettings()
        {
            GenAIConfigs = new List<GenAIConfig>();
        }
    }

    public class DocumentSettings
    {
        public bool ShowPageNumbers { get; set; } = true;
        public string DefaultChapterStyle { get; set; } = "Prose Chapter";
        public string LastUsedTemplate { get; set; }
        public string LastUsedGenAI { get; set; }
        public DocumentSettings()
        {
            LastUsedTemplate = string.Empty;
            LastUsedGenAI = string.Empty;
        }

        public XElement ToXML()
        {
            XNamespace ns = "urn:prosetools:docsettings";
            return new XElement(ns + "DocumentSettings",
                new XElement(ns + "ShowPageNumbers", ShowPageNumbers),
                new XElement(ns + "DefaultChapterStyle", DefaultChapterStyle),
                new XElement(ns + "LastUsedTemplate", LastUsedTemplate),
                new XElement(ns + "LastUsedGenAI", LastUsedGenAI)
            );
        }

        public static DocumentSettings FromXML(XElement element)
        {
            XNamespace ns = "urn:prosetools:docsettings";
            return new DocumentSettings
            {
                ShowPageNumbers = bool.Parse(element.Element(ns + "ShowPageNumbers")?.Value ?? "true"),
                DefaultChapterStyle = element.Element(ns + "DefaultChapterStyle")?.Value ?? "Prose Chapter",
                LastUsedTemplate = element.Element(ns + "LastUsedTemplate")?.Value ?? string.Empty,
                LastUsedGenAI = element.Element(ns + "LastUsedGenAI")?.Value ?? string.Empty
            };
        }
    }
}


