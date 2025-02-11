namespace ProseTools
{
    partial class aiSettingsDlg
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
            this.Ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelGenAI = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(270, 18);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(270, 47);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 0;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelGenAI
            // 
            this.labelGenAI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelGenAI.Location = new System.Drawing.Point(12, 18);
            this.labelGenAI.Name = "labelGenAI";
            this.labelGenAI.Size = new System.Drawing.Size(245, 23);
            this.labelGenAI.TabIndex = 1;
            this.labelGenAI.Text = "label1";
            // 
            // aiSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 331);
            this.Controls.Add(this.labelGenAI);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aiSettingsDlg";
            this.Text = "aiSettingsDlg";
            this.Load += new System.EventHandler(this.aiSettingsDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelGenAI;
    }
}