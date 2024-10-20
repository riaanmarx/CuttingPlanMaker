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
    public partial class frmPlacePart : Form
    {
        BindingList<Part> Parts;
        BindingList<Board> Boards;
        PictureBox pb;

        public frmPlacePart(BindingList<Part> Parts, BindingList<Board> Boards, PictureBox pb)
        {
            InitializeComponent();
            this.Parts = Parts;
            this.Boards = Boards;
            this.pb = pb;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //set the part's position as specified
            string partname = cbParts.Text.Split(':')[0];
            Part part = this.Parts.FirstOrDefault(t => t.Name == partname);
            string boardname = cbBoards.Text.Split(':')[0];
            Board board = this.Boards.FirstOrDefault(t => t.Name == boardname);

            part.Source = board;
            part.Material = board.Material;
            part.OffsetLength = float.Parse(tbLenOffset.Text);
            part.OffsetWidth = float.Parse(tbWidOffset.Text);

            pb.Invalidate();
        }

        private void frmPlacePart_Load(object sender, EventArgs e)
        {
            //populate the parts dropdown
            foreach (Part ipart in this.Parts)
            {
                string item = $"{ipart.Name}: [{ipart.Length} x {ipart.Width}]";
                cbParts.Items.Add(item);
            }
            //populate the boards dropdown

            foreach (Board iboard in this.Boards)
            {
                string item = $"{iboard.Name}: [{iboard.Length} x {iboard.Width}]";
                cbBoards.Items.Add(item);
            }

        }
    }
}
