﻿namespace ProseTools
{
    partial class StartBook
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
            this.label1 = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.subtitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.author = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.dropDownFont = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.proseChapter = new System.Windows.Forms.Label();
            this.proseText = new System.Windows.Forms.Label();
            this.SetProseChapterFontInfoButton = new System.Windows.Forms.Button();
            this.SetProseTextFontInfoButton = new System.Windows.Forms.Button();
            this.proseSubchapter = new System.Windows.Forms.Label();
            this.SetProseSubchapterFontInfoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(15, 34);
            this.title.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(367, 22);
            this.title.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Subtitle";
            // 
            // subtitle
            // 
            this.subtitle.Location = new System.Drawing.Point(15, 87);
            this.subtitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.subtitle.Name = "subtitle";
            this.subtitle.Size = new System.Drawing.Size(367, 22);
            this.subtitle.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Author";
            // 
            // author
            // 
            this.author.Location = new System.Drawing.Point(15, 150);
            this.author.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.author.Name = "author";
            this.author.Size = new System.Drawing.Size(367, 22);
            this.author.TabIndex = 3;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(406, 10);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(79, 23);
            this.buttonStart.TabIndex = 8;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(406, 37);
            this.cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(79, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // dropDownFont
            // 
            this.dropDownFont.FormattingEnabled = true;
            this.dropDownFont.Location = new System.Drawing.Point(15, 211);
            this.dropDownFont.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dropDownFont.Name = "dropDownFont";
            this.dropDownFont.Size = new System.Drawing.Size(367, 24);
            this.dropDownFont.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Font";
            // 
            // proseChapter
            // 
            this.proseChapter.AutoSize = true;
            this.proseChapter.Location = new System.Drawing.Point(15, 259);
            this.proseChapter.Name = "proseChapter";
            this.proseChapter.Size = new System.Drawing.Size(93, 16);
            this.proseChapter.TabIndex = 5;
            this.proseChapter.Text = "Prose Chapter";
            // 
            // proseText
            // 
            this.proseText.AutoSize = true;
            this.proseText.Location = new System.Drawing.Point(15, 326);
            this.proseText.Name = "proseText";
            this.proseText.Size = new System.Drawing.Size(72, 16);
            this.proseText.TabIndex = 5;
            this.proseText.Text = "Prose Text";
            // 
            // SetProseChapterFontInfoButton
            // 
            this.SetProseChapterFontInfoButton.Location = new System.Drawing.Point(301, 257);
            this.SetProseChapterFontInfoButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SetProseChapterFontInfoButton.Name = "SetProseChapterFontInfoButton";
            this.SetProseChapterFontInfoButton.Size = new System.Drawing.Size(80, 28);
            this.SetProseChapterFontInfoButton.TabIndex = 5;
            this.SetProseChapterFontInfoButton.Text = "Set...";
            this.SetProseChapterFontInfoButton.UseVisualStyleBackColor = true;
            this.SetProseChapterFontInfoButton.Click += new System.EventHandler(this.SetProseChapterFontInfoButton_Click);
            // 
            // SetProseTextFontInfoButton
            // 
            this.SetProseTextFontInfoButton.Location = new System.Drawing.Point(301, 321);
            this.SetProseTextFontInfoButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SetProseTextFontInfoButton.Name = "SetProseTextFontInfoButton";
            this.SetProseTextFontInfoButton.Size = new System.Drawing.Size(80, 26);
            this.SetProseTextFontInfoButton.TabIndex = 7;
            this.SetProseTextFontInfoButton.Text = "Set...";
            this.SetProseTextFontInfoButton.UseVisualStyleBackColor = true;
            this.SetProseTextFontInfoButton.Click += new System.EventHandler(this.SetProseTextFontInfoButton_Click);
            // 
            // proseSubchapter
            // 
            this.proseSubchapter.AutoSize = true;
            this.proseSubchapter.Location = new System.Drawing.Point(15, 291);
            this.proseSubchapter.Name = "proseSubchapter";
            this.proseSubchapter.Size = new System.Drawing.Size(115, 16);
            this.proseSubchapter.TabIndex = 5;
            this.proseSubchapter.Text = "Prose Subchapter";
            // 
            // SetProseSubchapterFontInfoButton
            // 
            this.SetProseSubchapterFontInfoButton.Location = new System.Drawing.Point(301, 290);
            this.SetProseSubchapterFontInfoButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SetProseSubchapterFontInfoButton.Name = "SetProseSubchapterFontInfoButton";
            this.SetProseSubchapterFontInfoButton.Size = new System.Drawing.Size(80, 27);
            this.SetProseSubchapterFontInfoButton.TabIndex = 6;
            this.SetProseSubchapterFontInfoButton.Text = "Set...";
            this.SetProseSubchapterFontInfoButton.UseVisualStyleBackColor = true;
            this.SetProseSubchapterFontInfoButton.Click += new System.EventHandler(this.SetProseTextFontInfoButton_Click);
            // 
            // StartBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 379);
            this.Controls.Add(this.SetProseSubchapterFontInfoButton);
            this.Controls.Add(this.SetProseTextFontInfoButton);
            this.Controls.Add(this.SetProseChapterFontInfoButton);
            this.Controls.Add(this.proseSubchapter);
            this.Controls.Add(this.proseText);
            this.Controls.Add(this.proseChapter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dropDownFont);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.author);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.subtitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.title);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartBook";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Start Book";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox subtitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox author;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ComboBox dropDownFont;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label proseChapter;
        private System.Windows.Forms.Label proseText;
        private System.Windows.Forms.Button SetProseChapterFontInfoButton;
        private System.Windows.Forms.Button SetProseTextFontInfoButton;
        private System.Windows.Forms.Label proseSubchapter;
        private System.Windows.Forms.Button SetProseSubchapterFontInfoButton;
    }
}