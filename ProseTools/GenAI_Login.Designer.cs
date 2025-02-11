using System;

namespace ProseTools
{
    partial class GenAI_Login
    {
        private System.ComponentModel.IContainer components = null;

        // Designer-generated controls
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblGenAI;
        private System.Windows.Forms.ComboBox cbGenAI;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblAuthUrl;
        private System.Windows.Forms.TextBox txtAuthUrl;
        private System.Windows.Forms.Label lblOAuthKey;
        private System.Windows.Forms.TextBox txtOAuthKey;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnAddNewAI; // optional: a button to add new entries

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblGenAI = new System.Windows.Forms.Label();
            this.cbGenAI = new System.Windows.Forms.ComboBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblAuthUrl = new System.Windows.Forms.Label();
            this.txtAuthUrl = new System.Windows.Forms.TextBox();
            this.lblOAuthKey = new System.Windows.Forms.Label();
            this.txtOAuthKey = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnAddNewAI = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(80, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(139, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AI Login Form";
            // 
            // lblGenAI
            // 
            this.lblGenAI.AutoSize = true;
            this.lblGenAI.Location = new System.Drawing.Point(25, 70);
            this.lblGenAI.Name = "lblGenAI";
            this.lblGenAI.Size = new System.Drawing.Size(64, 13);
            this.lblGenAI.TabIndex = 1;
            this.lblGenAI.Text = "Choose AI:";
            // 
            // cbGenAI
            // 
            this.cbGenAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGenAI.FormattingEnabled = true;
            this.cbGenAI.Location = new System.Drawing.Point(95, 67);
            this.cbGenAI.Name = "cbGenAI";
            this.cbGenAI.Size = new System.Drawing.Size(170, 21);
            this.cbGenAI.TabIndex = 2;
            this.cbGenAI.SelectedIndexChanged += new System.EventHandler(this.cbGenAI_SelectedIndexChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(25, 110);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(95, 107);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(170, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(25, 150);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(58, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(95, 147);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(170, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblAuthUrl
            // 
            this.lblAuthUrl.AutoSize = true;
            this.lblAuthUrl.Location = new System.Drawing.Point(25, 190);
            this.lblAuthUrl.Name = "lblAuthUrl";
            this.lblAuthUrl.Size = new System.Drawing.Size(52, 13);
            this.lblAuthUrl.TabIndex = 7;
            this.lblAuthUrl.Text = "Auth URL";
            // 
            // txtAuthUrl
            // 
            this.txtAuthUrl.Location = new System.Drawing.Point(95, 187);
            this.txtAuthUrl.Name = "txtAuthUrl";
            this.txtAuthUrl.Size = new System.Drawing.Size(170, 20);
            this.txtAuthUrl.TabIndex = 8;
            // 
            // lblOAuthKey
            // 
            this.lblOAuthKey.AutoSize = true;
            this.lblOAuthKey.Location = new System.Drawing.Point(25, 230);
            this.lblOAuthKey.Name = "lblOAuthKey";
            this.lblOAuthKey.Size = new System.Drawing.Size(61, 13);
            this.lblOAuthKey.TabIndex = 9;
            this.lblOAuthKey.Text = "OAuth Key:";
            // 
            // txtOAuthKey
            // 
            this.txtOAuthKey.Location = new System.Drawing.Point(95, 227);
            this.txtOAuthKey.Name = "txtOAuthKey";
            this.txtOAuthKey.Size = new System.Drawing.Size(170, 20);
            this.txtOAuthKey.TabIndex = 10;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(95, 270);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(90, 30);
            this.btnLogin.TabIndex = 11;
            this.btnLogin.Text = "Sign In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnAddNewAI
            // 
            this.btnAddNewAI.Location = new System.Drawing.Point(190, 270);
            this.btnAddNewAI.Name = "btnAddNewAI";
            this.btnAddNewAI.Size = new System.Drawing.Size(75, 30);
            this.btnAddNewAI.TabIndex = 12;
            this.btnAddNewAI.Text = "Add New AI";
            this.btnAddNewAI.UseVisualStyleBackColor = true;
            this.btnAddNewAI.Click += new System.EventHandler(this.btnAddNewAI_Click);
            // 
            // AIForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 321);
            this.Controls.Add(this.btnAddNewAI);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtOAuthKey);
            this.Controls.Add(this.lblOAuthKey);
            this.Controls.Add(this.txtAuthUrl);
            this.Controls.Add(this.lblAuthUrl);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.cbGenAI);
            this.Controls.Add(this.lblGenAI);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}