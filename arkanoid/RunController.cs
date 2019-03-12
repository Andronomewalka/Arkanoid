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
            //new Level(1); // генерация уровня
            map = new Map(parent, level);
            map.Create();
            FindBallAndPad();
            ChangeCursorState();

            frame = new Timer();
            frame.Interval = 1;
            frame.Tick += Frame_Tick;
            frame.Start();

            parent.KeyDown += Parent_KeyDown1;
            map.PictureField.Click += Parent_Click;
            map.PictureField.MouseMove += Parent_MouseMove1;
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
                    balls.Add(item as Ball);
                if (item is Pad)
                    pad = item as Pad;
            }
        }

        private void Frame_Tick(object sender, EventArgs e)
        {
            List<int> destroyedBlocks = new List<int>();
            if (!onPause)
            {
                pad.SetPosition(cursor.X, pad.Area.Y);

                foreach (var item in balls)
                {
                    for (int i = map.Objects.Count - 1; i >= 0; i--)
                    {
                        if (!(map.Objects[i] is Ball) && map.Objects[i].IfCollision(item))
                        {
                            item.CollisionWith(map.Objects[i]);
                            if (!(map.Objects[i] is Pad))
                                map.Objects.RemoveAt(i);
                        }
                    }

                    if (item.BondedToPad)
                        item.SetPosition(pad.Area.X + pad.Area.Width / 2, pad.Area.Y - 18);
                    else
                        item.Move();
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
            }
        }

        public void Parent_MouseMove1(object sender, MouseEventArgs e)
        {
            cursor = e.Location;
        }
    }
}
