using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public struct Line // не нашел такую стандартную структуру
    {
        public PointF A { get; private set; }
        public PointF B { get; private set; }

        public Line(PointF a, PointF b)
        {
            A = a;
            B = b;
        }

        public bool Contain(Line line)
        {
            float minY = line.A.Y < line.B.Y ? line.A.Y : line.B.Y;
            float maxY = line.A.Y >= line.B.Y ? line.A.Y : line.B.Y;
            float minX = line.A.X < line.B.X ? line.A.X : line.B.X;
            float maxX = line.A.X >= line.B.X ? line.A.X : line.B.X;
            for (float i = minY; i <= maxY; i += 1f)
            {
                for (float k = minX; k <= maxX; k += 1f)
                {
                    PointF cur = new PointF(k, i);
                    if (A.Y < B.Y)
                    {
                        if (A.X < B.X)
                        {
                            if (i >= A.Y && i <= B.Y
                                && k >= A.X && k <= B.X)
                                return true;
                        }
                        else
                        {
                            if (i >= A.Y && i <= B.Y
                                && k >= B.X && k <= A.X)
                                return true;
                        }
                    }
                    else
                    {
                        if (A.X < B.X)
                        {
                            if (i >= B.Y && i <= A.Y
                                && k >= A.X && k <= B.X)
                                return true;
                        }
                        else
                        {
                            if (i >= B.Y && i <= A.Y
                                && k >= B.X && k <= A.X)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool Contain(Ball ball)
        {
            float minY = A.Y < B.Y ? A.Y : B.Y;
            float maxY = A.Y >= B.Y ? A.Y : B.Y;
            float minX = A.X < B.X ? A.X : B.X;
            float maxX = A.X >= B.X ? A.X : B.X;
            for (float i = minY; i <= maxY; i++)
            {
                for (float k = minX; k <= maxX; k++)
                {
                    PointF cur = new PointF(k, i);
                    foreach (var item in ball.RigidBody)
                    {
                        if ((int)cur.X == (int)item.X &&
                            (int)cur.Y == (int)item.Y)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
