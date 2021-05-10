using System;
namespace HigherOrLower
{
    public class Result
    {
        public readonly int points;
        private bool exited = false;
        public bool HasExited => exited;
        private bool hasWon = false;
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

            string message = hasWon ? "Game won!" : "Game over.";

            new Menu($"{message} Your score was {points}")
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

            while (name.Length <= 0)
            {
                Console.Clear();
                new Menu("Well played!\r\nWhat is your name?")
                    .DisplayBox();
                name = Console.ReadLine();
            }

            return name;

        }

        public Result HasWon() {
            hasWon = true;
            return this;
        }

        public Result Exited()
        {
            exited = true;

            return this;
        }
    }
}
