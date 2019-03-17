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
        public int Iteration { get; private set; } // количество ударов необходимое сделать до разрушения блока
        public Block(RectangleF area, int iteration)
        {
            Iteration = iteration;
            Texture = DefineTexture();
            Area = area;
            Texture.SetResolution(72, 72);
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            Collision += Block_Collision;
            LineTexture = DefineLineTexture();
        }

        protected void Block_Collision(object sender, EventArgs e)
        {
            Iteration--;
            Texture = DefineTexture();
        }

        protected virtual Bitmap DefineTexture()
        {
            if (Iteration == 1)
                return Properties.Resources.redBlock;
            else if (Iteration == 2)
                return Properties.Resources.orangeBlock;

            return Properties.Resources.redBlock;
        }

        protected override RectangleF DefineRigidBody()
        {
            return new RectangleF(Area.Left + 1, Area.Y + 1, 43, 30);
        }

        protected override List<Line> DefineBody()
        {
            List<Line> res = new List<Line>();
            res.Add(new Line(new PointF(Area.Left + 8, Area.Top + 1), new PointF(Area.Right - 8, Area.Top + 1)));

            res.Add(new Line(new PointF(Area.Left + 7, Area.Top + 1), new PointF(Area.Left + 5, Area.Top + 2)));
            res.Add(new Line(new PointF(Area.Right - 7, Area.Top + 1), new PointF(Area.Right - 5, Area.Top + 2)));

            res.Add(new Line(new PointF(Area.Left + 4, Area.Top + 3), new PointF(Area.Left + 3, Area.Top + 4)));
            res.Add(new Line(new PointF(Area.Right - 4, Area.Top + 3), new PointF(Area.Right - 3, Area.Top + 4)));

            res.Add(new Line(new PointF(Area.Left + 2, Area.Top + 5), new PointF(Area.Left + 1, Area.Top + 7)));
            res.Add(new Line(new PointF(Area.Right - 2, Area.Top + 5), new PointF(Area.Right - 1, Area.Top + 7)));

            res.Add(new Line(new PointF(Area.Left + 1, Area.Top + 9), new PointF(Area.Left + 1, Area.Top + 23)));
            res.Add(new Line(new PointF(Area.Right - 1, Area.Top + 9), new PointF(Area.Right - 1, Area.Top + 23)));

            res.Add(new Line(new PointF(Area.Left + 8, Area.Bottom - 1), new PointF(Area.Right - 8, Area.Bottom - 1)));

            res.Add(new Line(new PointF(Area.Left + 7, Area.Bottom - 1), new PointF(Area.Left + 5, Area.Bottom - 2)));
            res.Add(new Line(new PointF(Area.Right - 7, Area.Bottom - 1), new PointF(Area.Right - 5, Area.Bottom - 2)));

            res.Add(new Line(new PointF(Area.Left + 4, Area.Bottom - 3), new PointF(Area.Left + 3, Area.Bottom - 4)));
            res.Add(new Line(new PointF(Area.Right - 4, Area.Bottom - 3), new PointF(Area.Right - 3, Area.Bottom - 4)));

            res.Add(new Line(new PointF(Area.Left + 2, Area.Bottom - 5), new PointF(Area.Left + 1, Area.Bottom - 7)));
            res.Add(new Line(new PointF(Area.Right - 2, Area.Bottom - 5), new PointF(Area.Right - 1, Area.Bottom - 7)));

            return res;
        }
    }
}
