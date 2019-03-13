using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class RunController
    {
        Form1 parent;
        Map map;
        Level level;
        Point cursor;
        Timer frame;

        Pad pad;
        List<Ball> balls;
        bool onPause;
        public RunController(Form1 parent, int levelNum)
        {
            this.parent = parent;

            level = Level.Deserialization(levelNum);
           //   new Level(1); // генерация уровня
            map = new Map(parent, level);
            map.Create();
            FindBallAndPad();
            CustomBlockEventSign();
            ChangeCursorState();

            frame = new Timer();
            frame.Interval = 5;
            frame.Tick += Frame_Tick;
            frame.Start();

            parent.KeyDown += Parent_KeyDown1;
            map.PictureField.Click += Parent_Click;
            map.PictureField.MouseMove += Parent_MouseMove1;
        }

        private void CustomBlockEventSign()
        {
            foreach (var item in map.Objects)
            {
                if (item is BonusBall)
                    item.Collision += Item_Collision;
            }
        }

        private void Item_Collision(object sender, EventArgs e)
        {
            BonusBall cur = sender as BonusBall;
            if (cur != null)
            {
                int newBallIndex = map.Objects.FindIndex((obj) => obj == cur) + 1;
                // если отправитель события BonusBall - добалвяем шарик на место бонусного блока + 1
                map.Objects.Insert(newBallIndex, new Ball(cur.Area) { BondedToPad = false });
                balls.Add(map.Objects[newBallIndex] as Ball);
            }
        }

        private void Parent_Click(object sender, EventArgs e)
        {
            foreach (var item in balls)
            {
                if (item.BondedToPad)
                    item.BondedToPad = false;
            }
        }

        private void FindBallAndPad()
        {
            balls = new List<Ball>();
            foreach (var item in map.Objects)
            {
                if (item is Ball)
                {
                    balls.Add(item as Ball);
                }
                else if (item is Pad)
                    pad = item as Pad;
            }
        }

        private void Frame_Tick(object sender, EventArgs e)
        {
            if (!onPause)
            {
                pad.SetPosition(cursor.X, pad.Area.Y);

                for (int i = balls.Count - 1; i >= 0; i--)
                {
                    if (!balls[i].BondedToPad)
                    {
                        for (int k = map.Objects.Count - 1; k >= 0; k--)
                        {
                            if (map.Objects[k] != balls[i] && map.Objects[k].IfCollision(balls[i]))
                            {
                                balls[i].CollisionWith(map.Objects[k].DefineCollisionLine(balls[i]));

                                if (map.Objects[k] is Ball)
                                    (map.Objects[k] as Ball).CollisionWith(balls[i].DefineCollisionLine(map.Objects[k] as Ball));

                                else if (!(map.Objects[k] is Pad))
                                    map.Objects.RemoveAt(k);

                                break;
                            }
                            if (balls[i].Area.Top > 600)
                            {
                                map.Objects.Remove(balls[i]);
                                balls.RemoveAt(i);
                                if (balls.Count == 0)
                                    return;
                            }
                        }
                        balls[i].Move();
                    }
                    else
                        balls[i].SetPosition(pad.Area.X + pad.Area.Width / 2, pad.Area.Y - 20);
                }
                map.PictureField.Invalidate();
            }
        }

        private void ChangeCursorState()
        {
            if (!onPause)
            {
                Cursor.Hide();
                Task.Run(() => Cursor.Clip = new Rectangle(
                    new Point(parent.Location.X + 10, parent.Location.Y + 35),
                    new Size(parent.Size.Width - 20, parent.Size.Height - 50)));
            }
            else
            {
                Cursor.Show();
                Task.Run(() => Cursor.Clip = new Rectangle());
            }
        }

        private void Parent_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                onPause = !onPause;
                ChangeCursorState();
               //if (onPause)
               //    map.PictureField.MouseMove -= Parent_MouseMove1;
               //else
               //    map.PictureField.MouseMove += Parent_MouseMove1;
            }
        }

        public void Parent_MouseMove1(object sender, MouseEventArgs e)
        {
            cursor = e.Location;
        }
    }
}
