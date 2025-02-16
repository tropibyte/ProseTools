namespace ProseTools
{
    partial class aiSettingsManagerDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controls for the tabs and lists.
        private System.Windows.Forms.TabControl tabControlGenAI;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.TabPage tabUser;
        private System.Windows.Forms.ListBox lstSystemGenAI;
        private System.Windows.Forms.ListBox lstUserGenAI;

        // Buttons.
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

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
        /// Required method for Designer support – do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlGenAI = new System.Windows.Forms.TabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.lstSystemGenAI = new System.Windows.Forms.ListBox();
            this.lstUserGenAI = new System.Windows.Forms.ListBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControlGenAI.SuspendLayout();
            this.tabSystem.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlGenAI
            // 
            this.tabControlGenAI.Controls.Add(this.tabSystem);
            this.tabControlGenAI.Controls.Add(this.tabUser);
            this.tabControlGenAI.Location = new System.Drawing.Point(12, 12);
            this.tabControlGenAI.Name = "tabControlGenAI";
            this.tabControlGenAI.SelectedIndex = 0;
            this.tabControlGenAI.Size = new System.Drawing.Size(400, 300);
            this.tabControlGenAI.TabIndex = 0;
            // 
            // tabSystem
            // 
            this.tabSystem.Controls.Add(this.lstSystemGenAI);
            this.tabSystem.Location = new System.Drawing.Point(4, 29);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystem.Size = new System.Drawing.Size(392, 267);
            this.tabSystem.TabIndex = 0;
            this.tabSystem.Text = "System";
            this.tabSystem.UseVisualStyleBackColor = true;
            // 
            // lstSystemGenAI
            // 
            this.lstSystemGenAI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSystemGenAI.FormattingEnabled = true;
            this.lstSystemGenAI.ItemHeight = 20;
            this.lstSystemGenAI.Location = new System.Drawing.Point(3, 3);
            this.lstSystemGenAI.Name = "lstSystemGenAI";
            this.lstSystemGenAI.Size = new System.Drawing.Size(386, 261);
            this.lstSystemGenAI.TabIndex = 0;
            // 
            // tabUser
            // 
            this.tabUser.Controls.Add(this.lstUserGenAI);
            this.tabUser.Location = new System.Drawing.Point(4, 29);
            this.tabUser.Name = "tabUser";
            this.tabUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabUser.Size = new System.Drawing.Size(392, 267);
            this.tabUser.TabIndex = 1;
            this.tabUser.Text = "User";
            this.tabUser.UseVisualStyleBackColor = true;
            // 
            // lstUserGenAI
            // 
            this.lstUserGenAI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstUserGenAI.FormattingEnabled = true;
            this.lstUserGenAI.ItemHeight = 20;
            this.lstUserGenAI.Location = new System.Drawing.Point(3, 3);
            this.lstUserGenAI.Name = "lstUserGenAI";
            this.lstUserGenAI.Size = new System.Drawing.Size(386, 261);
            this.lstUserGenAI.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(418, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Configure";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(418, 282);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(418, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // aiSettingsManagerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 360);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.tabControlGenAI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "aiSettingsManagerDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI Settings Manager";
            this.tabControlGenAI.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
