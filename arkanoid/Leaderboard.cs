using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    [Serializable]
    public class Leaderboard
    {
        public string[] Name { get; set; }
        public int[] Value { get; set; }

        public Leaderboard()
        {
            Name = new string[3];
            Value = new int[3];
        }
    }
}
