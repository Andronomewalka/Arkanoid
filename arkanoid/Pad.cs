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
        public Pad(RectangleF area)
        {
            Texture = new Bitmap(Properties.Resources.pad.Width * 3, Properties.Resources.pad.Height);
            Texture.SetResolution(72, 72);
            PadPaint();
            Area = area;
            Body = DefineBody(area);
            Direction = new System.Windows.Vector(0f, 0f);
            speed = 0f;
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

        protected override List<Line> DefineBody(RectangleF area)
        {
            List<Line> res = new List<Line>();
            PointF rightConnection = new Point();
            PointF leftConnection = new Point();
            int indent = 21;

            for (float i = area.Top; i <= area.Bottom; i++)
            {
                if (i <= area.Y + 12)
                {
                    if (i == area.Top)
                    {
                        res.Add(new Line(new PointF(area.Left + indent, i), new PointF(area.Right - indent, i)));
                        leftConnection = new PointF(area.Left + indent, i);
                        rightConnection = new PointF(area.Right - indent, i);
                    }
                    else
                    {
                        res.Add(new Line(leftConnection, new PointF(area.Left + indent, i)));
                        leftConnection = new PointF(area.Left + indent, i);
                        res.Add(new Line(rightConnection, new PointF(area.Right - indent, i)));
                        rightConnection = new PointF(area.Right - indent, i);
                    }
                }
                else
                {
                    res.Add(new Line(leftConnection, new PointF(leftConnection.X, Area.Bottom)));
                    res.Add(new Line(rightConnection, new PointF(rightConnection.X, Area.Bottom)));
                    res.Add(new Line(new PointF(leftConnection.X, Area.Bottom), new PointF(rightConnection.X, Area.Bottom)));
                    break;
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
    }
}
