using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace arkanoid
{
    [Serializable]
    public class Level
    {
        public int[,] LogicField { get; private set; } // логическое поле игры (загружаем из файла уровня)
        public Image Background { get; private set; } // задник
        public static int FieldWidth { get; private set; } = 18; // размеры логического поля
        public static int FieldHeight { get; private set; } = 18;
        public int Num { get; private set; }
        public Leaderboard Leaderboard { get; set; }
        private static string path;

        static Level()
        {
            path = Environment.CurrentDirectory.ToString() + "\\Levels";
        }

        public Level(int levelNum)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FieldWidth = 18;
            FieldHeight = 18;
            // 0 - пустота, 1 - красный блок (2 - платформа, 3 - шар, добавляются автоматически)
            // 4 - красный бонус шар блок
            // 5 - оранжевый блок
            // 6 - оранжевый бонус шар блок
            // 7 - желтый блок
            // 8 - желтый бонус шар блок
            // 9 - бонус
            LogicField = new int[18, 18]
            {
                { 0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,1,5,5,5,5,1,0,0,0,0,0,0 },
                { 0,0,0,0,0,1,5,8,8,8,8,5,1,0,0,0,0,0 },
                { 0,0,0,0,1,5,8,5,5,5,5,8,5,1,0,0,0,0 },
                { 0,0,0,0,1,8,8,9,9,9,9,8,8,1,0,0,0,0 },
                { 0,0,0,0,1,5,8,5,5,5,5,8,5,1,0,0,0,0 },
                { 0,0,0,0,0,1,5,8,8,8,8,5,1,0,0,0,0,0 },
                { 0,0,0,0,0,0,1,5,5,5,5,1,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,9,9,0,0,0,0,9,9,0,0,0,0,0 },
                { 0,0,0,0,0,4,4,0,0,0,0,4,4,0,0,0,0,0 },
                { 0,0,0,0,4,4,4,4,0,0,4,4,4,4,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
            };
            Background = Properties.Resources.level_6;
            Num = levelNum;
            Leaderboard = new Leaderboard("empty", 0);
            Serialization(levelNum);
        }

        private Level()
        {
            LogicField = new int[18, 18];
            Random r = new Random();
            for (int i = 0; i < FieldHeight - 6; i++)
            {
                for (int k = 0; k < FieldWidth; k++)
                {
                    int res = r.Next(0, 10);
                    if (res == 2 || res == 3)
                        LogicField[i, k] = 0;
                    else
                        LogicField[i, k] = res;
                }
            }

            LogicField[LogicField.GetLength(0) - 2, LogicField.GetLength(1) - 3] = 2;
            LogicField[LogicField.GetLength(0) - 2, LogicField.GetLength(1) - 2] = 2;
            LogicField[LogicField.GetLength(0) - 2, LogicField.GetLength(1) - 1] = 2;
            LogicField[LogicField.GetLength(0) - 3, LogicField.GetLength(1) - 2] = 3;

            int backgroundId = r.Next(1, 7);
            if (backgroundId == 1)
                Background = Properties.Resources.level_1;
            else if (backgroundId == 2)
                Background = Properties.Resources.level_2;
            else if (backgroundId == 3)
                Background = Properties.Resources.level_3;
            else if (backgroundId == 4)
                Background = Properties.Resources.level_4;
            else if (backgroundId == 5)
                Background = Properties.Resources.level_5;
            else if (backgroundId == 6)
                Background = Properties.Resources.level_6;
            Leaderboard = new Leaderboard("empty", 0);
            Leaderboard.Name[0] = "random";
        }

        internal static Level Random()
        {
            return new Level();
        }

        private void Serialization(int levelNum)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path + "\\" + levelNum.ToString() + ".dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }
        }

        public void Serialization()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path + "\\" + Num.ToString() + ".dat", FileMode.Truncate))
            {
                formatter.Serialize(fs, this);
            }
        }

        public static Level Deserialization(int levelNum)
        {
            Level current = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path + "\\" + levelNum.ToString() + ".dat", FileMode.OpenOrCreate))
            {
                current = formatter.Deserialize(fs) as Level;
            }
            current.LogicField[current.LogicField.GetLength(0) - 2, current.LogicField.GetLength(1) - 3] = 2;
            current.LogicField[current.LogicField.GetLength(0) - 2, current.LogicField.GetLength(1) - 2] = 2;
            current.LogicField[current.LogicField.GetLength(0) - 2, current.LogicField.GetLength(1) - 1] = 2;
            current.LogicField[current.LogicField.GetLength(0) - 3, current.LogicField.GetLength(1) - 2] = 3;

            //current.LogicField[0, 0] = 2;
            //current.LogicField[0, 1] = 2;
            //current.LogicField[0, 2] = 2;
            //current.LogicField[0, 1] = 3;
            //current.LogicField[0, 0] = 9;
            //current.LogicField[1, 0] = 9;
            //current.LogicField[2, 0] = 9;
            //current.LogicField[3, 0] = 9;

            return current;
        }
    }

    [Serializable]
    public class Leaderboard
    {
        public string[] Name { get; set; }
        public int[] Value { get; set; }

        public Leaderboard(string name, int value)
        {
            Name = new string[3];
            Value = new int[3];
            for (int i = 0; i < 3; i++)
            {
                Name[i] = name;
                Value[i] = value;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                Name[i] = "empty";
                Value[i] = 0;
            }
        }
    }
}
