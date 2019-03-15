using System;
using System.Collections.Generic;
using System.Drawing;

namespace arkanoid
{
    public class Ball : Moveable
    {
        enum CollisionSide { vertical, horizontal }
        public bool BondedToPad { get; set; } = true;
        public PointF Center { get; private set; }
        public DateTime BallHitTime { get; protected set; }
        List<GameObject> recentCollisionObjects; // иногда при столкновениях объекты залипают друг с другом, из-за того что не успевая покинуть область коллизии, меняют направление

        public Ball(RectangleF area)
        {
            Texture = Properties.Resources.ball_mid;
            Area = area;
            Texture.SetResolution(72, 72);
            Body = DefineBody(area);
            RigidBody = DefineRigidBody();
            Center = DefineCenter();
            Direction = new System.Windows.Vector(0f, -1f);
            speed = 5f;
            recentCollisionObjects = new List<GameObject>();
        }

        private PointF DefineCenter()
        {
            PointF res = new PointF(RigidBody.Left + RigidBody.Width / 2, RigidBody.Top + RigidBody.Height / 2);

            // System.Windows.Point cur = new System.Windows.Point(Area.X+14,Area.Y+4);
            // System.Windows.Point intBallCenter = new System.Windows.Point((int)res.X, (int)res.Y);
            // double distance = Math.Abs((intBallCenter - cur).LengthSquared);

            return res;
        }
        protected override RectangleF DefineRigidBody()
        {
            return new RectangleF(Area.X + 14, Area.Y, 17, 17);
        }

        protected override List<Line> DefineBody(RectangleF area)
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

        public override void Move()
        {
            RectangleF newPos = new RectangleF((float)(Area.X + speed * Direction.X), (float)(Area.Y + speed * Direction.Y), Area.Width, Area.Height);

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
                Body = DefineBody(Area);
                RigidBody = DefineRigidBody();
                Center = DefineCenter();
            }
            DefineRecentCollisionObjects();
        }

        private void DefineRecentCollisionObjects()
        {
            double minDistance = 85;
            System.Windows.Point intBallCenter = new System.Windows.Point((int)Center.X, (int)Center.Y);

            for (int i = recentCollisionObjects.Count - 1; i >= 0; i--)
            {
                bool stillCollision = false;

                foreach (var line in recentCollisionObjects[i].Body)
                {
                    int minY = (int)(line.A.Y < line.B.Y ? line.A.Y : line.B.Y);
                    int maxY = (int)(line.A.Y >= line.B.Y ? line.A.Y : line.B.Y);
                    int minX = (int)(line.A.X < line.B.X ? line.A.X : line.B.X);
                    int maxX = (int)(line.A.X >= line.B.X ? line.A.X : line.B.X);

                    for (int k = minY; k <= maxY; k++)
                    {
                        if (stillCollision)
                            break;

                        for (int l = minX; l <= maxX; l++)
                        {
                            System.Windows.Point cur = new System.Windows.Point(l, k);
                            double distance = Math.Abs((intBallCenter - cur).LengthSquared);
                            if (distance < minDistance)
                            {
                                stillCollision = true;
                                break;
                            }
                        }
                    }
                }
                if (!stillCollision)
                    recentCollisionObjects.Remove(recentCollisionObjects[i]);
            }
        }

        public override void SetPosition(float posX, float posY)
        {
            // if (RigidBody.Left > Map.WindowSize.Left &&
            //     RigidBody.Right < Map.WindowSize.Right)
            // {
            Area = new RectangleF(posX, posY, Area.Width, Area.Height);
            Body = DefineBody(Area);
            RigidBody = DefineRigidBody();
            Center = DefineCenter();
            //  }
        }

        public bool CollisionWith(GameObject obj, Line? line)
        {
            bool res = true;

            if (!recentCollisionObjects.Contains(obj) && line != null)
            {
                System.Windows.Vector normVector = new System.Windows.Vector(line.Value.A.Y - line.Value.B.Y, line.Value.B.X - line.Value.A.X);
                System.Windows.Vector newDirection = Direction - 2 * normVector * ((Direction * normVector) / (normVector * normVector));

                string directXText = Direction.X.ToString("N5");
                string directYText = Direction.Y.ToString("N5");
                string newDirectXText = newDirection.X.ToString("N5");
                string newDirectYText = newDirection.Y.ToString("N5");
                if (directXText.Equals(newDirectXText) &&
                    directYText.Equals(newDirectYText))
                {
                    res = false;
                }
                newDirection.Normalize();
                Direction = newDirection;

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
                RectangleF newPos = new RectangleF((float)(Area.X - speed * Direction.X), (float)(Area.Y - speed * Direction.Y), Area.Width, Area.Height);
                Direction = new System.Windows.Vector(Direction.X, Direction.Y * -1);
                SetPosition(newPos.Left, newPos.Top);
            }
            else if (side == CollisionSide.horizontal)
            {
                RectangleF newPos = new RectangleF((float)(Area.X - speed * Direction.X), (float)(Area.Y - speed * Direction.Y), Area.Width, Area.Height);
                Direction = new System.Windows.Vector(Direction.X * -1, Direction.Y);
                SetPosition(newPos.Left, newPos.Top);
            }
        }
    }
}
