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
            this.SuspendLayout();
            // 
            // listViewCharacters
            // 
            this.listViewCharacters.GridLines = true;
            this.listViewCharacters.HideSelection = false;
            this.listViewCharacters.Location = new System.Drawing.Point(13, 40);
            this.listViewCharacters.Name = "listViewCharacters";
            this.listViewCharacters.Size = new System.Drawing.Size(557, 363);
            this.listViewCharacters.TabIndex = 0;
            this.listViewCharacters.UseCompatibleStateImageBehavior = false;
            this.listViewCharacters.View = System.Windows.Forms.View.Details;
            // 
            // doScanButton
            // 
            this.doScanButton.Location = new System.Drawing.Point(590, 40);
            this.doScanButton.Name = "doScanButton";
            this.doScanButton.Size = new System.Drawing.Size(106, 32);
            this.doScanButton.TabIndex = 1;
            this.doScanButton.Text = "Scan";
            this.doScanButton.UseVisualStyleBackColor = true;
            this.doScanButton.Click += new System.EventHandler(this.doScanButton_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(590, 78);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(106, 32);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // labelWordCountParsing
            // 
            this.labelWordCountParsing.AutoSize = true;
            this.labelWordCountParsing.Location = new System.Drawing.Point(13, 418);
            this.labelWordCountParsing.Name = "labelWordCountParsing";
            this.labelWordCountParsing.Size = new System.Drawing.Size(78, 20);
            this.labelWordCountParsing.TabIndex = 2;
            this.labelWordCountParsing.Text = "Parsing ...";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(590, 116);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(106, 32);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete...";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkBoxIgnoreFirstWord
            // 
            this.chkBoxIgnoreFirstWord.Location = new System.Drawing.Point(590, 155);
            this.chkBoxIgnoreFirstWord.Name = "chkBoxIgnoreFirstWord";
            this.chkBoxIgnoreFirstWord.Size = new System.Drawing.Size(104, 96);
            this.chkBoxIgnoreFirstWord.TabIndex = 3;
            this.chkBoxIgnoreFirstWord.Text = "Ignore first word of a sentence.";
            this.chkBoxIgnoreFirstWord.UseVisualStyleBackColor = true;
            // 
            // ScanForCharacters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 450);
            this.Controls.Add(this.chkBoxIgnoreFirstWord);
            this.Controls.Add(this.labelWordCountParsing);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.close);
            this.Controls.Add(this.doScanButton);
            this.Controls.Add(this.listViewCharacters);
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
    }
}