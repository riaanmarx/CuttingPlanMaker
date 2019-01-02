using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                    _filePath = Path.Combine(Path.GetDirectoryName(value),Path.GetFileName(value).Split('.').First());

                this.Text = $"Cutting Plan Maker {(_isFileSaved ? "" : "*")}{(FileName==""?"":$"({FileName})")}";
            }
        }
        private string _filePath = "";

        // Data for the loaded file
        BindingList<Settings> Settings { get; set; }
        BindingList<Material> Materials { get; set; }
        BindingList<StockItem> Stock { get; set; }
        BindingList<Part> Parts { get; set; }

        #endregion

        #region // Constructor ...
        // constructor for the main form
        public frmMain()
        {
            InitializeComponent();
        }
        #endregion

        #region // Internal helper functions ...

        /// <summary>
        /// Save the current data to the file set with the specified name
        /// </summary>
        /// <param name="path"></param>
        private void SaveFile()
        {
            // write the file data to the csv files

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

        /// <summary>
        /// Load a file into the application
        /// </summary>
        /// <param name="path"></param>
        private void LoadFile(string path)
        {
            // update the properties to keep track of the file
            FilePath = path;
            IsFileSaved = true;

            // Load the file into data structures
            Settings = CSVFile.Read<Settings>($"{FilePath}.Settings.CSV");

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
        #endregion

        private void mniToolsOptions_Click(object sender, EventArgs e)
        {
            // show settings dialog, if close with save, save the config
            if (new frmSettingsDialog(Settings.First()).ShowDialog() == DialogResult.OK)
                SaveConfig();
        }

        
    }
}
