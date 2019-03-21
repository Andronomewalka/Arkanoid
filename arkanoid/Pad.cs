using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace arkanoid
{
    class Pad : GameObject, IChangePosition
    {
        public BonusType BonusType { get; set; }
        private float reflectionAngle; // насколько градусов за пиксель вектор отклоняется от нормали в зависимости от положения от центра платформы
        private float centerX;
        private int length;
        public Pad(RectangleF area)
        {
            Texture = new Bitmap(Properties.Resources.pad.Width * 3, Properties.Resources.pad.Height);
            Texture.SetResolution(72, 72);
            length = 1;
            PadPaint(length);
            Area = area;
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            LineTexture = DefineLineTexture();
            BonusType = default;
            reflectionAngle = 0.4f;
        }

        // protected virtual Bitmap DefineTexture()
        // {
        //     return Properties.Resources.middlePad2;
        // }

        protected override RectangleF DefineRigidBody()
        {
            centerX = Area.X + Area.Width / 2 - 1;
            return new RectangleF(Area.X + 1, Area.Y + 6, 133, 17);
        }

        private void PadPaint(int length)
        {
            int xCoord = 0;
            using (Graphics g = Graphics.FromImage(Texture))
            {
                g.DrawImage(Properties.Resources.padLeft, xCoord, 0);
                for (int i = 0; i < length; i++)
                {
                    xCoord += 45;
                    g.DrawImage(Properties.Resources.pad, xCoord, 0);
                }
                xCoord += 45;
                g.DrawImage(Properties.Resources.padRight, xCoord, 0);
            }
        }

        public void SetPosition(float posX, float posY)
        {
            // if (posX > Area.X)
            //     Direction = new System.Windows.Vector(1,0);
            // else if (posX < Area.X)
            //     Direction = new System.Windows.Vector(-1, 0);
            Area = new RectangleF(posX, posY, Area.Width, Area.Height);

            Body = DefineBody();
            RigidBody = DefineRigidBody();
            LineTexture = DefineLineTexture();

        }

        protected override List<Line> DefineBody()
        {
            List<Line> res = new List<Line>();

            PointF rightConnection = new PointF();
            PointF leftConnection = new PointF();
            for (float i = Area.X + 1; i < Area.Width + Area.X - 2; i += 3)
            {
                if (i == Area.X + 1)
                    leftConnection = new PointF(i, Area.Y + 6);

                else
                    leftConnection = new PointF(rightConnection.X + 1, Area.Y + 6);

                rightConnection = new PointF(i + 2, Area.Y + 6);

                res.Add(new Line(leftConnection, rightConnection));
            }
            return res;
        }

        public void DefineBonusType(BonusType bonus)
        {
            if (bonus == BonusType.longerPad)
            {
                if (length < 3)
                {
                    length++;
                    Texture = new Bitmap(Properties.Resources.pad.Width * (2 + length), Properties.Resources.pad.Height);
                    Texture.SetResolution(72, 72);
                    PadPaint(length);
                    Area = new RectangleF(Area.X, Area.Y, Area.Width + 45, Area.Height);
                    Body = DefineBody();
                    RigidBody = DefineRigidBody();
                    LineTexture = DefineLineTexture();
                    reflectionAngle = (5 - length) / 10.0f;
                }
            }
            else if (bonus == BonusType.shorterPad)
            {
                if (length > 0)
                {
                    length--;
                    Texture = new Bitmap(Properties.Resources.pad.Width * (2 + length), Properties.Resources.pad.Height);
                    Texture.SetResolution(72, 72);
                    PadPaint(length);
                    Area = new RectangleF(Area.X, Area.Y, Area.Width - 45, Area.Height);
                    Body = DefineBody();
                    RigidBody = DefineRigidBody();
                    LineTexture = DefineLineTexture();
                    reflectionAngle = (5 - length) / 10.0f;
                }
            }
        }

        internal System.Numerics.Vector2 DefineReflectVector(Line? line)
        {
            float angle = (line.Value.A.X + 1 - centerX) * reflectionAngle;
            float sin = (float)(Math.Sin(Math.PI * angle / 180.0));
            float cos = (float)(Math.Cos(Math.PI * angle / 180.0));
            return new System.Numerics.Vector2(sin, -cos);
        }
    }
}
