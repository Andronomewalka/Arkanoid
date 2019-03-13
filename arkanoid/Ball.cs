﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace arkanoid
{
    public class Ball : Moveable
    {
       // public List<PointF> RigidBody { get; private set; }
        enum CollisionSide { vertical, horizontal }
        private DateTime BallHitTimeHorizontal;
        private DateTime BallHitTimeVertical;
        public bool BondedToPad { get; set; } = true;

        public Ball(RectangleF area)
        {
            Texture = Properties.Resources.ball_mid;
            Area = area;
            Texture.SetResolution(72, 72);
            Body = DefineBody(area);
            Direction = new System.Windows.Vector(0f, -1f);
            speed = 5f;
            RigidBody = DefineRigidBody();
            BallHitTime = DateTime.Now;
            BallHitTimeHorizontal = DateTime.Now;
            BallHitTimeVertical = DateTime.Now;
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

            if (newPos.Left >= Map.WindowSize.Right || newPos.Right <= Map.WindowSize.Left)
                CollisionWith(CollisionSide.horizontal);
            else if (newPos.Top <= Map.WindowSize.Top)
                CollisionWith(CollisionSide.vertical);

            Area = newPos;
            RigidBody = DefineRigidBody();
            Body = DefineBody(Area);
        }

        public void CollisionWith(Line? line)
        {
            if (line != null)
            {
                System.Windows.Vector normVector = new System.Windows.Vector(line.Value.A.Y - line.Value.B.Y, line.Value.B.X - line.Value.A.X);
                Direction = Direction - 2 * normVector * ((Direction * normVector) / (normVector * normVector));
            }
        }

        private void CollisionWith(CollisionSide side) // для границ карты
        {
            DateTime current = DateTime.Now;
            if (side == CollisionSide.vertical)
            {
                if ((current - BallHitTimeVertical).TotalMilliseconds < 50)
                    return;

                BallHitTimeVertical = current;
                Direction = new System.Windows.Vector(Direction.X, Direction.Y * -1);

            }
            else if (side == CollisionSide.horizontal)
            {
                if ((current - BallHitTimeHorizontal).TotalMilliseconds < 50)
                    return;

                BallHitTimeHorizontal = current;
                Direction = new System.Windows.Vector(Direction.X * -1, Direction.Y);
            }
        }
    }
}
