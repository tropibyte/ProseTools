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
        }

        /// <summary>
        /// Handles the Save button click. Validates the input, creates a Character,
        /// and sets the CharacterData property.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate that First Name and Last Name are provided.
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Please provide at least a first name and a last name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a Name object for the character.
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
                    MessageBox.Show("Please enter a valid date for Date of Birth (e.g., 2025-01-18).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Create the Character instance.
            CharacterData = new Character(name,
                txtOccupation.Text.Trim(),
                txtCharacterType.Text.Trim(),
                dob,
                txtAddress.Text.Trim()
            );

            // Ensure there is at least one alias by default (using the character's primary name).
            if (CharacterData.Alias == null || CharacterData.Alias.Count == 0)
            {
                CharacterData.Alias.Add(name);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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
