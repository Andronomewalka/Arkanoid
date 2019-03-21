using System.Drawing;

namespace arkanoid
{
    class StatScore : StatComponent
    {
        public bool AnimationDown { get; private set; } = false;
        public override int Value
        {
            get
            {
                return curValue;
            }
            set
            {
                prevValue = curValue;
                curValue = value;
                if (prevValue < curValue)
                {
                    yUpKoef = -20;
                    yDownKoef = 0;
                    AnimationDown = true;
                }
            }
        }
        public StatScore()
        {
            curValue = 0;
            prevValue = 0;
        }

        public override void Animation(Graphics g, Font font, Brush brush, Rectangle workArea)
        {
                g.DrawString("Score: ", font, brush, workArea);
                g.DrawString(prevValue.ToString(), font, brush, workArea.X + 68, workArea.Y + yDownKoef);
                g.DrawString(Value.ToString(), font, brush, workArea.X + 68, workArea.Y + yUpKoef);
                yUpKoef++;
                yDownKoef++;
                if (yUpKoef == 0)
                    AnimationDown = false;
            }
        }
}
