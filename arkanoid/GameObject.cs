using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public abstract class GameObject // все игровые объекты наследуются от этого класса
    {
        protected Point[] bounds; // границы объекта
        protected Rectangle area; // объект целиком
        protected event EventHandler Collision; // событие вызываемое при столкновении
        protected Image texture; // текстура

        protected abstract Point[] AreaToBounds(Rectangle area); // парсим блок на границы объекта (для более быстрого определения наличия коллизии)
        protected abstract Rectangle BoundsToArea(); // обратная история (для отрисовки)
        public virtual void Draw(Bitmap bitmap, int x, int y)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
                g.DrawImage(texture, x, y);
        }
        public virtual void Erase()
        {
            using (Graphics g = Graphics.FromImage(Map.FieldPictures.Image))
            {
                // рисуем задник поверх области area графического представления уровня
                Rectangle area = BoundsToArea();
                g.DrawImage(Properties.Resources.background, area, area, GraphicsUnit.Pixel);
                Map.FieldPictures.Refresh();
            }
        }
    }

    interface Iinteractable // объекты, с которыми может взаимодействовать шарик должны реализовывать этот интерфейс
    {
        bool Contain(Point ball);
    }
}
