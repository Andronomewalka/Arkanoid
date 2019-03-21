using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class RunController : IController
    {
        private Form1 parent;
        private MainController mainMenu;
        private PauseController pauseMenu;
        private Map map;
        private Point cursor;
        private Timer frame;
        private Pad pad;
        private List<Ball> balls;
        private List<Bonus> bonuses;
        private Stats stats;
        private bool onPause;
        private Point mousePoint;

        public RunController(Form1 parent, MainController mainMenu)
        {
            this.parent = parent;
            this.mainMenu = mainMenu;
            pauseMenu = new PauseController(parent, mainMenu, this);
        }

        private void CustomBlockEventSign()
        {
            foreach (var item in map.Objects)
            {
                if (item is Block)
                    item.Collision += Item_Collision;
            }
        }

        private void Item_Collision(object sender, EventArgs e)
        {
            if (sender as BonusBallBlock != null)
            {
                BonusBallBlock cur = sender as BonusBallBlock;
                if (cur.Iteration == 0)
                {
                    // если отправитель события BonusBall - добалвяем шарик на место бонусного блока + 1
                    int newBallIndex = map.Objects.FindIndex((obj) => obj == cur) + 1;
                    map.Objects.Insert(newBallIndex, new Ball(cur.Area) { BondedToPad = false });
                    balls.Add(map.Objects[newBallIndex] as Ball);
                }
            }
            else if (sender as BonusBlock != null)
            {
                BonusBlock cur = sender as BonusBlock;
                int newBonusIndex = map.Objects.FindIndex((obj) => obj == cur) + 1;
                map.Objects.Insert(newBonusIndex, new Bonus(cur.Area));
                bonuses.Add(map.Objects[newBonusIndex] as Bonus);
            }

            if (sender as Block != null)
            {
                stats.Score.Value += 10 * stats.ScoreMultiplier.Multiplier;
                stats.ScoreMultiplier.Multiplier++;
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
                // действия с платформой 

                DefinePadPosition();

                for (int i = bonuses.Count - 1; i >= 0; i--)
                {
                    bonuses[i].SetPosition(bonuses[i].Area.Left, bonuses[i].Area.Top + 3);
                    if (pad.IfCollision(bonuses[i]))
                    {
                        if (bonuses[i].BonusType == BonusType.life)
                            stats.Life.Value++;
                        else
                            pad.DefineBonusType(bonuses[i].BonusType);

                        map.Objects.Remove(bonuses[i]);
                        bonuses.RemoveAt(i);
                    }
                }

                // действия с шариками

                for (int i = balls.Count - 1; i >= 0; i--)
                {
                    if (!balls[i].BondedToPad)
                    {
                        for (int k = map.Objects.Count - 1; k >= 0; k--)
                        {
                            if (map.Objects[k] != balls[i]
                                && !(map.Objects[k] is Bonus)
                                && map.Objects[k].IfCollision(balls[i]))
                            {
                                Line? CollisionLine = map.Objects[k].DefineCollisionLine(balls[i]);
                                if (CollisionLine != null)
                                {

                                    System.Numerics.Vector2 previousDirection = balls[i].Direction;

                                    bool ChangeDirectionResult = balls[i].CollisionWith(map.Objects[k], CollisionLine);
                                    if (!ChangeDirectionResult)
                                        Log.Write(map.Objects[k].RigidBody, balls[i].RigidBody, CollisionLine, previousDirection, balls[i].Direction);

                                    if (map.Objects[k] is Ball)
                                        (map.Objects[k] as Ball).CollisionWith(balls[i], balls[i].DefineCollisionLine(map.Objects[k] as Ball));

                                    else if (map.Objects[k] is Block && (map.Objects[k] as Block).Iteration == 0)
                                        map.Objects.RemoveAt(k);

                                    break;
                                }
                            }
                        }

                        if (balls[i].Area.Top > 600)
                        {
                            if (balls.Count == 1)
                            {
                                balls[i].BondedToPad = true;
                                balls[i].SetPosition(pad.Area.X + pad.Area.Width / 3, pad.Area.Y - 20);
                                balls[i].Direction = new System.Numerics.Vector2(0, -1);
                                stats.Life.Value--;
                            }
                            else if (balls.Count != 0)
                            {
                                map.Objects.Remove(balls[i]);
                                balls.RemoveAt(i);
                            }
                            else
                                return;
                        }
                        else
                            balls[i].Move();
                    }
                    else
                        balls[i].SetPosition(pad.Area.X + pad.Area.Width / 3, pad.Area.Y - 20);
                }
                map.PictureField.Invalidate();
            }
        }

        private void DefinePadPosition()
        {
            pad.SetPosition(cursor.X - pad.Area.Width / 2, pad.Area.Y);
        }

        private void ChangeCursorState()
        {
            if (!onPause)
            {
                Cursor.Hide();
                Cursor.Position = mousePoint;
                Task.Run(() => Cursor.Clip = new Rectangle(
                    new Point(parent.Location.X + 10, parent.Location.Y + 35),
                    new Size(parent.Size.Width - 20, parent.Size.Height - 50)));
            }
            else
            {
                mousePoint = Cursor.Position;
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
                frame.Stop();
                //Hide();
                pauseMenu.Show();
            }
        }

        public void Parent_MouseMove1(object sender, MouseEventArgs e)
        {
            cursor = e.Location;
        }

        public void Load(Level level)
        {
            map = new Map(parent, level);
            map.Create();
            map.PictureField.Location = new Point(parent.ClientRectangle.Right, parent.ClientRectangle.Y);
            stats = new Stats(map.PictureField);
            bonuses = new List<Bonus>();
            FindBallAndPad();
            CustomBlockEventSign();
            ChangeCursorState();
        }

        public void Show()
        {
            frame = new Timer();
            frame.Interval = 5;
            frame.Tick += Frame_Tick;
            map.PictureField.Click += Parent_Click;
            map.PictureField.MouseMove += Parent_MouseMove1;
            frame.Start();

            map.PictureField.Location =
                new Point(map.PictureField.Location.X -
                parent.AnimationKoef, map.PictureField.Location.Y);

            parent.KeyDown += Parent_KeyDown1;
        }


        public void Hide()
        {
            stats = null;
            bonuses = null;
            frame = null;
            balls = null;
            pad = null;
            onPause = false;
            parent.KeyDown -= Parent_KeyDown1;

            map.PictureField.Location =
                new Point(map.PictureField.Location.X +
                parent.AnimationKoef, map.PictureField.Location.Y);
        }
    }
}

