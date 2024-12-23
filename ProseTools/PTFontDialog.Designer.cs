namespace ProseTools
{
    partial class PTFontDialog
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
            this.dropDownFontList = new System.Windows.Forms.ComboBox();
            this.pointSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBoxBold = new System.Windows.Forms.CheckBox();
            this.chkBoxItalic = new System.Windows.Forms.CheckBox();
            this.chkBoxUnderline = new System.Windows.Forms.CheckBox();
            this.chkBoxStrike = new System.Windows.Forms.CheckBox();
            this.chkBoxLeft = new System.Windows.Forms.CheckBox();
            this.chkBoxCenter = new System.Windows.Forms.CheckBox();
            this.chkBoxRight = new System.Windows.Forms.CheckBox();
            this.chkBoxJustify = new System.Windows.Forms.CheckBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelSample = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pointSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // dropDownFontList
            // 
            this.dropDownFontList.FormattingEnabled = true;
            this.dropDownFontList.Location = new System.Drawing.Point(13, 51);
            this.dropDownFontList.Name = "dropDownFontList";
            this.dropDownFontList.Size = new System.Drawing.Size(294, 28);
            this.dropDownFontList.TabIndex = 0;
            this.dropDownFontList.SelectedIndexChanged += new System.EventHandler(this.dropDownFontList_SelectedIndexChanged);
            // 
            // pointSizeUpDown
            // 
            this.pointSizeUpDown.Location = new System.Drawing.Point(334, 51);
            this.pointSizeUpDown.Name = "pointSizeUpDown";
            this.pointSizeUpDown.Size = new System.Drawing.Size(93, 26);
            this.pointSizeUpDown.TabIndex = 2;
            this.pointSizeUpDown.ValueChanged += new System.EventHandler(this.pointSizeUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Font ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(330, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Point Size";
            // 
            // chkBoxBold
            // 
            this.chkBoxBold.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxBold.Location = new System.Drawing.Point(13, 99);
            this.chkBoxBold.Name = "chkBoxBold";
            this.chkBoxBold.Size = new System.Drawing.Size(46, 48);
            this.chkBoxBold.TabIndex = 5;
            this.chkBoxBold.Text = "B";
            this.chkBoxBold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxBold.UseVisualStyleBackColor = true;
            this.chkBoxBold.CheckedChanged += new System.EventHandler(this.chkBoxBold_CheckedChanged);
            // 
            // chkBoxItalic
            // 
            this.chkBoxItalic.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxItalic.Location = new System.Drawing.Point(59, 99);
            this.chkBoxItalic.Name = "chkBoxItalic";
            this.chkBoxItalic.Size = new System.Drawing.Size(50, 48);
            this.chkBoxItalic.TabIndex = 5;
            this.chkBoxItalic.Text = "I";
            this.chkBoxItalic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxItalic.UseVisualStyleBackColor = true;
            this.chkBoxItalic.CheckedChanged += new System.EventHandler(this.chkBoxItalic_CheckedChanged);
            // 
            // chkBoxUnderline
            // 
            this.chkBoxUnderline.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxUnderline.Location = new System.Drawing.Point(109, 99);
            this.chkBoxUnderline.Name = "chkBoxUnderline";
            this.chkBoxUnderline.Size = new System.Drawing.Size(49, 48);
            this.chkBoxUnderline.TabIndex = 5;
            this.chkBoxUnderline.Text = "U";
            this.chkBoxUnderline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxUnderline.UseVisualStyleBackColor = true;
            this.chkBoxUnderline.CheckedChanged += new System.EventHandler(this.chkBoxUnderline_CheckedChanged);
            // 
            // chkBoxStrike
            // 
            this.chkBoxStrike.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxStrike.Location = new System.Drawing.Point(158, 99);
            this.chkBoxStrike.Name = "chkBoxStrike";
            this.chkBoxStrike.Size = new System.Drawing.Size(50, 48);
            this.chkBoxStrike.TabIndex = 5;
            this.chkBoxStrike.Text = "S";
            this.chkBoxStrike.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxStrike.UseVisualStyleBackColor = true;
            this.chkBoxStrike.CheckedChanged += new System.EventHandler(this.chkBoxStrike_CheckedChanged);
            // 
            // chkBoxLeft
            // 
            this.chkBoxLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxLeft.Location = new System.Drawing.Point(278, 99);
            this.chkBoxLeft.Name = "chkBoxLeft";
            this.chkBoxLeft.Size = new System.Drawing.Size(50, 48);
            this.chkBoxLeft.TabIndex = 5;
            this.chkBoxLeft.Text = "L";
            this.chkBoxLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxLeft.UseVisualStyleBackColor = true;
            this.chkBoxLeft.CheckedChanged += new System.EventHandler(this.chkBoxLeft_CheckedChanged);
            // 
            // chkBoxCenter
            // 
            this.chkBoxCenter.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxCenter.Location = new System.Drawing.Point(328, 99);
            this.chkBoxCenter.Name = "chkBoxCenter";
            this.chkBoxCenter.Size = new System.Drawing.Size(50, 48);
            this.chkBoxCenter.TabIndex = 5;
            this.chkBoxCenter.Text = "C";
            this.chkBoxCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxCenter.UseVisualStyleBackColor = true;
            this.chkBoxCenter.CheckedChanged += new System.EventHandler(this.chkBoxCenter_CheckedChanged);
            // 
            // chkBoxRight
            // 
            this.chkBoxRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxRight.Location = new System.Drawing.Point(378, 99);
            this.chkBoxRight.Name = "chkBoxRight";
            this.chkBoxRight.Size = new System.Drawing.Size(51, 48);
            this.chkBoxRight.TabIndex = 5;
            this.chkBoxRight.Text = "R";
            this.chkBoxRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxRight.UseVisualStyleBackColor = true;
            this.chkBoxRight.CheckedChanged += new System.EventHandler(this.chkBoxRight_CheckedChanged);
            // 
            // chkBoxJustify
            // 
            this.chkBoxJustify.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBoxJustify.Location = new System.Drawing.Point(429, 99);
            this.chkBoxJustify.Name = "chkBoxJustify";
            this.chkBoxJustify.Size = new System.Drawing.Size(52, 48);
            this.chkBoxJustify.TabIndex = 5;
            this.chkBoxJustify.Text = "J";
            this.chkBoxJustify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBoxJustify.UseVisualStyleBackColor = true;
            this.chkBoxJustify.CheckedChanged += new System.EventHandler(this.chkBoxJustify_CheckedChanged);
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(527, 19);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(82, 32);
            this.Ok.TabIndex = 6;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(527, 57);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(82, 30);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Location = new System.Drawing.Point(17, 207);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(223, 28);
            this.comboBoxColor.TabIndex = 7;
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxColor_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Color";
            // 
            // labelSample
            // 
            this.labelSample.Location = new System.Drawing.Point(278, 185);
            this.labelSample.Name = "labelSample";
            this.labelSample.Size = new System.Drawing.Size(223, 98);
            this.labelSample.TabIndex = 9;
            this.labelSample.Text = "label4";
            // 
            // PTFontDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 311);
            this.Controls.Add(this.labelSample);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.chkBoxJustify);
            this.Controls.Add(this.chkBoxRight);
            this.Controls.Add(this.chkBoxStrike);
            this.Controls.Add(this.chkBoxCenter);
            this.Controls.Add(this.chkBoxUnderline);
            this.Controls.Add(this.chkBoxLeft);
            this.Controls.Add(this.chkBoxItalic);
            this.Controls.Add(this.chkBoxBold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pointSizeUpDown);
            this.Controls.Add(this.dropDownFontList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PTFontDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Font/Style";
            this.Load += new System.EventHandler(this.PTFontDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pointSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox dropDownFontList;
        private System.Windows.Forms.NumericUpDown pointSizeUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBoxBold;
        private System.Windows.Forms.CheckBox chkBoxItalic;
        private System.Windows.Forms.CheckBox chkBoxUnderline;
        private System.Windows.Forms.CheckBox chkBoxStrike;
        private System.Windows.Forms.CheckBox chkBoxLeft;
        private System.Windows.Forms.CheckBox chkBoxCenter;
        private System.Windows.Forms.CheckBox chkBoxRight;
        private System.Windows.Forms.CheckBox chkBoxJustify;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ComboBox comboBoxColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSample;
    }
}