using System;
using System.Collections.Generic;
using System.Text;

namespace Recognizer
{
    public class Unistroke
    {
        public string Name { get; set; }
        public List<Point> Points { get; set; }
        public float Radians { get; set; }

        public Unistroke(string name, List<Point> points)
        {
            this.Name = name;
            this.Points = points;

            Radians = MathUtility.IndicativeAngle(this.Points);
            this.Points = MathUtility.RotateBy(this.Points, -Radians);
            this.Points = MathUtility.ScaleTo(this.Points, (float) 250.0);
            this.Points = MathUtility.TranslateTo(this.Points, new Point((float) 0.0, (float) 0.0));
        }
    }
}