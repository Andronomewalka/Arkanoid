using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public class Block : GameObject, Iinteractable
    {

        public Block(Rectangle area)
        {
            texture = Properties.Resources.block;
            bounds = AreaToBounds(area);
           // Map.FieldPictures.Paint += FieldPictures_Paint;
        }

        protected override Point[] AreaToBounds(Rectangle area)
        {
            Point[] res = new Point[120]; // вычилено опытным путём
            int Ires = 0;
            for (int i = area.Y + 4; i <= area.Bottom - 5; i++)
            {
                for (int k = area.X + 4; k <= area.Right - 4; k++)
                {
                    if (i == area.Y + 4 || i == area.Bottom - 5
                        || k == area.X + 4 || k == area.Right - 4)

                    {
                        res[Ires] = new Point(k, i);
                        Ires++;
                    }
                }
            }
            return res;
        }

        protected override Rectangle BoundsToArea()
        {
            return new Rectangle()
            {
                X = bounds[0].X,
                Y = bounds[0].Y,
                Width = bounds[37].X - bounds[0].X + 4,
                Height = bounds[119].Y - bounds[0].Y + 4
            };
        }

        public bool Contain(Point ball)
        {
            foreach (var item in bounds)
            {
                if (item.X == ball.X && item.Y == ball.Y)
                    return true;
            }
            return false;
        }

    }
}
