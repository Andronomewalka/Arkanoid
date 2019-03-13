using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public class Block : GameObject
    {
        public Block(RectangleF area)
        {
            Texture = Properties.Resources.block2;
            Area = area;
            Texture.SetResolution(72, 72);
            Body = DefineBody(area);
            RigidBody = DefineRigidBody();
        }

        protected override RectangleF DefineRigidBody()
        {
            return new RectangleF(Area.Left + 4, Area.Y + 3, 37, 25);
        }

        protected override List<Line> DefineBody(RectangleF area)
        {
            List<Line> res = new List<Line>();
            res.Add(new Line(new PointF(Area.Left + 12, Area.Top + 3), new PointF(Area.Right - 12, Area.Top + 3)));

            res.Add(new Line(new PointF(Area.Left + 12, Area.Top + 3), new PointF(Area.Left + 9, Area.Top + 4)));
            res.Add(new Line(new PointF(Area.Right - 12, Area.Top + 3), new PointF(Area.Right - 9, Area.Top + 4)));

            res.Add(new Line(new PointF(Area.Left + 9, Area.Top + 4), new PointF(Area.Left + 6, Area.Top + 7)));
            res.Add(new Line(new PointF(Area.Right - 9, Area.Top + 4), new PointF(Area.Right - 6, Area.Top + 7)));

            res.Add(new Line(new PointF(Area.Left + 6, Area.Top + 7), new PointF(Area.Left + 5, Area.Top + 10)));
            res.Add(new Line(new PointF(Area.Right - 6, Area.Top + 7), new PointF(Area.Right - 5, Area.Top + 10)));

            res.Add(new Line(new PointF(Area.Left + 5, Area.Top + 10), new PointF(Area.Left + 5, Area.Top + 19)));
            res.Add(new Line(new PointF(Area.Right - 5, Area.Top + 10), new PointF(Area.Right - 5, Area.Top + 19)));

            res.Add(new Line(new PointF(Area.Left + 5, Area.Top + 19), new PointF(Area.Left + 6, Area.Top + 22)));
            res.Add(new Line(new PointF(Area.Right - 5, Area.Top + 19), new PointF(Area.Right - 6, Area.Top + 22)));

            res.Add(new Line(new PointF(Area.Left + 6, Area.Top + 22), new PointF(Area.Left + 9, Area.Top + 25)));
            res.Add(new Line(new PointF(Area.Right - 6, Area.Top + 22), new PointF(Area.Right - 9, Area.Top + 25)));

            res.Add(new Line(new PointF(Area.Left + 9, Area.Top + 25), new PointF(Area.Left + 12, Area.Top + 26)));
            res.Add(new Line(new PointF(Area.Right - 9, Area.Top + 25), new PointF(Area.Right - 12, Area.Top + 26)));

            res.Add(new Line(new PointF(Area.Left + 12, Area.Top + 26), new PointF(Area.Right - 12, Area.Top + 26)));
            return res;
        }
    }
}
