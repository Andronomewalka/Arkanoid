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
    }
}
