using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace arkanoid
{
    class MainController : IController
    {
        Panel scenePanel; // все элементы привязаны к scenePanel, перемещая её, перемещаем все элементы
        RunController run;
        Form1 parent;
        List<Button> levelButtons;
        List<Label> levelButtonsLabels;
        List<ToolTip> labelsToolTip;
        Level[] levels;


        public MainController(Form1 parent)
        {
            this.parent = parent;

            scenePanel = new Panel()
            {
                Location = parent.ClientRectangle.Location,
                Size = parent.ClientRectangle.Size,
                BackgroundImage = Properties.Resources.background_menu,
                Parent = parent
            };

            run = new RunController(parent, this);
            GetLevels();
            LevelSelectorScreen();

        }


        private void GetLevels()
        {
            string path = Environment.CurrentDirectory.ToString() + "\\Levels";
            int filesCount = Directory.GetFiles(path, "*.dat", SearchOption.TopDirectoryOnly).Length;
            levels = new Level[filesCount];
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = Level.Deserialization(i);
            }
        }

        private void LevelSelectorScreen()
        {
            levelButtons = new List<Button>();
            levelButtonsLabels = new List<Label>();
            labelsToolTip = new List<ToolTip>();
            for (int i = 0; i < levels.Length; i++)
            {
                levelButtons.Add(CreateLevelButton(levels[i], i));
                levelButtonsLabels.Add(CreateLevelButtonLabel(levelButtons.Last(), levels[i]));
                scenePanel.Controls.Add(levelButtons.Last());
                scenePanel.Controls.Add(levelButtonsLabels.Last());
            }

            Button randomLevel = new Button()
            {
                Image = Properties.Resources.RandomLevel,
                Size = new Size(Properties.Resources.RandomLevel.Width + 8, Properties.Resources.RandomLevel.Height + 8),
                Location = DefineButtonLevelLocation(),
                Name = "random",
            };
            randomLevel.Click += levelButtons_Click;
            scenePanel.Controls.Add(randomLevel);
            //scenePanel.Controls.Add(CreateLevelButton(Level.Random(), levels.Length));
        }

        private Label CreateLevelButtonLabel(Button button, Level level)
        {
            Label label = new Label()
            {
                Size = new Size(button.Size.Width, 30),
                Location = new Point(button.Location.X, button.Location.Y + button.Height),
                Text = DefineHighScoreLevel(level),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            label.ContextMenuStrip = new ContextMenuStrip();

            string allHighScoresLabel = "Name\t\tScore\n";
            for (int i = 0; i < level.Leaderboard.Name.Length; i++)
                allHighScoresLabel += level.Leaderboard.Name[i] +
                    "\t\t" + level.Leaderboard.Value[i].ToString() + "\n";

            ToolTip scores = new ToolTip()
            {
                ToolTipTitle = "Scores",
                AutoPopDelay = 0,
            };
            labelsToolTip.Add(scores);

            ToolStripMenuItem eraseHighScores = new ToolStripMenuItem("Erase Scores");
            eraseHighScores.Click += EraseHighScores_Click;

            void EraseHighScores_Click(object sender, EventArgs e)
            {
                level.Leaderboard.Clear();
                level.Serialization();
                UpdateLabel(label, level, scores);
            }

            label.ContextMenuStrip.Items.Add(eraseHighScores);

            scores.SetToolTip(label, allHighScoresLabel);
            return label;
        }

        private void UpdateLabel(Label label, Level level, ToolTip toolTip)
        {
            label.Text = DefineHighScoreLevel(level);
            string allHighScoresLabel = "Name     Score\n";
            for (int i = 0; i < level.Leaderboard.Name.Length; i++)
                allHighScoresLabel += level.Leaderboard.Name[i] +
                    "  " + level.Leaderboard.Value[i].ToString() + "\n";

            toolTip.SetToolTip(label, allHighScoresLabel);
        }

        private string DefineHighScoreLevel(Level level)
        {
            int maxValue = level.Leaderboard.Value.Max();

            for (int i = 0; i < level.Leaderboard.Name.Length; i++)
                if (level.Leaderboard.Value[i] == maxValue)
                    return level.Leaderboard.Name[i] + "           " + maxValue;

            return null;
        }

        private Button CreateLevelButton(Level level, int levelNum)
        {
            Bitmap bitmapField = new Bitmap(level.Background);
            bitmapField.SetResolution(72, 72);
            Point bitmapCoord = new Point();
            Image sprite = Properties.Resources.empty;

            int TileWidth = sprite.Width;
            int TileHeight = sprite.Height;

            for (int i = 0; i < Level.FieldHeight - 3; i++)
            {
                for (int k = 0; k < Level.FieldWidth; k++)
                {
                    if (level.LogicField[i, k] == 0)
                        sprite = Properties.Resources.empty;

                    else if (level.LogicField[i, k] == 1)
                        sprite = Properties.Resources.redBlock;

                    else if (level.LogicField[i, k] == 4)
                        sprite = Properties.Resources.redBonusBallBlock;

                    else if (level.LogicField[i, k] == 5)
                        sprite = Properties.Resources.orangeBlock;

                    else if (level.LogicField[i, k] == 6)
                        sprite = Properties.Resources.orangeBonusBallBlock;

                    else if (level.LogicField[i, k] == 7)
                        sprite = Properties.Resources.yellowBlock;

                    else if (level.LogicField[i, k] == 8)
                        sprite = Properties.Resources.yellowBonusBallBlock;

                    else if (level.LogicField[i, k] == 9)
                        sprite = Properties.Resources.bonusBlock;


                    using (Graphics g = Graphics.FromImage(bitmapField))
                        g.DrawImage(sprite, bitmapCoord.X, bitmapCoord.Y);

                    bitmapCoord.X += TileWidth;
                }
                bitmapCoord = new Point(0, bitmapCoord.Y + TileHeight);
            }

            Bitmap smallImage = new Bitmap(bitmapField, new Size(200, 148));
            Button button = new Button()
            {
                Image = smallImage,
                Size = new Size(smallImage.Width + 8, smallImage.Height + 8),
                Location = DefineButtonLevelLocation(),
                Name = levelNum.ToString(),
            };
            button.Click += levelButtons_Click;
            return button;
        }

        private Point DefineButtonLevelLocation()
        {
            if (levelButtons.Count == 0)
                return new Point(45, 20);

            else if (levelButtons.Last().Right + levelButtons.Last().Width + 25 > parent.ClientRectangle.Right)
                return new Point(levelButtons.First().Left, levelButtons.Last().Bottom + 50);

            return new Point(levelButtons.Last().Right + 50, levelButtons.Last().Top);
        }

        private void levelButtons_Click(object sender, EventArgs e)
        {
            Button cur = sender as Button;
            Hide();
            if (cur.Name != "random")
                run.LoadNew(levels[Convert.ToUInt32(cur.Name)]);
            else
                run.LoadNew(Level.Random());

            run.Show();
        }

        internal void NextRun()
        {
            if (run.Level + 1 < levels.Length)
                run.LoadCont(levels[run.Level + 1]);
            else
                run.LoadCont(levels[0]);
            run.Show();
        }

        public void Show()
        {
            for (int i = 0; i < levelButtons.Count; i++)
                UpdateLabel(levelButtonsLabels[i], levels[i], labelsToolTip[i]);

            scenePanel.Show();
        }

        public void Hide()
        {
            scenePanel.Hide();
        }
    }
}
