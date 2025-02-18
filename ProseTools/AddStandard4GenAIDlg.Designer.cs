﻿namespace ProseTools
{
    partial class AddStandard4GenAIDlg
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
            this.chkboxChatGPT = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxCoPilot = new System.Windows.Forms.CheckBox();
            this.chkboxGemini = new System.Windows.Forms.CheckBox();
            this.chkBoxClaude = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkboxChatGPT
            // 
            this.chkboxChatGPT.AutoSize = true;
            this.chkboxChatGPT.Location = new System.Drawing.Point(10, 25);
            this.chkboxChatGPT.Name = "chkboxChatGPT";
            this.chkboxChatGPT.Size = new System.Drawing.Size(84, 20);
            this.chkboxChatGPT.TabIndex = 0;
            this.chkboxChatGPT.Text = "ChatGPT";
            this.chkboxChatGPT.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkBoxClaude);
            this.groupBox1.Controls.Add(this.chkboxGemini);
            this.groupBox1.Controls.Add(this.chkBoxCoPilot);
            this.groupBox1.Controls.Add(this.chkboxChatGPT);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Standard GenAI";
            // 
            // chkBoxCoPilot
            // 
            this.chkBoxCoPilot.AutoSize = true;
            this.chkBoxCoPilot.Location = new System.Drawing.Point(10, 51);
            this.chkBoxCoPilot.Name = "chkBoxCoPilot";
            this.chkBoxCoPilot.Size = new System.Drawing.Size(72, 20);
            this.chkBoxCoPilot.TabIndex = 0;
            this.chkBoxCoPilot.Text = "CoPilot";
            this.chkBoxCoPilot.UseVisualStyleBackColor = true;
            // 
            // chkboxGemini
            // 
            this.chkboxGemini.AutoSize = true;
            this.chkboxGemini.Location = new System.Drawing.Point(10, 77);
            this.chkboxGemini.Name = "chkboxGemini";
            this.chkboxGemini.Size = new System.Drawing.Size(71, 20);
            this.chkboxGemini.TabIndex = 0;
            this.chkboxGemini.Text = "Gemini";
            this.chkboxGemini.UseVisualStyleBackColor = true;
            // 
            // chkBoxClaude
            // 
            this.chkBoxClaude.AutoSize = true;
            this.chkBoxClaude.Location = new System.Drawing.Point(10, 103);
            this.chkBoxClaude.Name = "chkBoxClaude";
            this.chkBoxClaude.Size = new System.Drawing.Size(72, 20);
            this.chkBoxClaude.TabIndex = 0;
            this.chkBoxClaude.Text = "Claude";
            this.chkBoxClaude.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(210, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(210, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 55);
            this.label1.TabIndex = 3;
            this.label1.Text = "User is still required to enter the proper API Key or username/password conbinati" +
    "on to utilize these services!";
            // 
            // AddStandard4GenAIDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 218);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddStandard4GenAIDlg";
            this.Text = "Standard Four";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkboxChatGPT;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBoxClaude;
        private System.Windows.Forms.CheckBox chkboxGemini;
        private System.Windows.Forms.CheckBox chkBoxCoPilot;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
    }
}