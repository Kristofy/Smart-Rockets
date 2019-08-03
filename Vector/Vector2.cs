using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector
{
    public class Vector2 : ICloneable
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        private static Random rnd = new Random();

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public void Multiply(float times)
        {
            X *= times;
            Y *= times;
        }

        public void Add(Vector2 toAdd)
        {
            X += toAdd.X;
            Y += toAdd.Y;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float MagnitudeSquare()
        {
            return X * X + Y * Y;
        }

        public Vector2 Normalize()
        {
            float mag = Magnitude();
            return new Vector2 (x: X/mag, y: Y/mag);
        }

        private static float PRECISION = 100000;
        public void MultiplyRandom(float start, float end)
        {
            Multiply(
                rnd.Next(
                    Convert.ToInt32(start * PRECISION),
                    Convert.ToInt32(end * PRECISION)
                    ) / PRECISION
                );
        }

        public object Clone()
        {
            return new Vector2(X, Y);
        }

        public static Vector2 Random()
        {
            float angle = rnd.Next(0, 360);
            float rad = (float)((angle / 180) * Math.PI);
            return new Vector2(
                x: (float)Math.Cos(rad),
                y: (float)Math.Sin(rad)
                );
        }

        public static float Distance(Vector2 A, Vector2 B)
        {
            return (A - B).Magnitude();
        }
        public static float DistanceSquare(Vector2 A, Vector2 B)
        {
            return (A - B).MagnitudeSquare();
        }

        public static Vector2 operator -(Vector2 A)
        {
            return new Vector2(x: -A.X, y: -A.Y);
        }

        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            return new Vector2(x: A.X + B.X, y: A.Y + B.Y); 
        }

        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            return new Vector2(x: A.X - B.X, y: A.Y - B.Y);
        }

        public static Vector2 operator +(Vector2 A, float B)
        {
            return new Vector2(x: A.X + B, y: A.Y + B);
        }

        public static Vector2 operator -(Vector2 A, float B)
        {
            return new Vector2(x: A.X - B, y: A.Y - B);
        }

    }
}
