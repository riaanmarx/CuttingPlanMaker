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
            cbAutoRecalc.Checked = _settings.AutoRecalc == "true";
            cbDrawUnused.Checked = _settings.DrawUnusedStock == "true";
            ddlOrientation.Text = _settings.ResultOrientation;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // populate the new values back to the instance
            _settings.BladeKerf = tbSawBladeKerf.Text;
            _settings.PartPaddingLength = tbPartPaddingLength.Text;
            _settings.PartPaddingWidth = tbPartPaddingWidth.Text;
            _settings.AutoRecalc = cbAutoRecalc.Checked ? "true" : "false";
            _settings.DrawUnusedStock = cbDrawUnused.Checked ? "true" : "false";
            _settings.ResultOrientation = ddlOrientation.Text;
        }
    }
}
