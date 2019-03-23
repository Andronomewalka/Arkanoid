using System.Drawing;

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
