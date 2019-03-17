using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{

    public class BonusBlock : Block
    {
        
        public BonusBlock(RectangleF area, int iteration) : base(area, iteration)
        {
            Texture = DefineTexture();
        }

        protected override Bitmap DefineTexture()
        {
            return Properties.Resources.bonusBlock;
        }
    }


    public class BonusBallBlock : Block
    {
        public BonusBallBlock(RectangleF area, int iteration) : base(area, iteration)
        {
            Texture = DefineTexture();
        }

        protected override Bitmap DefineTexture()
        {
            if (Iteration == 1)
                return Properties.Resources.redBonusBallBlock;
            else if (Iteration == 2)
                return Properties.Resources.orangeBonusBallBlock;
            else if (Iteration == 3)
                return Properties.Resources.yellowBonusBallBlock;

            return Properties.Resources.redBonusBallBlock;
        }
    }
}
