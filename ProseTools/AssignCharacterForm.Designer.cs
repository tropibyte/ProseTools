namespace ProseTools
{
    partial class AssignCharacterForm
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
            this.lblScannedName = new System.Windows.Forms.Label();
            this.lvCharacters = new System.Windows.Forms.ListView();
            this.Ok = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblScannedName
            // 
            this.lblScannedName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblScannedName.Location = new System.Drawing.Point(13, 13);
            this.lblScannedName.Name = "lblScannedName";
            this.lblScannedName.Size = new System.Drawing.Size(478, 23);
            this.lblScannedName.TabIndex = 0;
            this.lblScannedName.Text = "label1";
            // 
            // lvCharacters
            // 
            this.lvCharacters.HideSelection = false;
            this.lvCharacters.Location = new System.Drawing.Point(12, 48);
            this.lvCharacters.Name = "lvCharacters";
            this.lvCharacters.Size = new System.Drawing.Size(479, 310);
            this.lvCharacters.TabIndex = 1;
            this.lvCharacters.UseCompatibleStateImageBehavior = false;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(522, 13);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(118, 23);
            this.Ok.TabIndex = 2;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(522, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(522, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "New Character";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnNewCharacter_Click);
            // 
            // AssignCharacterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.lvCharacters);
            this.Controls.Add(this.lblScannedName);
            this.Name = "AssignCharacterForm";
            this.Text = "AssignCharacterForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblScannedName;
        private System.Windows.Forms.ListView lvCharacters;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}