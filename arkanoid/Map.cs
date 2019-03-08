using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    public enum Obj { block = 1, pad = 2 }
    public class Map
    {
        public static Form1 Parent { get; private set; }

        private int[,] logicField; // логическое поле игры (загружаем из файла уровня)
        private PictureBox PictureField; // public static PictureBox PictureField { get; private set; } // графическое представление уровня
        private List<GameObject> objects; // список игровых объектов карты

        private Image background = Properties.Resources.background;
        int TileWidth;
        int TileHeight;



        public Map(Form1 parent)
        {
            Parent = parent;

            objects = new List<GameObject>();

            PictureField = new PictureBox()
            {
                Size = parent.ClientSize,
                // BackgroundImage = Properties.Resources.background,
                Parent = parent
            };
            PictureField.Click += MazePicture_Click;
            PictureField.Paint += PictureField_Paint;

            TileWidth = Properties.Resources.block.Width;
            TileHeight = Properties.Resources.block.Height;

            Parent.KeyDown += Parent_KeyDown;

            logicField = new int[10, 10]
            {
                { 1,0,0,0,1,1,1,0,0,1,},
                { 0,1,0,0,0,1,1,0,0,0},
                { 0,0,0,1,1,1,0,0,0,1},
                { 0,1,1,0,1,1,1,0,0,0},
                { 0,0,0,0,1,1,1,0,0,0},
                { 0,1,0,0,1,0,1,0,0,0},
                { 0,0,0,0,0,0,1,0,1,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,0,0,0,0,0,0,0,0,0},
                { 0,2,2,2,0,0,0,0,0,0},
            };
        }

        private void PictureField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var pBack = new TextureBrush(background);
            g.FillRectangle(pBack, PictureField.ClientRectangle);

            foreach (var item in objects)
                g.DrawImage(item.Texture, item.Area);
        }

        private void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                // последний объект в списке всегда платформа
                (objects.Last() as Moveable).Move(1, 0);
                PictureField.Invalidate();
            }
            else if (e.KeyCode == Keys.Left)
            {
                (objects.Last() as Moveable).Move(-1, 0);
                PictureField.Invalidate();
            }
        }

        private void MazePicture_Click(object sender, EventArgs e)
        {
            MouseEventArgs em = e as MouseEventArgs;
            Point point = new Point(em.X, em.Y);

            foreach (var item in objects)
            {
                if (item is Block && (item as Iinteractable).Contain(point))
                {
                    objects.Remove(item);
                    PictureField.Invalidate();
                    return;
                }
            }
        }

        // накладываем текстуры на логическое поле(0 - пустота, 1 - блок)
        public void Create()
        {
            Bitmap bitmapField = new Bitmap(Parent.ClientSize.Width, Parent.ClientSize.Height);
            bitmapField.SetResolution(72, 72); // по мере заполнения поля добавляем тайлы в битмап, после чего записываем его в PictureBox
            Point bitmapCoord = new Point(); // координаты для ориентирования в битмапе

            int height = 10;
            int width = 10;

            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    if (logicField[i, k] == (int)Obj.block)
                    {
                        objects.Add(new Block(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth, TileHeight)));
                    }
                     else if (logicField[i, k] == (int)Obj.pad)
                     {
                         // платформа состоит из трёх тайлов
                         objects.Add(new Pad(new Rectangle(bitmapCoord.X, bitmapCoord.Y, TileWidth * 3, TileHeight)));
                         bitmapCoord.X += TileWidth * 2;
                         k += 2;
                     }
                    bitmapCoord.X += TileWidth;
                }
                bitmapCoord = new Point(0, bitmapCoord.Y + TileHeight);
            }
            // PictureField.Image = bitmapField;
        }


    }
    //partial class Form1
    //{
    //    protected override void Paint(PaintEventArgs e)
    //    {
    //        base.OnPaint(e);
    //        Graphics g = e.Graphics;
    //
    //        var pBack = new TextureBrush(background);
    //        g.FillRectangle(pBack, ClientRectangle);
    //
    //        for (int i = 1; i < 5; i++)
    //        {
    //            g.TranslateTransform(objects[i].x, objects[i].y);
    //            //g.RotateTransform(objects[i].angle);
    //            g.DrawImage(objects[i].pic, -objects[i].width / 2, -objects[i].height / 2, objects[i].width, objects[i].height);
    //            g.ResetTransform();
    //        }
    //    }
    //}
}
