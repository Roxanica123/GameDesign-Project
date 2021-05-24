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
        private const string path = "Assets\\Resources\\Unistrokes\\";
        public static int NumberOfPoints { get; private set; }
        private readonly double _angleRange = MathUtility.Deg2Rad(45.0);
        private readonly double _anglePrecision = MathUtility.Deg2Rad(2.0);
        private readonly double _halfDiagonal = (0.5 * Math.Sqrt(250.0 * 250.0 + 250.0 * 250.0));
        private readonly List<Unistroke> _unistrokes;
        private List<Unistroke> candidates = new List<Unistroke>();

        public DollarRecognizer()
        {
            NumberOfPoints = 64;
            _unistrokes = LoadUnistrokes("Unistrokes");
            Debug.Log($"Loaded {_unistrokes.Count} unistrokes");
        }

        private List<Unistroke> LoadUnistrokes(String path)
        {
            return Resources.LoadAll(path, typeof(TextAsset))
                .Cast<TextAsset>()
                .SelectMany(file => JsonConvert.DeserializeObject<List<Unistroke>>(file.text))
                .ToList();
        }


        public Result Recognize(List<Point> points)
        {
            var candidate = new Unistroke("star", points);
            // candidates.Add(candidate);
            // File.WriteAllText($"{path}star.json", JsonConvert.SerializeObject(candidates));
            var u = -1;

            double b = Single.PositiveInfinity;
            for (var i = 0;
                i < this._unistrokes.Count;
                i++) // for each unistroke template
            {
                var d = MathUtility.DistanceAtBestAngle(candidate.Points, this._unistrokes[i], -_angleRange, +_angleRange,
                    _anglePrecision); // Golden Section Search (original $1)
                if (d < b)
                {
                    b = d; // best (least) distance
                    u = i; // unistroke index
                }
            }

            return (u == -1)
                ? new Result("No match.", 0.0)
                : new Result(this._unistrokes[u].Name, (1.0 - b / _halfDiagonal));
        }
    }
}