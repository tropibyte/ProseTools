using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class CharacterManagement : Form
    {
        public List<Character> ListCharacters { get; set; }

        public CharacterManagement()
        {
            InitializeComponent();
            InitializeListView();
            LoadCharacterData();
        }

        /// <summary>
        /// Configures the ListView control (if columns aren’t already set in the designer).
        /// </summary>
        private void InitializeListView()
        {
            listViewCharacters.View = View.Details;
            listViewCharacters.FullRowSelect = true;
            listViewCharacters.GridLines = true;

            // Add columns if they haven't been defined in the designer.
            if (listViewCharacters.Columns.Count == 0)
            {
                listViewCharacters.Columns.Add("Full Name", 200);
                listViewCharacters.Columns.Add("Aliases", 250);
                listViewCharacters.Columns.Add("Type", 150);
            }
        }

        /// <summary>
        /// Loads the characters from the NovelMetaData into the ListView.
        /// </summary>
        private void LoadCharacterData()
        {
            listViewCharacters.Items.Clear();

            if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
            {
                ListCharacters = novel.ListCharacters;

                foreach (var character in ListCharacters)
                {
                    string fullName = character.CharacterName.ToString();
                    var item = new ListViewItem(fullName);

                    // Build alias string from the Alias list
                    string aliasStr = string.Join(", ", character.Alias.Select(a => a.ToString()));
                    item.SubItems.Add(aliasStr);
                    item.SubItems.Add(character.CharacterType);

                    listViewCharacters.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Handles the Add button click by opening CharacterForm in "new" mode.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var charForm = new CharacterForm();
            if (charForm.ShowDialog() == DialogResult.OK)
            {
                ListCharacters.Add(charForm.CharacterData);
                LoadCharacterData();
            }
        }

        /// <summary>
        /// Handles the Edit button click by opening CharacterForm preloaded with the selected character.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listViewCharacters.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a character to edit.", "Edit Character", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string selectedName = listViewCharacters.SelectedItems[0].Text;

            var character = ListCharacters.FirstOrDefault(c => c.CharacterName.ToString() == selectedName);
            if (character != null)
            {
                var charForm = new CharacterForm();
                charForm.LoadCharacter(character);
                if (charForm.ShowDialog() == DialogResult.OK)
                {
                    // Update the character in the list
                    int index = ListCharacters.IndexOf(character);
                    ListCharacters[index] = charForm.CharacterData;
                    LoadCharacterData();
                }
            }
        }

        /// <summary>
        /// Handles the Delete button click by removing the selected character after confirmation.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewCharacters.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a character to delete.", "Delete Character", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string selectedName = listViewCharacters.SelectedItems[0].Text;

            var character = ListCharacters.FirstOrDefault(c => c.CharacterName.ToString() == selectedName);
            if (character != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete {selectedName}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    ListCharacters.Remove(character);
                    LoadCharacterData();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Optionally, perform validation or commit changes here.
            if (Globals.ThisAddIn._ProseMetaData is NovelMetaData novel)
            {
                novel.ListCharacters = ListCharacters;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Optionally, roll back any changes here.
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
