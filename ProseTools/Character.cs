using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProseTools
{
    public class Name
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MaternalSurname { get; set; }

        public Name(string firstName, string lastName, string middleName = null, string maternalSurname = null)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            MaternalSurname = maternalSurname;
        }

        public override string ToString()
        {
            var fullName = new List<string> { FirstName, MiddleName, LastName, MaternalSurname };
            return string.Join(" ", fullName.Where(name => !string.IsNullOrEmpty(name)));
        }
    }

    public class Character
    {
        public Name CharacterName { get; set; }
        public string Occupation { get; set; }
        public string CharacterType { get; set; } // e.g., "Protagonist", "Antagonist", etc.
        public DateTime? DateOfBirth { get; set; } // Nullable in case DOB is unknown
        public string Address { get; set; }

        // List of related characters (e.g., father, mother)
        public List<(string Relation, Name RelatedName)> Related { get; set; }

        // List of aliases (e.g., nicknames, pseudonyms)
        public List<Name> Alias { get; set; }

        public Character(Name characterName, string occupation, string characterType, DateTime? dateOfBirth = null, string address = null)
        {
            CharacterName = characterName;
            Occupation = occupation;
            CharacterType = characterType;
            DateOfBirth = dateOfBirth;
            Address = address;
            Related = new List<(string, Name)>();
            Alias = new List<Name>();
        }

        public Character()
        {
            Related = new List<(string, Name)>();
            Alias = new List<Name>();
        }

        // Add a relation
        public void AddRelation(string relation, Name relatedName)
        {
            Related.Add((relation, relatedName));
        }

        // Add an alias
        public void AddAlias(Name alias)
        {
            Alias.Add(alias);
        }

        public override string ToString()
        {
            return $"{CharacterName} ({CharacterType}) - {Occupation}";
        }

        // Serialize to XML
        public XElement ToXML()
        {
            return new XElement("Character",
                new XElement("CharacterName", CharacterName.ToXML()),
                new XElement("Occupation", Occupation),
                new XElement("CharacterType", CharacterType),
                new XElement("DateOfBirth", DateOfBirth?.ToString("o")), // ISO 8601 format
                new XElement("Address", Address),
                new XElement("Related",
                    Related.Select(r => new XElement("Relation",
                        new XElement("Type", r.Relation),
                        new XElement("Name", r.RelatedName.ToXML())))),
                new XElement("Alias",
                    Alias.Select(a => a.ToXML()))
            );
        }

        // Deserialize from XML
        public static Character FromXML(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

            var character = new Character
            {
                CharacterName = element.Element("CharacterName") != null
                    ? NameExtensions.FromXML(element.Element("CharacterName"))
                    : null,
                Occupation = element.Element("Occupation")?.Value,
                CharacterType = element.Element("CharacterType")?.Value,
                DateOfBirth = DateTime.TryParse(element.Element("DateOfBirth")?.Value, out DateTime dob) ? dob : (DateTime?)null,
                Address = element.Element("Address")?.Value,
                Related = element.Element("Related")?.Elements("Relation").Select(r => (
                    Relation: r.Element("Type")?.Value,
                    RelatedName: r.Element("Name") != null ? NameExtensions.FromXML(r.Element("Name")) : null
                )).ToList(),
                Alias = element.Element("Alias")?.Elements("Name").Select(NameExtensions.FromXML).ToList()
            };

            return character;
        }
    }

    public static class NameExtensions
    {
        // Serialize Name to XML
        public static XElement ToXML(this Name name)
        {
            return new XElement("Name",
                new XElement("FirstName", name.FirstName),
                new XElement("MiddleName", name.MiddleName),
                new XElement("LastName", name.LastName),
                new XElement("MaternalSurname", name.MaternalSurname)
            );
        }

        // Deserialize Name from XML
        public static Name FromXML(XElement element)
        {
            return new Name(
                element.Element("FirstName")?.Value,
                element.Element("LastName")?.Value,
                element.Element("MiddleName")?.Value,
                element.Element("MaternalSurname")?.Value
            );
        }
    }
}
