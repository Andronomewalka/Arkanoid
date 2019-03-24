using System.Drawing;
using System.Windows.Forms;

namespace arkanoid
{
    class Stats
    {
        public StatLife Life { get; set; }
        public StatScore Score { get; set; }
        public StatsScoreMultiplier ScoreMultiplier { get; set; }
        public bool AnimationOver { get; private set; }
        private PictureBox pictureField;
        private Font font;
        private SolidBrush brush;

        public Stats(PictureBox pictureField)
        {
            this.pictureField = pictureField;
            pictureField.Paint += PictureField_Paint;
            font = new Font("Arial", 16);
            brush = new SolidBrush(Color.Black);
            Life = new StatLife();
            Score = new StatScore();
            ScoreMultiplier = new StatsScoreMultiplier(new Size(150, 15));
        }

        private void PictureField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle workArea = new Rectangle(700, 550, 100, 25);
            Region prev = g.Clip;
            g.Clip = new Region(workArea);

            //отображение жизни
            if (Life.AnimationDown || Life.AnimationUp)
            {
                Life.Animation(g, font, brush, workArea);
                AnimationOver = false;
            }
            else
            {
                g.DrawString("Lifes: " + Life.Value.ToString(), font, brush, workArea);
                AnimationOver = true;
            }

            // отображение счёта
            workArea = new Rectangle(80, 545, 200, 25);
            g.Clip = new Region(workArea);
            if (Score.AnimationDown)
            {
                Score.Animation(g, font, brush, workArea);
                AnimationOver = false;
            }
            else
            {
                g.DrawString("Score: " + Score.Value.ToString(), font, brush, workArea);
                AnimationOver = true;
            }

            g.Clip = prev;
            g.DrawImage(ScoreMultiplier.Bar.Field, 50, 570);
        }
    }
}
