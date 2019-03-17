using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    class Pad : GameObject, IChangePosition
    {
        public BonusType BonusType { get; set; }
        public Pad(RectangleF area)
        {
            Texture = new Bitmap(Properties.Resources.pad.Width * 3, Properties.Resources.pad.Height);
            Texture.SetResolution(72, 72);
            PadPaint();
            Area = area;
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            LineTexture = DefineLineTexture();
            BonusType = default;
        }

        protected override RectangleF DefineRigidBody()
        {
            return Area;
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

        public virtual void SetPosition(float posX, float posY)
        {
            Area = new RectangleF(posX, posY, Area.Width, Area.Height);

            Body = DefineBody();
            RigidBody = DefineRigidBody();
            LineTexture = DefineLineTexture();
        }

        protected override List<Line> DefineBody()
        {
            List<Line> res = new List<Line>();
            PointF rightConnection = new Point();
            PointF leftConnection = new Point();
            int indent = 21;

            for (float i = Area.Top; i <= Area.Bottom; i++)
            {
                if (i <= Area.Y + 12)
                {
                    if (i == Area.Top)
                    {
                        res.Add(new Line(new PointF(Area.Left + indent, i), new PointF(Area.Right - indent, i)));
                        leftConnection = new PointF(Area.Left + indent, i);
                        rightConnection = new PointF(Area.Right - indent, i);
                    }
                    else
                    {
                        res.Add(new Line(leftConnection, new PointF(Area.Left + indent, i)));
                        leftConnection = new PointF(Area.Left + indent, i);
                        res.Add(new Line(rightConnection, new PointF(Area.Right - indent, i)));
                        rightConnection = new PointF(Area.Right - indent, i);
                    }
                }
                else
                {

                    res.Add(new Line(leftConnection, new PointF(leftConnection.X, Area.Bottom-1)));
                    res.Add(new Line(rightConnection, new PointF(rightConnection.X, Area.Bottom-1)));
                    res.Add(new Line(new PointF(leftConnection.X, Area.Bottom), new PointF(rightConnection.X, Area.Bottom)));
                    break;
                }

                if (i == Area.Y)
                    indent -= 4;
                else if (i == Area.Y + 1)
                    indent -= 3;
                else if (i >= Area.Y + 2 && i <= Area.Y + 5)
                    indent -= 2;
                else if (i >= Area.Y + 6 && i <= Area.Y + 11)
                    indent -= 1;
            }
            return res;
        }

        public void DefineBonusType(BonusType bonus)
        {
        }
    }
}
