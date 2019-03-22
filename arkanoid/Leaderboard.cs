using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    public static class Leaderboard
    {
        public struct LeadUnit
        {
            public int Level { get; internal set; }
            public string[] Name { get; internal set; }
            public int[] Value { get; internal set; }
        }

        public static List<LeadUnit> Leaderboards { get; set; }
        private static string path;

        static Leaderboard()
        {
            path = Environment.CurrentDirectory.ToString();
            Leaderboards = new List<LeadUnit>();
            LoadLeaderboard();
        }

        private static void LoadLeaderboard()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path + "\\Leaderboard.dat", FileMode.OpenOrCreate)))
            {
                LeadUnit unit = new LeadUnit()
                {
                    Name = new string[3],
                    Value = new int[3]
                };
                unit.Level = reader.ReadInt32();
                for (int i = 0; i < unit.Name.Length; i++)
                {
                    unit.Name[i] = reader.ReadString();
                    unit.Value[i] = reader.ReadInt32();
                }
                Leaderboards.Add(unit);
            }
        }

        public static LeadUnit ReadForLevel(int level)
        {
            return Leaderboards.Find((obj) => obj.Level == level);
        }

        public static void UpdateLeaderboard()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path + "\\Leaderboards.dat", FileMode.Open)))
            {
                foreach (var item in Leaderboards)
                {
                    writer.Write(item.Level);
                    for (int i = 0; i < item.Name.Length; i++)
                    {
                        writer.Write(item.Name[i]);
                        writer.Write(item.Value[i]);
                    }
                }
            }
        }
    }
}
