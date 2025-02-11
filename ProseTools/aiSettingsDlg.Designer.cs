namespace ProseTools
{
    partial class aiSettingsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Declare new controls for GenAI configuration.
        private System.Windows.Forms.Label labelGenAI;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.Label lblAuthUrl;
        private System.Windows.Forms.TextBox txtAuthUrl;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.CheckBox chkUseApiKey;
        private System.Windows.Forms.Button btnOk;
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
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelGenAI = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblAuthUrl = new System.Windows.Forms.Label();
            this.txtAuthUrl = new System.Windows.Forms.TextBox();
            this.lblModelName = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.chkUseApiKey = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelGenAI
            // 
            this.labelGenAI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelGenAI.Location = new System.Drawing.Point(12, 9);
            this.labelGenAI.Name = "labelGenAI";
            this.labelGenAI.Size = new System.Drawing.Size(245, 23);
            this.labelGenAI.TabIndex = 0;
            this.labelGenAI.Text = "GenAI Service";
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(12, 45);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 23);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(120, 45);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 22);
            this.txtUsername.TabIndex = 2;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(12, 75);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(120, 75);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 22);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblApiKey
            // 
            this.lblApiKey.Location = new System.Drawing.Point(12, 105);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(100, 23);
            this.lblApiKey.TabIndex = 5;
            this.lblApiKey.Text = "API Key:";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(120, 105);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(200, 22);
            this.txtApiKey.TabIndex = 6;
            // 
            // lblAuthUrl
            // 
            this.lblAuthUrl.Location = new System.Drawing.Point(12, 135);
            this.lblAuthUrl.Name = "lblAuthUrl";
            this.lblAuthUrl.Size = new System.Drawing.Size(100, 23);
            this.lblAuthUrl.TabIndex = 7;
            this.lblAuthUrl.Text = "Auth URL:";
            // 
            // txtAuthUrl
            // 
            this.txtAuthUrl.Location = new System.Drawing.Point(120, 135);
            this.txtAuthUrl.Name = "txtAuthUrl";
            this.txtAuthUrl.Size = new System.Drawing.Size(200, 22);
            this.txtAuthUrl.TabIndex = 8;
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(12, 165);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(100, 23);
            this.lblModelName.TabIndex = 9;
            this.lblModelName.Text = "Model:";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(120, 165);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(200, 22);
            this.txtModelName.TabIndex = 10;
            // 
            // chkUseApiKey
            // 
            this.chkUseApiKey.Location = new System.Drawing.Point(12, 195);
            this.chkUseApiKey.Name = "chkUseApiKey";
            this.chkUseApiKey.Size = new System.Drawing.Size(250, 24);
            this.chkUseApiKey.TabIndex = 11;
            this.chkUseApiKey.Text = "Use API Key for Authentication";
            this.chkUseApiKey.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(120, 235);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(245, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // aiSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkUseApiKey);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.lblModelName);
            this.Controls.Add(this.txtAuthUrl);
            this.Controls.Add(this.lblAuthUrl);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.lblApiKey);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.labelGenAI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aiSettingsDlg";
            this.Text = "AI Settings";
            this.Load += new System.EventHandler(this.aiSettingsDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
