
using CuttingPlanMaker.Packers;
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
    /// Main form of the application
    /// </summary>
    public partial class FrmMain : Form
    {
        //TODO: Add board column on parts grid to see what board the part was placed on
        //TODO: create interface for packers and allow user to pick pick a packer algorithm
        //TODO: create import/export functionality
        //TODO: on points algorithm, see if we can allign parts to improve sawing 
        //TODO: undo function....
        //TODO: splash screen

        #region // Fields & Properties ...

        /// <summary>
        /// Flag to keep track if the current file is saved
        /// </summary>
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

        /// <summary>
        /// The name of the current file
        /// </summary>        
        string FileName
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        /// <summary>
        /// The full path to the current file
        /// </summary>
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

        /// <summary>
        /// Settings collection for the currently loaded project
        /// </summary>
        BindingList<Settings> Settings { get; set; }

        /// <summary>
        /// A setting reference to the first record in the settings collection. This publishes all the settings for the project in a typed property.
        /// </summary>
        Settings Setting
        {
            get { return Settings.FirstOrDefault(); }
        }

        /// <summary>
        /// List of material types that the project is using. The standard board sizes is not used anywhere, except for the thickness and cost, which are used in the reports
        /// </summary>
        BindingList<Material> Materials { get; set; }

        /// <summary>
        /// A list of all the stock items (boards) that are available to place the parts on
        /// </summary>
        BindingList<StockItem> Stock { get; set; }

        /// <summary>
        /// A list of parts which will be placed on the boards
        /// </summary>
        BindingList<Part> Parts { get; set; }

        // grid sort options - keep track of which column the input grids are sorted by, and if it is ASC or DESC
        private string PartGridSort = "";       // The sorting option for the parts grid
        private string StockGridSort = "";      // The sorting option for the stock grid
        private string MaterialGridSort = "";   // The sorting option for the Materials grid

        // a flag to indicate when packing is required (when auto-repacking is disabled)
        private bool isPackingRequired = false;

        // layout painting fields
        float userZoomFactor = 1.0f;            // Zoom factor set by the user - a zoom factor of 1 will fill the picturbox with the image
        PointF userOffset = new PointF(0, 0);   // The offset with which the image has been panned, (0,0) would centre the image in the picturebox control
        Bitmap LayoutBitmap = null;             // Keep a copy of the drawn bitmap, to repaint the control as necesary
        float unityScaleFactor;                 // The scale needed to allow a userzoomfactor of 1 to fill the picturebox with the image
        PointF OrigMouseDownPoint;              // The original mouse location when starting to drag/pan the image
        PointF OrigUserOffset;                  // The original user offset of the image when starting to drag/pan the image

        // layout drawing configuration options
        double xMargin = 50;                    // the x-margin for drawing the layout image
        double yMargin = 50;                    // the y-nargub for drawing the layout image
        double boardSpacing = 70;               // the spacing between boards when drawing the layout image
        #endregion

        #region // Constructor ...
        // constructor for the main form
        public FrmMain()
        {
            InitializeComponent();
            tcInputs.Top -= 22;                 // Hide the standard tab control's tabs by moving it up 

            // subscribe to the picturebox's MouseWheel event - not in the property dialog to bind at design-time
            this.pbLayout.MouseWheel += PbLayout_MouseWheel;
        }
        #endregion

        #region // Internal helper functions ...
        /// <summary>
        /// function to check if the rows removed was due to a user instruction, or system processes
        /// </summary>
        /// <returns></returns>
        private bool HasUserRemovedRow()
        {
            // check if the grid's row removed event was fired due to internal processes or the user removing a row
            return !(System.Environment.StackTrace.Contains(".OnBindingContextChanged(") || System.Environment.StackTrace.Contains(".set_DataSource("));
        }

        /// <summary>
        /// function to check if the change to a cell value was due to a user instruction or system processes
        /// </summary>
        /// <returns></returns>
        private bool HasUserChangedCell()
        {
            // check if the cell was changed due to application processes or the user actually changed the cell value
            return System.Environment.StackTrace.Contains(".CommitEdit(");
        }

        /// <summary>
        /// Make a copy of the selected rows on the current input grid
        /// </summary>
        private void DuplicateGridRows()
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
            PackSolution();
        }

        /// <summary>
        /// Draw the layout of the specified stock-items/boards
        /// </summary>
        /// <param name="boards"></param>
        /// <param name="usedstockonly"></param>
        /// <returns></returns>
        public Bitmap Draw(StockItem[] boards, bool drawunusedstock = true)
        {
            double yOffset = yMargin;
            double imageHeight = 2 * yMargin;
            double imageWidth = 0;
            Font boardFont = new Font(new FontFamily("Microsoft Sans Serif"), 15.0f);

            // create list of boards to draw
            List<StockItem> boardsToDraw = new List<StockItem>(boards);
            if (!drawunusedstock)
                boardsToDraw = boards.Where(t => t.PackedPartsCount > 0).ToList();

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

            // draw algorithm
            Type algType = GetPackerType();
            string algname = algType.GetProperty("AlgorithmName").GetValue(null) as string;
            string algorithmstring = $"Algorithm: {algname}";
            SizeF algsz = g.MeasureString(algorithmstring, boardFont);
            g.DrawString(algorithmstring, boardFont, Brushes.Black, (float)imageWidth - algsz.Width, 0);// algsz.Height);

            // loop through all the boards to be drawn
            yOffset = yMargin;
            foreach (var iBoard in boardsToDraw)
            {
                // draw the board
                g.FillRectangle(Brushes.DarkRed, (float)xMargin, (float)yOffset, (float)iBoard.Length, (float)iBoard.Width);
                string boardheader = $"{iBoard.Name} [{iBoard.Length}x{iBoard.Width}] ({iBoard.PackedPartsTotalArea / iBoard.Area * 100:0.0}%)";
                SizeF textSizeBoard = g.MeasureString(boardheader, boardFont);
                g.DrawString(boardheader, boardFont, Brushes.Black, (float)(xMargin), (float)(yOffset - textSizeBoard.Height));

                // loop through all the parts and draw the ones on the current board
                //string overflowtext = "";
                for (int i = 0; i < iBoard.PackedPartsCount; i++)
                {
                    var iPlacement = iBoard.PackedParts[i];
                    double dLength = iPlacement.dLength;
                    double dWidth = iPlacement.dWidth;
                    double Length = iPlacement.Part.Length + (Setting.IncludePaddingInDisplay ? Setting.PartPaddingLength : 0f);
                    double Width = iPlacement.Part.Width + (Setting.IncludePaddingInDisplay ? Setting.PartPaddingWidth : 0f);

                    // draw the part
                    g.FillRectangle(Brushes.Green, (float)(xMargin + dLength), (float)(yOffset + dWidth), (float)Length, (float)Width);

                    // print the part text
                    string partLabel = $"{iPlacement.Part.Name} [{Length} x {iPlacement.Part.Width}]";

                    int sz = 16;
                    Font partFont;
                    SizeF textSize;
                    do
                    {
                        partFont = new Font(new FontFamily("Microsoft Sans Serif"), sz);
                        textSize = g.MeasureString(partLabel, partFont);
                        if (textSize.Width < iPlacement.Part.Length && textSize.Height < iPlacement.Part.Width)
                        {
                            g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPlacement.Part.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPlacement.Part.Width - 0.5 * textSize.Height));
                            break;
                        }
                        else sz--;

                    } while (sz > 8);
                    if (sz <= 8)
                    {
                        partLabel = $"{iPlacement.Part.Name}";
                        textSize = g.MeasureString(partLabel, partFont);
                        g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPlacement.Part.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPlacement.Part.Width - 0.5 * textSize.Height));

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

        /// <summary>
        /// Sort the parts input grid according to the sort specification
        /// </summary>
        /// <param name="partGridSort"></param>
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

        /// <summary>
        /// Sort the stock items input grid according to the sort specification
        /// </summary>
        /// <param name="sortOption"></param>
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

        /// <summary>
        /// Sort the Materials input grid according to the specification
        /// </summary>
        /// <param name="sortoption"></param>
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

        /// <summary>
        /// Save the current configuration only
        /// </summary>
        private void SaveConfig()
        {
            CSVFile.Write(Settings, $"{FilePath}.Settings.CSV");
        }

        /// <summary>
        /// Save the current data/project as a new file set by specifying the target using a file save dialog
        /// </summary>
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

        /// <summary>
        /// Bind the materials input grid to the materials list
        /// </summary>
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
            if (Setting.AutoRepack) PackSolution();

            // bind the materials grid
            BindMaterialsGrid();

            // bind the stock grid
            BindStockGrid();

            // bind the Part
            BindPartsGrid();

            // ensure the saved flag is set - we just opened the file
            IsFileSaved = true;
        }

        /// <summary>
        /// Bind the parts input grid to the parts list
        /// </summary>
        private void BindPartsGrid()
        {
            PartsGridView.AutoGenerateColumns = false;
            PartMaterialColumn.DataSource = Materials;
            PartMaterialColumn.DisplayMember = "Name";
            PartMaterialColumn.ValueMember = "Name";
            PartsGridView.DataSource = Parts;
        }

        /// <summary>
        /// Bind the Stock input grid to the stock list
        /// </summary>
        private void BindStockGrid()
        {
            StockGridView.AutoGenerateColumns = false;
            StockMaterialColumn.DataSource = Materials;
            StockMaterialColumn.DisplayMember = "Name";
            StockMaterialColumn.ValueMember = "Name";
            StockGridView.DataSource = Stock;
        }

        /// <summary>
        /// Load the defaults for the application by loading the files called Default.*.CSV
        /// </summary>
        private void LoadDefault()
        {
            // start from scratch
            LoadFile("DefaultDataFiles\\Default");
            FilePath = "";
        }

        /// <summary>
        /// Close the current project, but check if it is saved and prompt to save if not - prior to closing the project
        /// </summary>
        /// <returns></returns>
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

        private Type GetPackerType()
        {
            var type = typeof(PackerBase);
            var packertypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)).OrderBy(o => o.FullName);
            var selectedpackertype = packertypes.FirstOrDefault(q => q.GetProperty("AlgorithmName")?.GetValue(null) as string == Setting.Algorithm);
            if (selectedpackertype == null) selectedpackertype = packertypes.First();
            return selectedpackertype;

        }
        /// <summary>
        /// Place the parts on the available stock and refresh the display
        /// </summary>
        private void PackSolution()
        {
            try
            {
                //clear all packing info
                Parts.ToList().ForEach(t => t.IsPacked = false);
                Stock.ToList().ForEach(t =>
                {
                    t.IsComplete = false;
                    t.PackedParts = null;
                    t.PackedPartsCount = 0;
                });

                PackerBase packer = (PackerBase)Activator.CreateInstance(GetPackerType());

                // filter the parts and stock for te current material and pack them
                Materials.Select(t => t.Name).Where(q => q != "DISABLED").AsParallel().ForAll(iMaterialsName =>
                      {
                          Part[] iParts = Parts.Where(t => t.Material == iMaterialsName).ToArray();
                          StockItem[] iStock = Stock.Where(t => t.Material == iMaterialsName).ToArray();

                          packer.Pack(iParts
                              , iStock
                              , Setting.BladeKerf
                              , Setting.PartPaddingLength
                              , Setting.PartPaddingWidth);
                      });

            }
            catch (Exception ex)
            {
            }

            // clear the packing required flag
            isPackingRequired = false;

            // repaint the layout picture box
            pbLayout.Invalidate();
        }

        /// <summary>
        /// Populate the tabs for the materials tab control with all the currently registerred material types
        /// </summary>
        private void PopulateMaterialTabs()
        {
            // add any materials not already listed in the tab pages
            for (int i = 0; i < Materials.Count; i++)
            {
                Material iMaterial = Materials[i];
                if (iMaterial.Name != "DISABLED")
                    if (!tcMaterials.TabPages.ContainsKey(iMaterial.Name))
                        tcMaterials.TabPages.Add(iMaterial.Name, iMaterial.Name);
            }

            // remove any materials that are listed on the tab pages, that no longer exist in the list of materials
            for (int i = tcMaterials.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage iTab = tcMaterials.TabPages[i];
                if (Materials.FirstOrDefault(t => t.Name == iTab.Name) == null)
                    tcMaterials.TabPages.Remove(iTab);
            }

            // repaint the layout picture box in case the selected page changed
            pbLayout.Invalidate();
        }
        #endregion

        #region // Event handlers ...

        private void onGridDataChangeByUser(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (sender == MaterialsGridView)
                PopulateMaterialTabs();

            IsFileSaved = false;

            if (Setting.AutoRepack)
            {
                PackSolution();
            }
            else
            {
                isPackingRequired = true;
                pbLayout.Invalidate();
            }
        }

        private void onGridDataChangeByUser(object sender, DataGridViewCellEventArgs e)
        {
            if (sender == MaterialsGridView)
            {
                // if a material's name was changed
                if (e.ColumnIndex == 0 && oldMaterialName!="")
                {
                    Parts.Where(q => q.Material == oldMaterialName).AsParallel().ForAll(p => p.Material = newMaterialName);
                    Stock.Where(q => q.Material == oldMaterialName).AsParallel().ForAll(p => p.Material = newMaterialName);
                }

                PopulateMaterialTabs();
            }
            // if a stock item's material was changed
            if (sender == StockGridView && e.ColumnIndex == 3)
            {
                //retrieve the selected stock
                var selectedStockItem = (StockItem)StockGridView.Rows[e.RowIndex].DataBoundItem;
                if (selectedStockItem.PackedPartsCount > 0)
                    // ask if all parts should be changed
                    if (MessageBox.Show("Move packed parts to the new material too?", "Move parts", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //move the parts packed on the board
                        selectedStockItem.PackedParts.ToList().ForEach(p => p.Part.Material = selectedStockItem.Material);
                    }
            }

            IsFileSaved = false;

            if (Setting.AutoRepack)
            {
                PackSolution();
            }
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
                if (Setting.AutoRepack)
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

        private void MaterialsGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // If the user removed the row - set the file saved flag
            if (HasUserRemovedRow() && MaterialsGridView.SelectedRows.Count == 0)
                onGridDataChangeByUser(sender, e);
        }

        private void StockGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow() && StockGridView.SelectedRows.Count == 0)
                onGridDataChangeByUser(sender, e);
        }

        private void PartsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // the user changed the file data - set the file saved flag
            if (HasUserRemovedRow() && PartsGridView.SelectedRows.Count == 0)
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
            DuplicateGridRows();
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
            pbLayout.Invalidate();
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
                MaterialsGridView.SuspendLayout();
                foreach (DataGridViewRow item in MaterialsGridView.SelectedRows)
                {
                    Materials.Remove((Material)item.DataBoundItem);
                    IsFileSaved = false;
                }
                MaterialsGridView.ResumeLayout();
            }
            if (tcInputs.SelectedTab == tpParts)
            {
                PartsGridView.SuspendLayout();
                foreach (DataGridViewRow item in PartsGridView.SelectedRows)
                {
                    Parts.Remove((Part)item.DataBoundItem);
                    IsFileSaved = false;
                }
                PartsGridView.ResumeLayout();
            }
            if (tcInputs.SelectedTab == tpStock)
            {
                StockGridView.SuspendLayout();
                foreach (DataGridViewRow item in StockGridView.SelectedRows)
                {
                    Stock.Remove((StockItem)item.DataBoundItem);
                    IsFileSaved = false;
                }
                StockGridView.ResumeLayout();
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
            var t = new PartListReport()
                .Generate(Setting, Materials, Stock, Parts);

            string filename = "";
            for (int c = 0; c < 1000; c++)
                try
                {
                    filename = $"PartsReport{(c == 0 ? "" : c.ToString())}.pdf";
                    t.Save(filename);
                    break;
                }
                catch
                {
                }
            if (File.Exists(filename))
                Process.Start(filename);
        }

        private void mniReportStockList_Click(object sender, EventArgs e)
        {
            var t = new StockReport()
                .Generate(Setting, Materials, Stock, Parts);

            string filename = "";
            for (int c = 0; c < 1000; c++)
                try
                {
                    filename = $"StockReport{(c == 0 ? "" : c.ToString())}.pdf";
                    t.Save(filename);
                    break;
                }
                catch
                {
                }
            if (File.Exists(filename))
                Process.Start(filename);
        }

        private void mniReportLayout_Click(object sender, EventArgs e)
        {
            if (Setting.AutoRepack)
                PackSolution();
            else
            {
                if (isPackingRequired)
                {
                    MessageBox.Show("Parts have not been packed after changes were made. Please repack first");
                    return;
                }
            }

            var t = new LayoutReport()
                .Generate(Setting, Materials, Stock, Parts);

            string filename = "";
            for (int c = 0; c < 1000; c++)
                try
                {
                    filename = $"LayoutReport{(c == 0 ? "" : c.ToString())}.pdf";
                    t.Save(filename);
                    break;
                }
                catch
                {
                }
            if (File.Exists(filename))
                Process.Start(filename);
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
            if (e.RowIndex < Parts.Count && Parts[e.RowIndex].Area > 0 && !Parts[e.RowIndex].IsPacked)
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            else
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }

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
               -(Y - pbSize.Height * 0.5f) / (fillScale * userZoom) + imgCentre.Y,
               width / (fillScale * userZoom),
               height / (fillScale * userZoom)
               );
        }

        private void PbLayout_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
                userZoomFactor = userZoomFactor * 1.2f;
            else if (e.Delta < 0 && userZoomFactor >= 1)
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
            LayoutBitmap = Draw(stockItems, Setting.DrawUnusedStock);

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
            int partscount = Parts.Count(q => q.Material == SelectedMaterial);
            lblPartsCount.Text = partscount.ToString();
            lblPartsArea.Text = (Parts.Where(q => q.Material == SelectedMaterial).Sum(t => t.Area) / 1e6f).ToString("0.000");
            int placedpartcount = Parts.Count(t => t.Material == SelectedMaterial && t.IsPacked);
            lblUsedPartsCount.Text = placedpartcount.ToString();
            double UsedPartsArea = (Parts.Where(q => q.Material == SelectedMaterial && q.IsPacked).Sum(t => t.Area) / 1e6f);
            lblUsedPartsArea.Text = UsedPartsArea.ToString("0.000");
            if (placedpartcount < partscount)
            {
                lblUsedPartsCount.BackColor = Color.DarkRed;
                lblUsedPartsCount.ForeColor = Color.White;
            }
            else
            {
                lblUsedPartsCount.BackColor = SystemColors.Control;
                lblUsedPartsCount.ForeColor = Color.Black;
            }
            lblWastePerc.Text = ((UsedStockArea - UsedPartsArea) / UsedStockArea * 100.0).ToString("00.0");
            lblWasteArea.Text = (UsedStockArea - UsedPartsArea).ToString("0.000");
        }

        private void pbLayout_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                userOffset = PbToImageSpace(e.X, e.Y, 0, 0, pbLayout.Size, userOffset, userZoomFactor, unityScaleFactor).Location;
                if (userOffset.X < -LayoutBitmap.Width / 2) userOffset.X = -LayoutBitmap.Width / 2;
                if (userOffset.X > LayoutBitmap.Width / 2) userOffset.X = LayoutBitmap.Width / 2;
                if (userOffset.Y < -LayoutBitmap.Height / 2) userOffset.Y = -LayoutBitmap.Height / 2;
                if (userOffset.Y > LayoutBitmap.Height / 2) userOffset.Y = LayoutBitmap.Height / 2;
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
            if (e.Button == MouseButtons.Left)
            {
                //convert the mouse movement relative to its original mouse down position to image space movement
                //add the image space movemnt to the original user offset
                PointF DeltaMouse = new PointF(e.X - OrigMouseDownPoint.X, e.Y - OrigMouseDownPoint.Y);
                PointF DeltaImage = new PointF(DeltaMouse.X / (userZoomFactor * unityScaleFactor), DeltaMouse.Y / (userZoomFactor * unityScaleFactor));
                userOffset = new PointF(OrigUserOffset.X + DeltaImage.X, OrigUserOffset.Y + DeltaImage.Y);
                if (userOffset.X < -LayoutBitmap.Width / 2) userOffset.X = -LayoutBitmap.Width / 2;
                if (userOffset.X > LayoutBitmap.Width / 2) userOffset.X = LayoutBitmap.Width / 2;
                if (userOffset.Y < -LayoutBitmap.Height / 2) userOffset.Y = -LayoutBitmap.Height / 2;
                if (userOffset.Y > LayoutBitmap.Height / 2) userOffset.Y = LayoutBitmap.Height / 2;
                //redraw the image
                pbLayout.Invalidate();
            }
        }

        private void mniReportLayoutLabels_Click(object sender, EventArgs e)
        {
            var t = new CuttingLabelReport()
               .Generate(Setting, Materials, Stock, Parts);

            string filename = "";
            for (int c = 0; c < 1000; c++)
                try
                {
                    filename = $"CuttingLabelsReport{(c == 0 ? "" : c.ToString())}.pdf";
                    t.Save(filename);
                    break;
                }
                catch
                {
                }
            if (File.Exists(filename))
                Process.Start(filename);

        }

        private void mniCentreItem_Click(object sender, EventArgs e)
        {
            if (tcInputs.SelectedTab == tpParts && PartsGridView.SelectedCells.Count == 1)
            {
                // retrieve the selected part
                Part p = (Part)PartsGridView.SelectedCells[0].OwningRow.DataBoundItem;

                // find the board's offset that contains the part
                double yOffset = yMargin;
                StockItem si = null;
                bool found = false;
                for (int i = 0; i < Stock.Count; i++)
                {
                    si = Stock[i];
                    if (si.PackedParts != null && si.PackedParts.Any(t => t?.Part == p))
                    {
                        found = true;
                        break;
                    }
                    if (si.PackedPartsCount > 0 || Setting.DrawUnusedStock)
                        yOffset += si.Width + boardSpacing;
                }

                if (!found)
                {
                    MessageBox.Show("Part not placed on any board.");
                    return;
                }
                int partindex = 0;
                while (si.PackedParts[partindex].Part != p) partindex++;
                Placement iPlacement = si.PackedParts[partindex];
                double dLength = iPlacement.dLength;
                double dWidth = iPlacement.dWidth;
                double cx = xMargin + dLength + p.Length / 2;
                double cy = yOffset + dWidth + p.Width / 2;
                double dx = LayoutBitmap.Width / 2 - cx;
                double dy = LayoutBitmap.Height / 2 - cy;

                userOffset = new PointF((float)(dx), (float)(dy));
                pbLayout.Invalidate();
            }
        }

        private void mniSaveCopyAs_Click(object sender, EventArgs e)
        {
            // store old file path
            string currentfilepath = FilePath;

            // let the user pick a new filename to save the copy as
            SaveFileAs();

            // restore old path
            FilePath = currentfilepath;
        }

        private void mnuGridContextMenu_Opened(object sender, EventArgs e)
        {
            mniCentreItem.Enabled = (tcInputs.SelectedTab == tpParts);
            mniIsolateMaterial.Enabled = (tcInputs.SelectedTab == tpStock);
        }

        private void mniIsolateMaterial_Click(object sender, EventArgs e)
        {
            if (StockGridView.SelectedCells == null || StockGridView.SelectedCells.Count == 0) return;

            //retrieve the selected stock
            var selectedStockItem = (StockItem)StockGridView.SelectedCells[0].OwningRow.DataBoundItem;

            //create a new material
            Material oldMaterial = Materials.FirstOrDefault(q => q.Name == selectedStockItem.Material);

            string newmaterialname = $"{selectedStockItem.Material}[{selectedStockItem.Name}]";
            Materials.Add(new Material()
            {
                Name = newmaterialname,
                Cost = oldMaterial?.Cost ?? 0,
                Length = selectedStockItem.Length,
                Width = selectedStockItem.Width,
                Thickness = oldMaterial?.Thickness ?? 25
            });

            //move the selected stock to the new material
            selectedStockItem.Material = newmaterialname;

            //move all packed parts to the new material
            if (selectedStockItem.PackedParts != null)
                selectedStockItem.PackedParts.ToList().ForEach(p => p.Part.Material = newmaterialname);

            StockGridView.Invalidate();

            PopulateMaterialTabs();

            // repack
            PackSolution();
        }

        string oldMaterialName = "";
        string newMaterialName = "";
        private void MaterialsGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!MaterialsGridView.Rows[e.RowIndex].IsNewRow)
            {
                // if the material's name is edited
                if (e.ColumnIndex == 0)
                {
                    oldMaterialName = MaterialsGridView[e.ColumnIndex, e.RowIndex].Value as string;
                    newMaterialName = e.FormattedValue as string;
                }
            }
            else
                oldMaterialName = "";
        }

        private void StockGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < Stock.Count && Stock[e.RowIndex].PackedPartsCount == 0)
                StockGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
            else
                StockGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }
        #endregion
    }
}
