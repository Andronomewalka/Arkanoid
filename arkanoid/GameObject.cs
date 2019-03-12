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
        public List<Point> Bounds { get; protected set; } // границы объекта
        public Bitmap Texture { get; protected set; } // текстура
        public Rectangle Area { get; protected set; } //отрисовка объекта 
        
        public abstract bool IfCollision(Ball ball);
        protected abstract List<Point> AreaToBounds(Rectangle area); // парсим блок на границы объекта (для более быстрого определения наличия коллизии)
    }
}
