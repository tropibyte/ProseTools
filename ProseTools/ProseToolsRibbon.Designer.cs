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
            this.button1 = this.Factory.CreateRibbonButton();
            this.settingsButton = this.Factory.CreateRibbonButton();
            this.genAI = this.Factory.CreateRibbonGroup();
            this.genAIDropDown = this.Factory.CreateRibbonDropDown();
            this.queryGenAIButton = this.Factory.CreateRibbonButton();
            this.aiSettings = this.Factory.CreateRibbonButton();
            this.proseGroup = this.Factory.CreateRibbonGroup();
            this.dropDownProseType = this.Factory.CreateRibbonDropDown();
            this.startProse = this.Factory.CreateRibbonButton();
            this.novel = this.Factory.CreateRibbonGroup();
            this.characterManagementButton = this.Factory.CreateRibbonButton();
            this.scanForCharacters = this.Factory.CreateRibbonButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.NewChapter = this.Factory.CreateRibbonButton();
            this.Outline = this.Factory.CreateRibbonButton();
            this.researchPaper = this.Factory.CreateRibbonGroup();
            this.techPaper = this.Factory.CreateRibbonGroup();
            this.screenPlay = this.Factory.CreateRibbonGroup();
            this.settings = this.Factory.CreateRibbonGroup();
            this.tab1.SuspendLayout();
            this.openProseToolsButton.SuspendLayout();
            this.genAI.SuspendLayout();
            this.proseGroup.SuspendLayout();
            this.novel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.openProseToolsButton);
            this.tab1.Groups.Add(this.genAI);
            this.tab1.Groups.Add(this.proseGroup);
            this.tab1.Groups.Add(this.novel);
            this.tab1.Groups.Add(this.researchPaper);
            this.tab1.Groups.Add(this.techPaper);
            this.tab1.Groups.Add(this.screenPlay);
            this.tab1.Groups.Add(this.settings);
            this.tab1.Label = "ProseTools";
            this.tab1.Name = "tab1";
            // 
            // openProseToolsButton
            // 
            this.openProseToolsButton.Items.Add(this.button1);
            this.openProseToolsButton.Items.Add(this.settingsButton);
            this.openProseToolsButton.Label = "ProseTools";
            this.openProseToolsButton.Name = "openProseToolsButton";
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
            // settingsButton
            // 
            this.settingsButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.settingsButton.Image = global::ProseTools.Properties.Resources.gear_settings;
            this.settingsButton.Label = "Settings...";
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.ShowImage = true;
            this.settingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.settingsButton_Click);
            // 
            // genAI
            // 
            this.genAI.Items.Add(this.genAIDropDown);
            this.genAI.Items.Add(this.queryGenAIButton);
            this.genAI.Items.Add(this.aiSettings);
            this.genAI.Label = "Gen AI";
            this.genAI.Name = "genAI";
            // 
            // genAIDropDown
            // 
            this.genAIDropDown.Label = "AI";
            this.genAIDropDown.Name = "genAIDropDown";
            // 
            // queryGenAIButton
            // 
            this.queryGenAIButton.Label = "Query...";
            this.queryGenAIButton.Name = "queryGenAIButton";
            // 
            // aiSettings
            // 
            this.aiSettings.Image = global::ProseTools.Properties.Resources.gear_settings;
            this.aiSettings.Label = "AI Settings...";
            this.aiSettings.Name = "aiSettings";
            this.aiSettings.ShowImage = true;
            this.aiSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.aiSettingsButton_Click);
            // 
            // proseGroup
            // 
            this.proseGroup.Items.Add(this.dropDownProseType);
            this.proseGroup.Items.Add(this.startProse);
            this.proseGroup.Label = "Prose";
            this.proseGroup.Name = "proseGroup";
            // 
            // dropDownProseType
            // 
            this.dropDownProseType.Label = "Type";
            this.dropDownProseType.Name = "dropDownProseType";
            // 
            // startProse
            // 
            this.startProse.Label = "Start Prose";
            this.startProse.Name = "startProse";
            this.startProse.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.startProse_Click);
            // 
            // novel
            // 
            this.novel.Items.Add(this.characterManagementButton);
            this.novel.Items.Add(this.scanForCharacters);
            this.novel.Items.Add(this.separator1);
            this.novel.Items.Add(this.NewChapter);
            this.novel.Items.Add(this.Outline);
            this.novel.Label = "Novel";
            this.novel.Name = "novel";
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
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // NewChapter
            // 
            this.NewChapter.Label = "New Chapter";
            this.NewChapter.Name = "NewChapter";
            this.NewChapter.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewChapter_Click);
            // 
            // Outline
            // 
            this.Outline.Image = global::ProseTools.Properties.Resources.outline_icon;
            this.Outline.Label = "Outline";
            this.Outline.Name = "Outline";
            this.Outline.ShowImage = true;
            this.Outline.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Outline_Click);
            // 
            // researchPaper
            // 
            this.researchPaper.Label = "Research Paper";
            this.researchPaper.Name = "researchPaper";
            // 
            // techPaper
            // 
            this.techPaper.Label = "Technical Paper";
            this.techPaper.Name = "techPaper";
            // 
            // screenPlay
            // 
            this.screenPlay.Label = "Screenplay";
            this.screenPlay.Name = "screenPlay";
            // 
            // settings
            // 
            this.settings.Label = "Settings";
            this.settings.Name = "settings";
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
            this.genAI.ResumeLayout(false);
            this.genAI.PerformLayout();
            this.proseGroup.ResumeLayout(false);
            this.proseGroup.PerformLayout();
            this.novel.ResumeLayout(false);
            this.novel.PerformLayout();
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
        internal RibbonGroup novel;
        internal RibbonButton aiSettings;
        internal RibbonGroup researchPaper;
        internal RibbonGroup techPaper;
        internal RibbonGroup screenPlay;
        internal RibbonSeparator separator1;
    }

    partial class ThisRibbonCollection
    {
        internal ProseToolsRibbon ProseToolsRibbon
        {
            get { return this.GetRibbon<ProseToolsRibbon>(); }
        }
    }
}
