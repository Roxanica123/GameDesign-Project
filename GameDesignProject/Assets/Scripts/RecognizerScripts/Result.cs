using System;
using System.Collections.Generic;
using System.Text;

namespace Recognizer
{
    public class Result
    {
        private string name;
        private float score;

        public Result(string name, float score)
        {
            this.name = name;
            this.score = score;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public float Score
        {
            get => score;
            set => score = value;
        }
    }
}
