using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace SmartRockets
{
    public class Circle : IObstacle
    {
        public Vector2 Pos { get; set; }
        public Size WH { get; set; }

        public Circle(Vector2 pos, int size)
        {
            WH = new Size(size, size);
            Pos = pos;
        }

        public float RealDistance(Vector2 point)
        {
            return (Pos - point).Magnitude() - (WH.Width / 2.0f);
        }

        public float Distance(Vector2 point)
        {
            float dist = (Pos - point).Magnitude() - (WH.Width / 2.0f);
            if (dist <= 0)
            {
                return 0;
            }
            return dist * dist;
            //Vector2 dist = (Pos - point);
            //Vector2 dir = dist.Normalize();
            //dir.Multiply(WH.Width / 2);
            //dist -= dir;
            //return dist.MagnitudeSquare();  
        }

        public void Render(Graphics g)
        {
            g.FillEllipse(Brushes.White, Pos.X-WH.Width/2, Pos.Y-WH.Width/2, WH.Width, WH.Height);
        }
    }

    public class Rectangle : IObstacle
    {

        public Vector2 Pos { get; set; }
        public Size WH { get; set; }

        public Rectangle(Vector2 pos,Size size)
        {
            WH = size;
            Pos = pos;
        }


        public float Distance(Vector2 point)
        {
            // link: https://wiki.unity3d.com/index.php/Distance_from_a_point_to_a_rectangle

            //        I   |    II    |  III
            //      ======+==========+======   --yMin
            //       VIII |  IX (in) |  IV
            //      ======+==========+======   --yMax
            //       VII  |    VI    |   V
            //

            if (point.X<Pos.X) { // Region I, VIII, or VII
                if (point.Y<Pos.Y) { // I
                    Vector2 diff = point - new Vector2(Pos.X, Pos.Y);
                    return diff.MagnitudeSquare();
                }
                else if (point.Y > (Pos.Y + WH.Height)) { // VII
                    Vector2 diff = point - new Vector2(Pos.X, (Pos.Y + WH.Height));
                    return diff.MagnitudeSquare();
                }
                else { // VIII
                    return (float)Math.Pow(Pos.X - point.X,2);
                }
            }
            else if (point.X > (Pos.X + WH.Width)) { // Region III, IV, or V
                if (point.Y<Pos.Y) { // III
                    Vector2 diff = point - new Vector2((Pos.X + WH.Width), Pos.Y);
                    return diff.MagnitudeSquare();
                }
                else if (point.Y > (Pos.Y + WH.Height)) { // V
                    Vector2 diff = point - new Vector2((Pos.X + WH.Width), (Pos.Y + WH.Height));
                    return diff.MagnitudeSquare();
                }
                else { // IV
                    return (float)Math.Pow(point.X - (Pos.X + WH.Width),2);
                }
            }
            else { // Region II, IX, or VI
                if (point.Y<Pos.Y) { // II
                    return (float)Math.Pow(Pos.Y - point.Y,2);
                }
                else if (point.Y > (Pos.Y + WH.Height)) { // VI
                    return (float)Math.Pow(point.Y - (Pos.Y + WH.Height),2);
                }
                else { // IX
                    return 0f;
                }
            }
        }


        public void Render(Graphics g)
        {
            g.FillRectangle(Brushes.White, Pos.X, Pos.Y, WH.Width, WH.Height);
        }
    }

}
