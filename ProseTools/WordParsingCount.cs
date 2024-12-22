using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProseTools
{
    public partial class WordParsingCount : Form
    {
        public WordParsingCount()
        {
            InitializeComponent();
        }

        internal void InitializeCount(int nCount)
        {
            SetWordCountParsingText(0, nCount);
        }

        internal void SetWordCountParsingText(int nIndex, int nCount)
        {
            progressBar1.Maximum = nCount;
            labelWordParsingCount.Text = $"Parsing word {nIndex} of {nCount}...";
            progressBar1.Value = nIndex;
            labelWordParsingCount.Left = (this.ClientSize.Width - labelWordParsingCount.Width) / 2;
            Refresh();
        }

        internal void SetWordCountParsingText(string sNewText)
        {
            labelWordParsingCount.Text = sNewText;
            labelWordParsingCount.Left = (this.ClientSize.Width - labelWordParsingCount.Width) / 2;
        }
    }
}
