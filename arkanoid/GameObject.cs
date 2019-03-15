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
        public event EventHandler Collision;
        public List<Line> Body { get; protected set; } // границы объекта хранятся отрезками Line, для расчета нормали, и отраженного вектора направления шара
        public RectangleF RigidBody { get; protected set; } // твердое тело предмета, используется для выявления коллизий
        public Bitmap Texture { get; protected set; } // текстура
        public RectangleF Area { get; protected set; } // тайл, занимаемый объектом 
        protected abstract List<Line> DefineBody(RectangleF area); // парсим тайл на границы объекта
        protected abstract RectangleF DefineRigidBody(); // опеределяем его твердое тело
        public virtual bool IfCollision(GameObject ball)
        {
            if (RigidBody.Left < ball.RigidBody.Right && RigidBody.Right > ball.RigidBody.Left
                 && RigidBody.Top < ball.RigidBody.Bottom && RigidBody.Bottom > ball.RigidBody.Top)
            {
                Collision?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }
        public Line? DefineCollisionLine(Ball ball)
        {
            double minDistance = 85;
            Line? minDistaneLine = null;
            System.Windows.Point intBallCenter = new System.Windows.Point((int)ball.Center.X, (int)ball.Center.Y);

            foreach (var line in Body)
            {
                int minY = (int)(line.A.Y < line.B.Y ? line.A.Y : line.B.Y);
                int maxY = (int)(line.A.Y >= line.B.Y ? line.A.Y : line.B.Y);
                int minX = (int)(line.A.X < line.B.X ? line.A.X : line.B.X);
                int maxX = (int)(line.A.X >= line.B.X ? line.A.X : line.B.X);
                for (int i = minY; i <= maxY; i++)
                {
                    for (int k = minX; k <= maxX; k++)
                    {
                        System.Windows.Point cur = new System.Windows.Point (k, i);
                        double distance = Math.Abs((intBallCenter - cur).LengthSquared);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            minDistaneLine = line;
                        }
                    }
                }
            }
            return minDistaneLine;
        }
    }
}

