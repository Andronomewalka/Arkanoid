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
        protected event EventHandler Collision; // событие вызываемое при столкновении


        protected Rectangle area;
        public Bitmap Texture { get; protected set; } // текстура
        public Rectangle Area => area; //отрисовка объекта 
        

        protected abstract Point[] AreaToBounds(Rectangle area); // парсим блок на границы объекта (для более быстрого определения наличия коллизии)
    }

    interface Iinteractable // объекты, с которыми может взаимодействовать шарик должны реализовывать этот интерфейс
    {
        bool Contain(Point ball);
    }
}
