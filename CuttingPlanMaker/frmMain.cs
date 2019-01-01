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
        #region // Fields ...
        bool isFileSaved = true;            // flag to keep track if the current file is saved
        string fileName = "";               // the name of the current file

        #endregion

        #region // Constructor ...
        // constructor for the main form
        public frmMain()
        {
            InitializeComponent();
        }
        #endregion

        #region // internal helper functions ...
        /// <summary>
        /// Save the current data to the file set with the specified name
        /// </summary>
        /// <param name="path"></param>
        private void SaveFile(string path)
        {

        }

        /// <summary>
        /// Load a file into the application
        /// </summary>
        /// <param name="path"></param>
        private void LoadFile(string path)
        {

        }

        private void LoadDefault()
        {
            // start from scratch
            LoadFile("Default");
            fileName = "";
            isFileSaved = false;
        }

        private bool CloseFile()
        {
            // if the current file is not saved
            if (!isFileSaved)
            {
                // prompt the user to save/discard/cancel
                DialogResult response = MessageBox.Show($"File {fileName} is not saved. Save?", "Confirm", MessageBoxButtons.YesNoCancel);

                // if user chose cancel, exit function
                if (response == DialogResult.Cancel) return false;

                // if user chose to save
                if (response == DialogResult.Yes)
                {
                    // if the file has never been saved
                    if (fileName == "")
                    {
                        // display the fileSave dialog. if the user clicks save,
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // use the new selected name
                            fileName = saveFileDialog.FileName;
                            fileName = fileName.Replace(".Settings.csv", "");

                            // save the file
                            SaveFile(fileName);

                            // exit the application
                            return true;
                        }
                        else return false;
                    }
                    else
                        SaveFile(fileName);
                    // exit the application
                    return true;
                }
                else
                    // exit the application
                    return true;
            }
            else
                // exit the application
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
            isFileSaved = false;
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
        #endregion
    }
}
