using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    [Serializable]
    public class Level
    {
        public int[,] LogicField { get; private 
                set; } // логическое поле игры (загружаем из файла уровня)
        public Image Background { get; private set; } // задник
        public int FieldWidth { get; private set; } // размеры логического поля
        public int FieldHeight { get; private set; }
        public Leaderboard Leaderboard { get; private set; }

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
                { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                { 0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0 },
                { 0,1,1,1,0,0,5,5,5,5,5,5,0,0,1,1,1,0 },
                { 0,1,1,0,0,0,5,0,0,0,0,5,0,0,0,1,1,0 },
                { 0,1,1,0,0,0,5,0,6,6,0,5,0,0,0,1,1,0 },
                { 0,1,1,0,0,0,5,0,6,6,0,5,0,0,0,1,1,0 },
                { 0,1,1,0,0,0,5,0,0,0,0,5,0,0,0,1,1,0 },
                { 0,1,1,1,0,0,5,5,5,5,5,5,0,0,1,1,1,0 },
                { 0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,0 },
                { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
            };
            Background = Properties.Resources.background_0;
            Serialization(levelNum);
        }

        private void Serialization(int levelNum)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path + "\\" + levelNum.ToString() + ".dat", FileMode.OpenOrCreate))
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
}
