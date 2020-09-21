
using CuttingPlanMaker.Packers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        bool ManualMode = false;

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
        Settings Settings { get; set; }

        /// <summary>
        /// List of material types that the project is using. The standard board sizes is not used anywhere, except for the thickness and cost, which are used in the reports
        /// </summary>
        BindingList<Material> Materials { get; set; }

        /// <summary>
        /// A list of all the stock items (boards) that are available to place the parts on
        /// </summary>
        BindingList<Board> Stock { get; set; }

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
                    Board oldItem = (Board)item.DataBoundItem;
                    Board newItem = new Board()
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
        public Bitmap Draw(Board[] boards, Part[] parts, bool drawunusedstock = true)
        {
            double yOffset = yMargin;
            double imageHeight = 2 * yMargin;
            double imageWidth = 0;
            Font boardFont = new Font(new FontFamily("Microsoft Sans Serif"), 15.0f);

            // create list of boards to draw
            List<Board> boardsToDraw = new List<Board>(boards);
            if (!drawunusedstock)
                boardsToDraw = parts.Where(t => t.Source != null).Select(p => p.Source).Distinct().OrderBy(o => o.Name).ToList();


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


            if (boardsToDraw.Count() == 0)
            {
                g.Flush();
                return bitmap;
            }

            // draw algorithm
            //Type algType = GetPackerType();
            //string algname = algType.GetProperty("AlgorithmName").GetValue(null) as string;
            //string algorithmstring = $"Algorithm: {algname}";
            //SizeF algsz = g.MeasureString(algorithmstring, boardFont);
            //g.DrawString(algorithmstring, boardFont, Brushes.Black, (float)imageWidth - algsz.Width, 0);// algsz.Height);

            // loop through all the boards to be drawn
            yOffset = yMargin;
            foreach (var iBoard in boardsToDraw)
            {
                var packedParts = parts.Where(p => p.Source?.Name == iBoard.Name);


                // draw the board
                g.FillRectangle(Brushes.DarkRed, (float)xMargin, (float)yOffset, (float)iBoard.Length, (float)iBoard.Width);
                var TotalPartsArea = packedParts.Sum(p => p.Area);
                string boardheader = $"{iBoard.Name} [{iBoard.Length}x{iBoard.Width}] ({TotalPartsArea / iBoard.Area * 100:0.0}%)";
                SizeF textSizeBoard = g.MeasureString(boardheader, boardFont);
                g.DrawString(boardheader, boardFont, Brushes.Black, (float)(xMargin), (float)(yOffset - textSizeBoard.Height));

                // loop through all the parts and draw the ones on the current board
                //string overflowtext = "";
                foreach (var iPart in packedParts)
                {
                    double dLength = iPart.OffsetLength;
                    double dWidth = iPart.OffsetWidth;
                    double Length = iPart.Length;
                    double Width = iPart.Width;

                    // draw the part
                    g.FillRectangle(Brushes.Green, (float)(xMargin + dLength), (float)(yOffset + dWidth), (float)Length, (float)Width);

                    // print the part text
                    string partLabel = $"{iPart.Name} [{Length} x {iPart.Width}]";

                    int sz = 16;
                    Font partFont;
                    SizeF textSize;
                    do
                    {
                        partFont = new Font(new FontFamily("Microsoft Sans Serif"), sz);
                        textSize = g.MeasureString(partLabel, partFont);
                        if (textSize.Width < iPart.Length && textSize.Height < iPart.Width)
                        {
                            g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPart.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPart.Width - 0.5 * textSize.Height));
                            break;
                        }
                        else sz--;

                    } while (sz > 8);
                    if (sz <= 8)
                    {
                        partLabel = $"{iPart.Name}";
                        textSize = g.MeasureString(partLabel, partFont);
                        g.DrawString(partLabel, partFont, Brushes.White, (float)(xMargin + dLength + 0.5 * iPart.Length - 0.5 * textSize.Width), (float)(yOffset + dWidth + 0.5 * iPart.Width - 0.5 * textSize.Height));

                    }
                }
                if (DropPreviewTarget == iBoard)
                {
                    // draw preview drop rect if not empty
                    if (DropPreviewRect != RectangleF.Empty)
                    {
                        g.FillRectangle(Brushes.LightGreen, (float)(DropPreviewRect.X + xMargin),(float)(DropPreviewRect.Y + yOffset),DropPreviewRect.Width,DropPreviewRect.Height);
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
                case "Source ASC":
                    Parts = new BindingList<Part>(Parts.OrderBy(t => t.Source.Name).ToList());
                    break;
                case "Source DESC":
                    Parts = new BindingList<Part>(Parts.OrderByDescending(t => t.Source.Name).ToList());
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
                    Stock = new BindingList<Board>(Stock.OrderBy(t => t.Name).ToList());
                    break;
                case "Name DESC":
                    Stock = new BindingList<Board>(Stock.OrderByDescending(t => t.Name).ToList());
                    break;
                case "Length ASC":
                    Stock = new BindingList<Board>(Stock.OrderBy(t => t.Length).ToList());
                    break;
                case "Length DESC":
                    Stock = new BindingList<Board>(Stock.OrderByDescending(t => t.Length).ToList());
                    break;
                case "Width ASC":
                    Stock = new BindingList<Board>(Stock.OrderBy(t => t.Width).ToList());
                    break;
                case "Width DESC":
                    Stock = new BindingList<Board>(Stock.OrderByDescending(t => t.Width).ToList());
                    break;
                case "Material ASC":
                    Stock = new BindingList<Board>(Stock.OrderBy(t => t.Material).ToList());
                    break;
                case "Thickness DESC":
                    Stock = new BindingList<Board>(Stock.OrderByDescending(t => t.Material).ToList());
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
            File.WriteAllText($"{FilePath}.Materials.json", JsonConvert.SerializeObject(Materials));
            File.WriteAllText($"{FilePath}.Stock.json", JsonConvert.SerializeObject(Stock));
            File.WriteAllText($"{FilePath}.Parts.json", JsonConvert.SerializeObject(Parts));
            File.WriteAllText($"{FilePath}.Settings.json", JsonConvert.SerializeObject(Settings));
            // update the saved flag
            IsFileSaved = true;
        }

        /// <summary>
        /// Save the current configuration only
        /// </summary>
        private void SaveConfig()
        {
            File.WriteAllText($"{FilePath}.Settings.json", JsonConvert.SerializeObject(Settings));
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

        private string[] GetAlgorthmsList()
        {
            var type = typeof(PackerBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            var algnames = types.Select(s => s.GetProperty("AlgorithmName").GetValue(null) as string);

            return algnames.ToArray();
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
            Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{FilePath}.Settings.json"));
            Materials = new BindingList<Material>(JsonConvert.DeserializeObject<List<Material>>(File.ReadAllText($"{FilePath}.Materials.json")));
            Stock = new BindingList<Board>(JsonConvert.DeserializeObject<List<Board>>(File.ReadAllText($"{FilePath}.Stock.json")));
            //CSVFile.Read<StockItem>($"{FilePath}.Stock.CSV");
            //Parts = CSVFile.Read<Part>($"{FilePath}.Parts.CSV");
            Parts = new BindingList<Part>(JsonConvert.DeserializeObject<List<Part>>(File.ReadAllText($"{FilePath}.Parts.json")));

            //consolidate the sources of pre-placed parts of the same source
            Parts.Where(p => p.Source != null).ToList().
                ForEach(i => i.Source = Stock.FirstOrDefault(s => s.Name == i.Source.Name));


            //Parts.ToList().ForEach(p => {
            //    if (p.SourceName != null)
            //        p.Source = Stock.FirstOrDefault(s => s.Name == p.SourceName);
            //    });

            Type algType = GetPackerType();
            string algname = algType.GetProperty("AlgorithmName").GetValue(null) as string;
            foreach (ToolStripMenuItem item in mniAlgorithm.DropDownItems)
                item.Checked = (item.Text == algname);


            // update the tabs for the layout drawings
            PopulateMaterialTabs();

            // pack the solution before drawing the grids
            if (Settings.AutoRepack) PackSolution();

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
            var selectedpackertype = packertypes.FirstOrDefault(q => q.GetProperty("AlgorithmName")?.GetValue(null) as string == Settings.Algorithm);
            if (selectedpackertype == null) selectedpackertype = packertypes.First();
            return selectedpackertype;

        }
        /// <summary>
        /// Place the parts on the available stock and refresh the display
        /// </summary>
        private void PackSolution(bool ignoreManual = false)
        {
            if (!ignoreManual && ManualMode) return;
            try
            {
                //clear all packing info
                Parts.ToList().ForEach(t =>
                {
                    //if part not packed onto a frozen board
                    if (!t.IsFrozen)
                    {
                        t.Source = null;
                        //   t.OffsetLength = 0;
                        //   t.OffsetWidth = 0;
                    }
                });
                Stock.ToList().ForEach(t =>
                {
                    if (!t.IsFrozen)
                        t.IsComplete = false;
                });

                PackerBase packer = (PackerBase)Activator.CreateInstance(GetPackerType());

                // filter the parts and stock for te current material and pack them
                Materials.Select(t => t.Name).Where(q => q != "DISABLED").AsParallel().ForAll((Action<string>)(iMaterialsName =>
                      {
                          Part[] iParts = Parts.Where(t => t.Material == iMaterialsName && !t.IsFrozen).ToArray();
                          Board[] iStock = Stock.Where(t => t.Material == iMaterialsName && !t.IsFrozen).ToArray();

                          packer.Pack(iParts
                              , iStock
                              , this.Settings.BladeKerf);
                      }));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"EXCEPTION WHILE PACKING:{ex}");
            }

            // clear the packing required flag
            isPackingRequired = false;

            // repaint the layout picture box
            pbLayout.Invalidate();
            StockGridView.Invalidate();
            PartsGridView.Invalidate();
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

        #region // Point/Rect space conversion functions ...

        /// <summary>
        /// convert an offset-rectangle (centered at X,Y, with width,hight) from image space to control space
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pbSize"></param>
        /// <param name="CurrentOffset"></param>
        /// <param name="CurrentZoomFactor"></param>
        /// <param name="fillScale"></param>
        /// <returns></returns>
        private RectangleF OffsetBitmapRectToControl(float X, float Y, float width, float height, SizeF pbSize, PointF CurrentOffset, float CurrentZoomFactor, float fillScale)
        {
            return new RectangleF(
                pbSize.Width * 0.5f + fillScale * CurrentZoomFactor * (X - CurrentOffset.X),
                pbSize.Height * 0.5f + fillScale * CurrentZoomFactor * (Y - CurrentOffset.Y),
                width * fillScale * CurrentZoomFactor,
                height * fillScale * CurrentZoomFactor
                );
        }

        /// <summary>
        /// convert a control-space point to offset bitmap space
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="pbSize"></param>
        /// <param name="CurrentOffset"></param>
        /// <param name="CurrentZoomFactor"></param>
        /// <param name="fillScale"></param>
        /// <returns></returns>
        private PointF ControlPointToOffsetBitmap(float X, float Y, SizeF pbSize, PointF CurrentOffset, float CurrentZoomFactor, float fillScale)
        {
            return new PointF(
               CurrentOffset.X - (X - pbSize.Width * 0.5f) / (fillScale * CurrentZoomFactor),
               CurrentOffset.Y - (Y - pbSize.Height * 0.5f) / (fillScale * CurrentZoomFactor)
               );
        }

        private PointF ControlPointToAbsBitmap(float X, float Y, SizeF pbSize, PointF CurrentOffset, float CurrentZoomFactor)
        {
            // convert the click coordinates from screen-space to bitmap space
            return new PointF(
               0.5f * (float)LayoutBitmap.Width - (CurrentOffset.X - (X - pbSize.Width * 0.5f) / (unityScaleFactor * CurrentZoomFactor)),
               0.5f * (float)LayoutBitmap.Height - (CurrentOffset.Y - (Y - pbSize.Height * 0.5f) / (unityScaleFactor * CurrentZoomFactor))
               );
        }

        private Board FindBoardAtPoint(PointF point, out double boardYOffset)
        {
            // identify the boards that are drawn
            string SelectedMaterial = tcMaterials.SelectedTab.Name;
            Board clickedBoard = null;
            List<Board> DrawnBoards = null;
            if (Settings.DrawUnusedStock)
                DrawnBoards = Stock.Where(s => s.Material == SelectedMaterial).ToList();
            else
                DrawnBoards = Parts.Where(p => p.Material == SelectedMaterial && p.Source != null).Select(i => i.Source).Distinct().OrderBy(o => o.Name).ToList();

            // loop through drawn boards to identify the one at the point
            boardYOffset = yMargin;
            foreach (var iboard in DrawnBoards)
            {
                if (point.Y > boardYOffset && point.Y < boardYOffset + iboard.Width && point.X > xMargin && point.X < xMargin + iboard.Length)
                {
                    clickedBoard = iboard;
                    break;
                }
                boardYOffset = boardYOffset + iboard.Width + boardSpacing;
            }

            return clickedBoard;
        }

        /// <summary>
        /// Find part at abs bitmap pos
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private Part FindPartAtPoint(PointF point)
        {
            // identify the boards that are drawn
            string SelectedMaterial = tcMaterials.SelectedTab.Name;
            Board clickedBoard = null;
            List<Board> DrawnBoards = null;
            if (Settings.DrawUnusedStock)
                DrawnBoards = Stock.Where(s => s.Material == SelectedMaterial).ToList();
            else
                DrawnBoards = Parts.Where(p => p.Material == SelectedMaterial && p.Source != null).Select(i => i.Source).Distinct().OrderBy(o => o.Name).ToList();

            // loop through drawn boards to identify the one at the point
            double yOffset = yMargin;
            foreach (var iboard in DrawnBoards)
            {
                if (point.Y > yOffset && point.Y < yOffset + iboard.Width && point.X > xMargin && point.X < xMargin + iboard.Length)
                {
                    clickedBoard = iboard;
                    break;
                }
                yOffset = yOffset + iboard.Width + boardSpacing;
            }

            if (clickedBoard == null) return null;

            PointF relativeClickedPoint = new PointF(point.X - (float)xMargin, point.Y - (float)yOffset);

            Part clickedPart = Parts.FirstOrDefault(f => f.Source == clickedBoard
                && f.Contains(relativeClickedPoint));

            return clickedPart;
        }

        /// <summary>
        /// Find part at bitmap position relative to board origin
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private Part FindPartAtRelPoint(PointF point, Board board)
        {
            Part clickedPart = Parts.FirstOrDefault(f => f.Source == board
                && f.Contains(point));

            return clickedPart;
        }
        #endregion

        #region // Event handlers ...

        private void onGridDataChangeByUser(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (sender == MaterialsGridView)
                PopulateMaterialTabs();

            IsFileSaved = false;

            if (Settings.AutoRepack)
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
                if (e.ColumnIndex == 0 && oldMaterialName != "")
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
                var selectedStockItem = (Board)StockGridView.Rows[e.RowIndex].DataBoundItem;
                if (Parts.Count(p => p.Source == selectedStockItem) > 0)
                    // ask if all parts should be changed
                    if (MessageBox.Show("Move packed parts to the new material too?", "Move parts", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //move the parts packed on the board
                        Parts.Where(p => p.Source == selectedStockItem).ToList().ForEach(p => p.Material = selectedStockItem.Material);
                    }
            }

            IsFileSaved = false;

            if (Settings.AutoRepack)
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
            // load algorithm list
            mniAlgorithm.DropDownItems.AddRange(
                GetAlgorthmsList().Where(t => t != "BASE").Select(x =>
                {
                    var t = new ToolStripMenuItem(x) { CheckOnClick = true };
                    t.Click += mniAlgorithm_Click;
                    return t;
                }).ToArray());

            // load the default file as the default
            LoadDefault();
        }

        private void mniAlgorithm_Click(object sender, EventArgs e)
        {
            // clear all the other checks
            foreach (var item in mniAlgorithm.DropDownItems)
            {
                if (item != sender)
                    ((ToolStripMenuItem)item).Checked = false;
            }

            Settings.Algorithm = ((ToolStripMenuItem)sender).Text;
            PackSolution();

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
            if (new frmSettingsDialog(Settings).ShowDialog() == DialogResult.OK)
            {
                if (FilePath != "") SaveConfig();
                if (Settings.AutoRepack)
                {
                    PackSolution();
                    pbLayout.Invalidate();
                }
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
                    Stock.Remove((Board)item.DataBoundItem);
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
                .Generate(Settings, Materials, Stock, Parts);

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
                .Generate(Settings, Materials, Stock, Parts);

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
            if (Settings.AutoRepack)
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
                .Generate(Settings, Materials, Stock, Parts);

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
            PackSolution(true);
        }

        private void tcMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            userOffset = new PointF(0, 0);
            userZoomFactor = 1;
            pbLayout.Invalidate();
        }

        private void PartsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= Parts.Count) return;

            var t = Parts[e.RowIndex];

            if (t.IsFrozen)
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DeepSkyBlue;
            else if (t.Source == null)
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            else
                PartsGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }

        private void PbLayout_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //old M[bitmap]
            PointF Mbitmap = ControlPointToOffsetBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor, unityScaleFactor);

            if (e.Delta > 0)
                userZoomFactor = userZoomFactor * 1.2f;
            else if (e.Delta < 0 && userZoomFactor >= 1)
                userZoomFactor = userZoomFactor / 1.2f;

            //calculate new center so as to keep the mouse point stationary
            PointF newCentre = new PointF(
                Mbitmap.X + ((float)e.X - (float)pbLayout.Width/2.0f) / (userZoomFactor * unityScaleFactor),
                Mbitmap.Y + ((float)e.Y - (float)pbLayout.Height/2.0f) / (userZoomFactor * unityScaleFactor)
                );
            userOffset = newCentre;

            pbLayout.Invalidate();
        }

        private void pbLayout_Paint(object sender, PaintEventArgs e)
        {
            // filter stock for chosen material
            string SelectedMaterial = tcMaterials.SelectedTab.Name;
            Board[] stockItems = Stock.Where(t => t.Material == SelectedMaterial).ToArray();
            Part[] parts = Parts.Where(p => p.Material == SelectedMaterial).ToArray();

            // draw the layout for filterred stock
            if (LayoutBitmap != null) LayoutBitmap.Dispose();
            LayoutBitmap = Draw(stockItems, parts, Settings.DrawUnusedStock);

            // draw the image to the screen
            Graphics gfx = e.Graphics;

            // scale factor for filling the screen - will be used with user-scale factor so that when user scale factor =1, the image would fit the picturebox
            unityScaleFactor = Math.Min((float)e.ClipRectangle.Width / LayoutBitmap.Width, (float)e.ClipRectangle.Height / LayoutBitmap.Height);
            PointF imgCentre = new PointF()
            {
                X = LayoutBitmap.Width * 0.5f - userOffset.X,
                Y = LayoutBitmap.Height * 0.5f - userOffset.Y
            };

            RectangleF plotRect = OffsetBitmapRectToControl(0, 0, LayoutBitmap.Width, LayoutBitmap.Height, pbLayout.Size, imgCentre, userZoomFactor, unityScaleFactor);
            gfx.DrawImage(LayoutBitmap, plotRect);
            gfx.Flush();


            //update summary table
            lblStockCount.Text = Stock.Count(q => q.Material == SelectedMaterial).ToString();
            double StockArea = Stock.Where(q => q.Material == SelectedMaterial).Sum(t => t.Area) / 1e6;
            lblStockArea.Text = StockArea.ToString("0.000");
            lblUsedStockCount.Text = Parts.Where(t => t.Material == SelectedMaterial && t.Source != null).Select(p => p.Source).Distinct().Count().ToString();
            double UsedStockArea = Parts.Where(t => t.Material == SelectedMaterial && t.Source != null).Select(p => p.Source).Distinct().Sum(s => s.Area) / 1e6f;
            lblUsedStockArea.Text = UsedStockArea.ToString("0.000");
            int partscount = Parts.Count(q => q.Material == SelectedMaterial);
            lblPartsCount.Text = partscount.ToString();
            lblPartsArea.Text = (Parts.Where(q => q.Material == SelectedMaterial).Sum(t => t.Area) / 1e6f).ToString("0.000");
            int placedpartcount = Parts.Count(t => t.Material == SelectedMaterial && t.Source != null);
            lblUsedPartsCount.Text = placedpartcount.ToString();
            double UsedPartsArea = (Parts.Where(q => q.Material == SelectedMaterial && q.Source != null).Sum(t => t.Area) / 1e6f);
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
            if (DateTime.Now.Subtract(FormResizedAt).TotalMilliseconds < 500) return;
            if (e.Button == MouseButtons.Middle)
            {
                userOffset = ControlPointToOffsetBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor, unityScaleFactor);
                if (userOffset.X < -LayoutBitmap.Width / 2) userOffset.X = -LayoutBitmap.Width / 2;
                if (userOffset.X > LayoutBitmap.Width / 2) userOffset.X = LayoutBitmap.Width / 2;
                if (userOffset.Y < -LayoutBitmap.Height / 2) userOffset.Y = -LayoutBitmap.Height / 2;
                if (userOffset.Y > LayoutBitmap.Height / 2) userOffset.Y = LayoutBitmap.Height / 2;

                userZoomFactor = Math.Max(userZoomFactor ,(float)pbLayout.Width / (float)selectedPart.Length / unityScaleFactor / (float)Math.Pow(1.2f,3));

                pbLayout.Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
                userOffset = new PointF(0, 0);
                userZoomFactor = 1;
                pbLayout.Invalidate();
            }
        }


        Part selectedPart = null;
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        private Cursor CreateCursor(int width, int height, int X, int Y)
        {
            //Copy the control in a bitmap
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            Brush brush = new SolidBrush(Color.FromArgb(200, Color.Green));
            g.FillRectangle(brush, new Rectangle(0, 0, width, height));

            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = X;
            tmp.yHotspot = Y;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);

        }


        private void pbLayout_MouseDown(object sender, MouseEventArgs e)
        {
            OrigMouseDownPoint = new PointF(e.X, e.Y);
            OrigUserOffset = new PointF(userOffset.X, userOffset.Y);

            //convert mouse point to abs bmp location
            PointF bmpPoint = ControlPointToAbsBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor);
            selectedPart = FindPartAtPoint(bmpPoint);
            if (e.Button == MouseButtons.Left && ManualMode)
            {
                if (selectedPart == null) return;

                int dX = (int)(selectedPart.Length * userZoomFactor * unityScaleFactor);
                int dY = (int)(selectedPart.Width * userZoomFactor * unityScaleFactor);

                pbLayout.Cursor = CreateCursor(dX, dY, 0, 0);
            }
        }

        RectangleF DropPreviewRect = RectangleF.Empty;
        Board DropPreviewTarget = null;

        private void pbLayout_MouseMove(object sender, MouseEventArgs e)
        {
            //ignore if just resized form....??
            if (DateTime.Now.Subtract(FormResizedAt).TotalMilliseconds < 500) return;

            //drag a part if left button is down and we are in manual mode
            if (e.Button == MouseButtons.Left && ManualMode)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (DropPreviewTarget != null)
                    {
                        DropPreviewRect = RectangleF.Empty;
                        DropPreviewTarget = null;
                        pbLayout.Invalidate();
                    }
                    return;
                }
                if (selectedPart == null) return;
                //we have a "dragged"/"selected" part....
                //we now want to pre-calculate the drop point...
                PointF mouseUpPoint = ControlPointToAbsBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor);
                DropPreviewTarget = FindBoardAtPoint(mouseUpPoint, out double targetboardYOffset);
                if (DropPreviewTarget == null)
                {
                    if (DropPreviewRect != RectangleF.Empty)
                    {
                        DropPreviewRect = RectangleF.Empty;
                        pbLayout.Invalidate();
                    }
                    return;
                }

                // a list of all the parts to check for interference
                List<Part> packedParts = Parts.Where(p => p.Source == DropPreviewTarget).ToList();

                //generate packing grid for placed parts
                SortedSet<double> tX = new SortedSet<double> { 0 };
                SortedSet<double> tY = new SortedSet<double> { 0 };
                foreach (var iPart in packedParts)
                {
                    tX.Add((iPart.OffsetLength + iPart.Length + Settings.BladeKerf));
                    tY.Add((iPart.OffsetWidth + iPart.Width + Settings.BladeKerf));
                }
                //find intersection closest to drop position where part will fit
                double cX = double.MaxValue;
                double cY = double.MaxValue;
                double c = double.MaxValue;

                foreach (double iX in tX)
                {
                    if (iX + selectedPart.Length > DropPreviewTarget.Length) break;
                    foreach (double iY in tY)
                    {
                        if (iY + selectedPart.Width > DropPreviewTarget.Width) break;

                        // check if there is an intersect with another placed part
                        Part iOvelappedPart = packedParts.FirstOrDefault(
                            t => t != selectedPart &&
                            t.IntersectsWith(iX, iY, selectedPart.Length + Settings.BladeKerf, selectedPart.Width + Settings.BladeKerf));
                        if (iOvelappedPart == null)
                        {
                            // calculate dx^2+dy^2 (cubed distance)
                            double newc = Math.Pow(iX - (mouseUpPoint.X - xMargin), 2) + Math.Pow(iY - (mouseUpPoint.Y - targetboardYOffset), 2);
                            if (newc < c)
                            {
                                c = newc;
                                cX = iX;
                                cY = iY;
                            }
                        }

                    }
                }

                if (c < double.MaxValue)
                {
                    var t =new RectangleF((float)cX, (float)cY, (float)selectedPart.Length, (float)selectedPart.Width);
                    if (DropPreviewRect == RectangleF.Empty || DropPreviewRect.X != t.X || DropPreviewRect.Y != t.Y)
                    {
                        DropPreviewRect = t;
                        pbLayout.Invalidate();
                    }

                }
                else DropPreviewRect = RectangleF.Empty;
            }

            // scroll rendered image if middle button is down
            if (e.Button == MouseButtons.Middle)
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

        private void pbLayout_MouseUp(object sender, MouseEventArgs e)
        {
            pbLayout.Cursor = DefaultCursor;
            DropPreviewRect = RectangleF.Empty;

            if (DateTime.Now.Subtract(FormResizedAt).TotalMilliseconds < 500) return;

            // if it was not a drag, exit
            if (Math.Abs(e.X - OrigMouseDownPoint.X) < 2 && Math.Abs(e.Y - OrigMouseDownPoint.Y) < 2) return;
            // if it was not the left button, exit
            if (e.Button != MouseButtons.Left || !ManualMode) return;
            // exit if no part selected
            if (selectedPart == null) return;


            

            // dragging left - move part manually...
            // calculate new drop target (board)
            PointF mouseUpPoint = ControlPointToAbsBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor);
            Board droptargetBoard = FindBoardAtPoint(mouseUpPoint, out double targetboardYOffset);
            RectangleF proposedPartBounds = new RectangleF(mouseUpPoint.X - (float)xMargin, mouseUpPoint.Y - (float)targetboardYOffset, (float)selectedPart.Length, (float)selectedPart.Width);
            if (droptargetBoard == null) return;
            

            // for now, lets make the mouse point the desired origin of the part...this eliminates handling the offset at mouse down...


            // a list of all the parts to check for interference
            List<Part> packedParts = Parts.Where(p => p.Source == droptargetBoard).ToList();

            //if shift is not depressed
            if (Control.ModifierKeys == Keys.Control)
            {
                // if the proposed drop bounds is over the edge of the board
                if (!droptargetBoard.Contains(new PointF(proposedPartBounds.Right, proposedPartBounds.Bottom)))
                    return;
                //if part can be dropped at mouse point
                if (packedParts.FirstOrDefault(p => p != selectedPart && p.IntersectsWith(proposedPartBounds, Settings.BladeKerf)) == null)
                {
                    //  move part
                    selectedPart.OffsetLength = proposedPartBounds.X;
                    selectedPart.OffsetWidth = proposedPartBounds.Y;
                    selectedPart.Source = droptargetBoard;

                    // redraw the image
                    pbLayout.Invalidate();
                }
            }
            else
            {
                //generate packing grid for placed parts
                SortedSet<double> tX = new SortedSet<double> { 0 };
                SortedSet<double> tY = new SortedSet<double> { 0 };
                foreach (var iPart in packedParts)
                {
                    tX.Add((iPart.OffsetLength + iPart.Length + Settings.BladeKerf));
                    tY.Add((iPart.OffsetWidth + iPart.Width + Settings.BladeKerf));
                }


                //find intersection closest to drop position where part will fit
                double cX = double.MaxValue;
                double cY = double.MaxValue;
                double c = double.MaxValue;

                foreach (double iX in tX)
                {
                    if (iX + selectedPart.Length > droptargetBoard.Length) break;
                    foreach (double iY in tY)
                    {
                        if (iY + selectedPart.Width > droptargetBoard.Width) break;

                        // check if there is an intersect with another placed part
                        Part iOvelappedPart = packedParts.FirstOrDefault(
                            t => t != selectedPart && 
                            t.IntersectsWith(iX,iY,selectedPart.Length +Settings.BladeKerf ,selectedPart.Width+Settings.BladeKerf));
                        if (iOvelappedPart == null)
                        {
                            // calculate dx^2+dy^2 (cubed distance)
                            double newc = Math.Pow(iX - proposedPartBounds.X, 2) + Math.Pow(iY - proposedPartBounds.Y, 2);
                            if (newc < c)
                            {
                                c = newc;
                                cX = iX;
                                cY = iY;
                            }
                        }

                    }
                }

                if(c<double.MaxValue)
                {
                    //  move part
                    selectedPart.OffsetLength = cX;
                    selectedPart.OffsetWidth = cY;
                    selectedPart.Source = droptargetBoard;

                    // redraw the image
                    pbLayout.Invalidate();
                }

            }
        }

        private void PbLayout_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // ignore if not left mouse button
            if (e.Button != MouseButtons.Left) return;
            // ignore if the user dragged
            if (Math.Abs(e.X - OrigMouseDownPoint.X) > 5 || Math.Abs(e.Y - OrigMouseDownPoint.Y) > 5) return;

            // convert the click coordinates from screen-space to bitmap space
            PointF clickedPoint = ControlPointToAbsBitmap(e.X, e.Y, pbLayout.Size, userOffset, userZoomFactor);

            // identify the board below the clicked point
            Board clickedBoard = FindBoardAtPoint(clickedPoint, out double BoardYOffset);
            if (clickedBoard == null) return;

            // identify part under the clicked point
            PointF relativeClickedPoint = PointF.Add(clickedPoint, new SizeF(-(float)xMargin, -(float)BoardYOffset));
            Part clickedPart = FindPartAtRelPoint(relativeClickedPoint, clickedBoard);

            if (clickedPart == null)
            {
                // select the stock board
                btnStockTab_Click(this, null);
                for (int i = 0; i < StockGridView.RowCount; i++)
                {
                    if (StockGridView[0, i].Value.ToString() == clickedBoard.Name)
                    {
                        StockGridView.CurrentCell = StockGridView[0, i];
                        break;
                    }
                }
            }
            else
            {
                // select part on the parts list and focus the parts tab
                btnPartsTab_Click(this, null);
                for (int i = 0; i < PartsGridView.RowCount; i++)
                {
                    if (PartsGridView[0, i].Value.ToString() == clickedPart.Name)
                    {
                        PartsGridView.CurrentCell = PartsGridView[0, i];
                        break;
                    }
                }
            }


        }

        private void mniReportLayoutLabels_Click(object sender, EventArgs e)
        {
            var t = new CuttingLabelReport()
               .Generate(Settings, Materials, Stock, Parts);

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
                if (p.Source == null)
                {
                    MessageBox.Show("Part not placed on any board.");
                    return;
                }

                // find material for part
                string MaterialName = p.Material;

                // select the correct layout tab control page
                tcMaterials.SelectedTab = tcMaterials.TabPages[MaterialName];

                // filter the stock per the selected material and used stock
                //List<StockItem> boardsToDraw = new List<StockItem>(boards);
                //if (!drawunusedstock)
                //    boardsToDraw = parts.Where(t => t.Source != null).Select(p => p.Source).Distinct().OrderBy(o => o.Name).ToList();
                Board[] filterredStock = Stock.Where(q => q.Material == MaterialName && (Parts.Count(t => t.Source == q) > 0 || Settings.DrawUnusedStock)).OrderBy(o => o.Name).ToArray();

                // find the y-offset for the board that contains the part
                double yOffset = yMargin;
                foreach (var iStock in filterredStock)
                {
                    if (iStock == p.Source) break;
                    yOffset += iStock.Width + boardSpacing;
                }
                double dLength = p.OffsetLength;
                double dWidth = p.OffsetWidth;
                double cx = xMargin + dLength + p.Length / 2;
                double cy = yOffset + dWidth + p.Width / 2;
                // because the part may be on a tab that is not displayed, we cannot use the cached image's dimensions.
                // we need to calculate the new image on the new tab's dimensions:
                double imageHeight = 2 * yMargin;
                double imageWidth = 0;
                foreach (var iBoard in filterredStock)
                {
                    if (iBoard.Length > imageWidth) imageWidth = iBoard.Length;
                    imageHeight += iBoard.Width + boardSpacing;
                }
                imageWidth += 2 * xMargin;

                double dx = imageWidth / 2 - cx;
                double dy = imageHeight / 2 - cy;

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
            mniToggleFreeze.Enabled = (tcInputs.SelectedTab == tpStock);// || tcInputs.SelectedTab == tpParts);
            mniRemoveRows.Enabled = tcInputs.SelectedTab == tpParts && PartsGridView.SelectedRows.Count > 0
                                    || tcInputs.SelectedTab == tpStock && StockGridView.SelectedRows.Count > 0
                                    || tcInputs.SelectedTab == tpMaterials && MaterialsGridView.SelectedRows.Count > 0;
            mniDuplicateRows.Enabled = mniRemoveRows.Enabled;
        }

        private void mniIsolateMaterial_Click(object sender, EventArgs e)
        {
            if (StockGridView.SelectedCells == null || StockGridView.SelectedCells.Count == 0) return;

            //retrieve the selected stock
            var selectedStockItem = (Board)StockGridView.SelectedCells[0].OwningRow.DataBoundItem;

            //create a new material
            Material oldMaterial = Materials.FirstOrDefault(q => q.Name == selectedStockItem.Material);

            string newmaterialname = $"{selectedStockItem.Material}[{selectedStockItem.Name}]";
            Materials.Add(new Material()
            {
                Name = newmaterialname,
                Cost = oldMaterial?.Cost ?? 0,
                Thickness = oldMaterial?.Thickness ?? 25
            });

            //move the selected stock to the new material
            selectedStockItem.Material = newmaterialname;

            //move all packed parts to the new material
            Parts.Where(p => p.Source == selectedStockItem).ToList().ForEach(t => t.Material = newmaterialname);

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
            if (e.RowIndex >= Stock.Count) return;

            var t = Stock[e.RowIndex];
            if (t.IsFrozen)
                StockGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DeepSkyBlue;
            else if (Parts.Count(p => p.Source == t) == 0)
                StockGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
            else
                StockGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
        }

        private void mniToggleFreeze_Click(object sender, EventArgs e)
        {
            if (tcInputs.SelectedTab == tpStock)
            {
                if (StockGridView.SelectedCells == null || StockGridView.SelectedCells.Count == 0) return;
                List<Board> stockprocessed = new List<Board>();
                //retrieve the selected stock
                foreach (DataGridViewCell selectedCell in StockGridView.SelectedCells)
                {
                    Board selectedStockItem = selectedCell.OwningRow.DataBoundItem as Board;
                    if (selectedStockItem == null) continue;

                    if (stockprocessed.Contains(selectedStockItem)) continue;
                    stockprocessed.Add(selectedStockItem);

                    selectedStockItem.IsFrozen = !selectedStockItem.IsFrozen;

                    Parts.Where(p => p.Source == selectedStockItem).ToList().ForEach(t => t.IsFrozen = selectedStockItem.IsFrozen);
                }
            }

            PackSolution();

        }
        DateTime FormResizedAt;

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            FormResizedAt = DateTime.Now;
        }

        private void manualModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManualMode = !ManualMode;
            manualModeToolStripMenuItem.Checked = ManualMode;
            if (!ManualMode)
                PackSolution();
        }

        #endregion

    }
}
