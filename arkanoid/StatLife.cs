using System.Drawing;

namespace arkanoid
{
    class StatLife : StatComponent
    {
        public bool AnimationUp { get; private set; } = false;
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
                else
                {
                    yUpKoef = 0;
                    yDownKoef = 20;
                    AnimationUp = true;
                }
            }
        }

        public StatLife()
        {
            curValue = 3;
            prevValue = 3;
        }

        public override void Animation(Graphics g, Font font, Brush brush, Rectangle workArea)
        {
            if (AnimationDown)
            {
                g.DrawString("Lifes: ", font, brush, workArea);
                g.DrawString(prevValue.ToString(), font, brush, workArea.X + 58, workArea.Y + yDownKoef);
                g.DrawString(Value.ToString(), font, brush, workArea.X + 58, workArea.Y + yUpKoef);
                yUpKoef++;
                yDownKoef++;
                if (yUpKoef == 0)
                    AnimationDown = false;
            }

            else if (AnimationUp)
            {
                g.DrawString("Lifes: ", font, brush, workArea);
                g.DrawString(prevValue.ToString(), font, brush, workArea.X + 58, workArea.Y + yUpKoef);
                g.DrawString(Value.ToString(), font, brush, workArea.X + 58, workArea.Y + yDownKoef);
                yUpKoef--;
                yDownKoef--;
                if (yDownKoef == 0)
                    AnimationUp = false;
            }
        }
    }
}
