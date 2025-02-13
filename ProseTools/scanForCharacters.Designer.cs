namespace ProseTools
{
    partial class ScanForCharacters
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
            this.listViewCharacters = new System.Windows.Forms.ListView();
            this.doScanButton = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.labelWordCountParsing = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkBoxIgnoreFirstWord = new System.Windows.Forms.CheckBox();
            this.cbWhichScan = new System.Windows.Forms.ComboBox();
            this.chkBoxClearList = new System.Windows.Forms.CheckBox();
            this.btnLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewCharacters
            // 
            this.listViewCharacters.GridLines = true;
            this.listViewCharacters.HideSelection = false;
            this.listViewCharacters.Location = new System.Drawing.Point(12, 32);
            this.listViewCharacters.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listViewCharacters.Name = "listViewCharacters";
            this.listViewCharacters.Size = new System.Drawing.Size(568, 324);
            this.listViewCharacters.TabIndex = 0;
            this.listViewCharacters.UseCompatibleStateImageBehavior = false;
            this.listViewCharacters.View = System.Windows.Forms.View.Details;
            // 
            // doScanButton
            // 
            this.doScanButton.Location = new System.Drawing.Point(605, 32);
            this.doScanButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.doScanButton.Name = "doScanButton";
            this.doScanButton.Size = new System.Drawing.Size(94, 26);
            this.doScanButton.TabIndex = 1;
            this.doScanButton.Text = "Scan";
            this.doScanButton.UseVisualStyleBackColor = true;
            this.doScanButton.Click += new System.EventHandler(this.DoScanButton_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(605, 62);
            this.close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(94, 26);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // labelWordCountParsing
            // 
            this.labelWordCountParsing.AutoSize = true;
            this.labelWordCountParsing.Location = new System.Drawing.Point(12, 365);
            this.labelWordCountParsing.Name = "labelWordCountParsing";
            this.labelWordCountParsing.Size = new System.Drawing.Size(65, 16);
            this.labelWordCountParsing.TabIndex = 2;
            this.labelWordCountParsing.Text = "Parsing ...";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(605, 93);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(94, 26);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete...";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkBoxIgnoreFirstWord
            // 
            this.chkBoxIgnoreFirstWord.Location = new System.Drawing.Point(605, 191);
            this.chkBoxIgnoreFirstWord.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBoxIgnoreFirstWord.Name = "chkBoxIgnoreFirstWord";
            this.chkBoxIgnoreFirstWord.Size = new System.Drawing.Size(92, 71);
            this.chkBoxIgnoreFirstWord.TabIndex = 3;
            this.chkBoxIgnoreFirstWord.Text = "Ignore first word of a sentence.";
            this.chkBoxIgnoreFirstWord.UseVisualStyleBackColor = true;
            // 
            // cbWhichScan
            // 
            this.cbWhichScan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWhichScan.FormattingEnabled = true;
            this.cbWhichScan.Location = new System.Drawing.Point(605, 154);
            this.cbWhichScan.Name = "cbWhichScan";
            this.cbWhichScan.Size = new System.Drawing.Size(94, 24);
            this.cbWhichScan.TabIndex = 4;
            // 
            // chkBoxClearList
            // 
            this.chkBoxClearList.Location = new System.Drawing.Point(605, 257);
            this.chkBoxClearList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBoxClearList.Name = "chkBoxClearList";
            this.chkBoxClearList.Size = new System.Drawing.Size(92, 47);
            this.chkBoxClearList.TabIndex = 3;
            this.chkBoxClearList.Text = "Clear list after scan";
            this.chkBoxClearList.UseVisualStyleBackColor = true;
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(605, 123);
            this.btnLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(94, 26);
            this.btnLink.TabIndex = 1;
            this.btnLink.Text = "Link...";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // ScanForCharacters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 403);
            this.Controls.Add(this.cbWhichScan);
            this.Controls.Add(this.chkBoxClearList);
            this.Controls.Add(this.chkBoxIgnoreFirstWord);
            this.Controls.Add(this.labelWordCountParsing);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.close);
            this.Controls.Add(this.doScanButton);
            this.Controls.Add(this.listViewCharacters);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ScanForCharacters";
            this.Text = "Scan For Characters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewCharacters;
        private System.Windows.Forms.Button doScanButton;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Label labelWordCountParsing;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkBoxIgnoreFirstWord;
        private System.Windows.Forms.ComboBox cbWhichScan;
        private System.Windows.Forms.CheckBox chkBoxClearList;
        private System.Windows.Forms.Button btnLink;
    }
}