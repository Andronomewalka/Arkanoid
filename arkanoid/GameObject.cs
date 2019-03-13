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
        public List<Line> Body { get; protected set; } // границы объекта хранятся отрезками Line, для расчета нормали, и отраженного вектора направления шара
        protected RectangleF RigidBody { get; set; } // твердое тело предмета, используется для выявления коллизий
        public Bitmap Texture { get; protected set; } // текстура
        public RectangleF Area { get; protected set; } // тайл, занимаемый объектом 
        public DateTime BallHitTime { get; protected set; } = DateTime.Now;// при высокой скорости шар может застрять в платформе, фиксируем каждый удар только раз в BallHitTime миллисекунд
        public event EventHandler Collision;
        protected abstract List<Line> DefineBody(RectangleF area); // парсим тайл на границы объекта
        protected abstract RectangleF DefineRigidBody(); // опеределяем его твердое тело
        public virtual bool IfCollision(GameObject ball)
        {
            DateTime current = DateTime.Now;
            if ((current - BallHitTime).TotalMilliseconds < 50)
                return false;

            if (RigidBody.Left < ball.RigidBody.Right && RigidBody.Right > ball.RigidBody.Left
                 && RigidBody.Top < ball.RigidBody.Bottom && RigidBody.Bottom > ball.RigidBody.Top)
            {
                BallHitTime = current;
                Collision?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }
        public Line? DefineCollisionLine(GameObject ball)
        {
            foreach (var objLine in Body)
            {
                float minY = objLine.A.Y < objLine.B.Y ? objLine.A.Y : objLine.B.Y;
                float maxY = objLine.A.Y >= objLine.B.Y ? objLine.A.Y : objLine.B.Y;
                float minX = objLine.A.X < objLine.B.X ? objLine.A.X : objLine.B.X;
                float maxX = objLine.A.X >= objLine.B.X ? objLine.A.X : objLine.B.X;

                bool left = LineLine(minX, minY, maxX, maxY, ball.RigidBody.Left, ball.RigidBody.Top, ball.RigidBody.Left, ball.RigidBody.Top + ball.RigidBody.Height);
                bool right = LineLine(minX, minY, maxX, maxY, ball.RigidBody.Left + ball.RigidBody.Width, ball.RigidBody.Top, ball.RigidBody.Left + ball.RigidBody.Width, ball.RigidBody.Top + ball.RigidBody.Height);
                bool top = LineLine(minX, minY, maxX, maxY, ball.RigidBody.Left, ball.RigidBody.Top, ball.RigidBody.Left + ball.RigidBody.Width, ball.RigidBody.Top);
                bool bottom = LineLine(minX, minY, maxX, maxY, ball.RigidBody.Left, ball.RigidBody.Top + ball.RigidBody.Height, ball.RigidBody.Left + ball.RigidBody.Width, ball.RigidBody.Top + ball.RigidBody.Height);

                if (left || right || top || bottom)
                {
                    return objLine;
                }
            }

            return null;

            bool LineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
            {

                // calculate the direction of the lines
                float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
                float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

                // if uA and uB are between 0-1, lines are colliding
                if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
                    return true;

                return false;
            }
        }
    }
}

