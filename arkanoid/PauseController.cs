﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class PauseController : IController
    {
        MainController mainMenu;
        RunController runMenu;
        private Panel scenePanel;
        private Form1 parent;
        private Button toMain;
        private Label headline;

        public PauseController(Form1 parent, MainController mainMenu, string headlineText)
        {
            this.parent = parent;
            this.mainMenu = mainMenu;

            scenePanel = new Panel()
            {
                Size = new Size(200, 300),
                BackgroundImage = Properties.Resources.background,
                Parent = parent
            };
            scenePanel.Location = new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
                        parent.ClientSize.Height / 2 - scenePanel.Height / 2);

            headline = new Label()
            {
                Text = headlineText
            };


            toMain = new Button()
            {
                Text = "Main menu"
            };

            scenePanel.Controls.Add(toMain);
            scenePanel.Controls.Add(headline);
            scenePanel.Hide();
            toMain.Location = new Point(toMain.Parent.Size.Width / 2 - toMain.Width / 2, 10);
            toMain.Click += ToMain_Click;
        }

        private void ToMain_Click(object sender, EventArgs e)
        {
            Hide();
            runMenu.Hide();
            mainMenu.Show();
        }

        public void Hide()
        {
            scenePanel.Hide();
            //scenePanel.Location =
            //    new Point(parent.ClientSize.Width, parent.ClientSize.Height);
        }

        public void Show()
        {
            scenePanel.Show();
            //scenePanel.Location =
            //    new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
            //    parent.ClientSize.Height / 2 - scenePanel.Height / 2);
            //toMain.Location = new Point(parent.ClientSize.Width / 2 - scenePanel.Width / 2,
            //    parent.ClientSize.Height / 2 - scenePanel.Height / 2);
            //toMain.BringToFront();
        }
    }
}
