using System;
using System.Drawing;
using System.Windows.Forms;

namespace arkanoid
{
    class LeaderboardController : IController
    {
        private Panel scenePanel;
        private Form1 parent;
        public Level Level { get; set; }
        private PauseController pauseMenu;
        private Label cont;
        private ListView leadersList;
        private int score;
        private Label headline;
        private Font font;
        public int Score
        {
            set
            {
                score = value;
            }
        }

        public LeaderboardController(Form1 parent, MainController mainMenu, RunController run)
        {
            this.parent = parent;
            pauseMenu = new PauseController(parent, mainMenu, run);

            font = new Font("Rockwell", 16);

            scenePanel = new Panel()
            {
                Size = new Size(300, 300),
                BackgroundImage = Properties.Resources.pauseBackground,
                Parent = parent
            };
            scenePanel.Location = new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
            parent.ClientSize.Height / 2 - scenePanel.Height / 2);

            scenePanel.Hide();
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            (sender as Label).Image = null;
        }

        private void Label_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).Image = Properties.Resources.selectedItem;
            (sender as Label).ImageAlign = ContentAlignment.MiddleCenter;
        }

        private bool CheckNewHighScore()
        {
            for (int i = 0; i < Level.Leaderboard.Name.Length; i++)
            {
                if (Level.Leaderboard.Value[i] < score)
                    return true;
            }
            return false;
        }

        private void SetListViewItems()
        {
            leadersList = new ListView()
            {
                Size = new Size(scenePanel.Width - 20, 100),
                View = View.Details,
                LabelEdit = false,
                AllowColumnReorder = false,
                AllowDrop = false,
            };
            MakeTransparent(leadersList, 12, 80);

            for (int i = 0; i < Level.Leaderboard.Name.Length; i++)
            {
                ListViewItem item = new ListViewItem((i + 1).ToString());
                if (Level.Leaderboard.Name[i] != null)
                {
                    item.SubItems.Add(Level.Leaderboard.Name[i]);
                    item.SubItems.Add(Level.Leaderboard.Value[i].ToString());
                }
                else
                {
                    item.SubItems.Add("empty");
                    item.SubItems.Add("-");
                }
                leadersList.Items.Add(item);
            }
            leadersList.Columns.Add("#", 20, HorizontalAlignment.Center);
            leadersList.Columns.Add("Name", 150, HorizontalAlignment.Left);
            leadersList.Columns.Add("Score", 105, HorizontalAlignment.Center);
        }

        private void MakeTransparent(Control ctrl, int x, int y)
        {
            Bitmap bMap = new Bitmap(Properties.Resources.pauseBackground);
            Color[,] pixelArray = new Color[ctrl.Width, ctrl.Height];

            for (int i = 0; i < ctrl.Width; i++)
            {
                for (int j = 0; j < ctrl.Height; j++)
                {
                    pixelArray[i, j] = bMap.GetPixel(x + i, y + j);
                }
            }

            Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height);

            for (int i = 0; i < ctrl.Width; i++)
            {
                for (int j = 0; j < ctrl.Height; j++)
                {
                    bmp.SetPixel(i, j, pixelArray[i, j]);
                }
            }

            ctrl.BackgroundImage = bmp;
            ctrl.Location = new Point(x, y);
        }

        private void Cont_Click(object sender, EventArgs e)
        {
            Hide();
            pauseMenu.HeadlineText = "WIN";
            pauseMenu.Show();
        }

        public void Hide()
        {
            scenePanel.Hide();
        }

        public void Show()
        {
                scenePanel.Controls.Clear();

                cont = new Label()
                {
                    Text = "Continue",
                    Size = new Size(scenePanel.Width, 50),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = font
                };
                cont.MouseEnter += Label_MouseEnter;
                cont.MouseLeave += Label_MouseLeave;

                scenePanel.Controls.Add(cont);
                cont.Location = new Point(cont.Parent.Size.Width / 2 - cont.Width / 2, cont.Parent.Size.Height - cont.Height);
                cont.Click += Cont_Click;

                headline = new Label()
                {
                    Size = new Size(scenePanel.Width, 50),
                    Text = "New High Score: " + score.ToString(),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = font
                };
                scenePanel.Controls.Add(headline);

            if (Level.Leaderboard.Name[0] != "random")
            {
                SetListViewItems();
                leadersList.Location = new Point(10, headline.Height);
                scenePanel.Controls.Add(leadersList);


                if (CheckNewHighScore())
                {
                    TextBox textBox = new TextBox()
                    {
                        Size = new Size(scenePanel.Width - 12, 50),
                        Font = font,
                        MaxLength = 8
                    };
                    Label submit = new Label()
                    {
                        Size = new Size(scenePanel.Width / 2, 50),
                        Text = "Submit",
                        BackColor = Color.Transparent,
                        Font = font,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    bool submitClicked = false;
                    submit.Click += Submit_Click;
                    submit.MouseEnter += Label_MouseEnter;
                    submit.MouseLeave += Label_MouseLeave;

                    scenePanel.Controls.Add(textBox);
                    scenePanel.Controls.Add(submit);
                    textBox.Location = new Point(6, leadersList.Location.Y + leadersList.Height + 5);
                    submit.Location = new Point(scenePanel.Width / 4, textBox.Location.Y + textBox.Height + 5);

                    void Submit_Click(object sender, EventArgs e)
                    {
                        if (!submitClicked)
                        {
                            DefineScorePosition(score, textBox.Text, 0);
                            textBox.Enabled = false;
                            submit.Enabled = false;
                            submitClicked = true;
                            scenePanel.Controls.Remove(leadersList);
                            SetListViewItems();
                            leadersList.Location = new Point(10, headline.Height);
                            scenePanel.Controls.Add(leadersList);
                            Level.Serialization();
                        }
                    }
                }
                else
                    headline.Text = "Score: " + score.ToString();
            }
            scenePanel.Show();
        }

        private void DefineScorePosition(int score, string name, int i)
        {
            if (i < 3)
            {
                if (score > Level.Leaderboard.Value[i])
                {
                    DefineScorePosition(Level.Leaderboard.Value[i], Level.Leaderboard.Name[i], i + 1);
                    Level.Leaderboard.Name[i] = name;
                    Level.Leaderboard.Value[i] = score;
                }
                else
                    DefineScorePosition(score, name, i + 1);
            }
        }
    }
}
