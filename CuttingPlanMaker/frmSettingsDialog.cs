using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuttingPlanMaker
{
    public partial class frmSettingsDialog : Form
    {
        private Settings _settings;

        public frmSettingsDialog(Settings settings)
        {
            InitializeComponent();

            // keep a reference to the instance
            _settings = settings;

            // populate the local controls with the current values
            tbSawBladeKerf.Text = _settings.BladeKerf;
            tbPartPaddingLength.Text = _settings.PartPaddingLength;
            tbPartPaddingWidth.Text = _settings.PartPaddingWidth;
            cbAutoRecalc.Checked = _settings.AutoRepack == "true";
            cbDrawUnused.Checked = _settings.DrawUnusedStock == "true";
            ddlOrientation.Text = _settings.ResultOrientation;
            tbProjectName.Text = _settings.ProjectName;
            tbJobId.Text = _settings.JobID;
            tbClientName.Text = _settings.ClientName;
            tbClientNr.Text = _settings.ClientTelNr;
            tbClientAddr.Text = _settings.ClientAddr;
            dtpTargetDate.Value = DateTime.Parse(_settings.TargetDate ?? DateTime.Now.ToLongDateString());
            cbIncludePaddingOnReports.Checked = _settings.IncludePaddingInReports == "true";
            cbIncludePaddingOnDisplay.Checked = _settings.IncludePaddingInDisplay == "true";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // populate the new values back to the instance
            _settings.BladeKerf = tbSawBladeKerf.Text;
            _settings.PartPaddingLength = tbPartPaddingLength.Text;
            _settings.PartPaddingWidth = tbPartPaddingWidth.Text;
            _settings.AutoRepack = cbAutoRecalc.Checked ? "true" : "false";
            _settings.DrawUnusedStock = cbDrawUnused.Checked ? "true" : "false";
            _settings.ResultOrientation = ddlOrientation.Text;
            _settings.IncludePaddingInReports = cbIncludePaddingOnReports.Checked ? "true" : "false";
            _settings.IncludePaddingInDisplay = cbIncludePaddingOnDisplay.Checked ? "true" : "false";

            _settings.ProjectName = tbProjectName.Text;
            _settings.JobID = tbJobId.Text;
            _settings.ClientName = tbClientName.Text;
            _settings.ClientTelNr = tbClientNr.Text;
            _settings.ClientAddr = tbClientAddr.Text;
            _settings.TargetDate = dtpTargetDate.Value.ToLongDateString();
        }

        private void frmSettingsDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
