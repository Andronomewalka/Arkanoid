using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class StatsScoreMultiplier
    {
        private int minMultiplier;
        private int maxMultiplier;
        private int multiplier;
        private Timer t;
        public CustomStatusBar Bar { get; private set; }
        public int Multiplier
        {
            get
            {
                return multiplier;
            }
            set
            {
                Bar.Value = 0;
                if (value <= maxMultiplier)
                {
                    multiplier = value;
                    MultiplierIncrementOperation();
                }
                if (multiplier == 2)
                    t.Start();
            }
        }

        private void MultiplierIncrementOperation()
        {
            Bar.Text = multiplier.ToString();
            t.Interval = 60 - multiplier * 10;
        }

        public StatsScoreMultiplier(Size workArea)
        {
            minMultiplier = 1;
            maxMultiplier = 5;
            multiplier = minMultiplier;

            Bar = new CustomStatusBar(workArea) { Text = multiplier.ToString() };
            Bar.UpdateBar();
            t = new Timer();
            t.Interval = 50;
            t.Tick += T_Tick;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (Bar.Value > 100)
            {
                ResetBar();
                return;
            }
            Bar.UpdateBar();
            Bar.Value++;
        }

        private void ResetBar()
        {
            multiplier = 1;
            t.Interval = 50;
            t.Stop();
            Bar.Reset();
            Bar.UpdateBar();
        }
    }

    class CustomStatusBar
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public Bitmap Field { get; set; }
        private Size workArea;
        private Graphics g;
        private float units;


        public CustomStatusBar(Size workArea)
        {
            Field = new Bitmap(workArea.Width, workArea.Height);
            this.workArea = workArea;
            units = workArea.Width/100.0f;
            g = Graphics.FromImage(Field);
        }

        public void UpdateBar()
        {
            g.Clear(Color.WhiteSmoke);
            g.FillRectangle(Brushes.OrangeRed, new Rectangle(0, 0, (int)(Value * units), workArea.Height));
            g.DrawString("X" + Text, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(workArea.Width / 2 - 10, -1));
        }

        public void Reset()
        {
            g.Clear(Color.WhiteSmoke);
            Value = 0;
            Text = 1.ToString();
        }
    }
}
