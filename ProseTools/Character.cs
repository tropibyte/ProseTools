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
    }
}
