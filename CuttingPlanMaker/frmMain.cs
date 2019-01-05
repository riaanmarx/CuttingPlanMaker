
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
        public static Bitmap Draw(StockItem[] boards, bool usedstockonly = true)
        {
            double xMargin = 50;
            double yMargin = 50;
            double boardSpacing = 70;

            double yOffset = yMargin;
            double imageHeight = 2 * yMargin;
            double imageWidth = 0;
            Font boardFont = new Font(new FontFamily("Consolas"), 15.0f);

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
            g.FillRectangle(Brushes.Black, 0, 0, (int)imageWidth, (int)imageHeight);

            // loop through all the boards to be drawn
            yOffset = yMargin;
            foreach (var iBoard in boardsToDraw)
            {
                // draw the board
                g.FillRectangle(Brushes.DarkRed, (float)xMargin, (float)yOffset, (float)iBoard.Length, (float)iBoard.Width);
                string boardheader = $"{iBoard.Name} [{iBoard.Length}x{iBoard.Width}]";
                SizeF textSizeBoard = g.MeasureString(boardheader, boardFont);
                g.DrawString(boardheader, boardFont, Brushes.White, (float)(xMargin), (float)(yOffset - textSizeBoard.Height));

                // loop through all the parts and draw the ones on the current board
                //string overflowtext = "";
                for (int i = 0; i < iBoard.PackedPartsCount; i++)
                {
                    Part iPlacement = iBoard.PackedParts[i];
                    double dLength = iBoard.PackedPartdLengths[i];
                    double dWidth = iBoard.PackedPartdWidths[i];

                    // draw the part
                    g.FillRectangle(Brushes.Green, (float)(xMargin + dLength), (float)(yOffset + dWidth), (float)iPlacement.Length, (float)iPlacement.Width);

                    //    // print the part text
                    //    string text1 = $"{iPlacement.Name} [{iPlacement.Length} x {iPlacement.Width}]";
                    //    string text2a = $"{iPlacement.Name}";
                    //    string text2b = $"[{iPlacement.Length} x {iPlacement.Width}]";
                    //    g.TranslateTransform((float)(xOffset + dWidth + iPlacement.Width / 2), (float)(dLength + iPlacement.Length / 2 + yMargin));
                    //    g.RotateTransform(-90);

                    //    int sz = 16;
                    //    do
                    //    {
                    //        Font partFont = new Font(new FontFamily("Consolas"), --sz);
                    //        SizeF textSize = g.MeasureString(text1, partFont);
                    //        if (textSize.Width < iPlacement.Length && textSize.Height < iPlacement.Width)
                    //        {
                    //            g.DrawString(text1, partFont, Brushes.White, -(textSize.Width / 2), -(textSize.Height / 2));
                    //            break;
                    //        }
                    //        textSize = g.MeasureString(text2a, partFont);
                    //        SizeF textSize2 = g.MeasureString(text2b, partFont);
                    //        if (Math.Max(textSize.Width, textSize2.Width) < iPlacement.Length && textSize.Height + textSize2.Height < iPlacement.Width)
                    //        {
                    //            g.DrawString(text2a, partFont, Brushes.White, -(textSize.Width / 2), -textSize.Height);
                    //            g.DrawString(text2b, partFont, Brushes.White, -(textSize2.Width / 2), 0);
                    //            break;
                    //        }
                    //        if (textSize.Width < iPlacement.Length && textSize.Height < iPlacement.Width)
                    //        {
                    //            g.DrawString(text2a, partFont, Brushes.White, -(textSize.Width / 2), -(textSize.Height / 2));
                    //            overflowtext += text1 + ", ";
                    //            break;
                    //        }
                    //    } while (sz > 1);


                    //    g.RotateTransform(90);
                    //    g.TranslateTransform(-((float)xOffset + (float)(dWidth + iPlacement.Width / 2)), -((float)(dLength + iPlacement.Length / 2 + yMargin)));
                }

                //g.TranslateTransform((float)(xOffset + iBoard.Width), (float)(iBoard.Length + yMargin));
                //g.RotateTransform(-90);
                //g.DrawString(overflowtext.TrimEnd(',', ' '), boardFont, Brushes.White, 0, 0);
                //g.RotateTransform(90);
                //g.TranslateTransform(-(float)(xOffset + iBoard.Width), -(float)(iBoard.Length + yMargin));

                yOffset += iBoard.Width + boardSpacing;
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

            // bind the materials grid
            BindMaterialsGrid();

            // bind the stock grid
            BindStockGrid();

            // bind the Part
            BindPartsGrid();

            PopulateMaterialTabs();

            PackSolution();

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


            Packer.Pack(Parts.ToArray()
                , Stock.ToArray()
                , double.Parse(Setting.BladeKerf)
                , double.Parse(Setting.PartPaddingLength)
                , double.Parse(Setting.PartPaddingWidth));
#if false
            
            Parts.ToList().ForEach(t => t.isPacked = true);

            var BoardA = Stock.First(t => t.Name == "A");
            BoardA.PackedParts = new Part[]
            {
                Parts.First(t => t.Name == "023"),
                Parts.First(t => t.Name == "008"),
                Parts.First(t => t.Name == "007"),
                Parts.First(t => t.Name == "005"),
                Parts.First(t => t.Name == "046"),
                Parts.First(t => t.Name == "037"),
                Parts.First(t => t.Name == "036"),
                Parts.First(t => t.Name == "027"),
                Parts.First(t => t.Name == "026"),
                Parts.First(t => t.Name == "014"),
            };
            BoardA.PackedPartsCount = 10;
            BoardA.isComplete = true;
            BoardA.PackedPartsTotalArea = .87 * BoardA.Area;
            BoardA.isInUse = true;
            BoardA.PackedPartdLengths = new double[]
            {
                    0.0,
                 958.2,
                 0.0,
                 958.2,
                   0.0,
                 313.2,
                 626.4,
                 939.6,
                1252.8,
                1566.0,
            };
            BoardA.PackedPartdWidths = new double[]
            {
                 0.0,
                 0.0,
                83.2,
                83.2,
                156.0,
                156.0,
                156.0,
                156.0,
                156.0,
                156.0
            };
            var BoardB = Stock.First(t => t.Name == "B");
            BoardB.PackedParts = new Part[]
            {
                Parts.First(t => t.Name == "002"),
            };
            BoardB.PackedPartsCount = 1;
            BoardB.isComplete = true;
            BoardB.PackedPartsTotalArea = .09 * BoardB.Area;
            BoardB.isInUse = true;
            BoardB.PackedPartdLengths = new double[]
            {
                    0.0,
            };
            BoardB.PackedPartdWidths = new double[]
            {
                 0.0,
            };
            var BoardC = Stock.First(t => t.Name == "C");
            BoardC.PackedParts = new Part[]
                 {
                Parts.First(t => t.Name == "043"),
                Parts.First(t => t.Name == "033"),
                Parts.First(t => t.Name == "038"),
                Parts.First(t => t.Name == "029"),
                Parts.First(t => t.Name == "028"),
                Parts.First(t => t.Name == "016"),
                Parts.First(t => t.Name == "015"),
                Parts.First(t => t.Name == "047"),
                Parts.First(t => t.Name == "022"),
                Parts.First(t => t.Name == "021"),
                Parts.First(t => t.Name == "050"),
                Parts.First(t => t.Name == "040"),
                Parts.First(t => t.Name == "030"),
                Parts.First(t => t.Name == "020"),
                 };
            BoardC.PackedPartsCount = 14;
            BoardC.isComplete = true;
            BoardC.PackedPartsTotalArea = .882 * BoardC.Area;
            BoardC.isInUse = true;
            BoardC.PackedPartdLengths = new double[]
            {
  0.0,
    958.2, 
            0.0, 
            357.7,
            715.4,
           1073.1, 
          1430.8, 
          1788.5, 
           0.0, 
            330.7,
          661.4, 
         963.6, 
          1265.8, 
            1568.0,
            };
            BoardC.PackedPartdWidths = new double[]
            {
            0.0,
            0.0,
             83.2,
             83.2,
             83.2,
             83.2,
             83.2,
             83.2,
             122.4,
             122.4,
             122.4,
             122.4,
             122.4,
             122.4,
            };
            var BoardD = Stock.First(t => t.Name == "D");
            BoardD.PackedParts = new Part[]
                 {
                Parts.First(t => t.Name == "010"),
                Parts.First(t => t.Name == "009"),
                Parts.First(t => t.Name == "006"),
                Parts.First(t => t.Name == "045"),
                Parts.First(t => t.Name == "044"),
                Parts.First(t => t.Name == "035"),
                Parts.First(t => t.Name == "013"),
                Parts.First(t => t.Name == "019"),
                Parts.First(t => t.Name == "018"),
                Parts.First(t => t.Name == "017"),
                 };
            BoardD.PackedPartsCount = 10;
            BoardD.isComplete = true;
            BoardD.PackedPartsTotalArea = .824 * BoardD.Area;
            BoardD.isInUse = true;
            BoardD.PackedPartdLengths = new double[]
            {
       0.0,
       958.2,
       0.0,
       958.2,
       1271.4,
       1584.6,
       0.0,
       313.2,
       313.2,
       615.4,
            };
            BoardD.PackedPartdWidths = new double[]
            {
                   0.0,
        0.0,
        63.2,
        63.2,
        63.2,
        63.2,
        126.4,
        126.4,
        149.6,
        126.4,
            };
            var BoardE = Stock.First(t => t.Name == "E");
           
            var BoardF = Stock.First(t => t.Name == "F");
            BoardF.PackedParts = new Part[]
    {
                Parts.First(t => t.Name == "003"),
                Parts.First(t => t.Name == "001"),
                Parts.First(t => t.Name == "049"),
                Parts.First(t => t.Name == "048"),
                Parts.First(t => t.Name == "039"),
                Parts.First(t => t.Name == "042"),
                Parts.First(t => t.Name == "041"),
                Parts.First(t => t.Name == "032"),
                Parts.First(t => t.Name == "032"),
    };
            BoardF.PackedPartsCount = 9;
            BoardF.isComplete = true;
            BoardF.PackedPartsTotalArea = .927 * BoardF.Area;
            BoardF.isInUse = true;
            BoardF.PackedPartdLengths = new double[]
            {
       0.0,
       0.0,
       1724.9,
       1724.9,
       1724.9,
       1724.9,
       1724.9,
       1724.9,
       1724.9,
            };
            BoardF.PackedPartdWidths = new double[]
            {
                   0.0,
        103.2,
        0.0,
        39.2,
        78.4,
        117.6,
        140.8,
        164.0,
        187.2,
            };
            var BoardG = Stock.First(t => t.Name == "G");
            
            BoardG.PackedParts = new Part[]
{
                Parts.First(t => t.Name == "034"),
                Parts.First(t => t.Name == "025"),
                Parts.First(t => t.Name == "024"),
                Parts.First(t => t.Name == "012"),
                Parts.First(t => t.Name == "011"),
                Parts.First(t => t.Name == "004"),
};
            BoardG.PackedPartsCount = 6;
            BoardG.isComplete = true;
            BoardG.PackedPartsTotalArea = .647 * BoardG.Area;
            BoardG.isInUse = true;
            BoardG.PackedPartdLengths = new double[]
            {
            0.0,
            313.2,
            626.4,
            939.6,
            1252.8,
            1566.0,
            };
            BoardG.PackedPartdWidths = new double[]
            {
                   0.0,
             0.0,
             0.0,
             0.0,
             0.0,
             0.0,
            };
            var BoardH = Stock.First(t => t.Name == "H");

#endif
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
            PackSolution();
        }

        private void onGridDataChangeByUser(object sender, DataGridViewCellEventArgs e)
        {
            if (sender == MaterialsGridView)
                PopulateMaterialTabs();

            IsFileSaved = false;
            PackSolution();
        }

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
            if (new frmSettingsDialog(Setting).ShowDialog() == DialogResult.OK)
            {
                if (FilePath != "") SaveConfig();
                PackSolution();
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
            //PackSolution();

            new LayoutReport()
                .Generate(Setting, Materials, Stock, Parts)
                .Save("LayoutReport.pdf");

            Process.Start("LayoutReport.pdf");
        }

        private void mniToolsPack_Click(object sender, EventArgs e)
        {
            PackSolution();
        }

        private void pbLayout_Paint(object sender, PaintEventArgs e)
        {
            string SelectedMaterial = tcMaterials.SelectedTab.Name;
            // 
            // filter stock and parts for chosen material
            StockItem[] stockItems = Stock.Where(t => t.Material == SelectedMaterial).ToArray();
            // draw the layout
            Graphics gfx = e.Graphics;
            Bitmap bitmap = Draw(stockItems);
            float ScaleF = Math.Min((float)e.ClipRectangle.Width/ bitmap.Width, (float) e.ClipRectangle.Height/ bitmap.Height);

            gfx.DrawImage(bitmap, 0f,0f,bitmap.Width * ScaleF,bitmap.Height * ScaleF);

            gfx.Flush();
        }
        #endregion

        private void tcMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbLayout.Invalidate();
        }
    }
}
