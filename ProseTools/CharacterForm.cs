using System;
using System.Linq;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class CharacterForm : Form
    {
        // Exposes the character created or edited by the form.
        public Character CharacterData { get; private set; }

        public CharacterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pre-fills the name fields based on a scanned full name.
        /// Splits the full name by spaces and assigns:
        /// - First token to First Name
        /// - Last token to Last Name
        /// - Any tokens in between to Middle Name.
        /// </summary>
        /// <param name="scannedName">The scanned full name string.</param>
        public void PreFillData(string scannedName)
        {
            if (string.IsNullOrWhiteSpace(scannedName))
                return;

            var parts = scannedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
                txtFirstName.Text = parts[0];
            if (parts.Length > 1)
                txtLastName.Text = parts[parts.Length - 1];
            if (parts.Length > 2)
                txtMiddleName.Text = string.Join(" ", parts.Skip(1).Take(parts.Length - 2));
        }

        /// <summary>
        /// Loads an existing character into the form for editing.
        /// </summary>
        /// <param name="character">The character to load.</param>
        public void LoadCharacter(Character character)
        {
            if (character == null)
                return;

            txtFirstName.Text = character.CharacterName.FirstName;
            txtMiddleName.Text = character.CharacterName.MiddleName;
            txtLastName.Text = character.CharacterName.LastName;
            txtMaternalSurname.Text = character.CharacterName.MaternalSurname;
            txtOccupation.Text = character.Occupation;
            txtCharacterType.Text = character.CharacterType;
            txtDateOfBirth.Text = character.DateOfBirth?.ToString("yyyy-MM-dd"); // Format as YYYY-MM-DD
            txtAddress.Text = character.Address;

            // Populate the alias text box with each alias on a new line.
            if (character.Alias != null && character.Alias.Count > 0)
            {
                textBoxAlias.Lines = character.Alias.Select(a => a.ToString()).ToArray();
            }
            else
            {
                textBoxAlias.Text = string.Empty;
            }

            textBoxNotes.Text = character.Notes;
        }


        /// <summary>
        /// Handles the Save button click. Validates the input, creates a Character,
        /// and sets the CharacterData property.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate required fields (first name and last name).
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Please provide at least a first name and a last name.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a Name object for the primary character name.
            var name = new Name(
                txtFirstName.Text.Trim(),
                txtLastName.Text.Trim(),
                txtMiddleName.Text.Trim(),
                txtMaternalSurname.Text.Trim()
            );

            // Attempt to parse Date of Birth, if provided.
            DateTime? dob = null;
            if (!string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                if (DateTime.TryParse(txtDateOfBirth.Text, out DateTime parsedDob))
                    dob = parsedDob;
                else
                {
                    MessageBox.Show("Please enter a valid date for Date of Birth (e.g., 2025-01-18).",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Create the Character instance.
            CharacterData = new Character(
                name,
                txtOccupation.Text.Trim(),
                txtCharacterType.Text.Trim(),
                dob,
                txtAddress.Text.Trim()
            );

            // Ensure there is at least one alias using the primary name.
            if (CharacterData.Alias == null || CharacterData.Alias.Count == 0)
            {
                CharacterData.Alias.Add(name);
            }

            CharacterData.Notes = textBoxNotes.Text.Trim();

            // Process each line in textBoxAlias.
            foreach (var line in textBoxAlias.Lines)
            {
                string sanitized = SanitizeAlias(line);
                if (!string.IsNullOrWhiteSpace(sanitized))
                {
                    // Use Name.Parse to split the alias into components.
                    var aliasName = ProseTools.Name.Parse(sanitized);
                    // Avoid duplicates: compare the string representation.
                    if (!CharacterData.Alias.Any(a => a.ToString().Equals(aliasName.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        CharacterData.Alias.Add(aliasName);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Removes leading and trailing whitespace and punctuation from a string.
        /// </summary>
        /// <param name="input">The input alias text.</param>
        /// <returns>The sanitized alias.</returns>
        private string SanitizeAlias(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            // Trim whitespace.
            input = input.Trim();

            // Remove punctuation characters from the start.
            while (input.Length > 0 && char.IsPunctuation(input[0]))
            {
                input = input.Substring(1);
            }
            // Remove punctuation characters from the end.
            while (input.Length > 0 && char.IsPunctuation(input[input.Length - 1]))
            {
                input = input.Substring(0, input.Length - 1);
            }
            return input.Trim();
        }

        /// <summary>
        /// Handles the Cancel button click.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
