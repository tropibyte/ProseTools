using System;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Word;
using Python.Runtime;
using System.IO;
using System.Windows.Forms;

namespace ProseTools
{

    public partial class ThisAddIn
    {
        private Microsoft.Office.Tools.CustomTaskPane MyProseToolsTaskPane;
        internal ProseMetaData _ProseMetaData { get; set; }

        internal static class RuntimeInitializer
        {
            static RuntimeInitializer()
            {
 //               Runtime.PythonDLL = EnsurePythonNetDll();
            }

            public static void Initialize()
            {
                // This method is intentionally left empty.
                // It exists to trigger the static constructor.
            }
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Initialize _ProseMetaData as null; DocumentChange will load metadata (or set it to NullMetaData)
            _ProseMetaData = null;

            // Subscribe to relevant Word application events
            this.Application.DocumentBeforeSave += Application_DocumentBeforeSave;
            this.Application.DocumentChange += Application_DocumentChange;

            // Initialize the Python runtime
            InitializePython();
            // Set the PythonDLL in a static constructor
            RuntimeInitializer.Initialize();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Unsubscribe from events to prevent memory leaks or unexpected behavior
            this.Application.DocumentBeforeSave -= Application_DocumentBeforeSave;
            this.Application.DocumentChange -= Application_DocumentChange;

            if (MyProseToolsTaskPane != null)
            {
                MyProseToolsTaskPane.VisibleChanged -= MyProseToolsTaskPane_VisibleChanged;
                MyProseToolsTaskPane = null;
            }
        }

        /// <summary>
        /// Before saving the document, write the metadata (if applicable)
        /// Only writes if the document is not protected.
        /// </summary>
        private void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            try
            {
                // Check if the document is protected or read-only
                if (Doc.ProtectionType != Word.WdProtectionType.wdNoProtection)
                {
                    System.Diagnostics.Debug.WriteLine("Document is protected; skipping metadata save.");
                    return;
                }
                _ProseMetaData?.WriteToActiveDocument();
            }
            catch (Exception ex)
            {
                // In production, replace Debug.WriteLine with a logging framework
                System.Diagnostics.Debug.WriteLine($"Error saving ProseMetaData: {ex.Message}");
            }
        }

        /// <summary>
        /// When the active document changes, attempt to load metadata.
        /// If no metadata exists, LoadFromDocument() will return a NullMetaData instance.
        /// </summary>
        private void Application_DocumentChange()
        {
            try
            {
                var doc = this.Application.ActiveDocument;
                if (doc != null)
                {
                    _ProseMetaData = ProseMetaData.LoadFromDocument();
                }
                else
                {
                    _ProseMetaData = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DocumentChange: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the bundled Python runtime for Python.NET.
        /// Assumes that a bundled Python distribution is located in a folder named "python"
        /// within the same directory as the executable.
        /// </summary>
        public static void InitializePython()
        {
            string pythonHome = Environment.GetEnvironmentVariable("PYTHONHOME");
            if (pythonHome == null)
            {
                pythonHome = Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Machine);
            }

            if (pythonHome == null)
            {
                // Get the directory of the current executable.
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                // Set the path to your bundled Python runtime.
                // For example, if you have unzipped the Python embeddable package into a folder named "python".
                pythonHome = Path.Combine(baseDir, "python");
                // Set environment variables so Python.NET knows where to find the Python installation.
                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
                PythonEngine.PythonHome = pythonHome;
            }

            string pythonPath = Environment.GetEnvironmentVariable("PYTHONPATH");
            if (pythonPath == null)
            {
                pythonPath = Path.Combine(pythonHome, "Lib");
                // Set the path to the Python standard library.
                Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath);
            }

            string pythonDllPath = EnsurePythonNetDll();
            string python313 = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL", EnvironmentVariableTarget.Machine);

            if (pythonDllPath == null)
            {
                MessageBox.Show("Could not find Python DLL. Please ensure that the Python runtime is installed and PYTHONHOME is set correctly.", "ProseTools Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path = pythonHome + ";" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            
            try
            {
                // Initialize the Python engine if it has not already been initialized.
                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();  //Continues to generate an exception
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing Python: {ex.Message}");
                MessageBox.Show("Error initializing Python: " + ex.Message, "ProseTools Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        public static string DeducePythonDll()
        {
            // First, try to get PYTHONHOME from the machine environment variables.
            string pythonHome = Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(pythonHome))
            {
                // Fall back to the process environment if necessary.
                pythonHome = Environment.GetEnvironmentVariable("PYTHONHOME");
            }

            if (string.IsNullOrEmpty(pythonHome) || !Directory.Exists(pythonHome))
            {
                return null;
            }

            // Get the final folder name of the PYTHONHOME path.
            string folderName = Path.GetFileName(pythonHome.TrimEnd(Path.DirectorySeparatorChar));
            if (string.IsNullOrEmpty(folderName))
            {
                return null;
            }

            string versionPart = string.Empty;
            // If the folder name starts with "Python" (case-insensitive), extract the version part.
            if (folderName.StartsWith("Python", StringComparison.OrdinalIgnoreCase))
            {
                versionPart = folderName.Substring("Python".Length);
            }

            // If we obtained a version part, construct the expected DLL name.
            if (!string.IsNullOrEmpty(versionPart))
            {
                string expectedDllName = "python" + versionPart + ".dll";
                string potentialDllPath = Path.Combine(pythonHome, expectedDllName);
                if (File.Exists(potentialDllPath))
                {
                    return potentialDllPath;
                }
            }

            // Fallback: search for any DLL in PYTHONHOME that matches "python*.dll"
            string[] dllFiles = Directory.GetFiles(pythonHome, "python*.dll");
            if (dllFiles != null && dllFiles.Length > 0)
            {
                // For example, return the first match.
                return dllFiles[0];
            }

            // If nothing is found, return null.
            return null;
        }

        public static string EnsurePythonNetDll()
        {
            // Check the process-level environment variable first.
            string dllPath = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL", EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(dllPath))
            {
                // If not found, check the machine-level variable.
                dllPath = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL");
                if (string.IsNullOrWhiteSpace(dllPath))
                {
                    // If not found, check the machine-level variable.
                    dllPath = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL", EnvironmentVariableTarget.Machine);
                }

            }

            // If already set, return it.
            if (!string.IsNullOrWhiteSpace(dllPath))
            {
                return dllPath;
            }

            // Otherwise, try to deduce the DLL path.
            dllPath = DeducePythonDll();
            if (!string.IsNullOrWhiteSpace(dllPath))
            {
                // Set it in the process environment so that Python.NET can use it.
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", dllPath, EnvironmentVariableTarget.Machine);
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", dllPath, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", dllPath, EnvironmentVariableTarget.User);
                return dllPath;
            }

            // Could not deduce the DLL; return null or throw an exception as desired.
            return null;
        }
        /// <summary>
        /// Displays the ProseTools task pane.
        /// If the task pane has not been created, it is created and a visibility handler is added.
        /// </summary>
        public void ShowProseToolsTaskPane()
        {
            if (MyProseToolsTaskPane == null)
            {
                ProseToolsTaskPane proseToolsTaskPane = new ProseToolsTaskPane();
                MyProseToolsTaskPane = this.CustomTaskPanes.Add(proseToolsTaskPane, "ProseTools");
                MyProseToolsTaskPane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                MyProseToolsTaskPane.VisibleChanged += MyProseToolsTaskPane_VisibleChanged;
            }

            MyProseToolsTaskPane.Visible = true;
        }

        /// <summary>
        /// Handles changes to the task pane's visibility by updating the ribbon controls.
        /// </summary>
        private void MyProseToolsTaskPane_VisibleChanged(object sender, EventArgs e)
        {
            Globals.Ribbons.ProseToolsRibbon.SetGroupControlsEnabled(MyProseToolsTaskPane.Visible);
        }

        /// <summary>
        /// Returns whether the ProseTools task pane is visible.
        /// </summary>
        public bool IsProseToolsTaskPaneVisible()
        {
            return MyProseToolsTaskPane != null && MyProseToolsTaskPane.Visible;
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support – do not modify
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
