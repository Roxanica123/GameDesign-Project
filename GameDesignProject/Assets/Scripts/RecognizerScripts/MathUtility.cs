using System;
using System.Collections.Generic;

namespace Recognizer
{
    public static class MathUtility
    {
        public static double DistanceAtAngle(List<Point> points, Unistroke t, double radians)
        {
            var newpoints = RotateBy(points, radians);
            return PathDistance(newpoints, t.Points);
        }
        public static double DistanceAtBestAngle(List<Point> points, Unistroke T, double  a, double b, double threshold)
        {
            var Phi = 0.5 * (-1.0 + Math.Sqrt(5.0));
            var x1 = Phi * a + (1.0 - Phi) * b;
            var f1 = DistanceAtAngle(points, T, x1);
            var x2 = (1.0 - Phi) * a + Phi * b;
            var f2 = DistanceAtAngle(points, T, x2);
            while (Math.Abs(b - a) > threshold)
            {
                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = Phi * a + (1.0 - Phi) * b;
                    f1 = DistanceAtAngle(points, T, x1);
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = (1.0 - Phi) * a + Phi * b;
                    f2 = DistanceAtAngle(points, T, x2);
                }
            }
            return Math.Min(f1, f2);
        }
        public static double IndicativeAngle(List<Point> points)
        {
            var c = Centroid(points);
            return Math.Atan2(c.Y - points[0].Y, c.X - points[0].X);
        }
        public static List<Point> ScaleTo(List<Point>points, double size) // non-uniform scale; assumes 2D gestures (i.e., no lines)
        {
            var B = BoundingBox(points);
            var newpoints = new List<Point>();
            for (var i = 0; i < points.Count; i++)
            {
                var qx = points[i].X * (size / B.Width);
                var qy = points[i].Y * (size / B.Height);
                newpoints.Add(new Point(qx, qy));
            }
            return newpoints;
        }
        public static List<Point> TranslateTo(List<Point> points, Point pt) // translates points' centroid
        {
            var c = Centroid(points);
            var newpoints = new List<Point>();
            for (var i = 0; i < points.Count; i++)
            {
                var qx = points[i].X + pt.X - c.X;
                var qy = points[i].Y + pt.Y - c.Y;
                newpoints.Add(new Point(qx, qy));
            }
            return newpoints;
        }
        public static List<Point> Resample(List<Point>points, int n)
        {
            var I = PathLength(points) / (n - 1); // interval length
            var D = 0.0;
            var newpoints = new List<Point>();
            newpoints.Add(points[0]);
            for (var i = 1; i < points.Count; i++)
            {
                var d = Distance(points[i - 1], points[i]);
                if ((D + d) >= I)
                {
                    var qx = points[i - 1].X + ((I - D) / d) * (points[i].X - points[i - 1].X);
                    var qy = points[i - 1].Y + ((I - D) / d) * (points[i].Y - points[i - 1].Y);
                    var q = new Point(qx, qy);
                    newpoints.Add(q); // append new point 'q'
                    points.Insert(i, q);
                    //points.splice(i, 0, q); // insert 'q' at position i in points s.t. 'q' will be the next i
                    D = 0.0;
                }
                else D += d;
            }
            if (newpoints.Count == n - 1) // somtimes we fall a rounding-error short of adding the last point, so add it if so
                newpoints[newpoints.Count-1] = new Point(points[points.Count - 1].X, points[points.Count - 1].Y);
            return newpoints;
        }

        public static List<Point> RotateBy(List<Point> points, double radians) // rotates points around centroid
        {
            var c = Centroid(points);
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            var newpoints = new List<Point>();
            for (var i = 0; i < points.Count; i++)
            {
                var qx = (points[i].X - c.X) * cos - (points[i].Y - c.Y) * sin + c.X;

                var qy = (points[i].X - c.X) * sin + (points[i].Y - c.Y) * cos + c.Y;
                newpoints.Add(new Point(qx, qy));
            }

            return newpoints;
        }

        public static Point Centroid(List<Point> points)
        {
            var x = 0.0;
            var y = 0.0;
            for (var i = 0; i < points.Count; i++)
            {
                x += points[i].X;
                y += points[i].Y;
            }

            x /= points.Count;
            y /= points.Count;
            return new Point(x, y);
        }

        public static Rectangle BoundingBox(List<Point> points)
        {
            double minX = Single.PositiveInfinity;
            double maxX = Single.NegativeInfinity;
            double minY = Single.PositiveInfinity;
            double maxY = Single.NegativeInfinity;

            for (var i = 0; i < points.Count; i++)
            {
                minX = Math.Min(minX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
                maxX = Math.Max(maxX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public static double PathDistance(List<Point> pts1, List<Point> pts2)
        {
            var d = 0.0;
            for (var i = 0; i < pts1.Count&& i < pts2.Count; i++) // assumes pts1.length == pts2.length
                d += Distance(pts1[i], pts2[i]);
            return d / pts1.Count;
        }

        public static double PathLength(List<Point> points)
        {
            var d = 0.0;
            for (var i = 1; i < points.Count; i++)
                d += Distance(points[i - 1], points[i]);
            return d;
        }

        public static double Distance(Point p1, Point p2)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double Deg2Rad(double d)
        {
            return (d * Math.PI / 180.0);
        }

    }
}