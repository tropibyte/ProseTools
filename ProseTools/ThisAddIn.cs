using System;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Word;
using Python.Runtime;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;


namespace ProseTools
{

    public partial class ThisAddIn
    {
        private Microsoft.Office.Tools.CustomTaskPane MyProseToolsTaskPane;
        internal ProseMetaData _ProseMetaData { get; set; }
        // New property to hold our settings.
        public ProseToolsSettings Settings { get; set; }

        // Path to the settings file.
        private const string SettingsFileName = "ProseToolsSettings.xml";

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Initialize _ProseMetaData as null; DocumentChange will load metadata (or set it to NullMetaData)
            _ProseMetaData = null;

            // Subscribe to relevant Word application events
            this.Application.DocumentBeforeSave += Application_DocumentBeforeSave;
            this.Application.DocumentChange += Application_DocumentChange;

            // Initialize the Python runtime
            InitializePython();

            // Load Settings
            LoadSettings();

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

            if(PythonEngine.IsInitialized)
                PythonEngine.Shutdown();
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

            Globals.Ribbons.ProseToolsRibbon.UpdateRibbonVisibility();
        }

        /// <summary>
        /// Initializes the bundled Python runtime for Python.NET.
        /// Assumes that a bundled Python distribution is located in a folder named "python"
        /// within the same directory as the executable.
        /// </summary>
        public static void InitializePython()
        {
            // 1. Set PYTHONHOME (use bundled runtime if not found)
            string pythonHome = Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Process)
                                 ?? Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(pythonHome) || !Directory.Exists(pythonHome))
            {
                // Assume your bundled Python runtime is in a "python" folder next to your executable.
                pythonHome = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "python");
                if (!Directory.Exists(pythonHome))
                {
                    MessageBox.Show("Python runtime not found. Ensure that PYTHONHOME is set or the bundled runtime exists.",
                                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome, EnvironmentVariableTarget.Process);
            }

            // 2. Set PYTHONPATH to the Lib folder of your Python runtime.
            string pythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(pythonPath))
            {
                pythonPath = Path.Combine(pythonHome, "Lib");
                Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath, EnvironmentVariableTarget.Process);
            }

            // 3. Deduce and set the Python DLL via PYTHONNET_PYDLL.
            string pythonDll = Environment.GetEnvironmentVariable("PYTHONNET_PYDLL", EnvironmentVariableTarget.Process)
                               ?? DeducePythonDll(pythonHome);
            if (string.IsNullOrEmpty(pythonDll) || !File.Exists(pythonDll))
            {
                MessageBox.Show("Could not find the Python DLL. Ensure your Python runtime is complete and properly located.",
                                "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll, EnvironmentVariableTarget.Process);

            // 4. Optionally update the PATH so that dependencies can be found.
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", pythonHome + ";" + currentPath, EnvironmentVariableTarget.Process);

            // 5. Initialize Python.NET.
            try
            {
                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing Python: " + ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Deduce the full path to the Python DLL from the given pythonHome directory.
        /// </summary>
        /// <param name="pythonHome">The base directory of the Python runtime.</param>
        /// <returns>The full path to the Python DLL, or null if not found.</returns>
        public static string DeducePythonDll(string pythonHome)
        {
            // Get the folder name (e.g., "Python38") and try to extract the version.
            string folderName = new DirectoryInfo(pythonHome).Name;
            string versionPart = string.Empty;
            if (folderName.StartsWith("Python", StringComparison.OrdinalIgnoreCase))
            {
                versionPart = folderName.Substring("Python".Length);
            }

            // If a version is available, try the expected DLL name.
            if (!string.IsNullOrEmpty(versionPart))
            {
                string expectedDll = $"python{versionPart}.dll";
                string dllPath = Path.Combine(pythonHome, expectedDll);
                if (File.Exists(dllPath))
                {
                    return dllPath;
                }
            }

            // Fallback: search for any file matching "python*.dll" in the pythonHome folder.
            string[] dllFiles = Directory.GetFiles(pythonHome, "python*.dll");
            if (dllFiles != null && dllFiles.Length > 0)
            {
                return dllFiles[0]; // Return the first match.
            }

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

        /// <summary>
        /// Loads the settings from a file.
        /// </summary>
        private void LoadSettings()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
            if (File.Exists(path))
            {
                try
                {
                    XElement xml = XElement.Load(path);
                    Settings = ProseToolsSettings.FromXML(xml);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading settings: " + ex.Message, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Settings = new ProseToolsSettings();
                }
            }
            else
            {
                Settings = new ProseToolsSettings();
            }
        }

        /// <summary>
        /// Saves the settings to a file.
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);
                XElement xml = Settings.ToXML();
                xml.Save(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message, "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
