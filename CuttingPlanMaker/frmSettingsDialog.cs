using CuttingPlanMaker.Packers;
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
            tbSawBladeKerf.Text = _settings.BladeKerf.ToString();
            tbPartPaddingLength.Text = _settings.PartPaddingLength.ToString();
            tbPartPaddingWidth.Text = _settings.PartPaddingWidth.ToString();
            cbAutoRecalc.Checked = _settings.AutoRepack;
            cbDrawUnused.Checked = _settings.DrawUnusedStock;
            tbProjectName.Text = _settings.ProjectName;
            tbJobId.Text = _settings.JobID;
            tbClientName.Text = _settings.ClientName;
            tbClientNr.Text = _settings.ClientTelNr;
            tbClientAddr.Text = _settings.ClientAddr;
            dtpTargetDate.Value = DateTime.Parse(_settings.TargetDate ?? DateTime.Now.ToLongDateString());
            cbIncludePaddingOnReports.Checked = _settings.IncludePaddingInReports ;
            cbIncludePaddingOnDisplay.Checked = _settings.IncludePaddingInDisplay ;
            //cbDrawID.Checked = _settings.DrawID;

            // populate items for algorithms
            var type = typeof(PackerBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.FullName != "CuttingPlanMaker.Packers.PackerBase" && p.FullName != "CuttingPlanMaker.Packers.PerBoardPackerBase");

            types.ToList().ForEach(t => ddlPacker.Items.Add(t.GetProperty("AlgorithmName").GetValue(null) as string));
            if (_settings.Algorithm == "") ddlPacker.SelectedIndex = 0;
            else ddlPacker.Text = _settings.Algorithm;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // populate the new values back to the instance
            _settings.BladeKerf = double.Parse(tbSawBladeKerf.Text);
            //_settings.PartPaddingLength = double.Parse(tbPartPaddingLength.Text);
            //_settings.PartPaddingWidth = double.Parse(tbPartPaddingWidth.Text);
            _settings.AutoRepack = cbAutoRecalc.Checked;
            _settings.DrawUnusedStock = cbDrawUnused.Checked;
            _settings.ResultOrientation = ddlPacker.Text;
            _settings.IncludePaddingInReports = cbIncludePaddingOnReports.Checked;
            _settings.IncludePaddingInDisplay = cbIncludePaddingOnDisplay.Checked;

            _settings.ProjectName = tbProjectName.Text;
            _settings.JobID = tbJobId.Text;
            _settings.ClientName = tbClientName.Text;
            _settings.ClientTelNr = tbClientNr.Text;
            _settings.ClientAddr = tbClientAddr.Text;
            _settings.TargetDate = dtpTargetDate.Value.ToLongDateString();
            _settings.Algorithm = ddlPacker.Text;
            //_settings.DrawID = cbDrawID.Checked;
        }

    }
}
