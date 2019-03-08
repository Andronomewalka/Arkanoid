using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arkanoid
{
    class RunController
    {
        Form1 parent;
        Map map;
        Level level;

        public RunController(Form1 parent, int levelNum)
        {
            this.parent = parent;
            level = new Level(levelNum).Deserialization();
            map = new Map(parent, level);
            map.Create();
            parent.KeyDown += Parent_KeyDown;
        }

        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Parent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                // последний объект в списке всегда платформа
                (map.Objects.Last() as Moveable).Move(1, 0);
                map.PictureField.Invalidate();
            }
            else if (e.KeyCode == Keys.Left)
            {
                (map.Objects.Last() as Moveable).Move(-1, 0);
                map.PictureField.Invalidate();
            }
        }
    }
}
