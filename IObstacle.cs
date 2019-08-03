using System.Drawing;
using Vector;

namespace SmartRockets
{
    public interface IObstacle
    {
        Vector2 Pos { get; set; }
        Size WH { get; set; }

        float Distance(Vector2 point);

        void Render(Graphics g);
    }
}
