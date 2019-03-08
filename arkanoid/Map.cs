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
        enum Obj { block = 1, pad = 2 }
        private Level level; // текущий уровень
        public PictureBox PictureField { get; private set; } // графическое представление уровня
        public List<GameObject> Objects { get; private set; } // список игровых объектов текущего уровня

        private int TileWidth; // размеры тайла
        private int TileHeight;



        public Map(Form1 parent, Level level)
        {
            Objects = new List<GameObject>();

            PictureField = new PictureBox()
            {
                Size = parent.ClientSize,
                Parent = parent
            };
            PictureField.Click += MazePicture_Click;
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

            foreach (var item in Objects)
                g.DrawImage(item.Texture, item.Area);
        }

        private void MazePicture_Click(object sender, EventArgs e)
        {
            MouseEventArgs em = e as MouseEventArgs;
            Point point = new Point(em.X, em.Y);

            foreach (var item in Objects)
            {
                if (item is Block && (item as Iinteractable).Contain(point))
                {
                    Objects.Remove(item);
                    PictureField.Invalidate();
                    return;
                }
            }
        }

        // накладываем текстуры на логическое поле(0 - пустота, 1 - блок)
        public void Create()
        {
            Bitmap bitmapField = new Bitmap(PictureField.Width, PictureField.Height);
            bitmapField.SetResolution(72, 72); // по мере заполнения поля добавляем тайлы в битмап, после чего записываем его в PictureBox
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
                    bitmapCoord.X += TileWidth;
                }
                bitmapCoord = new Point(0, bitmapCoord.Y + TileHeight);
            }
        }
    }
}
