using Microsoft.Office.Tools.Ribbon;
using System;

namespace ProseTools
{
    partial class ProseToolsRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ProseToolsRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.openProseToolsButton = this.Factory.CreateRibbonGroup();
            this.character = this.Factory.CreateRibbonGroup();
            this.genAI = this.Factory.CreateRibbonGroup();
            this.genAIDropDown = this.Factory.CreateRibbonDropDown();
            this.proseGroup = this.Factory.CreateRibbonGroup();
            this.dropDownProseType = this.Factory.CreateRibbonDropDown();
            this.settings = this.Factory.CreateRibbonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            this.characterManagementButton = this.Factory.CreateRibbonButton();
            this.scanForCharacters = this.Factory.CreateRibbonButton();
            this.queryGenAIButton = this.Factory.CreateRibbonButton();
            this.startProse = this.Factory.CreateRibbonButton();
            this.NewChapter = this.Factory.CreateRibbonButton();
            this.Outline = this.Factory.CreateRibbonButton();
            this.settingsButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.openProseToolsButton.SuspendLayout();
            this.character.SuspendLayout();
            this.genAI.SuspendLayout();
            this.proseGroup.SuspendLayout();
            this.settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.openProseToolsButton);
            this.tab1.Groups.Add(this.character);
            this.tab1.Groups.Add(this.genAI);
            this.tab1.Groups.Add(this.proseGroup);
            this.tab1.Groups.Add(this.settings);
            this.tab1.Label = "ProseTools";
            this.tab1.Name = "tab1";
            // 
            // openProseToolsButton
            // 
            this.openProseToolsButton.Items.Add(this.button1);
            this.openProseToolsButton.Label = "ProseTools";
            this.openProseToolsButton.Name = "openProseToolsButton";
            // 
            // character
            // 
            this.character.Items.Add(this.characterManagementButton);
            this.character.Items.Add(this.scanForCharacters);
            this.character.Label = "Character";
            this.character.Name = "character";
            // 
            // genAI
            // 
            this.genAI.Items.Add(this.genAIDropDown);
            this.genAI.Items.Add(this.queryGenAIButton);
            this.genAI.Label = "Gen AI";
            this.genAI.Name = "genAI";
            // 
            // genAIDropDown
            // 
            this.genAIDropDown.Label = "AI";
            this.genAIDropDown.Name = "genAIDropDown";
            // 
            // proseGroup
            // 
            this.proseGroup.Items.Add(this.dropDownProseType);
            this.proseGroup.Items.Add(this.startProse);
            this.proseGroup.Items.Add(this.NewChapter);
            this.proseGroup.Items.Add(this.Outline);
            this.proseGroup.Label = "Prose";
            this.proseGroup.Name = "proseGroup";
            // 
            // dropDownProseType
            // 
            this.dropDownProseType.Label = "Type";
            this.dropDownProseType.Name = "dropDownProseType";
            // 
            // settings
            // 
            this.settings.Items.Add(this.settingsButton);
            this.settings.Label = "Settings";
            this.settings.Name = "settings";
            // 
            // button1
            // 
            this.button1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button1.Image = global::ProseTools.Properties.Resources.green_icon_512x512;
            this.button1.Label = "Open ProseTools";
            this.button1.Name = "button1";
            this.button1.ShowImage = true;
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // characterManagementButton
            // 
            this.characterManagementButton.Label = "Character Management...";
            this.characterManagementButton.Name = "characterManagementButton";
            this.characterManagementButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.characterManagementButton_Click);
            // 
            // scanForCharacters
            // 
            this.scanForCharacters.Label = "Scan For Characters...";
            this.scanForCharacters.Name = "scanForCharacters";
            this.scanForCharacters.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.scanForCharactersButton_Click);
            // 
            // queryGenAIButton
            // 
            this.queryGenAIButton.Label = "Query...";
            this.queryGenAIButton.Name = "queryGenAIButton";
            // 
            // startProse
            // 
            this.startProse.Label = "Start Prose";
            this.startProse.Name = "startProse";
            this.startProse.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.startProse_Click);
            // 
            // NewChapter
            // 
            this.NewChapter.Label = "New Chapter";
            this.NewChapter.Name = "NewChapter";
            this.NewChapter.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewChapter_Click);
            // 
            // Outline
            // 
            this.Outline.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.Outline.Image = global::ProseTools.Properties.Resources.outline_icon;
            this.Outline.Label = "Outline";
            this.Outline.Name = "Outline";
            this.Outline.ShowImage = true;
            this.Outline.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Outline_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Image = global::ProseTools.Properties.Resources.gear_settings;
            this.settingsButton.Label = "Settings...";
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.ShowImage = true;
            this.settingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.settingsButton_Click);
            // 
            // ProseToolsRibbon
            // 
            this.Name = "ProseToolsRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ProseToolsRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.openProseToolsButton.ResumeLayout(false);
            this.openProseToolsButton.PerformLayout();
            this.character.ResumeLayout(false);
            this.character.PerformLayout();
            this.genAI.ResumeLayout(false);
            this.genAI.PerformLayout();
            this.proseGroup.ResumeLayout(false);
            this.proseGroup.PerformLayout();
            this.settings.ResumeLayout(false);
            this.settings.PerformLayout();
            this.ResumeLayout(false);

        }

        private void buttonScanCharacters_Click(object sender, RibbonControlEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("ProseTools Task Pane opened!", "ProseTools");
            Globals.ThisAddIn.ShowProseToolsTaskPane();
        }



        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal RibbonGroup openProseToolsButton;
        internal RibbonButton button1;
        internal RibbonGroup character;
        internal RibbonButton scanForCharacters;
        internal RibbonGroup genAI;
        internal RibbonDropDown genAIDropDown;
        internal RibbonButton queryGenAIButton;
        internal RibbonButton characterManagementButton;
        internal RibbonGroup settings;
        internal RibbonButton settingsButton;
        internal RibbonGroup proseGroup;
        internal RibbonDropDown dropDownProseType;
        internal RibbonButton startProse;
        internal RibbonButton NewChapter;
        internal RibbonButton Outline;
    }

    partial class ThisRibbonCollection
    {
        internal ProseToolsRibbon ProseToolsRibbon
        {
            get { return this.GetRibbon<ProseToolsRibbon>(); }
        }
    }
}
