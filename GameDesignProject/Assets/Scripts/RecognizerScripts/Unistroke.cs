using System.Collections.Generic;

namespace Recognizer
{
    public class Unistroke
    {
        private string name;
        private List<Point> points;
        private float radians;

        public Unistroke(string name, List<Point> points)
        {
            this.name = name;
            this.points = points;
            radians = MathUtility.IndicativeAngle(this.points);
            this.points = MathUtility.RotateBy(this.points, -radians);
            this.points = MathUtility.ScaleTo(this.points, (float) 250.0);
            this.points = MathUtility.TranslateTo(this.points, new Point((float) 0.0, (float) 0.0));
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public List<Point> Points
        {
            get => points;
            set => points = value;
        }

        public float Radians
        {
            get => radians;
            set => radians = value;
        }
    }
}