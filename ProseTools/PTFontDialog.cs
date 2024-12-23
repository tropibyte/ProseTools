using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            // Set default values for controls
            pointSizeUpDown.Value = 12;
            chkBoxBold.Checked = false;
            chkBoxItalic.Checked = false;
            chkBoxUnderline.Checked = false;
            chkBoxStrike.Checked = false;
            chkBoxLeft.Checked = true;
            SelectedAlignment = WdParagraphAlignment.wdAlignParagraphLeft;

            // Populate the color combo box with color names and swatches
            comboBoxColor.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxColor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxColor.DrawItem += ComboBoxColor_DrawItem;
            comboBoxColor.Items.AddRange(GetColorList().Cast<object>().ToArray());
            comboBoxColor.SelectedIndex = 0;

            // Set fonts for all controls
            var defaultFont = new System.Drawing.Font("Segoe UI", 9, FontStyle.Regular); // Default font for controls
            var defaultColor = Color.Black; // Default color for controls
            SetControlFontAndColor(this, defaultFont, defaultColor);

            // Update the sample text label
            UpdateSampleLabel();
        }

        private void SetControlFontAndColor(Control control, System.Drawing.Font font, Color color)
        {
            control.Font = font;
            control.ForeColor = color;

            foreach (Control childControl in control.Controls)
            {
                SetControlFontAndColor(childControl, font, color);
            }

            chkBoxBold.Font = new System.Drawing.Font(font, FontStyle.Bold);
            chkBoxItalic.Font = new System.Drawing.Font(font, FontStyle.Italic);
            chkBoxUnderline.Font = new System.Drawing.Font(font, FontStyle.Underline);
            chkBoxStrike.Font = new System.Drawing.Font(font, FontStyle.Strikeout);
        }

        private void UpdateSampleLabel()
        {
            // Update the sample text label to reflect the current settings
            var fontStyle = FontStyle.Regular;
            if (chkBoxBold.Checked) fontStyle |= FontStyle.Bold;
            if (chkBoxItalic.Checked) fontStyle |= FontStyle.Italic;
            if (chkBoxUnderline.Checked) fontStyle |= FontStyle.Underline;
            if (chkBoxStrike.Checked) fontStyle |= FontStyle.Strikeout;

            labelSample.Font = new System.Drawing.Font(dropDownFontList.SelectedItem.ToString(), 
                (pointSizeUpDown.Value > 0 ? (float)pointSizeUpDown.Value : 12.0f),
                fontStyle);
            labelSample.ForeColor = comboBoxColor.SelectedItem != null
                ? (Color)comboBoxColor.SelectedItem
                : Color.Black;
            labelSample.Text = "Sample Text";
            labelSample.TextAlign = chkBoxLeft.Checked ? ContentAlignment.MiddleLeft :
                chkBoxCenter.Checked ? ContentAlignment.MiddleCenter :
                chkBoxRight.Checked ? ContentAlignment.MiddleRight :
                ContentAlignment.MiddleLeft;
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

            if (Color.FromArgb(255, color.R, color.G, color.B).GetBrightness() < 0.4)
            {
                e.Graphics.DrawString(color.Name, e.Font, Brushes.White, new PointF(e.Bounds.X + 40, e.Bounds.Y));
            }
            else
            {
                e.Graphics.DrawString(color.Name, e.Font, Brushes.Black, new PointF(e.Bounds.X + 40, e.Bounds.Y));
            }
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

        }

        public new DialogResult ShowDialog()
        {
            dropDownFontList.SelectedIndex = SelectedFontName != null
                ? dropDownFontList.Items.IndexOf(SelectedFontName)
                : dropDownFontList.Items.IndexOf("Times New Roman") != -1
                    ? dropDownFontList.Items.IndexOf("Times New Roman")
                    : 0;

            pointSizeUpDown.Value = (decimal)SelectedFontSize;
            chkBoxBold.Checked = SelectedBold;
            chkBoxItalic.Checked = SelectedItalic;
            chkBoxUnderline.Checked = SelectedUnderline;
            chkBoxStrike.Checked = SelectedStrike;
            comboBoxColor.SelectedIndex = comboBoxColor.Items.IndexOf(SelectedFontColor);

            // Temporarily unsubscribe from CheckedChanged events
            chkBoxLeft.CheckedChanged -= chkBoxLeft_CheckedChanged;
            chkBoxCenter.CheckedChanged -= chkBoxCenter_CheckedChanged;
            chkBoxRight.CheckedChanged -= chkBoxRight_CheckedChanged;
            chkBoxJustify.CheckedChanged -= chkBoxJustify_CheckedChanged;

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

            // Re-subscribe to CheckedChanged events
            chkBoxLeft.CheckedChanged += chkBoxLeft_CheckedChanged;
            chkBoxCenter.CheckedChanged += chkBoxCenter_CheckedChanged;
            chkBoxRight.CheckedChanged += chkBoxRight_CheckedChanged;
            chkBoxJustify.CheckedChanged += chkBoxJustify_CheckedChanged;

            UpdateSampleLabel();
            return base.ShowDialog();
        }

        private void chkBoxLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxLeft.Checked)
            {
                chkBoxCenter.Checked = false;
                chkBoxRight.Checked = false;
                chkBoxJustify.Checked = false;
                UpdateSampleLabel();
            }
        }

        private void chkBoxCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxCenter.Checked)
            {
                chkBoxLeft.Checked = false;
                chkBoxRight.Checked = false;
                chkBoxJustify.Checked = false;
                UpdateSampleLabel();
            }
        }

        private void chkBoxRight_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxRight.Checked)
            {
                chkBoxLeft.Checked = false;
                chkBoxCenter.Checked = false;
                chkBoxJustify.Checked = false;
                UpdateSampleLabel();
            }
        }

        private void chkBoxJustify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxJustify.Checked)
            {
                chkBoxLeft.Checked = false;
                chkBoxCenter.Checked = false;
                chkBoxRight.Checked = false;
                UpdateSampleLabel();
            }
        }

        private void chkBoxBold_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void chkBoxItalic_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void chkBoxUnderline_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void chkBoxStrike_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void pointSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }

        private void dropDownFontList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSampleLabel();
        }
    }
}