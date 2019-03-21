using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    abstract class StatComponent
    {
        protected int prevValue;
        protected int curValue;
        protected int yUpKoef;
        protected int yDownKoef;
        public abstract int Value { get; set; }
        public abstract void Animation(Graphics g, Font font, Brush brush, Rectangle workArea);
    }
}
