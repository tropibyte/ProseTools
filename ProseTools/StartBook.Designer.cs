namespace ProseTools
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(17, 42);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(412, 26);
            this.title.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Subtitle";
            // 
            // subtitle
            // 
            this.subtitle.Location = new System.Drawing.Point(17, 109);
            this.subtitle.Name = "subtitle";
            this.subtitle.Size = new System.Drawing.Size(412, 26);
            this.subtitle.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Author";
            // 
            // author
            // 
            this.author.Location = new System.Drawing.Point(17, 187);
            this.author.Name = "author";
            this.author.Size = new System.Drawing.Size(412, 26);
            this.author.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(463, 50);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(463, 79);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // dropDownFont
            // 
            this.dropDownFont.FormattingEnabled = true;
            this.dropDownFont.Location = new System.Drawing.Point(17, 264);
            this.dropDownFont.Name = "dropDownFont";
            this.dropDownFont.Size = new System.Drawing.Size(412, 28);
            this.dropDownFont.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Font";
            // 
            // proseChapter
            // 
            this.proseChapter.AutoSize = true;
            this.proseChapter.Location = new System.Drawing.Point(17, 324);
            this.proseChapter.Name = "proseChapter";
            this.proseChapter.Size = new System.Drawing.Size(111, 20);
            this.proseChapter.TabIndex = 5;
            this.proseChapter.Text = "Prose Chapter";
            // 
            // proseText
            // 
            this.proseText.AutoSize = true;
            this.proseText.Location = new System.Drawing.Point(17, 364);
            this.proseText.Name = "proseText";
            this.proseText.Size = new System.Drawing.Size(84, 20);
            this.proseText.TabIndex = 5;
            this.proseText.Text = "Prose Text";
            // 
            // SetProseChapterFontInfoButton
            // 
            this.SetProseChapterFontInfoButton.Location = new System.Drawing.Point(353, 321);
            this.SetProseChapterFontInfoButton.Name = "SetProseChapterFontInfoButton";
            this.SetProseChapterFontInfoButton.Size = new System.Drawing.Size(75, 23);
            this.SetProseChapterFontInfoButton.TabIndex = 6;
            this.SetProseChapterFontInfoButton.Text = "Set...";
            this.SetProseChapterFontInfoButton.UseVisualStyleBackColor = true;
            this.SetProseChapterFontInfoButton.Click += new System.EventHandler(this.SetProseChapterFontInfoButton_Click);
            // 
            // SetProseTextFontInfoButton
            // 
            this.SetProseTextFontInfoButton.Location = new System.Drawing.Point(353, 363);
            this.SetProseTextFontInfoButton.Name = "SetProseTextFontInfoButton";
            this.SetProseTextFontInfoButton.Size = new System.Drawing.Size(75, 23);
            this.SetProseTextFontInfoButton.TabIndex = 6;
            this.SetProseTextFontInfoButton.Text = "Set...";
            this.SetProseTextFontInfoButton.UseVisualStyleBackColor = true;
            // 
            // StartBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 450);
            this.Controls.Add(this.SetProseTextFontInfoButton);
            this.Controls.Add(this.SetProseChapterFontInfoButton);
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
            this.Name = "StartBook";
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
    }
}