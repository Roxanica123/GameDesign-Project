
namespace Recognizer
{
    public class Result
    {
        private string name;
        private double score;

        public Result(string name, double score)
        {
            this.name = name;
            this.score = score;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public double Score
        {
            get => score;
            set => score = value;
        }
    }
}
