namespace CuttingPlanMaker
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveCopyAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileImportSketchupCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToolsPack = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportPartsList = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportStockList = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.mniReportLayoutLabels = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ctrSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tlpSummary = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblStockCount = new System.Windows.Forms.Label();
            this.lblUsedStockCount = new System.Windows.Forms.Label();
            this.lblPartsCount = new System.Windows.Forms.Label();
            this.lblUsedPartsCount = new System.Windows.Forms.Label();
            this.lblWastePerc = new System.Windows.Forms.Label();
            this.lblWasteArea = new System.Windows.Forms.Label();
            this.lblStockArea = new System.Windows.Forms.Label();
            this.lblUsedStockArea = new System.Windows.Forms.Label();
            this.lblPartsArea = new System.Windows.Forms.Label();
            this.lblUsedPartsArea = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tcInputs = new System.Windows.Forms.TabControl();
            this.mnuGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mniDuplicateRows = new System.Windows.Forms.ToolStripMenuItem();
            this.mniRemoveRows = new System.Windows.Forms.ToolStripMenuItem();
            this.mniCentreItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniIsolateMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.mniToggleFreeze = new System.Windows.Forms.ToolStripMenuItem();
            this.tpMaterials = new System.Windows.Forms.TabPage();
            this.MaterialsGridView = new System.Windows.Forms.DataGridView();
            this.MaterialNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialThicknessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialCostColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpStock = new System.Windows.Forms.TabPage();
            this.StockGridView = new System.Windows.Forms.DataGridView();
            this.stocknamecolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockWidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockMaterialColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tpParts = new System.Windows.Forms.TabPage();
            this.PartsGridView = new System.Windows.Forms.DataGridView();
            this.PartNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartWidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartMaterialColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tcMaterials = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pbLayout = new System.Windows.Forms.PictureBox();
            this.btnPartsTab = new System.Windows.Forms.Button();
            this.btnStockTab = new System.Windows.Forms.Button();
            this.btnMaterialsTab = new System.Windows.Forms.Button();
            this.btnCollapseExpandTab = new System.Windows.Forms.Button();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctrSplitContainer)).BeginInit();
            this.ctrSplitContainer.Panel1.SuspendLayout();
            this.ctrSplitContainer.Panel2.SuspendLayout();
            this.ctrSplitContainer.SuspendLayout();
            this.tlpSummary.SuspendLayout();
            this.tcInputs.SuspendLayout();
            this.mnuGridContextMenu.SuspendLayout();
            this.tpMaterials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaterialsGridView)).BeginInit();
            this.tpStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StockGridView)).BeginInit();
            this.tpParts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartsGridView)).BeginInit();
            this.tcMaterials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLayout)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniTools,
            this.mniAlgorithm,
            this.mniReport,
            this.mniHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1053, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileNew,
            this.mniFileOpen,
            this.mniFileRevert,
            this.mniFileSave,
            this.saveAsToolStripMenuItem,
            this.mniSaveCopyAs,
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
            this.mniFileNew.Size = new System.Drawing.Size(206, 22);
            this.mniFileNew.Text = "&New";
            this.mniFileNew.Click += new System.EventHandler(this.mniFileNew_Click);
            // 
            // mniFileOpen
            // 
            this.mniFileOpen.Name = "mniFileOpen";
            this.mniFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniFileOpen.Size = new System.Drawing.Size(206, 22);
            this.mniFileOpen.Text = "&Open";
            this.mniFileOpen.Click += new System.EventHandler(this.mniFileOpen_Click);
            // 
            // mniFileRevert
            // 
            this.mniFileRevert.Name = "mniFileRevert";
            this.mniFileRevert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mniFileRevert.Size = new System.Drawing.Size(206, 22);
            this.mniFileRevert.Text = "&Revert";
            this.mniFileRevert.Click += new System.EventHandler(this.mniFileRevert_Click);
            // 
            // mniFileSave
            // 
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mniFileSave.Size = new System.Drawing.Size(206, 22);
            this.mniFileSave.Text = "&Save";
            this.mniFileSave.Click += new System.EventHandler(this.mniFileSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // mniSaveCopyAs
            // 
            this.mniSaveCopyAs.Name = "mniSaveCopyAs";
            this.mniSaveCopyAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.mniSaveCopyAs.Size = new System.Drawing.Size(206, 22);
            this.mniSaveCopyAs.Text = "Save &Copy as";
            this.mniSaveCopyAs.Click += new System.EventHandler(this.mniSaveCopyAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // mniFileImport
            // 
            this.mniFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileImportSketchupCSV});
            this.mniFileImport.Enabled = false;
            this.mniFileImport.Name = "mniFileImport";
            this.mniFileImport.Size = new System.Drawing.Size(206, 22);
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
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // mniFileExit
            // 
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.Size = new System.Drawing.Size(206, 22);
            this.mniFileExit.Text = "E&xit";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mniTools
            // 
            this.mniTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniToolsOptions,
            this.mniToolsPack});
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new System.Drawing.Size(47, 20);
            this.mniTools.Text = "&Tools";
            // 
            // mniToolsOptions
            // 
            this.mniToolsOptions.Name = "mniToolsOptions";
            this.mniToolsOptions.Size = new System.Drawing.Size(128, 22);
            this.mniToolsOptions.Text = "&Options";
            this.mniToolsOptions.Click += new System.EventHandler(this.mniToolsOptions_Click);
            // 
            // mniToolsPack
            // 
            this.mniToolsPack.Name = "mniToolsPack";
            this.mniToolsPack.Size = new System.Drawing.Size(128, 22);
            this.mniToolsPack.Text = "Pack parts";
            this.mniToolsPack.Click += new System.EventHandler(this.mniToolsPack_Click);
            // 
            // mniAlgorithm
            // 
            this.mniAlgorithm.Name = "mniAlgorithm";
            this.mniAlgorithm.Size = new System.Drawing.Size(82, 20);
            this.mniAlgorithm.Text = "Algorithm...";
            // 
            // mniReport
            // 
            this.mniReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniReportPartsList,
            this.mniReportStockList,
            this.mniReportLayout,
            this.mniReportLayoutLabels});
            this.mniReport.Name = "mniReport";
            this.mniReport.Size = new System.Drawing.Size(54, 20);
            this.mniReport.Text = "&Report";
            // 
            // mniReportPartsList
            // 
            this.mniReportPartsList.Name = "mniReportPartsList";
            this.mniReportPartsList.Size = new System.Drawing.Size(143, 22);
            this.mniReportPartsList.Text = "Parts list";
            this.mniReportPartsList.Click += new System.EventHandler(this.mniReportPartsList_Click);
            // 
            // mniReportStockList
            // 
            this.mniReportStockList.Name = "mniReportStockList";
            this.mniReportStockList.Size = new System.Drawing.Size(143, 22);
            this.mniReportStockList.Text = "Stock list";
            this.mniReportStockList.Click += new System.EventHandler(this.mniReportStockList_Click);
            // 
            // mniReportLayout
            // 
            this.mniReportLayout.Name = "mniReportLayout";
            this.mniReportLayout.Size = new System.Drawing.Size(143, 22);
            this.mniReportLayout.Text = "Layout";
            this.mniReportLayout.Click += new System.EventHandler(this.mniReportLayout_Click);
            // 
            // mniReportLayoutLabels
            // 
            this.mniReportLayoutLabels.Name = "mniReportLayoutLabels";
            this.mniReportLayoutLabels.Size = new System.Drawing.Size(143, 22);
            this.mniReportLayoutLabels.Text = "Layout labels";
            this.mniReportLayoutLabels.Click += new System.EventHandler(this.mniReportLayoutLabels_Click);
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
            this.saveFileDialog.DefaultExt = "Settings.csv";
            this.saveFileDialog.Filter = "Cutting Plan Maker files|*.Settings.csv";
            this.saveFileDialog.SupportMultiDottedExtensions = true;
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
            this.ctrSplitContainer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ctrSplitContainer.Location = new System.Drawing.Point(21, 24);
            this.ctrSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.ctrSplitContainer.Name = "ctrSplitContainer";
            // 
            // ctrSplitContainer.Panel1
            // 
            this.ctrSplitContainer.Panel1.Controls.Add(this.tlpSummary);
            this.ctrSplitContainer.Panel1.Controls.Add(this.tcInputs);
            // 
            // ctrSplitContainer.Panel2
            // 
            this.ctrSplitContainer.Panel2.Controls.Add(this.tcMaterials);
            this.ctrSplitContainer.Panel2.Controls.Add(this.pbLayout);
            this.ctrSplitContainer.Size = new System.Drawing.Size(1031, 560);
            this.ctrSplitContainer.SplitterDistance = 274;
            this.ctrSplitContainer.TabIndex = 5;
            // 
            // tlpSummary
            // 
            this.tlpSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpSummary.ColumnCount = 5;
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSummary.Controls.Add(this.label1, 0, 0);
            this.tlpSummary.Controls.Add(this.label2, 0, 1);
            this.tlpSummary.Controls.Add(this.label3, 0, 2);
            this.tlpSummary.Controls.Add(this.label4, 0, 3);
            this.tlpSummary.Controls.Add(this.label5, 0, 4);
            this.tlpSummary.Controls.Add(this.label6, 2, 4);
            this.tlpSummary.Controls.Add(this.lblStockCount, 1, 0);
            this.tlpSummary.Controls.Add(this.lblUsedStockCount, 1, 1);
            this.tlpSummary.Controls.Add(this.lblPartsCount, 1, 2);
            this.tlpSummary.Controls.Add(this.lblUsedPartsCount, 1, 3);
            this.tlpSummary.Controls.Add(this.lblWastePerc, 1, 4);
            this.tlpSummary.Controls.Add(this.lblWasteArea, 3, 4);
            this.tlpSummary.Controls.Add(this.lblStockArea, 3, 0);
            this.tlpSummary.Controls.Add(this.lblUsedStockArea, 3, 1);
            this.tlpSummary.Controls.Add(this.lblPartsArea, 3, 2);
            this.tlpSummary.Controls.Add(this.lblUsedPartsArea, 3, 3);
            this.tlpSummary.Controls.Add(this.label7, 4, 0);
            this.tlpSummary.Controls.Add(this.label8, 4, 1);
            this.tlpSummary.Controls.Add(this.label9, 4, 2);
            this.tlpSummary.Controls.Add(this.label10, 4, 3);
            this.tlpSummary.Controls.Add(this.label11, 4, 4);
            this.tlpSummary.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlpSummary.Location = new System.Drawing.Point(2, 464);
            this.tlpSummary.Name = "tlpSummary";
            this.tlpSummary.RowCount = 5;
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSummary.Size = new System.Drawing.Size(267, 90);
            this.tlpSummary.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stock";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Used stock";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Parts";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Placed parts";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Waste";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(106, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "%";
            // 
            // lblStockCount
            // 
            this.lblStockCount.AutoSize = true;
            this.lblStockCount.Location = new System.Drawing.Point(87, 0);
            this.lblStockCount.Name = "lblStockCount";
            this.lblStockCount.Size = new System.Drawing.Size(13, 13);
            this.lblStockCount.TabIndex = 1;
            this.lblStockCount.Text = "0";
            // 
            // lblUsedStockCount
            // 
            this.lblUsedStockCount.AutoSize = true;
            this.lblUsedStockCount.Location = new System.Drawing.Point(87, 17);
            this.lblUsedStockCount.Name = "lblUsedStockCount";
            this.lblUsedStockCount.Size = new System.Drawing.Size(13, 13);
            this.lblUsedStockCount.TabIndex = 1;
            this.lblUsedStockCount.Text = "0";
            // 
            // lblPartsCount
            // 
            this.lblPartsCount.AutoSize = true;
            this.lblPartsCount.Location = new System.Drawing.Point(87, 34);
            this.lblPartsCount.Name = "lblPartsCount";
            this.lblPartsCount.Size = new System.Drawing.Size(13, 13);
            this.lblPartsCount.TabIndex = 1;
            this.lblPartsCount.Text = "0";
            // 
            // lblUsedPartsCount
            // 
            this.lblUsedPartsCount.AutoSize = true;
            this.lblUsedPartsCount.Location = new System.Drawing.Point(87, 51);
            this.lblUsedPartsCount.Name = "lblUsedPartsCount";
            this.lblUsedPartsCount.Size = new System.Drawing.Size(13, 13);
            this.lblUsedPartsCount.TabIndex = 1;
            this.lblUsedPartsCount.Text = "0";
            // 
            // lblWastePerc
            // 
            this.lblWastePerc.AutoSize = true;
            this.lblWastePerc.Location = new System.Drawing.Point(87, 68);
            this.lblWastePerc.Name = "lblWastePerc";
            this.lblWastePerc.Size = new System.Drawing.Size(13, 13);
            this.lblWastePerc.TabIndex = 1;
            this.lblWastePerc.Text = "0";
            // 
            // lblWasteArea
            // 
            this.lblWasteArea.AutoSize = true;
            this.lblWasteArea.Location = new System.Drawing.Point(127, 68);
            this.lblWasteArea.Name = "lblWasteArea";
            this.lblWasteArea.Size = new System.Drawing.Size(13, 13);
            this.lblWasteArea.TabIndex = 1;
            this.lblWasteArea.Text = "0";
            // 
            // lblStockArea
            // 
            this.lblStockArea.AutoSize = true;
            this.lblStockArea.Location = new System.Drawing.Point(127, 0);
            this.lblStockArea.Name = "lblStockArea";
            this.lblStockArea.Size = new System.Drawing.Size(13, 13);
            this.lblStockArea.TabIndex = 1;
            this.lblStockArea.Text = "0";
            // 
            // lblUsedStockArea
            // 
            this.lblUsedStockArea.AutoSize = true;
            this.lblUsedStockArea.Location = new System.Drawing.Point(127, 17);
            this.lblUsedStockArea.Name = "lblUsedStockArea";
            this.lblUsedStockArea.Size = new System.Drawing.Size(13, 13);
            this.lblUsedStockArea.TabIndex = 1;
            this.lblUsedStockArea.Text = "0";
            // 
            // lblPartsArea
            // 
            this.lblPartsArea.AutoSize = true;
            this.lblPartsArea.Location = new System.Drawing.Point(127, 34);
            this.lblPartsArea.Name = "lblPartsArea";
            this.lblPartsArea.Size = new System.Drawing.Size(13, 13);
            this.lblPartsArea.TabIndex = 1;
            this.lblPartsArea.Text = "0";
            // 
            // lblUsedPartsArea
            // 
            this.lblUsedPartsArea.AutoSize = true;
            this.lblUsedPartsArea.Location = new System.Drawing.Point(127, 51);
            this.lblUsedPartsArea.Name = "lblUsedPartsArea";
            this.lblUsedPartsArea.Size = new System.Drawing.Size(13, 13);
            this.lblUsedPartsArea.TabIndex = 1;
            this.lblUsedPartsArea.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "m²";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "m²";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(146, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "m²";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(146, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "m²";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(146, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(18, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "m²";
            // 
            // tcInputs
            // 
            this.tcInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcInputs.ContextMenuStrip = this.mnuGridContextMenu;
            this.tcInputs.Controls.Add(this.tpMaterials);
            this.tcInputs.Controls.Add(this.tpStock);
            this.tcInputs.Controls.Add(this.tpParts);
            this.tcInputs.ItemSize = new System.Drawing.Size(60, 18);
            this.tcInputs.Location = new System.Drawing.Point(-4, 0);
            this.tcInputs.Margin = new System.Windows.Forms.Padding(0);
            this.tcInputs.Name = "tcInputs";
            this.tcInputs.Padding = new System.Drawing.Point(0, 0);
            this.tcInputs.SelectedIndex = 0;
            this.tcInputs.Size = new System.Drawing.Size(288, 481);
            this.tcInputs.TabIndex = 0;
            // 
            // mnuGridContextMenu
            // 
            this.mnuGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniDuplicateRows,
            this.mniRemoveRows,
            this.mniCentreItem,
            this.mniIsolateMaterial,
            this.mniToggleFreeze});
            this.mnuGridContextMenu.Name = "mnuGridContextMenu";
            this.mnuGridContextMenu.Size = new System.Drawing.Size(207, 114);
            this.mnuGridContextMenu.Opened += new System.EventHandler(this.mnuGridContextMenu_Opened);
            // 
            // mniDuplicateRows
            // 
            this.mniDuplicateRows.Name = "mniDuplicateRows";
            this.mniDuplicateRows.Size = new System.Drawing.Size(206, 22);
            this.mniDuplicateRows.Text = "Duplicate selected row(s)";
            this.mniDuplicateRows.Click += new System.EventHandler(this.mniDuplicateRows_Click);
            // 
            // mniRemoveRows
            // 
            this.mniRemoveRows.Name = "mniRemoveRows";
            this.mniRemoveRows.Size = new System.Drawing.Size(206, 22);
            this.mniRemoveRows.Text = "Remove selected row(s)";
            this.mniRemoveRows.Click += new System.EventHandler(this.mniRemoveRows_Click);
            // 
            // mniCentreItem
            // 
            this.mniCentreItem.Name = "mniCentreItem";
            this.mniCentreItem.Size = new System.Drawing.Size(206, 22);
            this.mniCentreItem.Text = "Centre selected item";
            this.mniCentreItem.Click += new System.EventHandler(this.mniCentreItem_Click);
            // 
            // mniIsolateMaterial
            // 
            this.mniIsolateMaterial.Name = "mniIsolateMaterial";
            this.mniIsolateMaterial.Size = new System.Drawing.Size(206, 22);
            this.mniIsolateMaterial.Text = "Isolate board";
            this.mniIsolateMaterial.Click += new System.EventHandler(this.mniIsolateMaterial_Click);
            // 
            // mniToggleFreeze
            // 
            this.mniToggleFreeze.Name = "mniToggleFreeze";
            this.mniToggleFreeze.Size = new System.Drawing.Size(206, 22);
            this.mniToggleFreeze.Text = "(Un)Freeze board";
            this.mniToggleFreeze.Click += new System.EventHandler(this.mniToggleFreeze_Click);
            // 
            // tpMaterials
            // 
            this.tpMaterials.BackColor = System.Drawing.SystemColors.Control;
            this.tpMaterials.Controls.Add(this.MaterialsGridView);
            this.tpMaterials.Location = new System.Drawing.Point(4, 22);
            this.tpMaterials.Margin = new System.Windows.Forms.Padding(0);
            this.tpMaterials.Name = "tpMaterials";
            this.tpMaterials.Size = new System.Drawing.Size(280, 455);
            this.tpMaterials.TabIndex = 0;
            this.tpMaterials.Text = "Materials";
            // 
            // MaterialsGridView
            // 
            this.MaterialsGridView.AllowUserToOrderColumns = true;
            this.MaterialsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.MaterialsGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.MaterialsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MaterialsGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.MaterialsGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MaterialsGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.MaterialsGridView.ColumnHeadersHeight = 20;
            this.MaterialsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MaterialsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaterialNameColumn,
            this.MaterialLengthColumn,
            this.MaterialWidth,
            this.MaterialThicknessColumn,
            this.MaterialCostColumn});
            this.MaterialsGridView.ContextMenuStrip = this.mnuGridContextMenu;
            this.MaterialsGridView.Location = new System.Drawing.Point(-1, -1);
            this.MaterialsGridView.Name = "MaterialsGridView";
            this.MaterialsGridView.RowHeadersWidth = 25;
            this.MaterialsGridView.Size = new System.Drawing.Size(274, 456);
            this.MaterialsGridView.TabIndex = 2;
            this.MaterialsGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.MaterialsGridView_CellValidating);
            this.MaterialsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.MaterialsGridView_CellValueChanged);
            this.MaterialsGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MaterialsGridView_ColumnHeaderMouseClick);
            this.MaterialsGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MaterialsGridView_DataError);
            this.MaterialsGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.MaterialsGridView_RowsRemoved);
            // 
            // MaterialNameColumn
            // 
            this.MaterialNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaterialNameColumn.DataPropertyName = "Name";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.MaterialNameColumn.DefaultCellStyle = dataGridViewCellStyle14;
            this.MaterialNameColumn.HeaderText = "Name";
            this.MaterialNameColumn.Name = "MaterialNameColumn";
            // 
            // MaterialLengthColumn
            // 
            this.MaterialLengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaterialLengthColumn.DataPropertyName = "Length";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N1";
            this.MaterialLengthColumn.DefaultCellStyle = dataGridViewCellStyle15;
            this.MaterialLengthColumn.FillWeight = 40F;
            this.MaterialLengthColumn.HeaderText = "Length";
            this.MaterialLengthColumn.Name = "MaterialLengthColumn";
            // 
            // MaterialWidth
            // 
            this.MaterialWidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaterialWidth.DataPropertyName = "Width";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N1";
            this.MaterialWidth.DefaultCellStyle = dataGridViewCellStyle16;
            this.MaterialWidth.FillWeight = 40F;
            this.MaterialWidth.HeaderText = "Width";
            this.MaterialWidth.Name = "MaterialWidth";
            // 
            // MaterialThicknessColumn
            // 
            this.MaterialThicknessColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaterialThicknessColumn.DataPropertyName = "Thickness";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N1";
            this.MaterialThicknessColumn.DefaultCellStyle = dataGridViewCellStyle17;
            this.MaterialThicknessColumn.FillWeight = 40F;
            this.MaterialThicknessColumn.HeaderText = "Thickness";
            this.MaterialThicknessColumn.Name = "MaterialThicknessColumn";
            // 
            // MaterialCostColumn
            // 
            this.MaterialCostColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaterialCostColumn.DataPropertyName = "Cost";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "C2";
            this.MaterialCostColumn.DefaultCellStyle = dataGridViewCellStyle18;
            this.MaterialCostColumn.FillWeight = 50F;
            this.MaterialCostColumn.HeaderText = "R/m3";
            this.MaterialCostColumn.Name = "MaterialCostColumn";
            // 
            // tpStock
            // 
            this.tpStock.BackColor = System.Drawing.SystemColors.Control;
            this.tpStock.Controls.Add(this.StockGridView);
            this.tpStock.Location = new System.Drawing.Point(4, 22);
            this.tpStock.Name = "tpStock";
            this.tpStock.Padding = new System.Windows.Forms.Padding(3);
            this.tpStock.Size = new System.Drawing.Size(280, 455);
            this.tpStock.TabIndex = 1;
            this.tpStock.Text = "Stock";
            // 
            // StockGridView
            // 
            this.StockGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.StockGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.StockGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StockGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.StockGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.StockGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.StockGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.StockGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StockGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.stocknamecolumn,
            this.StockLengthColumn,
            this.StockWidthColumn,
            this.StockMaterialColumn});
            this.StockGridView.ContextMenuStrip = this.mnuGridContextMenu;
            this.StockGridView.Location = new System.Drawing.Point(-1, -1);
            this.StockGridView.Margin = new System.Windows.Forms.Padding(0);
            this.StockGridView.Name = "StockGridView";
            this.StockGridView.RowHeadersWidth = 25;
            this.StockGridView.Size = new System.Drawing.Size(274, 456);
            this.StockGridView.TabIndex = 1;
            this.StockGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.StockGridView_CellFormatting);
            this.StockGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.StockGridView_CellValueChanged);
            this.StockGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.StockGridView_ColumnHeaderMouseClick);
            this.StockGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.StockGridView_DataError);
            this.StockGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.StockGridView_RowsRemoved);
            // 
            // stocknamecolumn
            // 
            this.stocknamecolumn.DataPropertyName = "Name";
            this.stocknamecolumn.HeaderText = "Name";
            this.stocknamecolumn.Name = "stocknamecolumn";
            // 
            // StockLengthColumn
            // 
            this.StockLengthColumn.DataPropertyName = "Length";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Format = "N1";
            dataGridViewCellStyle19.NullValue = null;
            this.StockLengthColumn.DefaultCellStyle = dataGridViewCellStyle19;
            this.StockLengthColumn.FillWeight = 40F;
            this.StockLengthColumn.HeaderText = "Length";
            this.StockLengthColumn.Name = "StockLengthColumn";
            // 
            // StockWidthColumn
            // 
            this.StockWidthColumn.DataPropertyName = "Width";
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle20.Format = "N1";
            dataGridViewCellStyle20.NullValue = null;
            this.StockWidthColumn.DefaultCellStyle = dataGridViewCellStyle20;
            this.StockWidthColumn.FillWeight = 40F;
            this.StockWidthColumn.HeaderText = "Width";
            this.StockWidthColumn.Name = "StockWidthColumn";
            // 
            // StockMaterialColumn
            // 
            this.StockMaterialColumn.DataPropertyName = "Material";
            this.StockMaterialColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StockMaterialColumn.HeaderText = "Material";
            this.StockMaterialColumn.Name = "StockMaterialColumn";
            this.StockMaterialColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StockMaterialColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tpParts
            // 
            this.tpParts.BackColor = System.Drawing.SystemColors.Control;
            this.tpParts.Controls.Add(this.PartsGridView);
            this.tpParts.Location = new System.Drawing.Point(4, 22);
            this.tpParts.Name = "tpParts";
            this.tpParts.Size = new System.Drawing.Size(280, 455);
            this.tpParts.TabIndex = 2;
            this.tpParts.Text = "Parts";
            // 
            // PartsGridView
            // 
            this.PartsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PartsGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.PartsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PartsGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.PartsGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.PartsGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.PartsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PartsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartNameColumn,
            this.PartLengthColumn,
            this.PartWidthColumn,
            this.PartMaterialColumn});
            this.PartsGridView.ContextMenuStrip = this.mnuGridContextMenu;
            this.PartsGridView.Location = new System.Drawing.Point(-1, -1);
            this.PartsGridView.Name = "PartsGridView";
            this.PartsGridView.RowHeadersWidth = 25;
            this.PartsGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.PartsGridView.Size = new System.Drawing.Size(274, 456);
            this.PartsGridView.TabIndex = 2;
            this.PartsGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.PartsGridView_CellFormatting);
            this.PartsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PartsDataGridView_CellValueChanged);
            this.PartsGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PartsDataGridView_ColumnHeaderMouseClick);
            this.PartsGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.PartsGridView_DataError);
            this.PartsGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.PartsDataGridView_RowsRemoved);
            // 
            // PartNameColumn
            // 
            this.PartNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PartNameColumn.DataPropertyName = "Name";
            this.PartNameColumn.HeaderText = "Name";
            this.PartNameColumn.Name = "PartNameColumn";
            // 
            // PartLengthColumn
            // 
            this.PartLengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PartLengthColumn.DataPropertyName = "Length";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.Format = "N1";
            dataGridViewCellStyle21.NullValue = null;
            this.PartLengthColumn.DefaultCellStyle = dataGridViewCellStyle21;
            this.PartLengthColumn.FillWeight = 40F;
            this.PartLengthColumn.HeaderText = "Length";
            this.PartLengthColumn.Name = "PartLengthColumn";
            // 
            // PartWidthColumn
            // 
            this.PartWidthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PartWidthColumn.DataPropertyName = "Width";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Format = "N1";
            dataGridViewCellStyle22.NullValue = null;
            this.PartWidthColumn.DefaultCellStyle = dataGridViewCellStyle22;
            this.PartWidthColumn.FillWeight = 40F;
            this.PartWidthColumn.HeaderText = "Width";
            this.PartWidthColumn.Name = "PartWidthColumn";
            // 
            // PartMaterialColumn
            // 
            this.PartMaterialColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PartMaterialColumn.DataPropertyName = "Material";
            this.PartMaterialColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PartMaterialColumn.HeaderText = "Material";
            this.PartMaterialColumn.Name = "PartMaterialColumn";
            this.PartMaterialColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PartMaterialColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tcMaterials
            // 
            this.tcMaterials.Controls.Add(this.tabPage1);
            this.tcMaterials.Dock = System.Windows.Forms.DockStyle.Top;
            this.tcMaterials.Location = new System.Drawing.Point(0, 0);
            this.tcMaterials.Name = "tcMaterials";
            this.tcMaterials.SelectedIndex = 0;
            this.tcMaterials.Size = new System.Drawing.Size(751, 22);
            this.tcMaterials.TabIndex = 1;
            this.tcMaterials.SelectedIndexChanged += new System.EventHandler(this.tcMaterials_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(743, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Material1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pbLayout
            // 
            this.pbLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLayout.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pbLayout.Location = new System.Drawing.Point(1, 20);
            this.pbLayout.Name = "pbLayout";
            this.pbLayout.Size = new System.Drawing.Size(746, 535);
            this.pbLayout.TabIndex = 0;
            this.pbLayout.TabStop = false;
            this.pbLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLayout_Paint);
            this.pbLayout.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbLayout_MouseDoubleClick);
            this.pbLayout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbLayout_MouseDown);
            this.pbLayout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbLayout_MouseMove);
            // 
            // btnPartsTab
            // 
            this.btnPartsTab.AutoSize = true;
            this.btnPartsTab.BackColor = System.Drawing.Color.Transparent;
            this.btnPartsTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Parts_Materials;
            this.btnPartsTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPartsTab.FlatAppearance.BorderSize = 0;
            this.btnPartsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPartsTab.Location = new System.Drawing.Point(0, 77);
            this.btnPartsTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnPartsTab.Name = "btnPartsTab";
            this.btnPartsTab.Size = new System.Drawing.Size(22, 80);
            this.btnPartsTab.TabIndex = 3;
            this.btnPartsTab.UseVisualStyleBackColor = false;
            this.btnPartsTab.Click += new System.EventHandler(this.btnPartsTab_Click);
            // 
            // btnStockTab
            // 
            this.btnStockTab.AutoSize = true;
            this.btnStockTab.BackColor = System.Drawing.Color.Transparent;
            this.btnStockTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Stock_Materials;
            this.btnStockTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStockTab.FlatAppearance.BorderSize = 0;
            this.btnStockTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockTab.Location = new System.Drawing.Point(0, 157);
            this.btnStockTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnStockTab.Name = "btnStockTab";
            this.btnStockTab.Size = new System.Drawing.Size(22, 75);
            this.btnStockTab.TabIndex = 2;
            this.btnStockTab.UseVisualStyleBackColor = false;
            this.btnStockTab.Click += new System.EventHandler(this.btnStockTab_Click);
            // 
            // btnMaterialsTab
            // 
            this.btnMaterialsTab.AutoSize = true;
            this.btnMaterialsTab.BackColor = System.Drawing.Color.Transparent;
            this.btnMaterialsTab.BackgroundImage = global::CuttingPlanMaker.Properties.Resources.Materials_Materials;
            this.btnMaterialsTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMaterialsTab.FlatAppearance.BorderSize = 0;
            this.btnMaterialsTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterialsTab.Location = new System.Drawing.Point(0, 232);
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
            this.btnCollapseExpandTab.Location = new System.Drawing.Point(0, 33);
            this.btnCollapseExpandTab.Margin = new System.Windows.Forms.Padding(0);
            this.btnCollapseExpandTab.Name = "btnCollapseExpandTab";
            this.btnCollapseExpandTab.Size = new System.Drawing.Size(22, 35);
            this.btnCollapseExpandTab.TabIndex = 4;
            this.btnCollapseExpandTab.UseVisualStyleBackColor = false;
            this.btnCollapseExpandTab.Click += new System.EventHandler(this.btnCollapseExpandTab_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 584);
            this.Controls.Add(this.btnPartsTab);
            this.Controls.Add(this.btnStockTab);
            this.Controls.Add(this.btnMaterialsTab);
            this.Controls.Add(this.ctrSplitContainer);
            this.Controls.Add(this.btnCollapseExpandTab);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "FrmMain";
            this.Text = "Cutting Plan Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ctrSplitContainer.Panel1.ResumeLayout(false);
            this.ctrSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctrSplitContainer)).EndInit();
            this.ctrSplitContainer.ResumeLayout(false);
            this.tlpSummary.ResumeLayout(false);
            this.tlpSummary.PerformLayout();
            this.tcInputs.ResumeLayout(false);
            this.mnuGridContextMenu.ResumeLayout(false);
            this.tpMaterials.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MaterialsGridView)).EndInit();
            this.tpStock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StockGridView)).EndInit();
            this.tpParts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PartsGridView)).EndInit();
            this.tcMaterials.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLayout)).EndInit();
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
        private System.Windows.Forms.TabControl tcInputs;
        private System.Windows.Forms.TabPage tpMaterials;
        private System.Windows.Forms.TabPage tpStock;
        private System.Windows.Forms.TabPage tpParts;
        private System.Windows.Forms.DataGridView MaterialsGridView;
        private System.Windows.Forms.DataGridView StockGridView;
        private System.Windows.Forms.DataGridView PartsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialThicknessColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialCostColumn;
        private System.Windows.Forms.ToolStripMenuItem mniFileRevert;
        private System.Windows.Forms.ContextMenuStrip mnuGridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mniDuplicateRows;
        private System.Windows.Forms.ToolStripMenuItem mniRemoveRows;
        private System.Windows.Forms.ToolStripMenuItem mniToolsPack;
        private System.Windows.Forms.TabControl tcMaterials;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pbLayout;
        private System.Windows.Forms.DataGridViewTextBoxColumn stocknamecolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockWidthColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn StockMaterialColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartWidthColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PartMaterialColumn;
        private System.Windows.Forms.TableLayoutPanel tlpSummary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblStockCount;
        private System.Windows.Forms.Label lblUsedStockCount;
        private System.Windows.Forms.Label lblPartsCount;
        private System.Windows.Forms.Label lblUsedPartsCount;
        private System.Windows.Forms.Label lblWastePerc;
        private System.Windows.Forms.Label lblWasteArea;
        private System.Windows.Forms.Label lblStockArea;
        private System.Windows.Forms.Label lblUsedStockArea;
        private System.Windows.Forms.Label lblPartsArea;
        private System.Windows.Forms.Label lblUsedPartsArea;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem mniReportLayoutLabels;
        private System.Windows.Forms.ToolStripMenuItem mniCentreItem;
        private System.Windows.Forms.ToolStripMenuItem mniSaveCopyAs;
        private System.Windows.Forms.ToolStripMenuItem mniIsolateMaterial;
        private System.Windows.Forms.ToolStripMenuItem mniAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem mniToggleFreeze;
    }
}

