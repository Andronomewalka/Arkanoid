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
        public System.Windows.Vector Direction { get; set; }
        protected float speed; // скорость

        public virtual void Move() { }
        public void SetPosition(int posX, int posY)
        {
            Area = new Rectangle(posX - Area.Width / 2, posY, Area.Width, Area.Height);
            Bounds = AreaToBounds(Area);
        }
    }
}
