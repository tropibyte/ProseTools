using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProseTools
{
    internal class NovelMetaData : ProseMetaData
    {
        public static readonly string Name = "Novel/Book";
        public override string _name => Name;

        public LinkedList<Chapter> ListChapters { get; set; }
        public List<Character> ListCharacters { get; set; }
        public List<Location> ListLocations { get; set; }
        public List<Scene> ListScenes { get; set; }
        public List<Note> ListNotes { get; set; }

        public NovelMetaData()
        {
            ListChapters = new LinkedList<Chapter>();
            ListCharacters = new List<Character>();
            ListLocations = new List<Location>();
            ListScenes = new List<Scene>();
            ListNotes = new List<Note>();
        }

        public NovelMetaData(XElement metadataXml) : this()
        {
            ReadFromXml(metadataXml);
        }

        public override void ReadFromActiveDocument()
        {
            var doc = Globals.ThisAddIn.Application.ActiveDocument;
            if (doc == null) throw new InvalidOperationException("No active document is open.");

            foreach (CustomXMLPart part in doc.CustomXMLParts)
            {
                if (part.NamespaceURI == MetadataNamespace)
                {
                    ReadFromXml(XElement.Parse(part.XML));
                    break;
                }
            }
        }

        private void ReadFromXml(XElement metadataXml)
        {
            XNamespace ns = MetadataNamespace;  // Use the same namespace used in ToXML

            try
            {
                ListChapters = new LinkedList<Chapter>(
                    metadataXml.Descendants(ns + "Chapter").Select(ch => Chapter.FromXML(ch))
                );
            }
            catch
            {
                ListChapters = new LinkedList<Chapter>();
            }

            try
            {
                ListCharacters = metadataXml.Descendants(ns + "Character")
                                  .Select(ch => Character.FromXML(ch)).ToList();
            }
            catch
            {
                ListCharacters = new List<Character>();
            }

            try
            {
                ListLocations = metadataXml.Descendants(ns + "Location")
                                  .Select(loc => Location.FromXML(loc)).ToList();
            }
            catch
            {
                ListLocations = new List<Location>();
            }

            try
            {
                ListScenes = metadataXml.Descendants(ns + "Scene")
                                  .Select(scene => Scene.FromXML(scene)).ToList();
            }
            catch
            {
                ListScenes = new List<Scene>();
            }

            try
            {
                ListNotes = metadataXml.Descendants(ns + "Note")
                                  .Select(note => Note.FromXML(note)).ToList();
            }
            catch
            {
                ListNotes = new List<Note>();
            }

            try
            {
                // Look for the <Outline> element using the namespace.
                XElement outlineElement = metadataXml.Element(ns + "Outline");
                if (outlineElement != null)
                {
                    TheOutline = Outline.FromXML(outlineElement);
                }
                else
                {
                    TheOutline = new Outline();
                }
            }
            catch
            {
                TheOutline = new Outline();
            }
        }

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
            XNamespace ns = MetadataNamespace;
            return new XElement(ns + "ProseMetaData",
                new XElement(ns + "ProseType", Name),
                new XElement(ns + "Chapters", ListChapters.Select(ch => ch.ToXML())),
                new XElement(ns + "Characters", ListCharacters.Select(c => c.ToXML())),
                new XElement(ns + "Locations", ListLocations.Select(loc => loc.ToXML())),
                new XElement(ns + "Scenes", ListScenes.Select(s => s.ToXML())),
                new XElement(ns + "Notes", ListNotes.Select(n => n.ToXML())),
                new XElement(ns + "Outline", TheOutline.ToXML()),
                new XElement(ns + "DocumentSettings", DocumentSettings.ToXML())
            );
        }


        public void AddChapter(Chapter chapter) => ListChapters.AddLast(chapter);

        public void RemoveChapter(Chapter chapter) => ListChapters.Remove(chapter);

        public void RemoveChapter(int chapterNumber) =>
            ListChapters.Remove(ListChapters.ElementAt(chapterNumber));

        public void RemoveChapter(string chapterTitle) =>
            ListChapters.Remove(ListChapters.First(chapter => chapter.Title == chapterTitle));

        public void AddCharacter(Character character) => ListCharacters.Add(character);

        public void RemoveCharacter(Character character) => ListCharacters.Remove(character);

        public void RemoveCharacter(string characterName) =>
            ListCharacters.Remove(ListCharacters.First(character => character.CharacterName.ToString() == characterName));

        public void AddLocation(Location location) => ListLocations.Add(location);

        public void RemoveLocation(Location location) => ListLocations.Remove(location);

        public void RemoveLocation(string locationName) =>
            ListLocations.Remove(ListLocations.First(location => location.Name == locationName));

    }
}
