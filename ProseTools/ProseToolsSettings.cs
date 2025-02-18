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
        readonly XNamespace ns = "urn:prosetools:prosetoolssettings";

        // A list where index 0 is the "system" settings, and the rest are user settings.
        public List<UserSettings> UserSettingsList { get; set; }

        public DocumentSettings DocumentSettings { get; set; }

        public ProseToolsSettings()
        {
            // Initialize the list and ensure the system settings exist as the first element.
            UserSettingsList = new List<UserSettings>();
            if (!UserSettingsList.Any())
            {
                UserSettingsList.Add(new UserSettings { Username = "SYSTEM" });
            }
            DocumentSettings = new DocumentSettings();
        }

        public XElement ToXML()
        {
            return new XElement(ns + "ProseToolsSettings",
                new XElement(ns + "UserSettingsList",
                    UserSettingsList.Select(us => us.ToXML())
                ),
                new XElement(ns + "DocumentSettings",
                    DocumentSettings.ToXML().Elements() // flatten the inner DocumentSettings elements
                )
            );
        }

        internal static ProseToolsSettings FromXML(XElement xml)
        {
            XNamespace ns = "urn:prosetools:prosetoolssettings";
            ProseToolsSettings settings = new ProseToolsSettings();

            var userSettingsListElement = xml.Element(ns + "UserSettingsList");
            if (userSettingsListElement != null)
            {
                settings.UserSettingsList = userSettingsListElement.Elements(ns + "UserSettings")
                    .Select(UserSettings.FromXML)
                    .ToList();
            }
            else
            {
                // If missing, initialize with a system settings entry.
                settings.UserSettingsList = new List<UserSettings> { new UserSettings { Username = "SYSTEM" } };
            }

            var docSettingsElement = xml.Element(ns + "DocumentSettings");
            if (docSettingsElement != null)
            {
                settings.DocumentSettings = DocumentSettings.FromXML(docSettingsElement);
            }
            else
            {
                settings.DocumentSettings = new DocumentSettings();
            }

            return settings;
        }
    }

    public class UserSettings
    {
        readonly XNamespace ns = "urn:prosetools:prosetoolssettings";

        public List<GenAIConfig> GenAIConfigs { get; set; }
        public string Username { get; set; } = string.Empty;
        public Dictionary<string, string> Attributes { get; set; }  // Optional extra attributes

        public UserSettings()
        {
            GenAIConfigs = new List<GenAIConfig>();
            Attributes = new Dictionary<string, string>();
        }

        public XElement ToXML()
        {
            return new XElement(ns + "UserSettings",
                new XElement(ns + "Username", Username),
                new XElement(ns + "GenAIConfigs",
                    GenAIConfigs.Select(cfg => cfg.ToXML())
                ),
                new XElement(ns + "Attributes",
                    Attributes.Select(attr => new XElement(ns + attr.Key, attr.Value))
                )
            );
        }

        public static UserSettings FromXML(XElement element)
        {
            XNamespace ns = "urn:prosetools:prosetoolssettings";
            UserSettings user = new UserSettings
            {
                Username = element.Element(ns + "Username")?.Value ?? string.Empty
            };

            var genAIsElement = element.Element(ns + "GenAIConfigs");
            if (genAIsElement != null)
            {
                user.GenAIConfigs = genAIsElement.Elements(ns + "GenAIConfig")
                    .Select(GenAIConfig.FromXML)
                    .ToList();
            }

            var attributesElement = element.Element(ns + "Attributes");
            if (attributesElement != null)
            {
                user.Attributes = attributesElement.Elements()
                    .ToDictionary(x => x.Name.LocalName, x => x.Value);
            }

            return user;
        }
    }

    public class DocumentSettings
    {
        public bool ShowPageNumbers { get; set; } = true;
        public string DefaultChapterStyle { get; set; } = "Prose Chapter";
        public string LastUsedTemplate { get; set; } = string.Empty;
        public string LastUsedGenAI { get; set; } = string.Empty;

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


