using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;


namespace Recognizer
{
    public class DollarRecognizer
    {
        private List<Unistroke> Unistrokes;
        public static int NumberOfPoints { get; private set; }
        private double AngleRange = MathUtility.Deg2Rad(45.0);
        private double AnglePrecision = MathUtility.Deg2Rad(2.0);
        private double HalfDiagonal = (0.5 * Math.Sqrt(250.0 * 250.0 + 250.0 * 250.0));

        public DollarRecognizer()
        {
            NumberOfPoints = 64;
            Unistrokes = LoadUnistrokes("Unistrokes");
            Debug.Log($"Loaded {Unistrokes.Count} unistrokes");
        }

        private List<Unistroke> LoadUnistrokes(String path)
        {
            TextAsset[] jsonFiles = Resources.LoadAll(path, typeof(TextAsset))
                .Cast<TextAsset>()
                .ToArray();
            return jsonFiles
                .SelectMany(file => JsonConvert.DeserializeObject<List<Unistroke>>(file.text))
                .ToList();
        }


        public Result Recognize(List<Point> points)
        {
            var candidate = new Unistroke("", points);
            var u = -1;

            double b = Single.PositiveInfinity;
            for (var i = 0;
                i < this.Unistrokes.Count;
                i++) // for each unistroke template
            {
                var d = MathUtility.DistanceAtBestAngle(candidate.Points, this.Unistrokes[i], -AngleRange, +AngleRange,
                    AnglePrecision); // Golden Section Search (original $1)
                if (d < b)
                {
                    b = d; // best (least) distance
                    u = i; // unistroke index
                }
            }

            return (u == -1)
                ? new Result("No match.", 0.0)
                : new Result(this.Unistrokes[u].Name, (1.0 - b / HalfDiagonal));
        }
    }
}