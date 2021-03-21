using System.Collections.Generic;

namespace Recognizer
{
    public class Unistroke
    {
        public string Name { get; set; }
        public List<Point> Points { get; set; }
        public double Radians { get; set; }

        public Unistroke(string name, List<Point> points)
        {
            this.Name = name;
            this.Points = points;

            Radians = MathUtility.IndicativeAngle(this.Points);
            this.Points = MathUtility.RotateBy(this.Points, -Radians);
            this.Points = MathUtility.ScaleTo(this.Points, 250.0);
            this.Points = MathUtility.TranslateTo(this.Points, new Point(0.0, 0.0));
        }
    }
}