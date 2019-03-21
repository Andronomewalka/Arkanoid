using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace arkanoid
{
    static class Log
    {
        public static void Write(RectangleF block, RectangleF ball, Line? line, Vector2 prevDirection, Vector2 newDirection)
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
                writer.WriteLine("New Direction = " + VectorBall(prevDirection) + " - " + "2 * (" + VectorBall(prevDirection) + " * " + Normal(line) + ") * " + Normal(line));
                writer.WriteLine("After collision direction(x,y) = " + newDirection.X.ToString("N2") + " " + newDirection.Y.ToString("N2"));
                writer.WriteLine("______________________________________");
            }
        }

        private static string VectorBall( Vector2 prevDirection)
        {
            return "(" + prevDirection.X.ToString("N2") + ", " + prevDirection.Y.ToString("N2") + ")";
        }

        private static string Normal(Line? line)
        {
            return "(" + (line.Value.A.Y - line.Value.B.Y).ToString("N2") + ", " + (line.Value.B.X - line.Value.A.X).ToString("N2") + ")";
        }

        public static void Write(Line? line, Vector2 normal, Vector2 prevDirection, Vector2 newDirection)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(Directory.GetCurrentDirectory() + "\\Log.txt", FileMode.Append)))
            {
                writer.WriteLine("normal = x(" + line.Value.A.Y.ToString("N2") + " - " + line.Value.B.Y.ToString("N2") + ")" + " = " + (line.Value.A.Y - line.Value.B.Y).ToString("N2"));
                writer.WriteLine("         y(" + line.Value.B.X.ToString("N2") + " - " + line.Value.A.X.ToString("N2") + ")" + " = " + (line.Value.B.X - line.Value.A.X).ToString("N2"));
                writer.WriteLine("(Vector normal)  = " + normal.ToString());
                writer.WriteLine("Before collision direction(x,y) = " + prevDirection.X.ToString("N2") + ", " + prevDirection.Y.ToString("N2"));
                writer.WriteLine("New Direction = " + VectorBall(prevDirection) + " - " + "2 * (" + VectorBall(prevDirection) + " * " + Normal(line) + ") * " + Normal(line));
                writer.WriteLine("After collision direction(x,y) = " + newDirection.X.ToString("N2") + " " + newDirection.Y.ToString("N2"));
                writer.WriteLine("______________________________________");
            }
        }
    }
}
