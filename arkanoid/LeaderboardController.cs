using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class LeaderboardController : IController
    {
        private Panel scenePanel;
        private Form1 parent;
        public Level Level { get; set; }
        private PauseController pauseMenu;
        private Button cont;
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

            scenePanel = new Panel()
            {
                Size = new Size(300, 300),
                BackgroundImage = Properties.Resources.background,
                Parent = parent
            };
            scenePanel.Location = new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
            parent.ClientSize.Height / 2 - scenePanel.Height / 2);


            scenePanel.Hide();

            cont = new Button()
            {
                Text = "Continue",
                Size = new Size(scenePanel.Width, 50)
            };
            scenePanel.Controls.Add(cont);
            cont.Location = new Point(cont.Parent.Size.Width / 2 - cont.Width / 2, cont.Parent.Size.Height - cont.Height);
            cont.Click += Cont_Click;

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
                Size = new Size(scenePanel.Width, 100),
                View = View.Details,
                LabelEdit = false,
                AllowColumnReorder = false,
                AllowDrop = false,
                BackColor = Color.WhiteSmoke
            };


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
            leadersList.Columns.Add("Score", 126, HorizontalAlignment.Center);
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
            headline = new Label()
            {
                Size = new Size(scenePanel.Width, 50),
                Text = "New High Score :" + score.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = font
            };
            scenePanel.Controls.Add(headline);

            SetListViewItems();
            leadersList.Location = new Point(0, headline.Height);
            scenePanel.Controls.Add(leadersList);



            if (CheckNewHighScore())
            {
                TextBox textBox = new TextBox()
                {
                    Size = new Size(scenePanel.Width, 50),
                    Font = font
                };
                Button submit = new Button()
                {
                    Size = new Size(scenePanel.Width / 2, 50),
                    Text = "Submit",
                    BackColor = Color.WhiteSmoke,
                    Font = font
                };
                submit.Click += Submit_Click;
                scenePanel.Controls.Add(textBox);
                scenePanel.Controls.Add(submit);
                textBox.Location = new Point(leadersList.Location.X, leadersList.Location.Y + leadersList.Height + 5);
                submit.Location = new Point(scenePanel.Width / 4, textBox.Location.Y + textBox.Height + 5);

                void Submit_Click(object sender, EventArgs e)
                {
                    for (int i = 0; i < Level.Leaderboard.Name.Length; i++)
                    {
                        if (score > Level.Leaderboard.Value[i])
                        {
                            Level.Leaderboard.Name[i] = textBox.Text;
                            Level.Leaderboard.Value[i] = score;
                            break;
                        }
                    }
                    (sender as Button).Enabled = false;
                    scenePanel.Controls.Remove(leadersList);
                    SetListViewItems();
                    leadersList.Location = new Point(0, headline.Height);
                    scenePanel.Controls.Add(leadersList);
                    Level.Serialization();
                }
            }
            else
                headline.Text = "Score: " + score.ToString();

            scenePanel.Show();
        }
    }
}
