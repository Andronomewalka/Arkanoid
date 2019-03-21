using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    interface IController // интерфейс, который должны реализовывать все контроллеры игры
    {
        void Show();
        void Hide();
    }
}
