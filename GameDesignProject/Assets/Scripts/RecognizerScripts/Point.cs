using System;
using System.Collections.Generic;
using System.Text;

namespace Recognizer
{
    public class Point
    {
        private float x;
        private float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X
        {
            get => x;
            set => x = value;
        }

        public float Y
        {
            get => y;
            set => y = value;
        }
    }
}