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
        //
        // one built-in unistroke per gesture type
        //
        private const string path = "Assets\\Resources\\Unistrokes\\";
        private List<Unistroke> Unistrokes;
        private float AngleRange = MathUtility.Deg2Rad((float) 45.0);
        private float AnglePrecision = MathUtility.Deg2Rad((float) 2.0);
        private float HalfDiagonal = (float) (0.5 * Math.Sqrt(250.0 * 250.0 + 250.0 * 250.0));
        private List<Unistroke> candidates = new List<Unistroke>();
        public DollarRecognizer()
        {
            var checkUnistrokes = JsonConvert.DeserializeObject<List<Unistroke>>(File.ReadAllText($"{path}check"));
            var circleUnistrokes = JsonConvert.DeserializeObject<List<Unistroke>>(File.ReadAllText($"{path}cerc"));
            var lineUnistrokes = JsonConvert.DeserializeObject<List<Unistroke>>(File.ReadAllText($"{path}linie"));

            Unistrokes = new List<Unistroke>()
                .Concat(circleUnistrokes)
                .Concat(checkUnistrokes)
                .Concat(lineUnistrokes)
                .ToList();
        }


        //
        // The $1 Gesture Recognizer API begins here -- 3 methods: Recognize(), AddGesture(), and DeleteUserGestures()
        //
        public Result Recognize(List<Point> points)
        {
            var candidate = new Unistroke("Cerc", points);
            candidates.Add(candidate);
            //File.WriteAllText($"{path}cerc", JsonConvert.SerializeObject(candidates));
            var u = -1;

            var b = Single.PositiveInfinity;
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
                ? new Result("No match.", (float) 0.0)
                : new Result(this.Unistrokes[u].Name, (float) (1.0 - b / HalfDiagonal));
        }

        /*this.AddGesture = function(name, points)
        {
            this.Unistrokes[this.Unistrokes.length] = new Unistroke(name, points); // append new unistroke

            var num = 0;
            for (var i = 0;
                i < this.Unistrokes.length;
                i++)
            {
                if (this.Unistrokes[i].Name == name)
                    num++;
            }

            return num;
        }
        this.DeleteUserGestures = function()
        {
            this.Unistrokes.length = NumUnistrokes; // clear any beyond the original set
            return NumUnistrokes;
        }*/
    }
}