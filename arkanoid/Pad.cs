using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    class Pad : Moveable
    {
        public Pad(Rectangle area)
        {
            Texture = new Bitmap(Properties.Resources.pad.Width * 3, Properties.Resources.pad.Height);
            Texture.SetResolution(72, 72);
            PadPaint();
            Area = area;
            Bounds = AreaToBounds(area);
            Direction = new System.Windows.Vector(0f, 0f);
            speed = 100f;
        }

        private void PadPaint()
        {
            using (Graphics g = Graphics.FromImage(Texture))
            {
                g.DrawImage(Properties.Resources.pad_left, 0, 0);
                g.DrawImage(Properties.Resources.pad, 45, 0);
                g.DrawImage(Properties.Resources.pad_right, 90, 0);
            }
        }

        protected override List<Point> AreaToBounds(Rectangle area)
        {
            List<Point> res = new List<Point>();
            int indent = 21;
            for (int i = area.Y; i <= area.Y + area.Height; i++)
            {
                for (int k = area.X; k <= area.X + area.Width; k++)
                {
                    if (i <= area.Y + 13)
                    {
                        if ((i == area.Y && k > area.Left + indent && k <= area.Right - indent)
                            || (k > area.Left + indent && k <= area.Left + indent + 2)
                                || k >= area.Right - indent - 2 && k < area.Right - indent)
                        {
                            res.Add(new Point(k, i));
                        }
                    }
                    else
                    {
                        if (k == area.Left || k == area.Right)
                        {
                            res.Add(new Point(k, i));
                        }
                    }
                }

                if (i == area.Y)
                    indent -= 4;
                else if (i == area.Y + 1)
                    indent -= 3;
                else if (i >= area.Y + 2 && i <= area.Y + 5)
                    indent -= 2;
                else if (i >= area.Y + 6 && i <= area.Y + 11)
                    indent -= 1;
            }
            return res;
        }

        public override bool IfCollision(Ball ball)
        {
            foreach (var ballItem in ball.Bounds)
            {
                foreach (var padItem in Bounds)
                {
                    if (ballItem.X == padItem.X && ballItem.Y == padItem.Y)
                    {
                        System.Windows.Vector normVector = new System.Windows.Vector(ballItem.Y - ballItem.Y, ballItem.X + 1 - ballItem.X);
                        ball.Direction = ball.Direction - 2 * normVector * ((ball.Direction * normVector) / (normVector * normVector));
                        ball.Move();

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
