using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public abstract class Moveable : GameObject
    {
        protected int dx; // напралвение x
        protected int dy; // направление y
        protected float speed; // скорость
        public abstract void Move(int dx, int dy);
    }
}
