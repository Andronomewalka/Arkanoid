using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace arkanoid
{
    public class Ball : GameObject, IChangePosition
    {
        enum CollisionSide { vertical, horizontal }
        List<GameObject> recentCollisionObjects; // иногда при столкновениях объекты залипают друг с другом, из-за того что не успевая покинуть область коллизии, меняют направление
        public Vector2 Direction { get; set; }
        public bool BondedToPad { get; set; } = true;
        public PointF Center { get; private set; }
        public float DefaultSpeed { get; set; } = 5;
        private float speed;
        private float maxSpeed = 8;
        private float minSpeed = 3;
        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value < maxSpeed && value >= minSpeed)
                    speed = value;
            }
        } // скорость

        public Ball(RectangleF area)
        {
            Texture = Properties.Resources.ball_mid;
            Area = area;
            Texture.SetResolution(72, 72);
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            Center = DefineCenter();
            Direction = new Vector2(0f, -1f);
            Speed = DefaultSpeed;
            recentCollisionObjects = new List<GameObject>();
            LineTexture = DefineLineTexture();
        }

        private PointF DefineCenter()
        {
            return new PointF(RigidBody.Left + RigidBody.Width / 2,
                RigidBody.Top + RigidBody.Height / 2);
        }
        protected override RectangleF DefineRigidBody()
        {
            return new RectangleF(Area.X + 14, Area.Y, 17, 17);
        }

        protected override List<Line> DefineBody()
        {
            List<Line> res = new List<Line>();

            res.Add(new Line(new PointF(Area.Left + 19, Area.Top), new PointF(Area.Right - 19, Area.Top)));

            res.Add(new Line(new PointF(Area.Left + 19, Area.Top), new PointF(Area.Left + 17, Area.Top + 1)));
            res.Add(new Line(new PointF(Area.Right - 19, Area.Top), new PointF(Area.Right - 17, Area.Top + 1)));

            res.Add(new Line(new PointF(Area.Left + 17, Area.Top + 1), new PointF(Area.Left + 16, Area.Top + 2)));
            res.Add(new Line(new PointF(Area.Right - 17, Area.Top + 1), new PointF(Area.Right - 16, Area.Top + 2)));

            res.Add(new Line(new PointF(Area.Left + 16, Area.Top + 2), new PointF(Area.Left + 15, Area.Top + 3)));
            res.Add(new Line(new PointF(Area.Right - 16, Area.Top + 2), new PointF(Area.Right - 15, Area.Top + 3)));

            res.Add(new Line(new PointF(Area.Left + 15, Area.Top + 3), new PointF(Area.Left + 14, Area.Top + 5)));
            res.Add(new Line(new PointF(Area.Right - 15, Area.Top + 3), new PointF(Area.Right - 14, Area.Top + 5)));

            res.Add(new Line(new PointF(Area.Left + 14, Area.Top + 5), new PointF(Area.Left + 14, Area.Top + 11)));
            res.Add(new Line(new PointF(Area.Right - 14, Area.Top + 5), new PointF(Area.Right - 14, Area.Top + 11)));

            res.Add(new Line(new PointF(Area.Left + 14, Area.Top + 11), new PointF(Area.Left + 15, Area.Top + 13)));
            res.Add(new Line(new PointF(Area.Right - 14, Area.Top + 11), new PointF(Area.Right - 15, Area.Top + 13)));

            res.Add(new Line(new PointF(Area.Left + 15, Area.Top + 13), new PointF(Area.Left + 16, Area.Top + 14)));
            res.Add(new Line(new PointF(Area.Right - 15, Area.Top + 13), new PointF(Area.Right - 16, Area.Top + 14)));

            res.Add(new Line(new PointF(Area.Left + 16, Area.Top + 14), new PointF(Area.Left + 17, Area.Top + 15)));
            res.Add(new Line(new PointF(Area.Right - 16, Area.Top + 14), new PointF(Area.Right - 17, Area.Top + 15)));

            res.Add(new Line(new PointF(Area.Left + 17, Area.Top + 15), new PointF(Area.Left + 19, Area.Top + 16)));
            res.Add(new Line(new PointF(Area.Right - 17, Area.Top + 15), new PointF(Area.Right - 19, Area.Top + 16)));

            res.Add(new Line(new PointF(Area.Left + 19, Area.Top + 16), new PointF(Area.Right - 19, Area.Top + 16)));

            return res;
        }

        public void Move()
        {
            RectangleF newPos = new RectangleF((float)(Area.X + Speed * Direction.X), (float)(Area.Y + Speed * Direction.Y), Area.Width, Area.Height);

            bool outOfscreen = false;
            if (RigidBody.Left <= Map.WindowSize.Left || RigidBody.Right >= Map.WindowSize.Right)
            {
                CollisionWith(CollisionSide.horizontal);
                outOfscreen = true;
            }
            if (RigidBody.Top <= Map.WindowSize.Top /*|| RigidBody.Bottom >= Map.WindowSize.Bottom*/)
            {
                CollisionWith(CollisionSide.vertical);
                outOfscreen = true;
            }

            if (!outOfscreen)
            {
                Area = newPos;
                Body = DefineBody();
                RigidBody = DefineRigidBody();
                Center = DefineCenter();
                LineTexture = DefineLineTexture();
            }
            DefineRecentCollisionObjects();
        }

        private void DefineRecentCollisionObjects()
        {
            for (int i = recentCollisionObjects.Count - 1; i >= 0; i--)
            {
                if (!recentCollisionObjects[i].IfCollision(this))
                    recentCollisionObjects.RemoveAt(i);

            }
        }

        public void SetPosition(float posX, float posY)
        {
            Area = new RectangleF(posX, posY, Area.Width, Area.Height);
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            Center = DefineCenter();
            LineTexture = DefineLineTexture();
        }

        public bool CollisionWith(GameObject obj, Line? line)
        {
            bool res = true;

            if (!recentCollisionObjects.Contains(obj) && line != null)
            {
                Vector2 reflectVector = default;
                Vector2 newDirection = default;

                if (obj is Pad)
                {
                    reflectVector = (obj as Pad).DefineReflectVector(line);
                    newDirection = Vector2.Reflect(Direction, reflectVector);

                    if (newDirection.Y > 0 || Math.Abs(newDirection.X) < 0.1)
                        Direction = new Vector2(Direction.X, Direction.Y * -1);

                    else
                        Direction = newDirection;
                }

                else
                {
                    reflectVector = new Vector2(line.Value.A.Y - line.Value.B.Y, line.Value.B.X - line.Value.A.X);
                    reflectVector = Vector2.Normalize(reflectVector);
                    Direction = Vector2.Reflect(Direction, reflectVector);
                }

                //if (obj is Pad)
                //    Log.Write(line, reflectVector, Direction, newDirection);

                //string directXText = Direction.X.ToString("N5");
                //string directYText = Direction.Y.ToString("N5");
                //string newDirectXText = newDirection.X.ToString("N5");
                //string newDirectYText = newDirection.Y.ToString("N5");
                //if (directXText.Equals(newDirectXText) &&
                //    directYText.Equals(newDirectYText))
                //{
                //    res = false;
                //}

                // добавялем в список коллизий те объекты, которые не разрушаются при столкновении
                if (obj is Ball || obj is Pad ||
                    (obj is Block && (obj as Block).Iteration != 0))
                    recentCollisionObjects.Add(obj);
            }
            return res;
        }

        private void CollisionWith(CollisionSide side) // для границ карты
        {
            if (side == CollisionSide.vertical)
            {
                RectangleF newPos = new RectangleF((float)(Area.X - Speed * Direction.X), (float)(Area.Y - Speed * Direction.Y), Area.Width, Area.Height);
                Direction = new Vector2(Direction.X, Direction.Y * -1);
                SetPosition(newPos.Left, newPos.Top);
            }
            else if (side == CollisionSide.horizontal)
            {
                RectangleF newPos = new RectangleF((float)(Area.X - Speed * Direction.X), (float)(Area.Y - Speed * Direction.Y), Area.Width, Area.Height);
                Direction = new Vector2(Direction.X * -1, Direction.Y);
                SetPosition(newPos.Left, newPos.Top);
            }

            if (Direction.X == 0 && Area.X < Map.WindowSize.Left)
                Direction = new Vector2(1, Direction.Y);
            else if (Direction.X == 0 && Area.Right > Map.WindowSize.Right)
                Direction = new Vector2(-1, Direction.Y);
            if (Direction.Y == 0 && Area.Top < Map.WindowSize.Top)
                Direction = new Vector2(Direction.X, 1);
            else if (Direction.Y == 0 && Area.Bottom > Map.WindowSize.Bottom)
                Direction = new Vector2(Direction.X, -1);
            Direction = Vector2.Normalize(Direction);
        }
    }
}
