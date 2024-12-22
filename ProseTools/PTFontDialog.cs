using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace ProseTools
{
    public partial class PTFontDialog : Form
    {
        public string SelectedFontName { get; internal set; }
        public float SelectedFontSize { get; internal set; }
        public bool SelectedBold { get; internal set; }
        public bool SelectedItalic { get; internal set; }
        public bool SelectedUnderline { get; internal set; }
        public bool SelectedStrike { get; internal set; }
        public WdParagraphAlignment SelectedAlignment { get; internal set; }
        public Color SelectedFontColor { get; internal set; }
        public bool ShowColor { get; internal set; }
        public Color Color
        {
            get => SelectedFontColor;
            set => SelectedFontColor = value;
        }

        public PTFontDialog()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Populate the font dropdown with Word's fonts
            dropDownFontList.Items.AddRange(GetWordFonts().OrderBy(f => f).ToArray());
            dropDownFontList.SelectedIndex = 0;

            // Populate the color combo box with color names and swatches
            comboBoxColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxColor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxColor.DrawItem += ComboBoxColor_DrawItem;
            comboBoxColor.Items.AddRange(GetColorList().Cast<object>().ToArray());
            comboBoxColor.SelectedIndex = 0;

            // Set default values for controls
            pointSizeUpDown.Value = 12;
            chkBoxBold.Checked = false;
            chkBoxItalic.Checked = false;
            chkBoxUnderline.Checked = false;
            chkBoxStrike.Checked = false;
            chkBoxLeft.Checked = true;
            SelectedAlignment = WdParagraphAlignment.wdAlignParagraphLeft;
            UpdateSampleLabel();
        }

        private void UpdateSampleLabel()
        {
            // Update the sample text label to reflect the current settings
            var fontStyle = FontStyle.Regular;
            if (chkBoxBold.Checked) fontStyle |= FontStyle.Bold;
            if (chkBoxItalic.Checked) fontStyle |= FontStyle.Italic;
            if (chkBoxUnderline.Checked) fontStyle |= FontStyle.Underline;

            labelSample.Font = new System.Drawing.Font(dropDownFontList.SelectedItem.ToString(), (float)pointSizeUpDown.Value, fontStyle);
            labelSample.ForeColor = (Color)comboBoxColor.SelectedItem;
            labelSample.Text = "Sample Text";
        }

        private List<string> GetWordFonts()
        {
            // Retrieve and alphabetize the list of fonts from MS Word
            var fonts = Globals.ThisAddIn.Application.FontNames.Cast<string>().OrderBy(font => font).ToList();
            return fonts;
        }

        private List<Color> GetColorList()
        {
            // Retrieve a list of all named colors
            return typeof(Color).GetProperties()
                .Where(p => p.PropertyType == typeof(Color))
                .Select(p => (Color)p.GetValue(null))
                .ToList();
        }

        private void ComboBoxColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var color = (Color)((ComboBox)sender).Items[e.Index];
            using (var brush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            e.Graphics.DrawString(color.Name, e.Font, Brushes.Black, new PointF(e.Bounds.X + 40, e.Bounds.Y));
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            // Capture user selections
            SelectedFontName = dropDownFontList.SelectedItem.ToString();
            SelectedFontSize = (float)pointSizeUpDown.Value;
            SelectedBold = chkBoxBold.Checked;
            SelectedItalic = chkBoxItalic.Checked;
            SelectedUnderline = chkBoxUnderline.Checked;
            SelectedStrike = chkBoxStrike.Checked;
            SelectedFontColor = (Color)comboBoxColor.SelectedItem;

            // Determine alignment
            if (chkBoxLeft.Checked) SelectedAlignment = WdParagraphAlignment.wdAlignParagraphLeft;
            if (chkBoxCenter.Checked) SelectedAlignment = WdParagraphAlignment.wdAlignParagraphCenter;
            if (chkBoxRight.Checked) SelectedAlignment = WdParagraphAlignment.wdAlignParagraphRight;
            if (chkBoxJustify.Checked) SelectedAlignment = WdParagraphAlignment.wdAlignParagraphJustify;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PTFontDialog_Load(object sender, EventArgs e)
        {
            dropDownFontList.SelectedIndex = SelectedFontName != null ? dropDownFontList.Items.IndexOf(SelectedFontName) : 0;
            pointSizeUpDown.Value = (decimal)SelectedFontSize;
            chkBoxBold.Checked = SelectedBold;
            chkBoxItalic.Checked = SelectedItalic;
            chkBoxUnderline.Checked = SelectedUnderline;
            chkBoxStrike.Checked = SelectedStrike;
            comboBoxColor.SelectedIndex = comboBoxColor.Items.IndexOf(SelectedFontColor);
            switch (SelectedAlignment)
            {
                case WdParagraphAlignment.wdAlignParagraphLeft:
                    chkBoxLeft.Checked = true;
                    break;
                case WdParagraphAlignment.wdAlignParagraphCenter:
                    chkBoxCenter.Checked = true;
                    break;
                case WdParagraphAlignment.wdAlignParagraphRight:
                    chkBoxRight.Checked = true;
                    break;
                case WdParagraphAlignment.wdAlignParagraphJustify:
                    chkBoxJustify.Checked = true;
                    break;
            }
        }
    }
}
