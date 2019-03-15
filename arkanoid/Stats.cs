using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class Stats
    {
        private PictureBox pictureField;
        private int prevLife;
        private int life;
        private Font font;
        private SolidBrush brush;
        private int yUpKoef;
        private int yDownKoef;
        bool animationUp;
        bool animationDown;

        public Stats(PictureBox pictureField)
        {
            this.pictureField = pictureField;
            pictureField.Paint += PictureField_Paint;
            life = 3;
            prevLife = 3;
            font = new System.Drawing.Font("Arial", 16);
            brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        }

        public int Life
        {
            get
            {
                return life;
            }
            set
            {
                prevLife = life;
                life = value;
                if (prevLife < life)
                {
                    yUpKoef = -20;
                    yDownKoef = 0;
                    animationDown = true;
                }
                else
                {
                    yUpKoef = 0;
                    yDownKoef = 20;
                    animationUp = true;
                }
            }
        }

        private void PictureField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle workArea = new Rectangle(700, 550, 100, 25);
            Region prev = g.Clip;
            g.Clip = new Region(workArea);

            if (animationDown || animationUp)
                Animation(g, workArea);
            else
                g.DrawString("Lifes: " + Life.ToString(), font, brush, workArea);

            g.Clip = prev;
        }

        private void Animation(Graphics g, Rectangle workArea)
        {
            if (animationDown)
            {
                g.DrawString("Lifes: ", font, brush, workArea);
                g.DrawString(prevLife.ToString(), font, brush, workArea.X + 58, workArea.Y + yDownKoef);
                g.DrawString(Life.ToString(), font, brush, workArea.X + 58, workArea.Y + yUpKoef);
                yUpKoef++;
                yDownKoef++;
                if (yUpKoef == 0)
                    animationDown = false;
            }

            else if (animationUp)
            {
                g.DrawString("Lifes: ", font, brush, workArea);
                g.DrawString(prevLife.ToString(), font, brush, workArea.X + 58, workArea.Y + yUpKoef);
                g.DrawString(Life.ToString(), font, brush, workArea.X + 58, workArea.Y + yDownKoef);
                yUpKoef--;
                yDownKoef--;
                if (yDownKoef == 0)
                    animationUp = false;
            }
        }
    }
}
