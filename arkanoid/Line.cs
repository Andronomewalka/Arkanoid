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

        public Line(PointF A, float angle)
        {
            this.A = A;
            float lenA = 1;
            float lenb = (float)(lenA / Math.Sin(Math.PI * Math.Abs(angle) / 180.0)); // Math функции принимают радианы, конвертируем в градусы

            // lenb^2 = (By - Ay)^2 + (Bx - Ax)^2

            float a = 1;
            float b = -A.X * 2;
            float c = (float)(1 + Math.Pow(A.X, 2) - Math.Pow(lenb, 2));

            float D = (float)(Math.Pow(b, 2) - 4 * a * c);

            float Bx = 0;
            if (angle > 0)
                Bx = (float)((-b - Math.Sqrt(D)) / (2 * a));
            else if (angle < 0)
                Bx = (float)((-b + Math.Sqrt(D)) / (2 * a));

            B = new PointF(Bx, A.Y + lenA);
        }
    }
}
