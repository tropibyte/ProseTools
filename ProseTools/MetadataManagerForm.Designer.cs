namespace ProseTools
{
    partial class MetadataManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TreeView treeViewMetadata;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExtract;

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
            this.treeViewMetadata = new System.Windows.Forms.TreeView();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewMetadata
            // 
            this.treeViewMetadata.Location = new System.Drawing.Point(11, 10);
            this.treeViewMetadata.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeViewMetadata.Name = "treeViewMetadata";
            this.treeViewMetadata.Size = new System.Drawing.Size(445, 342);
            this.treeViewMetadata.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(469, 10);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(162, 28);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export Metadata...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(469, 42);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(162, 28);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import Metadata...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(469, 75);
            this.btnExtract.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(162, 52);
            this.btnExtract.TabIndex = 3;
            this.btnExtract.Text = "Extract from Document...";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // MetadataManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 360);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.treeViewMetadata);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MetadataManagerForm";
            this.Text = "Metadata Manager";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
