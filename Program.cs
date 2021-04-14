using System;
using HigherOrLower;

namespace HigherOrLower
{
    class Program
    {
        private static bool exit = false;
        private struct MainMenu
        {
            public const string NEW_GAME = "New Game";
            public const string HIGHSCORES = "Highscores";
            public const string HOW_TO_PLAY = "How to play";
            public const string EXIT = "Quit";
        }

        static void Main(string[] args)
        {

            Highscore hs = new Highscore("./highscores.json");

            while (!exit)
            {
                // Display main menu
                string mainMenuOption = new Menu("Higher or Lower")
                    .AddOptions(new string[] {
                        MainMenu.NEW_GAME,
                        MainMenu.HIGHSCORES,
                        MainMenu.HOW_TO_PLAY,
                        MainMenu.EXIT
                    })
                    .GetOption();

                switch (mainMenuOption)
                {
                    case MainMenu.HOW_TO_PLAY:
                        {
                            Game.Tutorial();
                            break;
                        }
                    case MainMenu.HIGHSCORES:
                        {
                            hs.Display();
                            break;
                        }
                    case MainMenu.EXIT:
                        {
                            exit = true;
                            break;
                        }
                    case MainMenu.NEW_GAME:
                        {
                            Game game = new Game(hs);

                            game.Start();
                            break;
                        }
                }
            }
        }
    }
}
