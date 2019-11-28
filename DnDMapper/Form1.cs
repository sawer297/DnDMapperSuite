using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DnDMapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            topMenu.Size = ClientSize;
            topMenu.Location = new Point(0,0);
            topMenu.BringToFront();

            mapsList.Size = ClientSize;
            mapsList.Location = new Point(0, 0);
            mapsList.SendToBack();

            mapPane.Size = ClientSize;
            mapPane.Location = new Point(0, 0);
            mapPane.SendToBack();
        }
    }
}
