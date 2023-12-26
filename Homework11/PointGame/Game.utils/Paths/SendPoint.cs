using System.Drawing;

namespace Game.utils.Paths
{
    public class SendPoint
    {
        public SendPoint(){}
        public SendPoint(Point point, string? color)
        {
            Point = point;
            Color = color;
        }

        public Point Point { get; set; }
        public string? Color { get; set; }
    }
}
