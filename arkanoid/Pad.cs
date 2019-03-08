using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    class Pad : Moveable, Iinteractable
    {
        private Image textureRight;
        private Image textureLeft;
        public Pad(Rectangle area)
        {
            texture = Properties.Resources.pad;
            textureLeft = Properties.Resources.pad_left;
            textureRight = Properties.Resources.pad_right;
            this.area = area;
            bounds = AreaToBounds(area);
            dx = 0;
            dy = 0;
            speed = 100f;
        }

        protected override Point[] AreaToBounds(Rectangle area)
        {
            List<Point> res = new List<Point>();
            int indent = 21;
            for (int i = area.Y; i <= area.Y + area.Height; i++)
            {
                for (int k = area.X; k <= area.X + area.Width; k++)
                {
                    if (i <= area.Y + 13)
                    {
                        if ((i == area.Y && k > area.Left + indent && k < area.Right - indent)
                            || (k > area.Left + indent && k <= area.Left + indent + 3)
                                || k >= area.Right - indent - 3 && k < area.Right - indent)
                        {
                            res.Add(new Point(k, i));
                        }
                    }
                    else
                    {
                        if (k == area.Left || k == area.Right)
                        {
                            res.Add(new Point(k, i));
                        }
                    }
                }

                if (i == area.Y)
                    indent -= 4;
                else if (i == area.Y + 1)
                    indent -= 3;
                else if (i >= area.Y + 2 && i <= area.Y + 5)
                    indent -= 2;
                else if (i >= area.Y + 6 && i <= area.Y + 11)
                    indent -= 1;
            }
            return res.ToArray();
        }

        public override void Draw(Bitmap bitmap, int x, int y)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(textureLeft, x, y);
                g.DrawImage(texture, x + Map.TileWidth, y);
                g.DrawImage(textureRight, x + Map.TileWidth * 2, y);
            }
        }

        public override void Erase()
        {
            using (Graphics g = Graphics.FromImage(Map.FieldPictures.Image))
            {
                Rectangle area = BoundsToArea();
                g.DrawImage(Properties.Resources.background, area, area, GraphicsUnit.Pixel);
                Map.FieldPictures.Refresh();
            }
        }

        private void Update()
        {
            using (Graphics g = Graphics.FromImage(Map.FieldPictures.Image))
            {
                Bitmap bitmap = new Bitmap(Map.TileWidth * 3, Map.TileHeight);
                bitmap.SetResolution(72, 72);
                Draw(bitmap, area.X, area.Y);
                g.DrawImageUnscaledAndClipped(bitmap, area);
               // Map.FieldPictures.Refresh();
            }
        }

        protected override Rectangle BoundsToArea()
        {
            return area;
        }

        public bool Contain(Point ball)
        {
            throw new NotImplementedException();
        }

        public override void Move(int dx, int dy)
        {
            Erase(); // обновляем изображение без текстуры
           // Bitmap newPad = new Bitmap(Map.FieldPictures.Image); // фиксируем его 
           // newPad.SetResolution(72, 72);
            area = new Rectangle(area.X + dx, area.Y, area.Width, area.Height);
            bounds = AreaToBounds(area);
            //  Draw(newPad, area.X, area.Y);
            // Map.FieldPictures.Image = newPad;
            Update(); // накладываем на него текстуру

        }
    }
}
