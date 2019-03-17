using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public enum BonusType { life }
    class Bonus : GameObject, IChangePosition
    {
        public BonusType BonusType { get; set; }
        public Bonus(RectangleF area)
        {
            Area = area;
            BonusType = DefineBonusType();
            RigidBody = DefineRigidBody();
            Body = DefineBody();
            Texture = DefineTexture();
        }
        public BonusType DefineBonusType()
        {
            Random r = new Random();
            return (BonusType)r.Next(0, Enum.GetNames(typeof(BonusType)).Length);
        }

        private Bitmap DefineTexture()
        {
            if (BonusType == BonusType.life)
                return Properties.Resources.life;

            return Properties.Resources.life;
        }

        public void SetPosition(float posX, float posY)
        {
            Area = new RectangleF(posX, posY, Area.Width, Area.Height);
            Body = DefineBody();
            RigidBody = DefineRigidBody();
            LineTexture = DefineLineTexture();
        }

        protected override RectangleF DefineRigidBody()
        {
            return new RectangleF(Area.X + 6, Area.Y + 14, 33, 7);
        }
    }
}
