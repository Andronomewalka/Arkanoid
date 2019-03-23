using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    public partial class Form1 : Form
    {
        public int AnimationKoef { get; private set; } = 810;
        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(810, 600);
            CenterToScreen();
            DoubleBuffered = true;
            KeyPreview = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
          //  new Level(5);
            MainController main = new MainController(this);
            FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Show();
            Task.Run(() => Cursor.Clip = new Rectangle());
        }
    }
}
