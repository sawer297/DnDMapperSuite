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
    public partial class MapsList : UserControl
    {
        public MapsList()
        {
            InitializeComponent();
        }

        private void MapsList_Resize(object sender, EventArgs e)
        {
            int w = Size.Width;
            int h = Size.Height;

            int halfX, halfY;
            halfX = Convert.ToInt32(w * 0.5);
            halfY = Convert.ToInt32(h * 0.5);

            lstBxMaps.Size = new Size(halfX - 4, h - 4);
            lstBxMaps.Location = new Point(2, 2);

            pnlMapInfo.Size = new Size(halfX - 4, h - 4);
            pnlMapInfo.Location = new Point(halfX, 2);

            picBxPreview.Size = new Size(pnlMapInfo.Size.Width - 4, (pnlMapInfo.Size.Height / 2) - 4);
            picBxPreview.Location = new Point(2, 2);

            btnNewMap.Location = new Point(2, pnlMapInfo.Size.Height / 2);
            btnEditMap.Location = new Point(2, btnNewMap.Location.Y + btnEditMap.Size.Height + 2);
            btnDelMap.Location = new Point(2, btnEditMap.Location.Y + btnDelMap.Size.Height + 2);
        }

        private void btnNewMap_Click(object sender, EventArgs e)
        {
            Form parent = (this.Parent as Form);
            parent.Controls["mapPane"].BringToFront();
        }
    }
}
