
namespace Recognizer
{
    public class Rectangle
    {
        private double x;
        private double y;
        private double width;
        private double height;

        public Rectangle(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public double X
        {
            get => x;
            set => x = value;
        }

        public double Y
        {
            get => y;
            set => y = value;
        }

        public double Width
        {
            get => width;
            set => width = value;
        }

        public double Height
        {
            get => height;
            set => height = value;
        }
    }
}
