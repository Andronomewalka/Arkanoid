using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public class BonusBall : Block
    {
        public BonusBall(RectangleF area) : base(area)
        {
            Texture = Properties.Resources.bonusBall;
        }
    }
}
