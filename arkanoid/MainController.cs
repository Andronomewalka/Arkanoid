using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class MainController : IController
    {
        Panel scenePanel; // все элементы привязаны к scenePanel, перемещая её, перемещаем все элементы
        RunController run;
        Form1 parent;
        List<Button> levelButtons;
        Level[] levels;

        public MainController(Form1 parent)
        {
            this.parent = parent;
            scenePanel = new Panel()
            {
                Location = parent.ClientRectangle.Location,
                Size = parent.ClientRectangle.Size,
                BackgroundImage = Properties.Resources.background,
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
            for (int i = 0; i < levels.Length; i++)
            {
                levelButtons.Add(CreateLevelButton(levels[i], i));
                scenePanel.Controls.Add(levelButtons.Last());
            }
        }

        private Button CreateLevelButton(Level level, int levelNum)
        {
            Bitmap bitmapField = new Bitmap(level.Background);
            bitmapField.SetResolution(72, 72);
            Point bitmapCoord = new Point();
            Image sprite = Properties.Resources.empty;

            int TileWidth = sprite.Width;
            int TileHeight = sprite.Height;

            for (int i = 0; i < level.FieldHeight - 5; i++)
            {
                for (int k = 0; k < level.FieldWidth; k++)
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
                Size = smallImage.Size,
                Location = DefineButtonLevelLocation(),
                Name = levelNum.ToString()
            };
            button.Click += levelButtons_Click;
            return button;
        }

        private Point DefineButtonLevelLocation()
        {
            if (levelButtons.Count == 0)
                return new Point(2, 20);

            else if (levelButtons.Last().Right + 148 > parent.ClientRectangle.Right)
                return new Point(levelButtons.First().Left, levelButtons.Last().Bottom + 70);

            return new Point(levelButtons.Last().Right + 2, levelButtons.Last().Top);
        }

        private void levelButtons_Click(object sender, EventArgs e)
        {
            Button cur = sender as Button;
            Hide();
            run.Load(levels[Convert.ToUInt32(cur.Name)]);
            run.Show();
        }

        public void Show()
        {
            scenePanel.Location =
                new Point(scenePanel.Location.X + parent.ClientSize.Width,
                scenePanel.Location.Y);
        }

        public void Hide()
        {
            scenePanel.Location =
                new Point(scenePanel.Location.X - parent.ClientSize.Width,
                scenePanel.Location.Y);
        }
    }
}
