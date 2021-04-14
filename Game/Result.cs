using System;
namespace HigherOrLower
{
    public class Result
    {
        public readonly int points;
        private bool exited = false;
        public bool HasExited => exited;
        private Highscore hs;

        public Result(int points, Highscore hs)
        {
            this.hs = hs;
            this.points = points;
        }

        public Result Display()
        {
            if (exited)
                return this;

            new Menu($"Game over. Your score was {points}")
                .AddOptions(new string[]
                {
                    "OK"
                })
                .GetOption();

            hs
                .AddData(AskForName(), points)
                .WriteData();

            return this;
        }

        public string AskForName()
        {
            string name = "";
            new Menu("Well played!\nWhat is your name?")
                    .DisplayBox();

            while (name.Length <= 0)
            {
                name = Console.ReadLine();
            }

            return name;

        }

        public Result Exited()
        {
            exited = true;

            return this;
        }
    }
}
