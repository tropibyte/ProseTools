namespace ProseTools
{
    partial class SettingsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controls for the tab pages.
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.TabPage tabUser;
        private System.Windows.Forms.TabPage tabDocument;

        // System settings controls.
        private System.Windows.Forms.Label lblDefaultPath;
        private System.Windows.Forms.TextBox txtDefaultPath;
        private System.Windows.Forms.Label lblUpdateInterval;
        private System.Windows.Forms.NumericUpDown numUpdateInterval;

        // User settings controls.
        private System.Windows.Forms.Label lblTheme;
        private System.Windows.Forms.ComboBox cmbTheme;
        private System.Windows.Forms.CheckBox chkAutoSave;

        // Document settings controls.
        private System.Windows.Forms.CheckBox chkShowPageNumbers;
        private System.Windows.Forms.Label lblDefaultChapterStyle;
        private System.Windows.Forms.TextBox txtDefaultChapterStyle;

        // Buttons.
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
            this.components = new System.ComponentModel.Container();

            // Instantiate TabControl and TabPages.
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.tabDocument = new System.Windows.Forms.TabPage();

            // System tab controls.
            this.lblDefaultPath = new System.Windows.Forms.Label();
            this.txtDefaultPath = new System.Windows.Forms.TextBox();
            this.lblUpdateInterval = new System.Windows.Forms.Label();
            this.numUpdateInterval = new System.Windows.Forms.NumericUpDown();

            // User tab controls.
            this.lblTheme = new System.Windows.Forms.Label();
            this.cmbTheme = new System.Windows.Forms.ComboBox();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();

            // Document tab controls.
            this.chkShowPageNumbers = new System.Windows.Forms.CheckBox();
            this.lblDefaultChapterStyle = new System.Windows.Forms.Label();
            this.txtDefaultChapterStyle = new System.Windows.Forms.TextBox();

            // Buttons.
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // TabControl settings.
            this.tabControlSettings.Controls.Add(this.tabSystem);
            this.tabControlSettings.Controls.Add(this.tabUser);
            this.tabControlSettings.Controls.Add(this.tabDocument);
            this.tabControlSettings.Location = new System.Drawing.Point(12, 12);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(520, 350);
            this.tabControlSettings.TabIndex = 0;

            // --- System Tab ---
            this.tabSystem.Text = "System";
            this.tabSystem.UseVisualStyleBackColor = true;

            // lblDefaultPath.
            this.lblDefaultPath.AutoSize = true;
            this.lblDefaultPath.Location = new System.Drawing.Point(20, 20);
            this.lblDefaultPath.Name = "lblDefaultPath";
            this.lblDefaultPath.Size = new System.Drawing.Size(200, 20);
            this.lblDefaultPath.TabIndex = 0;
            this.lblDefaultPath.Text = "Default Installation Path:";

            // txtDefaultPath.
            this.txtDefaultPath.Location = new System.Drawing.Point(20, 45);
            this.txtDefaultPath.Name = "txtDefaultPath";
            this.txtDefaultPath.Size = new System.Drawing.Size(460, 26);
            this.txtDefaultPath.TabIndex = 1;

            // lblUpdateInterval.
            this.lblUpdateInterval.AutoSize = true;
            this.lblUpdateInterval.Location = new System.Drawing.Point(20, 85);
            this.lblUpdateInterval.Name = "lblUpdateInterval";
            this.lblUpdateInterval.Size = new System.Drawing.Size(180, 20);
            this.lblUpdateInterval.TabIndex = 2;
            this.lblUpdateInterval.Text = "Update Interval (minutes):";

            // numUpdateInterval.
            this.numUpdateInterval.Location = new System.Drawing.Point(20, 110);
            this.numUpdateInterval.Name = "numUpdateInterval";
            this.numUpdateInterval.Size = new System.Drawing.Size(120, 26);
            this.numUpdateInterval.TabIndex = 3;

            // Add controls to the System tab.
            this.tabSystem.Controls.Add(this.lblDefaultPath);
            this.tabSystem.Controls.Add(this.txtDefaultPath);
            this.tabSystem.Controls.Add(this.lblUpdateInterval);
            this.tabSystem.Controls.Add(this.numUpdateInterval);

            // --- User Tab ---
            this.tabUser.Text = "User";
            this.tabUser.UseVisualStyleBackColor = true;

            // lblTheme.
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(20, 20);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(65, 20);
            this.lblTheme.TabIndex = 0;
            this.lblTheme.Text = "Theme:";

            // cmbTheme.
            this.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTheme.FormattingEnabled = true;
            this.cmbTheme.Location = new System.Drawing.Point(20, 45);
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.Size = new System.Drawing.Size(200, 28);
            this.cmbTheme.TabIndex = 1;

            // chkAutoSave.
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Location = new System.Drawing.Point(20, 85);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(135, 24);
            this.chkAutoSave.TabIndex = 2;
            this.chkAutoSave.Text = "Enable Auto-Save";
            this.chkAutoSave.UseVisualStyleBackColor = true;

            // Add controls to the User tab.
            this.tabUser.Controls.Add(this.lblTheme);
            this.tabUser.Controls.Add(this.cmbTheme);
            this.tabUser.Controls.Add(this.chkAutoSave);

            // --- Document Tab ---
            this.tabDocument.Text = "Document";
            this.tabDocument.UseVisualStyleBackColor = true;

            // chkShowPageNumbers.
            this.chkShowPageNumbers.AutoSize = true;
            this.chkShowPageNumbers.Location = new System.Drawing.Point(20, 20);
            this.chkShowPageNumbers.Name = "chkShowPageNumbers";
            this.chkShowPageNumbers.Size = new System.Drawing.Size(180, 24);
            this.chkShowPageNumbers.TabIndex = 0;
            this.chkShowPageNumbers.Text = "Show Page Numbers";
            this.chkShowPageNumbers.UseVisualStyleBackColor = true;

            // lblDefaultChapterStyle.
            this.lblDefaultChapterStyle.AutoSize = true;
            this.lblDefaultChapterStyle.Location = new System.Drawing.Point(20, 60);
            this.lblDefaultChapterStyle.Name = "lblDefaultChapterStyle";
            this.lblDefaultChapterStyle.Size = new System.Drawing.Size(150, 20);
            this.lblDefaultChapterStyle.TabIndex = 1;
            this.lblDefaultChapterStyle.Text = "Default Chapter Style:";

            // txtDefaultChapterStyle.
            this.txtDefaultChapterStyle.Location = new System.Drawing.Point(20, 85);
            this.txtDefaultChapterStyle.Name = "txtDefaultChapterStyle";
            this.txtDefaultChapterStyle.Size = new System.Drawing.Size(200, 26);
            this.txtDefaultChapterStyle.TabIndex = 2;

            // Add controls to the Document tab.
            this.tabDocument.Controls.Add(this.chkShowPageNumbers);
            this.tabDocument.Controls.Add(this.lblDefaultChapterStyle);
            this.tabDocument.Controls.Add(this.txtDefaultChapterStyle);

            // --- Buttons ---
            this.btnOK.Location = new System.Drawing.Point(376, 380);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OK_Click);

            this.btnCancel.Location = new System.Drawing.Point(457, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);

            // --- Form Settings ---
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControlSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.tabControlSettings.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.tabSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateInterval)).EndInit();
            this.tabUser.ResumeLayout(false);
            this.tabUser.PerformLayout();
            this.tabDocument.ResumeLayout(false);
            this.tabDocument.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
