using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
        BindingList<Material> Materials { get; set; }
        BindingList<StockItem> Stock { get; set; }
        BindingList<Part> Parts { get; set; }

        // grid sort options
        string PartGridSort = "";
        string StockGridSort = "";
        string MaterialGridSort = "";


        #endregion

        #region // Constructor ...
        // constructor for the main form
        public frmMain()
        {
            InitializeComponent();
            tcInputs.Top -= 22;
        }
        #endregion

        #region // Internal helper functions ...
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

            // bind the materials grid
            BindMaterialsGrid();

            // bind the stock grid
            BindStockGrid();

            // bind the Part
            BindPartsGrid();

            IsFileSaved = true;
        }

        private void BindPartsGrid()
        {
            PartMaterialColumn.DataSource = Materials;
            PartMaterialColumn.DisplayMember = "Name";
            PartMaterialColumn.ValueMember = "Name";
            PartsGridView.DataSource = Parts;
        }

        private void BindStockGrid()
        {
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

        #endregion

        #region // Event handlers ...

        private void mniFileExit_Click(object sender, EventArgs e)
        {
            // if the file was closed,
            if (CloseFile())
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
            if (new frmSettingsDialog(Settings.First()).ShowDialog() == DialogResult.OK)
                SaveConfig();
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
                IsFileSaved = false;
        }

        private void StockGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow())
                IsFileSaved = false;
        }

        private void PartsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow())
                IsFileSaved = false;
        }

        private void MaterialsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasUserChangedCell())
                IsFileSaved = false;
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
                IsFileSaved = false;
        }

        private void PartsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasUserChangedCell())
                IsFileSaved = false;
        }

        private void mniDuplicateRows_Click(object sender, EventArgs e)
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
            foreach (DataGridViewRow item in MaterialsGridView.SelectedRows)
            {
                Materials.Remove((Material)item.DataBoundItem);
                IsFileSaved = false;
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
                .Generate(Settings.First(), Materials, Stock, Parts)
                .Save("PartsReport.pdf");
            Process.Start("PartsReport.pdf");
        }

        private void mniReportStockList_Click(object sender, EventArgs e)
        {
            new StockReport()
                .Generate(Settings.First(), Materials, Stock, Parts)
                .Save("StockReport.pdf");
            Process.Start("StockReport.pdf");
        }
        

        private void mniReportLayout_Click(object sender, EventArgs e)
        {
            new LayoutReport()
                .Generate(Settings.First(), Materials, Stock, Parts)
                .Save("LayoutReport.pdf");
            Process.Start("LayoutReport.pdf");
        }
        #endregion
    }
}
