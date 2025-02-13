using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProseTools
{
    /// <summary>
    /// Represents a personal name with optional middle and maternal surnames.
    /// </summary>
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
            var parts = new List<string> { FirstName, MiddleName, LastName, MaternalSurname };
            return string.Join(" ", parts.Where(p => !string.IsNullOrEmpty(p)));
        }

        public static Name Parse(string fullName)
        {
            var tokens = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
                return new Name("", "");
            if (tokens.Length == 1)
                return new Name(tokens[0], "");
            if (tokens.Length == 2)
                return new Name(tokens[0], tokens[1]);
            if (tokens.Length == 3)
                return new Name(tokens[0], tokens[2], tokens[1]);
            // For four or more tokens, assume: FirstName, MiddleName, LastName, MaternalSurname (ignoring extras)
            return new Name(tokens[0], tokens[2], tokens[1], tokens[3]);
        }
    }

    /// <summary>
    /// Represents a character with a primary name, additional aliases, occupation, type, and other details.
    /// </summary>
    public class Character
    {
        public Name CharacterName { get; set; }
        public string Occupation { get; set; }
        public string CharacterType { get; set; } // e.g., "Protagonist", "Antagonist", etc.
        public DateTime? DateOfBirth { get; set; } // Nullable in case DOB is unknown
        public string Address { get; set; }

        /// <summary>
        /// List of related characters (e.g., father, mother). Each tuple holds a relation label and a Name.
        /// </summary>
        public List<(string Relation, Name RelatedName)> Related { get; set; }

        /// <summary>
        /// List of aliases (e.g., nicknames, pseudonyms)
        /// </summary>
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

        // Parameterless constructor for deserialization
        public Character()
        {
            Related = new List<(string, Name)>();
            Alias = new List<Name>();
        }

        /// <summary>
        /// Adds a related character using a relation label.
        /// </summary>
        public void AddRelation(string relation, Name relatedName)
        {
            if (!string.IsNullOrEmpty(relation) && relatedName != null)
            {
                Related.Add((relation, relatedName));
            }
        }

        /// <summary>
        /// Adds an alias to this character.
        /// </summary>
        public void AddAlias(Name alias)
        {
            if (alias != null)
            {
                Alias.Add(alias);
            }
        }

        public override string ToString()
        {
            return $"{CharacterName} ({CharacterType}) - {Occupation}";
        }

        /// <summary>
        /// Serializes this Character to an XML element.
        /// </summary>
        public XElement ToXML()
        {
            return new XElement("Character",
                new XElement("CharacterName", CharacterName.ToXML()),
                new XElement("Occupation", Occupation ?? string.Empty),
                new XElement("CharacterType", CharacterType ?? string.Empty),
                new XElement("DateOfBirth", DateOfBirth?.ToString("o") ?? string.Empty), // ISO 8601 format
                new XElement("Address", Address ?? string.Empty),
                new XElement("Related",
                    Related.Select(r => new XElement("Relation",
                        new XElement("Type", r.Relation ?? string.Empty),
                        new XElement("Name", r.RelatedName != null ? r.RelatedName.ToXML() : new XElement("Name"))
                    ))
                ),
                new XElement("Alias",
                    Alias.Select(a => a.ToXML())
                )
            );
        }

        /// <summary>
        /// Deserializes a Character from an XML element.
        /// </summary>
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
                Related = element.Element("Related") != null ?
                    element.Element("Related").Elements("Relation")
                        .Select(r => (
                            Relation: r.Element("Type")?.Value,
                            RelatedName: r.Element("Name") != null ? NameExtensions.FromXML(r.Element("Name")) : null
                        )).ToList()
                    : new List<(string, Name)>(),
                Alias = element.Element("Alias") != null ?
                    element.Element("Alias").Elements("Name").Select(NameExtensions.FromXML).ToList()
                    : new List<Name>()
            };

            return character;
        }
    }

    /// <summary>
    /// Provides XML serialization/deserialization extensions for the Name class.
    /// </summary>
    public static class NameExtensions
    {
        /// <summary>
        /// Serializes a Name to an XML element.
        /// </summary>
        public static XElement ToXML(this Name name)
        {
            if (name == null)
                return new XElement("Name");

            return new XElement("Name",
                new XElement("FirstName", name.FirstName ?? string.Empty),
                new XElement("MiddleName", name.MiddleName ?? string.Empty),
                new XElement("LastName", name.LastName ?? string.Empty),
                new XElement("MaternalSurname", name.MaternalSurname ?? string.Empty)
            );
        }

        /// <summary>
        /// Deserializes a Name from an XML element.
        /// </summary>
        public static Name FromXML(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), "Provided XML element is null.");

            return new Name(
                element.Element("FirstName")?.Value ?? string.Empty,
                element.Element("LastName")?.Value ?? string.Empty,
                element.Element("MiddleName")?.Value,
                element.Element("MaternalSurname")?.Value
            );
        }
    }
}
