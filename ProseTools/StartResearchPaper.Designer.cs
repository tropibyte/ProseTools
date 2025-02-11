using System;

namespace ProseTools
{
    partial class StartResearchPaper
    {
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.labelFormat = new System.Windows.Forms.Label();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxInstitution = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCourse = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxProfessor = new System.Windows.Forms.TextBox();
            this.SetPaperFontButton = new System.Windows.Forms.Button();
            this.labelPaperFont = new System.Windows.Forms.Label();
            this.checkBoxAddTOC = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(13, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(38, 20);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Title";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(17, 41);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(412, 26);
            this.textBoxTitle.TabIndex = 1;
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Location = new System.Drawing.Point(13, 80);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(57, 20);
            this.labelAuthor.TabIndex = 2;
            this.labelAuthor.Text = "Author";
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Location = new System.Drawing.Point(17, 107);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(412, 26);
            this.textBoxAuthor.TabIndex = 3;
            // 
            // labelFormat
            // 
            this.labelFormat.AutoSize = true;
            this.labelFormat.Location = new System.Drawing.Point(466, 121);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(60, 20);
            this.labelFormat.TabIndex = 4;
            this.labelFormat.Text = "Format";
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.Location = new System.Drawing.Point(470, 151);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(201, 28);
            this.comboBoxFormat.TabIndex = 4;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(565, 32);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(106, 33);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(565, 69);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(106, 33);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Institution";
            // 
            // textBoxInstitution
            // 
            this.textBoxInstitution.Location = new System.Drawing.Point(17, 178);
            this.textBoxInstitution.Name = "textBoxInstitution";
            this.textBoxInstitution.Size = new System.Drawing.Size(412, 26);
            this.textBoxInstitution.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Course";
            // 
            // textBoxCourse
            // 
            this.textBoxCourse.Location = new System.Drawing.Point(17, 246);
            this.textBoxCourse.Name = "textBoxCourse";
            this.textBoxCourse.Size = new System.Drawing.Size(412, 26);
            this.textBoxCourse.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Professor";
            // 
            // textBoxProfessor
            // 
            this.textBoxProfessor.Location = new System.Drawing.Point(17, 319);
            this.textBoxProfessor.Name = "textBoxProfessor";
            this.textBoxProfessor.Size = new System.Drawing.Size(412, 26);
            this.textBoxProfessor.TabIndex = 3;
            // 
            // SetPaperFontButton
            // 
            this.SetPaperFontButton.Location = new System.Drawing.Point(470, 308);
            this.SetPaperFontButton.Name = "SetPaperFontButton";
            this.SetPaperFontButton.Size = new System.Drawing.Size(201, 40);
            this.SetPaperFontButton.TabIndex = 8;
            this.SetPaperFontButton.Text = "Set Paper\'s Font";
            this.SetPaperFontButton.UseVisualStyleBackColor = true;
            this.SetPaperFontButton.Click += new System.EventHandler(this.SetPaperFontButton_Click);
            // 
            // labelPaperFont
            // 
            this.labelPaperFont.Location = new System.Drawing.Point(470, 204);
            this.labelPaperFont.Name = "labelPaperFont";
            this.labelPaperFont.Size = new System.Drawing.Size(201, 68);
            this.labelPaperFont.TabIndex = 9;
            this.labelPaperFont.Text = "label4";
            // 
            // checkBoxAddTOC
            // 
            this.checkBoxAddTOC.AutoSize = true;
            this.checkBoxAddTOC.Location = new System.Drawing.Point(470, 376);
            this.checkBoxAddTOC.Name = "checkBoxAddTOC";
            this.checkBoxAddTOC.Size = new System.Drawing.Size(194, 24);
            this.checkBoxAddTOC.TabIndex = 10;
            this.checkBoxAddTOC.Text = "Add Table of Contents";
            this.checkBoxAddTOC.UseVisualStyleBackColor = true;
            // 
            // StartResearchPaper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 430);
            this.Controls.Add(this.checkBoxAddTOC);
            this.Controls.Add(this.labelPaperFont);
            this.Controls.Add(this.SetPaperFontButton);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.comboBoxFormat);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.textBoxProfessor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCourse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAuthor);
            this.Controls.Add(this.textBoxInstitution);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartResearchPaper";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Start Research Paper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxInstitution;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCourse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxProfessor;
        private System.Windows.Forms.Button SetPaperFontButton;
        private System.Windows.Forms.Label labelPaperFont;
        private System.Windows.Forms.CheckBox checkBoxAddTOC;
    }
}
