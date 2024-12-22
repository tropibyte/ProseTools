using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace ProseTools
{
    public partial class ThisAddIn
    {
        private Microsoft.Office.Tools.CustomTaskPane MyProseToolsTaskPane;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        public void ShowProseToolsTaskPane()
        {
            if (MyProseToolsTaskPane == null)
            {
                ProseToolsTaskPane proseToolsTaskPane = new ProseToolsTaskPane();
                MyProseToolsTaskPane = this.CustomTaskPanes.Add(proseToolsTaskPane, "ProseTools");
                MyProseToolsTaskPane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                // Add visibility toggle handler
                MyProseToolsTaskPane.VisibleChanged += MyProseToolsTaskPane_VisibleChanged;
            }

            MyProseToolsTaskPane.Visible = true;
        }

        private void MyProseToolsTaskPane_VisibleChanged(object sender, EventArgs e)
        {
            Globals.Ribbons.ProseToolsRibbon.SetGroupControlsEnabled(MyProseToolsTaskPane.Visible);
        }

        public bool IsProseToolsTaskPaneVisible()
        {
            return MyProseToolsTaskPane != null && MyProseToolsTaskPane.Visible;
        }


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
