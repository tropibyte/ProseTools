namespace ProseTools
{
    partial class StartTechnicalPaper
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.TitleLabel = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.AuthorTextBox = new System.Windows.Forms.TextBox();
            this.InstitutionLabel = new System.Windows.Forms.Label();
            this.InstitutionTextBox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // TitleLabel
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Location = new System.Drawing.Point(12, 15);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(30, 13);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Title:";

            // TitleTextBox
            this.TitleTextBox.Location = new System.Drawing.Point(80, 12);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(280, 20);
            this.TitleTextBox.TabIndex = 1;

            // AuthorLabel
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.Location = new System.Drawing.Point(12, 45);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(41, 13);
            this.AuthorLabel.TabIndex = 2;
            this.AuthorLabel.Text = "Author:";

            // AuthorTextBox
            this.AuthorTextBox.Location = new System.Drawing.Point(80, 42);
            this.AuthorTextBox.Name = "AuthorTextBox";
            this.AuthorTextBox.Size = new System.Drawing.Size(280, 20);
            this.AuthorTextBox.TabIndex = 3;

            // InstitutionLabel
            this.InstitutionLabel.AutoSize = true;
            this.InstitutionLabel.Location = new System.Drawing.Point(12, 75);
            this.InstitutionLabel.Name = "InstitutionLabel";
            this.InstitutionLabel.Size = new System.Drawing.Size(56, 13);
            this.InstitutionLabel.TabIndex = 4;
            this.InstitutionLabel.Text = "Institution:";

            // InstitutionTextBox
            this.InstitutionTextBox.Location = new System.Drawing.Point(80, 72);
            this.InstitutionTextBox.Name = "InstitutionTextBox";
            this.InstitutionTextBox.Size = new System.Drawing.Size(280, 20);
            this.InstitutionTextBox.TabIndex = 5;

            // StartButton
            this.StartButton.Location = new System.Drawing.Point(80, 110);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);

            // buttonCancel
            this.buttonCancel.Location = new System.Drawing.Point(285, 110);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);

            // StartTechnicalPaper
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.InstitutionTextBox);
            this.Controls.Add(this.InstitutionLabel);
            this.Controls.Add(this.AuthorTextBox);
            this.Controls.Add(this.AuthorLabel);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.TitleLabel);
            this.Name = "StartTechnicalPaper";
            this.Text = "Start Technical Paper";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.TextBox AuthorTextBox;
        private System.Windows.Forms.Label InstitutionLabel;
        private System.Windows.Forms.TextBox InstitutionTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button buttonCancel;
    }
}
