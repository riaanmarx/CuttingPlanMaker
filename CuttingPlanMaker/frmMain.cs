
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuttingPlanMaker
{
    /// <summary>
    /// main form of the application
    /// </summary>
    public partial class frmMain : Form
    {
        #region // Fields & Properties ...

        // flag to keep track if the current file is saved
        bool IsFileSaved
        {
            get { return _isFileSaved; }
            set
            {
                _isFileSaved = value;
                this.Text = $"Cutting Plan Maker {(_isFileSaved ? "" : "*")}{(FileName == "" ? "" : $"({FileName})")}";
            }
        }
        bool _isFileSaved = true;

        // the name of the current file
        string FileName
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        // the full path to the current file
        string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if (value == "")
                    _filePath = "";
                else
                    _filePath = Path.Combine(Path.GetDirectoryName(value), Path.GetFileName(value).Split('.').First());

                this.Text = $"Cutting Plan Maker {(_isFileSaved ? "" : "*")}{(FileName == "" ? "" : $"({FileName})")}";
            }
        }
        private string _filePath = "";

        // Data for the loaded file
        BindingList<Settings> Settings { get; set; }
        Settings Setting
        {
            get { return Settings.FirstOrDefault(); }
        }
        BindingList<Material> Materials { get; set; }
        BindingList<StockItem> Stock { get; set; }
        BindingList<Part> Parts { get; set; }

        // grid sort options
        string PartGridSort = "";
        string StockGridSort = "";
        string MaterialGridSort = "";

        bool isPackingRequired = false;
                
        #endregion

        #region // Constructor ...
        // constructor for the main form
        public frmMain()
        {
            InitializeComponent();
            tcInputs.Top -= 22;
            this.pbLayout.MouseWheel += PbLayout_MouseWheel;
        }
        #endregion

        #region // Internal helper functions ...
        public Bitmap Draw(StockItem[] boards, bool usedstockonly = true)
        {

            double xMargin = 50;
            double yMargin = 50;
            double boardSpacing = 70;

            double yOffset = yMargin;
            double imageHeight = 2 * yMargin;
            double imageWidth = 0;
            Font boardFont = new Font(new FontFamily("Microsoft Sans Serif"), 15.0f);

            // create list of boards to draw
            List<StockItem> boardsToDraw = new List<StockItem>(boards);
            if (usedstockonly)
                boardsToDraw = boards.Where(t => t.PackedParts != null).ToList();

            // calculate width & height required for the bitmap
            foreach (var iBoard in boardsToDraw)
            {
                if (iBoard.Length > imageWidth) imageWidth = iBoard.Length;
                imageHeight += iBoard.Width + boardSpacing;
            }
            imageWidth += 2 * xMargin;

            // create bitmap
            Bitmap bitmap = new Bitmap((int)imageWidth, (int)imageHeight);



            Graphics g = Graphics.FromImage(bitmap);

            // fill the background with black
            g.FillRectangle(SystemBrushes.ButtonHighlight, g.ClipBounds);

            // loop through all the boards to be drawn
            yOffset = yMargin;
            foreach (var iBoard in boardsToDraw)
            {
                // draw the board
                g.FillRectangle(Brushes.DarkRed, (float)xMargin, (float)yOffset, (float)iBoard.Length, (float)iBoard.Width);
                string boardheader = $"{iBoard.Name} [{iBoard.Length}x{iBoard.Width}]";
                SizeF textSizeBoard = g.MeasureString(boardheader, boardFont);
                g.DrawString(boardheader, boardFont, Brushes.Black, (float)(xMargin), (float)(yOffset - textSizeBoard.Height));

                // loop through all the parts and draw the ones on the current board
                //string overflowtext = "";
                for (int i = 0; i < iBoard.PackedPartsCount; i++)
                {
                    Part iPlacement = iBoard.PackedParts[i];
                    double dLength = iBoard.PackedPartdLengths[i];
                    double dWidth = iBoard.PackedPartdWidths[i];

                    // draw the part
                    g.FillRectangle(Brushes.Green, (float)(xMargin + dLength), (float)(yOffset + dWidth), (float)iPlacement.Length, (float)iPlacement.Width);

                    // print the part text
                    string partLabel = $"{iPlacement.Name} [{iPlacement.Length} x {iPlacement.Width}]";

                    int sz = 16;
                    Font partFont;
                    SizeF textSize;
                    do
                    {
                        partFont = new Font(new FontFamily("Microsoft Sans Serif"), sz);
                        textSize = g.MeasureString(partLabel, partFont);
                        if (textSize.Width < iPlacement.Length && textSize.Height < iPlacement.Width)
                        {
                            g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPlacement.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPlacement.Width - 0.5 * textSize.Height));
                            break;
                        }
                        else sz--;

                    } while (sz > 8);
                    if (sz <= 8)
                    {
                        partLabel = $"{iPlacement.Name}";
                        textSize = g.MeasureString(partLabel, partFont);
                        g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPlacement.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPlacement.Width - 0.5 * textSize.Height));

                    }
                }

                yOffset += iBoard.Width + boardSpacing;
            }


            if (isPackingRequired)
            {
                Brush hbrush = new SolidBrush(Color.FromArgb(40, Color.Red));
                g.FillRectangle(hbrush, g.ClipBounds);
                //g.DrawString("Repacking is required", boardFont,Brushes.White,0,0);
            }


            g.Flush();
            //bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bitmap;
        }


        private void SortParts(string partGridSort)
        {
            switch (partGridSort)
            {
                case "Name ASC":
                    Parts = new BindingList<Part>(Parts.OrderBy(t => t.Name).ToList());
                    break;
                case "Name DESC":
                    Parts = new BindingList<Part>(Parts.OrderByDescending(t => t.Name).ToList());
                    break;
                case "Length ASC":
                    Parts = new BindingList<Part>(Parts.OrderBy(t => t.Length).ToList());
                    break;
                case "Length DESC":
                    Parts = new BindingList<Part>(Parts.OrderByDescending(t => t.Length).ToList());
                    break;
                case "Width ASC":
                    Parts = new BindingList<Part>(Parts.OrderBy(t => t.Width).ToList());
                    break;
                case "Width DESC":
                    Parts = new BindingList<Part>(Parts.OrderByDescending(t => t.Width).ToList());
                    break;
                case "Material ASC":
                    Parts = new BindingList<Part>(Parts.OrderBy(t => t.Material).ToList());
                    break;
                case "Thickness DESC":
                    Parts = new BindingList<Part>(Parts.OrderByDescending(t => t.Material).ToList());
                    break;

                default:
                    break;
            }

            // bind to the new sorted source
            BindPartsGrid();
        }

        private void SortStock(string sortOption)
        {
            switch (StockGridSort)
            {
                case "Name ASC":
                    Stock = new BindingList<StockItem>(Stock.OrderBy(t => t.Name).ToList());
                    break;
                case "Name DESC":
                    Stock = new BindingList<StockItem>(Stock.OrderByDescending(t => t.Name).ToList());
                    break;
                case "Length ASC":
                    Stock = new BindingList<StockItem>(Stock.OrderBy(t => t.Length).ToList());
                    break;
                case "Length DESC":
                    Stock = new BindingList<StockItem>(Stock.OrderByDescending(t => t.Length).ToList());
                    break;
                case "Width ASC":
                    Stock = new BindingList<StockItem>(Stock.OrderBy(t => t.Width).ToList());
                    break;
                case "Width DESC":
                    Stock = new BindingList<StockItem>(Stock.OrderByDescending(t => t.Width).ToList());
                    break;
                case "Material ASC":
                    Stock = new BindingList<StockItem>(Stock.OrderBy(t => t.Material).ToList());
                    break;
                case "Thickness DESC":
                    Stock = new BindingList<StockItem>(Stock.OrderByDescending(t => t.Material).ToList());
                    break;

                default:
                    break;
            }

            // bind to the new sorted source
            BindStockGrid();
        }

        private void SortMaterials(string sortoption)
        {
            // sort the grid according to choice
            switch (MaterialGridSort)
            {
                case "Name ASC":
                    Materials = new BindingList<Material>(Materials.OrderBy(t => t.Name).ToList());
                    break;
                case "Name DESC":
                    Materials = new BindingList<Material>(Materials.OrderByDescending(t => t.Name).ToList());
                    break;
                case "Length ASC":
                    Materials = new BindingList<Material>(Materials.OrderBy(t => t.Length).ToList());
                    break;
                case "Length DESC":
                    Materials = new BindingList<Material>(Materials.OrderByDescending(t => t.Length).ToList());
                    break;
                case "Width ASC":
                    Materials = new BindingList<Material>(Materials.OrderBy(t => t.Width).ToList());
                    break;
                case "Width DESC":
                    Materials = new BindingList<Material>(Materials.OrderByDescending(t => t.Width).ToList());
                    break;
                case "Thickness ASC":
                    Materials = new BindingList<Material>(Materials.OrderBy(t => t.Thickness).ToList());
                    break;
                case "Thickness DESC":
                    Materials = new BindingList<Material>(Materials.OrderByDescending(t => t.Thickness).ToList());
                    break;
                case "Cost ASC":
                    Materials = new BindingList<Material>(Materials.OrderBy(t => t.Cost).ToList());
                    break;
                case "Cost DESC":
                    Materials = new BindingList<Material>(Materials.OrderByDescending(t => t.Cost).ToList());
                    break;
                default:
                    break;
            }

            // bind to the new sorted source
            BindMaterialsGrid();
        }

        /// <summary>
        /// Save the current data to the file set with the specified name
        /// </summary>
        /// <param name="path"></param>
        private void SaveFile()
        {
            // write the file data to the csv files
            CSVFile.Write(Materials, $"{FilePath}.Materials.csv");
            CSVFile.Write(Stock, $"{FilePath}.Stock.csv");
            CSVFile.Write(Parts, $"{FilePath}.Parts.csv");
            CSVFile.Write(Settings, $"{FilePath}.Settings.csv");

            // update the saved flag
            IsFileSaved = true;
        }

        private void SaveConfig()
        {
            CSVFile.Write(Settings, $"{FilePath}.Settings.CSV");
        }

        private void SaveFileAs()
        {
            // ask the user where to save the file. If he picked a location and name
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // store the file details
                FilePath = saveFileDialog.FileName;

                // save the file data
                SaveFile();
            }
        }

        private void BindMaterialsGrid()
        {
            MaterialsGridView.AutoGenerateColumns = false;
            MaterialsGridView.DataSource = Materials;
        }

        /// <summary>
        /// Load a file into the application
        /// </summary>
        /// <param name="path"></param>
        private void LoadFile(string path)
        {
            // update the properties to keep track of the file
            FilePath = path;

            // Load the file into data structures
            Settings = CSVFile.Read<Settings>($"{FilePath}.Settings.CSV");
            Materials = CSVFile.Read<Material>($"{FilePath}.Materials.CSV");
            Stock = CSVFile.Read<StockItem>($"{FilePath}.Stock.CSV");
            Parts = CSVFile.Read<Part>($"{FilePath}.Parts.CSV");

            // update the tabs for the layout drawings
            PopulateMaterialTabs();

            // pack the solution before drawing the grids
            PackSolution();

            // bind the materials grid
            BindMaterialsGrid();

            // bind the stock grid
            BindStockGrid();

            // bind the Part
            BindPartsGrid();

            // ensure the saved flag is set - we just opened the file
            IsFileSaved = true;
        }

        private void BindPartsGrid()
        {
            PartsGridView.AutoGenerateColumns = false;
            PartMaterialColumn.DataSource = Materials;
            PartMaterialColumn.DisplayMember = "Name";
            PartMaterialColumn.ValueMember = "Name";
            PartsGridView.DataSource = Parts;
        }

        private void BindStockGrid()
        {
            StockGridView.AutoGenerateColumns = false;
            StockMaterialColumn.DataSource = Materials;
            StockMaterialColumn.DisplayMember = "Name";
            StockMaterialColumn.ValueMember = "Name";
            StockGridView.DataSource = Stock;
        }

        private void LoadDefault()
        {
            // start from scratch
            LoadFile("Default");
            FilePath = "";
        }

        private bool CloseFile()
        {
            // if the current file is not saved
            if (!IsFileSaved)
            {
                // prompt the user to save/discard/cancel
                DialogResult response = MessageBox.Show($"File {FileName} is not saved. Save?", "Confirm", MessageBoxButtons.YesNoCancel);

                // if user chose cancel, return cancel/failure
                if (response == DialogResult.Cancel) return false;

                // if user chose to save
                if (response == DialogResult.Yes)
                {
                    // if the file has never been saved
                    if (FileName == "")
                    {
                        // display the fileSave dialog. if the user clicks save,
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // use the new selected name
                            FilePath = saveFileDialog.FileName;

                            // save the file
                            SaveFile();

                            // return success
                            return true;
                        }
                        // return cancel/failure
                        else return false;
                    }
                    else
                        SaveFile();
                    // return success
                    return true;
                }
                else
                    // return success
                    return true;
            }
            else
                // return success
                return true;
        }

        private void PackSolution()
        {
            //clear current packing
            Parts.ToList().ForEach(t => t.isPacked = false);
            Stock.ToList().ForEach(t =>
            {
                t.AssociatedBoard = null;
                t.dLength = 0;
                t.dWidth = 0;
                t.isComplete = false;
                t.isInUse = false;
                t.PackedPartdLengths = null;
                t.PackedPartdWidths = null;
                t.PackedParts = null;
                t.PackedPartsCount = 0;
                t.PackedPartsTotalArea = 0;
            });

            // for every material, filter the parts and stock and pack each set separately
            for (int i = 0; i < Materials.Count; i++)
            {
                Material iMaterial = Materials[i];
                Part[] iParts = Parts.Where(t => t.Material == iMaterial.Name).ToArray();
                StockItem[] iStock = Stock.Where(t => t.Material == iMaterial.Name).ToArray();

                Packer.Pack(iParts
                , iStock
                , double.Parse(Setting.BladeKerf)
                , double.Parse(Setting.PartPaddingLength)
                , double.Parse(Setting.PartPaddingWidth));
            }

            isPackingRequired = false;



            pbLayout.Invalidate();

        }

        #endregion

        #region // Event handlers ...

        private void PopulateMaterialTabs()
        {
            //add any new pages
            for (int i = 0; i < Materials.Count; i++)
            {
                Material iMaterial = Materials[i];
                if (!tcMaterials.TabPages.ContainsKey(iMaterial.Name))
                    tcMaterials.TabPages.Add(iMaterial.Name, iMaterial.Name);
            }

            //remove any missing materials
            for (int i = tcMaterials.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage iTab = tcMaterials.TabPages[i];
                if (Materials.FirstOrDefault(t => t.Name == iTab.Name) == null)
                    tcMaterials.TabPages.Remove(iTab);
            }
            pbLayout.Invalidate();
        }

        private void onGridDataChangeByUser(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (sender == MaterialsGridView)
                PopulateMaterialTabs();

            IsFileSaved = false;

            if (Setting.AutoRepack == "true")
                PackSolution();
            else
            {
                isPackingRequired = true;
                pbLayout.Invalidate();
            }
        }

        private void onGridDataChangeByUser(object sender, DataGridViewCellEventArgs e)
        {
            if (sender == MaterialsGridView)
                PopulateMaterialTabs();

            IsFileSaved = false;
            if (Setting.AutoRepack == "true")
                PackSolution();
            else
            {
                isPackingRequired = true;
                pbLayout.Invalidate();
            }
        }

        private void mniFileExit_Click(object sender, EventArgs e)
        {
            // exit the application
            Application.Exit();
        }

        private void mniEditDuplicate_Click(object sender, EventArgs e)
        {
            IsFileSaved = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // load the default file as the default
            LoadDefault();
        }

        private void mniFileNew_Click(object sender, EventArgs e)
        {
            // if the current file was closed
            if (CloseFile())
                // load the default file as a new file
                LoadDefault();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if the user just closes the applciation, check if the file is saved
            e.Cancel = !CloseFile();
        }

        private void mniFileOpen_Click(object sender, EventArgs e)
        {
            if (CloseFile())
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    LoadFile(openFileDialog.FileName);
        }

        private void mniFileSave_Click(object sender, EventArgs e)
        {
            if (FilePath == "")
                SaveFileAs();
            else
                SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void mniToolsOptions_Click(object sender, EventArgs e)
        {
            // show settings dialog, if close with save, save the config
            if (new frmSettingsDialog(Setting).ShowDialog() == DialogResult.OK)
            {
                if (FilePath != "") SaveConfig();
                if (Setting.AutoRepack == "true")
                    PackSolution();
                else
                {
                    isPackingRequired = true;
                    pbLayout.Invalidate();
                }
            }
        }

        private void btnMaterialsTab_Click(object sender, EventArgs e)
        {
            // change button images to create appearance of tabs
            btnStockTab.BackgroundImage = Properties.Resources.Stock_Materials;
            btnPartsTab.BackgroundImage = Properties.Resources.Parts_Materials;
            btnMaterialsTab.BackgroundImage = Properties.Resources.Materials_Materials;

            // ensure info pane is visible
            ctrSplitContainer.Panel1Collapsed = false;
            btnCollapseExpandTab.BackgroundImage = Properties.Resources.collapse;
            ctrSplitContainer.SendToBack();

            // select the materials tab
            tcInputs.SelectedTab = tpMaterials;
        }

        private void btnStockTab_Click(object sender, EventArgs e)
        {
            // change button images to create appearance of tabs
            btnStockTab.BackgroundImage = Properties.Resources.Stock_Stock;
            btnPartsTab.BackgroundImage = Properties.Resources.Parts_Stock;
            btnMaterialsTab.BackgroundImage = Properties.Resources.Materials_Stock;

            // ensure info pane is visible
            ctrSplitContainer.Panel1Collapsed = false;
            btnCollapseExpandTab.BackgroundImage = Properties.Resources.collapse;
            ctrSplitContainer.SendToBack();

            // select the materials tab
            tcInputs.SelectedTab = tpStock;
        }

        private void btnPartsTab_Click(object sender, EventArgs e)
        {
            // change button images to create appearance of tabs
            btnStockTab.BackgroundImage = Properties.Resources.Stock_Parts;
            btnPartsTab.BackgroundImage = Properties.Resources.Parts_Parts;
            btnMaterialsTab.BackgroundImage = Properties.Resources.Materials_Parts;

            // ensure info pane is visible
            ctrSplitContainer.Panel1Collapsed = false;
            btnCollapseExpandTab.BackgroundImage = Properties.Resources.collapse;
            ctrSplitContainer.SendToBack();

            // select the materials tab
            tcInputs.SelectedTab = tpParts;
        }

        private void btnCollapseExpandTab_Click(object sender, EventArgs e)
        {
            if (ctrSplitContainer.Panel1Collapsed)
            {
                btnCollapseExpandTab.BackgroundImage = Properties.Resources.collapse;
                ctrSplitContainer.Panel1Collapsed = false;
                ctrSplitContainer.SendToBack();
            }
            else
            {
                btnCollapseExpandTab.BackgroundImage = Properties.Resources.expand;
                ctrSplitContainer.Panel1Collapsed = true;
                ctrSplitContainer.BringToFront();
            }
        }

        private bool HasUserRemovedRow()
        {
            // check if the grid's row removed event was fired due to internal processes or the user removing a row
            return !(System.Environment.StackTrace.Contains(".OnBindingContextChanged(") || System.Environment.StackTrace.Contains(".set_DataSource("));
        }

        private bool HasUserChangedCell()
        {
            // check if the cell was changed due to application processes or the user actually changed the cell value
            return System.Environment.StackTrace.Contains(".CommitEdit(");
        }

        private void MaterialsGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // If the user removed the row - set the file saved flag
            if (HasUserRemovedRow())
                onGridDataChangeByUser(sender, e);
        }

        private void StockGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow())
                onGridDataChangeByUser(sender, e);
        }

        private void PartsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow())
                onGridDataChangeByUser(sender, e);
        }

        private void MaterialsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasUserChangedCell())
                onGridDataChangeByUser(sender, e);
        }

        private void mniFileRevert_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discard all changes?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (FilePath == "")
                    LoadDefault();
                else
                    LoadFile(FilePath);
            }
        }

        private void StockGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasUserChangedCell())
                onGridDataChangeByUser(sender, e);
        }

        private void PartsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasUserChangedCell())
                onGridDataChangeByUser(sender, e);
        }

        private void mniDuplicateRows_Click(object sender, EventArgs e)
        {
            if (tcInputs.SelectedTab == tpMaterials)
            {
                foreach (DataGridViewRow item in MaterialsGridView.SelectedRows)
                {
                    Material oldItem = (Material)item.DataBoundItem;
                    Material newItem = new Material()
                    {
                        Name = oldItem.Name,
                        Length = oldItem.Length,
                        Width = oldItem.Width,
                        Thickness = oldItem.Thickness,
                        Cost = oldItem.Cost
                    };
                    Materials.Add(newItem);
                    IsFileSaved = false;
                }
            }
            if (tcInputs.SelectedTab == tpParts)
            {
                foreach (DataGridViewRow item in PartsGridView.SelectedRows)
                {
                    Part oldItem = (Part)item.DataBoundItem;
                    Part newItem = new Part()
                    {
                        Name = oldItem.Name,
                        Length = oldItem.Length,
                        Width = oldItem.Width,
                        Material = oldItem.Material,
                    };
                    Parts.Add(newItem);
                    IsFileSaved = false;
                }
            }
            if (tcInputs.SelectedTab == tpStock)
            {
                foreach (DataGridViewRow item in StockGridView.SelectedRows)
                {
                    StockItem oldItem = (StockItem)item.DataBoundItem;
                    StockItem newItem = new StockItem()
                    {
                        Name = oldItem.Name,
                        Length = oldItem.Length,
                        Width = oldItem.Width,
                        Material = oldItem.Material,
                    };
                    Stock.Add(newItem);
                    IsFileSaved = false;
                }
            }
        }

        private void MaterialsGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // decide how/with what to sort the grid
            string columnname = MaterialsGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (MaterialGridSort.StartsWith(columnname) && MaterialGridSort.EndsWith("ASC"))
                MaterialGridSort = $"{columnname} DESC";
            else
                MaterialGridSort = $"{columnname} ASC";

            SortMaterials(MaterialGridSort);
        }

        private void StockGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // decide how/with what to sort the grid
            string columnname = StockGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (StockGridSort.StartsWith(columnname) && StockGridSort.EndsWith("ASC"))
                StockGridSort = $"{columnname} DESC";
            else
                StockGridSort = $"{columnname} ASC";

            // sort the grid according to choice
            SortStock(StockGridSort);
        }

        private void PartsDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnname = PartsGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (PartGridSort.StartsWith(columnname) && PartGridSort.EndsWith("ASC"))
                PartGridSort = $"{columnname} DESC";
            else
                PartGridSort = $"{columnname} ASC";

            // sort the grid according to choice
            SortParts(PartGridSort);
        }

        private void mniRemoveRows_Click(object sender, EventArgs e)
        {
            if (tcInputs.SelectedTab == tpMaterials)
            {
                foreach (DataGridViewRow item in MaterialsGridView.SelectedRows)
                {
                    Materials.Remove((Material)item.DataBoundItem);
                    IsFileSaved = false;
                }
            }
            if (tcInputs.SelectedTab == tpParts)
            {
                foreach (DataGridViewRow item in PartsGridView.SelectedRows)
                {
                    Parts.Remove((Part)item.DataBoundItem);
                    IsFileSaved = false;
                }
            }
            if (tcInputs.SelectedTab == tpStock)
            {
                foreach (DataGridViewRow item in StockGridView.SelectedRows)
                {
                    Stock.Remove((StockItem)item.DataBoundItem);
                    IsFileSaved = false;
                }
            }
        }

        private void MaterialsGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "Input string was not in a correct format.")
                MessageBox.Show(e.Exception.Message);
        }

        private void StockGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "Input string was not in a correct format.")
                MessageBox.Show(e.Exception.Message);
        }

        private void PartsGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "Input string was not in a correct format.")
                MessageBox.Show(e.Exception.Message);
        }

        private void mniReportPartsList_Click(object sender, EventArgs e)
        {
            new PartListReport()
                .Generate(Setting, Materials, Stock, Parts)
                .Save("PartsReport.pdf");
            Process.Start("PartsReport.pdf");
        }

        private void mniReportStockList_Click(object sender, EventArgs e)
        {
            new StockReport()
                .Generate(Setting, Materials, Stock, Parts)
                .Save("StockReport.pdf");
            Process.Start("StockReport.pdf");
        }

        private void mniReportLayout_Click(object sender, EventArgs e)
        {
            if (Setting.AutoRepack == "true")
                PackSolution();
            else
            {
                if (isPackingRequired)
                {
                    MessageBox.Show("Parts have not been packed after changes were made. Please repack first");
                    return;
                }
            }

            new LayoutReport()
                .Generate(Setting, Materials, Stock, Parts)
                .Save("LayoutReport.pdf");

            Process.Start("LayoutReport.pdf");
        }

        private void mniToolsPack_Click(object sender, EventArgs e)
        {
            PackSolution();
        }






        private void tcMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            userOffset = new PointF(0, 0);
            userZoomFactor = 1;
            pbLayout.Invalidate();
        }

        private void PartsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < Parts.Count && Parts[e.RowIndex].Area > 0 && !Parts[e.RowIndex].isPacked)
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            else
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }
        #endregion

        // zoom & panning for layout image
        float userZoomFactor = 1.0f;  // a zoom factor of 1 will fill the picturbox with the image
        PointF userOffset = new PointF(0, 0); //the offset with which the image has been panned, (0,0) would centre the image in the PB
        Bitmap LayoutBitmap = null;
        float unityScaleFactor;
        PointF OrigMouseDownPoint;
        PointF OrigUserOffset;


        private RectangleF ImageToPbSpace(float X, float Y, float width, float height, SizeF pbSize, PointF imgCentre, float userZoom, float fillScale)
        {
            return new RectangleF(
                pbSize.Width * 0.5f + fillScale * userZoom * (X - imgCentre.X),
                pbSize.Height * 0.5f + fillScale * userZoom * (Y - imgCentre.Y),
                width * fillScale * userZoom,
                height * fillScale * userZoom
                );
        }
        
        private RectangleF PbToImageSpace(float X, float Y, float width, float height, SizeF pbSize, PointF imgCentre, float userZoom, float fillScale)
        {
            return new RectangleF(
               -(X - pbSize.Width * 0.5f) / (fillScale * userZoom) + imgCentre.X,
               -(Y - pbSize.Height * 0.5f) / (fillScale * userZoom) + imgCentre.Y ,
               width / (fillScale * userZoom),
               height / (fillScale * userZoom)
               );
        }
        
        private void PbLayout_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
                userZoomFactor = userZoomFactor * 1.2f;
            else if (e.Delta < 0)
                userZoomFactor = userZoomFactor / 1.2f;
            pbLayout.Invalidate();
        }

        private void pbLayout_Paint(object sender, PaintEventArgs e)
        {
            // filter stock for chosen material
            string SelectedMaterial = tcMaterials.SelectedTab.Name;
            StockItem[] stockItems = Stock.Where(t => t.Material == SelectedMaterial).ToArray();
            // draw the layout for filterred stock
            if (LayoutBitmap != null) LayoutBitmap.Dispose();
            LayoutBitmap = Draw(stockItems, Setting.DrawUnusedStock != "true");

            // draw the image to the screen
            Graphics gfx = e.Graphics;

            // scale factor for filling the screen - will be used with user-scale factor so that when user scale factor =1, the image would fit the picturebox
            unityScaleFactor = Math.Min((float)e.ClipRectangle.Width / LayoutBitmap.Width, (float)e.ClipRectangle.Height / LayoutBitmap.Height);
            PointF imgCentre = new PointF()
            {
                X = LayoutBitmap.Width * 0.5f - userOffset.X,
                Y = LayoutBitmap.Height * 0.5f - userOffset.Y
            };

            RectangleF plotRect = ImageToPbSpace(0, 0, LayoutBitmap.Width, LayoutBitmap.Height, pbLayout.Size, imgCentre, userZoomFactor, unityScaleFactor);
            gfx.DrawImage(LayoutBitmap, plotRect);
            gfx.Flush();


            //update summary table
            lblStockCount.Text = Stock.Count(q => q.Material == SelectedMaterial).ToString();
            double StockArea = Stock.Where(q => q.Material == SelectedMaterial).Sum(t => t.Area) / 1e6;
            lblStockArea.Text = StockArea.ToString("0.000");
            lblUsedStockCount.Text = Stock.Count(t => t.Material == SelectedMaterial && t.PackedPartsCount > 0).ToString();
            double UsedStockArea = Stock.Where(q => q.Material == SelectedMaterial && q.PackedPartsCount > 0).Sum(t => t.Area) / 1e6f;
            lblUsedStockArea.Text = UsedStockArea.ToString("0.000");
            lblPartsCount.Text = Parts.Count(q => q.Material == SelectedMaterial).ToString();
            lblPartsArea.Text = (Parts.Where(q => q.Material == SelectedMaterial).Sum(t => t.Area) / 1e6f).ToString("0.000");
            lblUsedPartsCount.Text = Parts.Count(t => t.Material == SelectedMaterial && t.isPacked).ToString();
            double UsedPartsArea = (Parts.Where(q => q.Material == SelectedMaterial && q.isPacked).Sum(t => t.Area) / 1e6f);
            lblUsedPartsArea.Text = UsedPartsArea.ToString("0.000");
            lblWastePerc.Text = ((UsedStockArea - UsedPartsArea) / UsedStockArea * 100.0).ToString("00.0");
            lblWasteArea.Text = (UsedStockArea - UsedPartsArea).ToString("0.000");
        }

        private void pbLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RectangleF nr = PbToImageSpace(e.X, e.Y, 0, 0, pbLayout.Size, userOffset, userZoomFactor, unityScaleFactor);
                //nr.X -= pbLayout.Width / 2;
                //nr.Y -= pbLayout.Height / 2;
                userOffset = nr.Location;
                pbLayout.Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
                userOffset = new PointF(0, 0);
                userZoomFactor = 1;
                pbLayout.Invalidate();
            }
        }

        private void pbLayout_MouseDown(object sender, MouseEventArgs e)
        {
            OrigMouseDownPoint = new PointF(e.X, e.Y);
            OrigUserOffset = new PointF(userOffset.X, userOffset.Y);
        }

        private void pbLayout_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                //convert the mouse movement relative to its original mouse down position to image space movement
                //add the image space movemnt to the original user offset
                PointF DeltaMouse = new PointF(e.X - OrigMouseDownPoint.X, e.Y - OrigMouseDownPoint.Y);
                PointF DeltaImage = new PointF(DeltaMouse.X / (userZoomFactor * unityScaleFactor), DeltaMouse.Y / (userZoomFactor * unityScaleFactor));
                userOffset = new PointF(OrigUserOffset.X + DeltaImage.X, OrigUserOffset.Y + DeltaImage.Y);

                //redraw the image
                pbLayout.Invalidate();
            }
        }
    }
}
