using System;
using System.Drawing;
using System.Windows.Forms;

namespace arkanoid
{
    class PauseController : IController
    {
        private MainController mainMenu;
        private RunController run;
        private Panel scenePanel;
        private Form1 parent;
        private Label toMain;
        private Label nextLevel;
        private Label headline;
        private Font font;
        private string headlineText;
        public string HeadlineText
        {
            get { return headlineText; }
            set
            {
                headlineText = value;
                headline.Text = headlineText;
            }
        }

        public PauseController(Form1 parent, MainController mainMenu, RunController run)
        {
            this.parent = parent;
            this.mainMenu = mainMenu;
            this.run = run;
            font = new Font("Rockwell", 16);
            scenePanel = new Panel()
            {
                Size = new Size(300, 300),
                BackgroundImage = Properties.Resources.pauseBackground,
                Parent = parent
            };
            scenePanel.Location = new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
                        parent.ClientSize.Height / 2 - scenePanel.Height / 2);

            headline = new Label()
            {
                Size = new Size(scenePanel.Width, 50),
                TextAlign = ContentAlignment.MiddleRight,
                Font = font,
                BackColor = Color.Transparent
            };

            toMain = new Label()
            {
                Size = new Size(scenePanel.Width, 50),
                Text = "Main menu",
                Font = font,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            toMain.MouseEnter += Label_MouseEnter;
            toMain.MouseLeave += Label_MouseLeave;

            nextLevel = new Label()
            {
                Size = new Size(scenePanel.Width, 50),
                Text = "Next Level",
                Font = font,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            nextLevel.MouseEnter += Label_MouseEnter;
            nextLevel.MouseLeave += Label_MouseLeave;

            scenePanel.Controls.Add(toMain);
            scenePanel.Controls.Add(headline);
            scenePanel.Controls.Add(nextLevel);
            scenePanel.Hide();
            toMain.Location = new Point(toMain.Parent.Size.Width / 2 - toMain.Width / 2, toMain.Parent.Size.Height - toMain.Height);
            toMain.Click += ToMain_Click;
            nextLevel.Location = new Point(toMain.Parent.Size.Width / 2 - toMain.Width / 2, toMain.Parent.Size.Height - toMain.Height - nextLevel.Height);
            nextLevel.Click += NextLevel_Click;
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

        private void NextLevel_Click(object sender, EventArgs e)
        {
            Hide();
            mainMenu.NextRun();
        }

        private void ToMain_Click(object sender, EventArgs e)
        {
            Hide();
            run.Hide();
            mainMenu.Show();
        }

        public void Hide()
        {
            scenePanel.Hide();
        }

        public void Show()
        {
            scenePanel.Show();
        }
    }
}
