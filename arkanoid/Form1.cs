using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    public partial class Form1 : Form
    {
        Map map;
        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(800, 600);
            CenterToScreen();
            DoubleBuffered = true;
            map = new Map(this);
            map.Create();
        }
    }
}
