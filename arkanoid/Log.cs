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
    static class Log
    {
        public static void Write(RectangleF block, RectangleF ball, Line? line, System.Windows.Vector prevDirection, System.Windows.Vector newDirection)
        {

            using (StreamWriter writer = new StreamWriter(File.Open(Directory.GetCurrentDirectory() + "\\Log.txt", FileMode.Append)))
            {
                writer.WriteLine("Block Rectangle: ");
                writer.WriteLine("    Left= " + block.Left.ToString("N2") + ", Right= " + block.Right.ToString("N2"));
                writer.WriteLine("    Top= " + block.Top.ToString("N2") + ", Bottom= " + block.Bottom.ToString("N2"));
                writer.WriteLine("Ball Rectangle: ");
                writer.WriteLine("    Left= " + ball.Left.ToString("N2") + ", Right= " + ball.Right.ToString("N2"));
                writer.WriteLine("    Top= " + ball.Top.ToString("N2") + ", Bottom= " + ball.Bottom.ToString("N2"));
                writer.WriteLine("Collision Line: ");
                writer.WriteLine("    A.x = " + line.Value.A.X.ToString("N2") + ", A.y = " + line.Value.A.Y.ToString("N2"));
                writer.WriteLine("    B.x = " + line.Value.B.X.ToString("N2") + ", B.y = " + line.Value.B.Y.ToString("N2"));
                writer.WriteLine("normal = x(" + line.Value.A.Y.ToString("N2") + " - " + line.Value.B.Y.ToString("N2") + ")" + " = " + (line.Value.A.Y - line.Value.B.Y).ToString("N2"));
                writer.WriteLine("         y(" + line.Value.B.X.ToString("N2") + " - " + line.Value.A.X.ToString("N2") + ")" + " = " + (line.Value.B.X - line.Value.A.X).ToString("N2"));
                writer.WriteLine("Before collision direction(x,y) = " + prevDirection.X.ToString("N2") + ", " + prevDirection.Y.ToString("N2"));
                writer.WriteLine("New Direction = " + vectorBall(prevDirection) + " - " + "2 * " + normal(line) + " * ((" + vectorBall(prevDirection) + " * " + normal(line) + ") / (" + normal(line) + " * " + normal(line) + "))");
                writer.WriteLine("After collision direction(x,y) = " + newDirection.X.ToString("N2") + " " + newDirection.Y.ToString("N2"));
                writer.WriteLine("______________________________________");
            }
        }

        private static string vectorBall( System.Windows.Vector prevDirection)
        {
            return "(" + prevDirection.X.ToString("N2") + ", " + prevDirection.Y.ToString("N2") + ")";
        }

        private static string normal(Line? line)
        {
            return "(" + (line.Value.A.Y - line.Value.B.Y).ToString("N2") + ", " + (line.Value.B.X - line.Value.A.X).ToString("N2") + ")";
        }
    }
}
