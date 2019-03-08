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
        public override void Move(int dx, int dy)
        {
            throw new NotImplementedException();
        }

        protected override Point[] AreaToBounds(Rectangle area)
        {
            throw new NotImplementedException();
        }

        protected override Rectangle BoundsToArea()
        {
            throw new NotImplementedException();
        }
    }
}
