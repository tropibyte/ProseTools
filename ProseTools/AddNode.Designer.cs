namespace ProseTools
{
    partial class AddNodeDlg
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
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelParentNode = new System.Windows.Forms.Label();
            this.btnManageAttributes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(72, 41);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(303, 22);
            this.txtTitle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Title";
            // 
            // txtDetails
            // 
            this.txtDetails.Location = new System.Drawing.Point(72, 70);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.Size = new System.Drawing.Size(303, 82);
            this.txtDetails.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Details";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(72, 158);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(303, 104);
            this.txtNotes.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Notes";
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(390, 13);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(83, 23);
            this.Ok.TabIndex = 2;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(390, 40);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(83, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // labelParentNode
            // 
            this.labelParentNode.Location = new System.Drawing.Point(13, 13);
            this.labelParentNode.Name = "labelParentNode";
            this.labelParentNode.Size = new System.Drawing.Size(362, 23);
            this.labelParentNode.TabIndex = 1;
            this.labelParentNode.Text = "Parent Node: ";
            // 
            // btnManageAttributes
            // 
            this.btnManageAttributes.Location = new System.Drawing.Point(72, 269);
            this.btnManageAttributes.Name = "btnManageAttributes";
            this.btnManageAttributes.Size = new System.Drawing.Size(303, 23);
            this.btnManageAttributes.TabIndex = 3;
            this.btnManageAttributes.Text = "Manage Attributes...";
            this.btnManageAttributes.UseVisualStyleBackColor = true;
            this.btnManageAttributes.Click += new System.EventHandler(this.btnManageAttributes_Click);
            // 
            // AddNodeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 299);
            this.Controls.Add(this.btnManageAttributes);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelParentNode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.txtTitle);
            this.Name = "AddNodeDlg";
            this.Text = "Add Node";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelParentNode;
        private System.Windows.Forms.Button btnManageAttributes;
    }
}