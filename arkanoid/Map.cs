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
        enum Obj { redBlock = 1, pad = 2, ball = 3, redBonusBallBlock = 4,
            orangeBlock = 5, orangeBonusBallBlock = 6,
        yellowBlock = 7, yellowBonusBallBlock = 8, bonus = 9}
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

            TileWidth = Properties.Resources.redBlock.Width;
            TileHeight = Properties.Resources.redBlock.Height;

            this.level = level;
        }

        private void PictureField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var pBack = new TextureBrush(level.Background);
            g.FillRectangle(pBack, PictureField.ClientRectangle);

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
                    if (level.LogicField[i, k] == (int)Obj.redBlock)
                    {
                        Objects.Add(new Block(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 1));
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
                    else if (level.LogicField[i, k] == (int)Obj.redBonusBallBlock)
                    {
                        Objects.Add(new BonusBallBlock(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight),1));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.orangeBlock)
                    {
                        Objects.Add(new Block(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 2));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.orangeBonusBallBlock)
                    {
                        Objects.Add(new BonusBallBlock(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 2));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.yellowBlock)
                    {
                        Objects.Add(new Block(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 3));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.yellowBonusBallBlock)
                    {
                        Objects.Add(new BonusBallBlock(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 3));
                    }
                    else if (level.LogicField[i, k] == (int)Obj.bonus)
                    {
                        Objects.Add(new BonusBlock(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight), 1));
                    }
                    bitmapCoord.X += TileWidth;
                }
                bitmapCoord = new Point(0, bitmapCoord.Y + TileHeight);
            }
        }
    }
}
