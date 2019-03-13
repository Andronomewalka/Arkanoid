using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public abstract class GameObject // все игровые объекты наследуются от этого класса
    {
        public List<Line> Body { get; protected set; } // границы объекта хранятся отрезками Line, по ударению об которые расчитывается нормаль, и отраженный вектор направления шара
        public Bitmap Texture { get; protected set; } // текстура
        public RectangleF Area { get; protected set; } //отрисовка объекта 
        public DateTime BallHitTime { get; protected set; } = DateTime.Now;// при высокой скорости шар может застрять в платформе, фиксируем каждый удар только раз в BallHitTime миллисекунд
        public event EventHandler Collision;
        protected abstract List<Line> DefineBody(RectangleF area); // парсим блок на границы объекта
        public virtual bool IfCollision(Ball ball)
        {
            DateTime current = DateTime.Now;
            if ((current - BallHitTime).TotalMilliseconds < 50)
                return false;

            if (Area.Left < ball.Area.Right && Area.Right > ball.Area.Left
                && Area.Top < ball.Area.Bottom && Area.Bottom > ball.Area.Top)
            {
                foreach (var objLine in Body)
                {
                    if (objLine.Contain(ball))
                    {
                        BallHitTime = current;
                        Collision?.Invoke(this, EventArgs.Empty);
                        return true;
                    }
                }
            }
            return false;
        }
        public Line DefineCollisionLine(Ball ball)
        {
            foreach (var objLine in Body)
            {
                if (objLine.Contain(ball))
                {
                    return objLine;
                }
            }
            return default;
        }
    }
}
