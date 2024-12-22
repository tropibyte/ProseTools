namespace ProseTools
{
    partial class CharacterManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewCharacters = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listViewCharacters
            // 
            this.listViewCharacters.GridLines = true;
            this.listViewCharacters.HideSelection = false;
            this.listViewCharacters.Location = new System.Drawing.Point(12, 54);
            this.listViewCharacters.Name = "listViewCharacters";
            this.listViewCharacters.Size = new System.Drawing.Size(557, 384);
            this.listViewCharacters.TabIndex = 1;
            this.listViewCharacters.UseCompatibleStateImageBehavior = false;
            this.listViewCharacters.View = System.Windows.Forms.View.Details;
            // 
            // CharacterManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewCharacters);
            this.Name = "CharacterManagement";
            this.Text = "CharacterManagement";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCharacters;
    }
}