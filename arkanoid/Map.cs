using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    public class Map
    {
        enum Obj { block = 1, pad = 2, ball = 3, bonusBallBlock = 4 }
        private Level level; // текущий уровень
        public PictureBox PictureField { get; private set; } // графическое представление уровня
        public List<GameObject> Objects { get; private set; } // список игровых объектов текущего уровня

        private int TileWidth; // размеры тайла
        private int TileHeight;

        public static Rectangle WindowSize { get; private set; }

        public Map(Form1 parent, Level level)
        {
            WindowSize = new Rectangle(
                    new Point(parent.ClientRectangle.X, parent.ClientRectangle.Y),
                    new Size(parent.ClientRectangle.Width, parent.ClientRectangle.Height));
            Objects = new List<GameObject>();

            PictureField = new PictureBox()
            {
                Size = parent.ClientSize,
                Parent = parent
            };
            PictureField.Paint += PictureField_Paint;

            TileWidth = Properties.Resources.block.Width;
            TileHeight = Properties.Resources.block.Height;

            this.level = level;
        }

        private void PictureField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var pBack = new TextureBrush(level.Background);
            g.FillRectangle(pBack, PictureField.ClientRectangle);

            //string vector = "X: " + (Objects.Find((obj) => obj is Ball) as Ball).Direction.X + " \nY: " + (Objects.Find((obj) => obj is Ball) as Ball).Direction.Y;
            //System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            //System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            //float x = 600.0F;
            //float y = 550.0F;
            //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            //g.DrawString(vector, drawFont, drawBrush, x, y, drawFormat);

            foreach (var item in Objects)
                g.DrawImage(item.Texture, item.Area);
        }

        // накладываем текстуры на логическое поле(1 - блок, 2 - платформа)
        public void Create()
        {
            Bitmap bitmapField = new Bitmap(PictureField.Width, PictureField.Height);
            bitmapField.SetResolution(72, 72); // по мере заполнения поля добавляем тайлы в битмап
            Point bitmapCoord = new Point(); // координаты для ориентирования в битмапе

            for (int i = 0; i < level.FieldHeight; i++)
            {
                for (int k = 0; k < level.FieldWidth; k++)
                {
                    if (level.LogicField[i, k] == (int)Obj.block)
                    {
                        Objects.Add(new Block(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight)));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.pad)
                    {
                        // платформа состоит из трёх тайлов
                        Objects.Add(new Pad(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth * 3, TileHeight)));
                        bitmapCoord.X += TileWidth * 2;
                        k += 2;
                    }
                    else if (level.LogicField[i, k] == (int)Obj.ball)
                    {
                        Objects.Add(new Ball(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight)));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.bonusBallBlock)
                    {
                        Objects.Add(new BonusBall(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight)));
                    }
                    bitmapCoord.X += TileWidth;
                }
                bitmapCoord = new Point(0, bitmapCoord.Y + TileHeight);
            }
        }
    }
}
