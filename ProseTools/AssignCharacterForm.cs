using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class AssignCharacterForm : Form
    {
        public string AssignedCharacterName { get; private set; }
        private string _scannedName;

        public AssignCharacterForm(string scannedName)
        {
            InitializeComponent();
            _scannedName = scannedName;
            lblScannedName.Text = $"Assign \"{_scannedName}\" to an existing character or create a new one.";
            LoadCharacterList();
        }

        private void LoadCharacterList()
        {
            // Clear existing items and columns
            lvCharacters.Items.Clear();

            // Set the view to Details (if not already set in designer)
            lvCharacters.View = View.Details;
            // Ensure columns exist; add them if they’re missing.
            if (lvCharacters.Columns.Count == 0)
            {
                lvCharacters.Columns.Add("Full Name", 200);
                lvCharacters.Columns.Add("Aliases", 250);
                lvCharacters.Columns.Add("Type", 150);
            }

            if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
            {
                foreach (var character in novel.ListCharacters)
                {
                    // Use the character's main name as the "full name"
                    string fullName = character.CharacterName.ToString();
                    var item = new ListViewItem(fullName);

                    // Build alias string from the Alias list (using ToString() on each Name)
                    string aliasStr = string.Join(", ", character.Alias.Select(a => a.ToString()));
                    item.SubItems.Add(aliasStr);
                    item.SubItems.Add(character.CharacterType);

                    lvCharacters.Items.Add(item);
                }
            }
        }

        private void btnNewCharacter_Click(object sender, EventArgs e)
        {
            // Open CharacterForm to create a new character.
            var charForm = new CharacterForm();
            charForm.PreFillData(_scannedName); // Pre-fill fields using the scanned name.
            if (charForm.ShowDialog() == DialogResult.OK)
            {
                if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
                {
                    novel.ListCharacters.Add(charForm.CharacterData);
                }
                AssignedCharacterName = charForm.CharacterData.CharacterName.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvCharacters.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a character or create a new one.");
                return;
            }
            AssignedCharacterName = lvCharacters.SelectedItems[0].Text;
            // Add the scanned name as an alias to the selected character.
            if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
            {
                var character = novel.ListCharacters.FirstOrDefault(c => c.CharacterName.ToString() == AssignedCharacterName);
                if (character != null && !character.Alias.Any(a => a.ToString().Equals(_scannedName, StringComparison.OrdinalIgnoreCase)))
                {
                    // Parse the scanned name into a Name object and add it as an alias.
                    character.Alias.Add(ProseTools.Name.Parse(_scannedName));
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
