namespace CuttingPlanMaker
{
    partial class frmMain
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
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileImportSketchupCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartsList = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportStockList = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ctrSplitContainer = new System.Windows.Forms.SplitContainer();
            this.btnPartsTab = new System.Windows.Forms.Button();
            this.btnStockTab = new System.Windows.Forms.Button();
            this.btnMaterialsTab = new System.Windows.Forms.Button();
            this.btnCollapseExpandTab = new System.Windows.Forms.Button();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctrSplitContainer)).BeginInit();
            this.ctrSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniTools,
            this.mniReport,
            this.mniHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1289, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileNew,
            this.mniFileOpen,
            this.mniFileSave,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.mniFileImport,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.mniFileExit});
            this.mniFile.Name = "mniFile";
            this.mniFile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.mniFile.Size = new System.Drawing.Size(37, 20);
            this.mniFile.Text = "&File";
            // 
            // mniFileNew
            // 
            this.mniFileNew.Name = "mniFileNew";
            this.mniFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mniFileNew.Size = new System.Drawing.Size(146, 22);
            this.mniFileNew.Text = "&New";
            this.mniFileNew.Click += new System.EventHandler(this.mniFileNew_Click);
            // 
            // mniFileOpen
            // 
            this.mniFileOpen.Name = "mniFileOpen";
            this.mniFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniFileOpen.Size = new System.Drawing.Size(146, 22);
            this.mniFileOpen.Text = "&Open";
            this.mniFileOpen.Click += new System.EventHandler(this.mniFileOpen_Click);
            // 
            // mniFileSave
            // 
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mniFileSave.Size = new System.Drawing.Size(146, 22);
            this.mniFileSave.Text = "&Save";
            this.mniFileSave.Click += new System.EventHandler(this.mniFileSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // mniFileImport
            // 
            this.mniFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileImportSketchupCSV});
            this.mniFileImport.Enabled = false;
            this.mniFileImport.Name = "mniFileImport";
            this.mniFileImport.Size = new System.Drawing.Size(146, 22);
            this.mniFileImport.Text = "&Import";
            // 
            // mniFileImportSketchupCSV
            // 
            this.mniFileImportSketchupCSV.Name = "mniFileImportSketchupCSV";
            this.mniFileImportSketchupCSV.Size = new System.Drawing.Size(147, 22);
            this.mniFileImportSketchupCSV.Text = "Sketchup CSV";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // mniFileExit
            // 
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mniFileExit.Size = new System.Drawing.Size(146, 22);
            this.mniFileExit.Text = "E&xit";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mniEdit
            // 
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniEditDuplicate,
            this.mniEditDelete,
            this.mniEditCopy,
            this.mniEditPaste});
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new System.Drawing.Size(39, 20);
            this.mniEdit.Text = "&Edit";
            // 
            // mniEditDuplicate
            // 
            this.mniEditDuplicate.Name = "mniEditDuplicate";
            this.mniEditDuplicate.Size = new System.Drawing.Size(124, 22);
            this.mniEditDuplicate.Text = "&Duplicate";
            this.mniEditDuplicate.Click += new System.EventHandler(this.mniEditDuplicate_Click);
            // 
            // mniEditDelete
            // 
            this.mniEditDelete.Name = "mniEditDelete";
            this.mniEditDelete.Size = new System.Drawing.Size(124, 22);
            this.mniEditDelete.Text = "D&elete";
            // 
            // mniEditCopy
            // 
            this.mniEditCopy.Name = "mniEditCopy";
            this.mniEditCopy.Size = new System.Drawing.Size(124, 22);
            this.mniEditCopy.Text = "&Copy";
            // 
            // mniEditPaste
            // 
            this.mniEditPaste.Name = "mniEditPaste";
            this.mniEditPaste.Size = new System.Drawing.Size(124, 22);
            this.mniEditPaste.Text = "&Paste";
            // 
            // mniTools
            // 
            this.mniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniToolsOptions});
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new System.Drawing.Size(47, 20);
            this.mniTools.Text = "&Tools";
            // 
            // mniToolsOptions
            // 
            this.mniToolsOptions.Name = "mniToolsOptions";
            this.mniToolsOptions.Size = new System.Drawing.Size(116, 22);
            this.mniToolsOptions.Text = "&Options";
            this.mniToolsOptions.Click += new System.EventHandler(this.mniToolsOptions_Click);
            // 
            // mniReport
            // 
            this.mniReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniReportPartsList,
            this.mniReportStockList,
            this.mniReportLayout});
            this.mniReport.Name = "mniReport";
            this.mniReport.Size = new System.Drawing.Size(54, 20);
            this.mniReport.Text = "&Report";
            // 
            // mniReportPartsList
            // 
            this.mniReportPartsList.Name = "mniReportPartsList";
            this.mniReportPartsList.Size = new System.Drawing.Size(121, 22);
            this.mniReportPartsList.Text = "Parts list";
            // 
            // mniReportStockList
            // 
            this.mniReportStockList.Name = "mniReportStockList";
            this.mniReportStockList.Size = new System.Drawing.Size(121, 22);
            this.mniReportStockList.Text = "Stock list";
            // 
            // mniReportLayout
            // 
            this.mniReportLayout.Name = "mniReportLayout";
            this.mniReportLayout.Size = new System.Drawing.Size(121, 22);
            this.mniReportLayout.Text = "Layout";
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniHelpAbout});
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.Size = new System.Drawing.Size(44, 20);
            this.mniHelp.Text = "&Help";
            // 
            // mniHelpAbout
            // 
            this.mniHelpAbout.Name = "mniHelpAbout";
            this.mniHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.mniHelpAbout.Text = "&About";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.CheckFileExists = true;
            this.saveFileDialog.DefaultExt = "Settings.csv";
            this.saveFileDialog.Filter = "Cutting Plan Maker files|*.Settings.csv";
            this.saveFileDialog.Title = "Save File";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "Settings.csv";
            this.openFileDialog.Filter = "Cutting Plan Maker files|*.Settings.csv";
            // 
            // ctrSplitContainer
            // 
            this.ctrSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctrSplitContainer.Location = new System.Drawing.Point(21, 24);
            this.ctrSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.ctrSplitContainer.Name = "ctrSplitContainer";
            this.ctrSplitContainer.Size = new System.Drawing.Size(1267, 686);
            this.ctrSplitContainer.SplitterDistance = 405;
            this.ctrSplitContainer.TabIndex = 5;
            // 
            // btnPartsTab
            // 
            this.btnPartsTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPartsTab.AutoSize = true;
            this.btnPartsTab.BackColor = System.Drawing.Color.Transparent;
            this.btnPartsTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Parts_Materials;
            this.btnPartsTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPartsTab.FlatAppearance.BorderSize = 0;
            this.btnPartsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPartsTab.Location = new System.Drawing.Point(0, 448);
            this.btnPartsTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnPartsTab.Name = "btnPartsTab";
            this.btnPartsTab.Size = new System.Drawing.Size(22, 80);
            this.btnPartsTab.TabIndex = 3;
            this.btnPartsTab.UseVisualStyleBackColor = false;
            this.btnPartsTab.Click += new System.EventHandler(this.btnPartsTab_Click);
            // 
            // btnStockTab
            // 
            this.btnStockTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStockTab.AutoSize = true;
            this.btnStockTab.BackColor = System.Drawing.Color.Transparent;
            this.btnStockTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Stock_Materials;
            this.btnStockTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStockTab.FlatAppearance.BorderSize = 0;
            this.btnStockTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockTab.Location = new System.Drawing.Point(0, 528);
            this.btnStockTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnStockTab.Name = "btnStockTab";
            this.btnStockTab.Size = new System.Drawing.Size(22, 75);
            this.btnStockTab.TabIndex = 2;
            this.btnStockTab.UseVisualStyleBackColor = false;
            this.btnStockTab.Click += new System.EventHandler(this.btnStockTab_Click);
            // 
            // btnMaterialsTab
            // 
            this.btnMaterialsTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMaterialsTab.AutoSize = true;
            this.btnMaterialsTab.BackColor = System.Drawing.Color.Transparent;
            this.btnMaterialsTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Materials_Materials;
            this.btnMaterialsTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMaterialsTab.FlatAppearance.BorderSize = 0;
            this.btnMaterialsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterialsTab.Location = new System.Drawing.Point(0, 603);
            this.btnMaterialsTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaterialsTab.Name = "btnMaterialsTab";
            this.btnMaterialsTab.Size = new System.Drawing.Size(22, 81);
            this.btnMaterialsTab.TabIndex = 1;
            this.btnMaterialsTab.UseVisualStyleBackColor = false;
            this.btnMaterialsTab.Click += new System.EventHandler(this.btnMaterialsTab_Click);
            // 
            // btnCollapseExpandTab
            // 
            this.btnCollapseExpandTab.AutoSize = true;
            this.btnCollapseExpandTab.BackColor = System.Drawing.Color.Transparent;
            this.btnCollapseExpandTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.collapse;
            this.btnCollapseExpandTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCollapseExpandTab.FlatAppearance.BorderSize = 0;
            this.btnCollapseExpandTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollapseExpandTab.Location = new System.Drawing.Point(0, 36);
            this.btnCollapseExpandTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnCollapseExpandTab.Name = "btnCollapseExpandTab";
            this.btnCollapseExpandTab.Size = new System.Drawing.Size(22, 35);
            this.btnCollapseExpandTab.TabIndex = 4;
            this.btnCollapseExpandTab.UseVisualStyleBackColor = false;
            this.btnCollapseExpandTab.Click += new System.EventHandler(this.btnCollapseExpandTab_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 710);
            this.Controls.Add(this.btnPartsTab);
            this.Controls.Add(this.btnStockTab);
            this.Controls.Add(this.btnMaterialsTab);
            this.Controls.Add(this.ctrSplitContainer);
            this.Controls.Add(this.btnCollapseExpandTab);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.Text = "Cutting Plan Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctrSplitContainer)).EndInit();
            this.ctrSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileNew;
        private System.Windows.Forms.ToolStripMenuItem mniFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniFileImport;
        private System.Windows.Forms.ToolStripMenuItem mniFileImportSketchupCSV;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditDuplicate;
        private System.Windows.Forms.ToolStripMenuItem mniEditDelete;
        private System.Windows.Forms.ToolStripMenuItem mniEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mniEditPaste;
        private System.Windows.Forms.ToolStripMenuItem mniTools;
        private System.Windows.Forms.ToolStripMenuItem mniToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem mniReport;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAbout;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem mniReportPartsList;
        private System.Windows.Forms.ToolStripMenuItem mniReportStockList;
        private System.Windows.Forms.ToolStripMenuItem mniReportLayout;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnMaterialsTab;
        private System.Windows.Forms.Button btnStockTab;
        private System.Windows.Forms.Button btnPartsTab;
        private System.Windows.Forms.Button btnCollapseExpandTab;
        private System.Windows.Forms.SplitContainer ctrSplitContainer;
    }
}

