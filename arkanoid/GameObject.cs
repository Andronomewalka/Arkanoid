using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
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
        public Bitmap LineTexture { get; protected set; }
        public Bitmap Texture { get; protected set; } // текстура
        public RectangleF Area { get; protected set; } // тайл, занимаемый объектом 
        protected abstract RectangleF DefineRigidBody(); // опеределяем его твердое тело

        protected virtual List<Line> DefineBody() // парсим тайл на границы объекта
        {
            List<Line> res = new List<Line>();
            res.Add(new Line(new PointF(RigidBody.Left, RigidBody.Top), new PointF(RigidBody.Right, RigidBody.Top)));
            res.Add(new Line(new PointF(RigidBody.Left, RigidBody.Top), new PointF(RigidBody.Left, RigidBody.Bottom)));
            res.Add(new Line(new PointF(RigidBody.Right, RigidBody.Top), new PointF(RigidBody.Right, RigidBody.Bottom)));
            res.Add(new Line(new PointF(RigidBody.Left, RigidBody.Bottom), new PointF(RigidBody.Right, RigidBody.Bottom)));
            return res;
        }
        public virtual bool IfCollision(GameObject obj)
        {
            if (RigidBody.Left < obj.RigidBody.Right && RigidBody.Right > obj.RigidBody.Left
                 && RigidBody.Top < obj.RigidBody.Bottom && RigidBody.Bottom > obj.RigidBody.Top)
            {
                return true;
            }

            return false;
        }

        public static bool IfCollision(GameObject obj, RectangleF rect)
        {
            if (rect.Left < obj.RigidBody.Right && rect.Right > obj.RigidBody.Left
                 && rect.Top < obj.RigidBody.Bottom && rect.Bottom > obj.RigidBody.Top)
            {
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
                        if (distance < minDistance && !ParallelDirection(line))
                        {
                            minDistance = distance;
                            minDistaneLine = line;
                        }
                    }
                }
            }
            if (minDistaneLine != null)
                Collision?.Invoke(this, EventArgs.Empty);
            return minDistaneLine;

            bool ParallelDirection(Line line)
            {
                Vector2 normVector = new Vector2(line.A.Y - line.B.Y, line.B.X - line.A.X);
               // if (normVector * ball.Direction == 0)
               //     return true;

                return false;
            }
        }

        protected virtual Bitmap DefineLineTexture()
        {
            Bitmap bitmap = new Bitmap(Texture.Width+1, Texture.Height+1);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Pen pen = new Pen(Color.Black);
                for (int i = 0; i < Body.Count; i++)
                {
                    g.DrawLine(pen, Body[i].A.X - Area.X, Body[i].A.Y - Area.Y,
                        Body[i].B.X - Area.X, Body[i].B.Y - Area.Y);
                }
            }
            return bitmap;
        }
    }
}

