using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DnDMapper
{
    public partial class TopMenu : UserControl
    {
        public TopMenu()
        {
            InitializeComponent();
        }

        private void btnMaps_Click(object sender, EventArgs e)
        {
            Form parent = (this.Parent as Form);
            parent.Controls["mapsList"].BringToFront();
        }

        private void TopMenu_Resize(object sender, EventArgs e)
        {
            int w = Size.Width, h = Size.Height;

            int w40p = Convert.ToInt32(w * 0.4);
            int h40p = Convert.ToInt32(h * 0.4);

            pnlBtns.Size = new Size(w40p - 4, h40p - 4);
            pnlBtns.Location = new Point(2, (h - h40p) - 4);

            int wPnlBtn75p = Convert.ToInt32(pnlBtns.Size.Width * 0.75);
            int hbtn = Convert.ToInt32(pnlBtns.Size.Height / 4);

            btnMaps.Size = new Size(wPnlBtn75p, hbtn);
            btnSessions.Size = new Size(wPnlBtn75p, hbtn);

            int x = Convert.ToInt32(wPnlBtn75p * 0.35);

            btnMaps.Location = new Point((x / 2), (hbtn / 2));
            btnSessions.Location = new Point((x / 2), ((2 * hbtn) + (hbtn / 2)));

        }
    }
}
