using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public class Ball : Moveable
    {
        enum CollisionSide { vertical, horizontal }
        public bool BondedToPad { get; set; } = true;
        public Ball(Rectangle area)
        {
            Texture = Properties.Resources.ball_mid;
            Area = area;
            Texture.SetResolution(72, 72);
            Bounds = AreaToBounds(area);
            Direction = new System.Windows.Vector(1f, -1f);
            speed = 1f;
        }

        protected override List<Point> AreaToBounds(Rectangle area)
        {
            List<Point> res = new List<Point>();
            // Point[] res = new Point[120]; // вычилено опытным путём
            // int Ires = 0;
            int indent = 19;
            for (int i = area.Y; i <= area.Bottom - 16; i++)
            {
                for (int k = area.X + 14; k <= area.Right - 14; k++)
                {
                    if (((i == area.Y || i == area.Bottom - 16) && k > area.Left + indent && k <= area.Right - indent)
                        || (k > area.Left + indent && k <= area.Left + indent + 2)
                            || (k >= area.Right - indent - 2 && k < area.Right - indent))
                    {
                        res.Add(new Point(k, i));
                    }
                }

                if (i == area.Y + 15)
                    indent = 19;
                else if (i == area.Y || i == area.Y + 14)
                    indent = 17;
                else if (i == area.Y + 1 || i == area.Y + 13)
                    indent = 16;
                else if (i == area.Y + 2 || i == area.Y + 3
                    || i == area.Y + 11 || i == area.Y + 12)
                    indent = 15;
                else if (i >= area.Y + 3 && i <= area.Y + 10)
                    indent = 14;
            }
            return res;
        }

        public override void Move()
        {
            Rectangle newPos = new Rectangle((int)(Area.X + speed * Direction.X), (int)(Area.Y + speed * Direction.Y), Area.Width, Area.Height);
            if (newPos.Left >= Map.WindowSize.Right || newPos.Right <= Map.WindowSize.Left)
                CollisionWith(CollisionSide.horizontal);
            else if (newPos.Top <= Map.WindowSize.Top)
                CollisionWith(CollisionSide.vertical);
            Area = newPos;
            Bounds = AreaToBounds(Area);
        }

        public void CollisionWith(GameObject obj) // для других объектов
        {
            //if (DefineCollisionSide(obj) == CollisionSide.vertical)
            //    Dy *= -1;
            //else if (DefineCollisionSide(obj) == CollisionSide.horizontal)
            //    Dx *= -1;
        }

        private void CollisionWith(CollisionSide side) // для границ карты
        {
            if (side == CollisionSide.vertical)
                Direction = new System.Windows.Vector(Direction.X, Direction.Y * -1);
            else if (side == CollisionSide.horizontal)
                Direction = new System.Windows.Vector(Direction.X * -1, Direction.Y);
        }

        public override bool IfCollision(Ball ball)
        {
            throw new NotImplementedException();
        }

        // определяем с какой стороны произошло столкновение, 
        // чтоб знать какое направление шара нужно инвертировать
        private CollisionSide DefineCollisionSide(GameObject obj)
        {

            if (obj.Area.Left + 4 >= Area.Right - 16 || obj.Area.Right - 4 <= Area.Left + 15)
                return CollisionSide.horizontal;
            else /*if (obj.Area.Bottom <= Area.Top || obj.Area.Top <= Area.Bottom)*/
                return CollisionSide.vertical;
        }
    }
}
