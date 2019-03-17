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
        RunController run;
        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(810, 600);
            CenterToScreen();
            DoubleBuffered = true;
            run = new RunController(this, 1);
        }
    }
}
