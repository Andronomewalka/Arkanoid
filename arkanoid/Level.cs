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
        public int[,] LogicField { get; private set; } // логическое поле игры (загружаем из файла уровня)
        public Image Background { get; private set; } // задник
        public int FieldWidth { get; private set; } // размеры логического поля
        public int FieldHeight { get; private set; }
        int levelNum;

        private string path;

        public Level(int levelNum)
        {
            path = Environment.CurrentDirectory.ToString() + "\\Levels";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FieldWidth = 18;
            FieldHeight = 18;
            //LogicField = new int[18, 18]
            //{
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0 },
            //    { 0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0 },
            //    { 0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0 },
            //    { 1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1 },
            //    { 1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1 },
            //    { 0,0,1,1,1,1,0,0,0,0,0,0,1,1,1,1,0,0 },
            //    { 0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0 },
            //    { 0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0 },
            //};
            //Background = Properties.Resources.background;
            this.levelNum = levelNum;
            // Serialization();
        }

        private void Serialization()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path + "\\" + levelNum.ToString() + ".dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }
        }

        public Level Deserialization()
        {
            Level current = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(path + "\\" + levelNum.ToString() + ".dat", FileMode.OpenOrCreate))
                {
                    current = formatter.Deserialize(fs) as Level;
                }
            }
            catch (Exception e)
            {

            }
            return current;
        }
    }
}
