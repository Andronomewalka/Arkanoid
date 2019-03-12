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
        public Block(Rectangle area)
        {
            Texture = Properties.Resources.block;
            Area = area;
            Texture.SetResolution(72, 72);
            Bounds = AreaToBounds(area);
        }

        protected override List<Point> AreaToBounds(Rectangle area)
        {
            Color test = Texture.GetPixel(44, 31);
            Color test2 = Texture.GetPixel(5, 5);
            List<Point> res = new List<Point>(); // вычилено опытным путём
            for (int i = area.Y + 4; i <= area.Bottom - 5; i++)
            {
                for (int k = area.X + 4; k <= area.Right - 4; k++)
                {
                    if (i == area.Y + 4 || i == area.Bottom - 5
                        || k == area.X + 4 || k == area.Right - 4)

                    {
                        res.Add(new Point(k, i));
                    }
                }
            }
            return res;
        }

        public override bool IfCollision(Ball ball)
        {
            foreach (var ballItem in ball.Bounds)
            {
                foreach (var blockItem in Bounds)
                {
                    if (ballItem.X == blockItem.X && ballItem.Y == blockItem.Y)
                    {
                        System.Windows.Vector normVector = new System.Windows.Vector(blockItem.Y - blockItem.Y, blockItem.X + 1 - blockItem.X);
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
