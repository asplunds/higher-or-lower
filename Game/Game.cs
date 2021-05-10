using System;
using System.Collections.Generic;


namespace HigherOrLower
{
    public class Game
    {
        public enum CardDeck
        {
            two     = 2,
            three   = 3,
            four    = 4,
            five    = 5,
            six     = 6,
            seven   = 7,
            eight   = 8,
            nine    = 9,
            ten     = 10,
            Jack    = 11,
            Queen   = 12,
            King    = 13,
            Ace     = 14
        };
        private int round = 0;
        private int deal = 0;
        private const int maxRounds = 4;
        private const int dealsPerRound = 13;
        private int streak = 0;
        readonly private List<string> deck;
        private int points = 0;
        private string previous = null;
        internal bool gameOver = false;
        internal bool hasLost = false;
        private Highscore hs;

        public Game(Highscore hs)
        {
            this.hs = hs;
            deck = GenerateNewDeck();
        }

        public Game Start()
        {
            Console.Clear();

            Console.WriteLine("Higher or Lower");

            Result result = DoRounds();

            result.Display();

            return this;
        }

        private Result DoRounds()
        {
            for (int i = 0; i < maxRounds; i++)
            {
                deal = 0;
                streak = 0;
                for (int j = 0; j < dealsPerRound; j++)
                {
                    Result result = Ask();

                    if (gameOver || result.HasExited) return result;

                    deal++;
                }
                if (streak == maxRounds)
                    points += 50;
                round++;
            }

            return new Result(points, hs);
        }

        private Result Ask()
        {
            // draw a random card
            string randCard1 = previous is null ? GetRandomCard() : previous;

            const string higher = "Higher";
            const string lower = "Lower";
            const string exit = "Exit to main menu";

            // ask the player for higher or lower
            string streak_ = streak > 1 ? $"Streak: {streak}\n" : "";
            string points_ = $"Your points: {points}\n";
            string rounds_ = $"Round {round + 1}/{maxRounds} - Deal {deal + 1}/{dealsPerRound}\n";
            string dealer_ = $"The card is: {randCard1}\n";
            string option = new Menu($"{streak_}{points_}{rounds_}{dealer_}")
                .AddOptions(new string[] {
                    higher,
                    lower,
                    exit
                })
                .GetOption();

            if (exit == option) return new Result(0, hs).Exited();

            // draw another, hidden card, on which the player has to guess
            string randCard2 = GetRandomCard();

            // get values of the randomly drawn cards
            int value1 = (int)Enum.Parse(typeof(CardDeck), randCard1);
            int value2 = (int)Enum.Parse(typeof(CardDeck), randCard2);

            // ensure next card is the same as the one just drawn
            previous = randCard2;

            // Game over
            if (value1 == value2)
            {
                SameCards(randCard1, randCard2);
            }
            // card is an ace
            else if (value1 == 14)
            {
                CorrectAnswer("higher and lower", randCard2, randCard1);
            }
            else if (option == higher)
            {
                // Card must be higher to be correct
                if (value2 > value1)
                {
                    CorrectAnswer("higher", randCard2, randCard1);
                }
                else
                {
                    WrongAnswer("lower", randCard2, randCard1);
                }
            } else
            {
                // Card must be lower to be correct
                if (value2 < value1)
                {
                    CorrectAnswer("lower", randCard2, randCard1);
                }
                else
                {
                    WrongAnswer("higher", randCard2, randCard1);
                }
            }
            ConfirmProceed();

            return new Result(points, hs);

        }

        private void SameCards(string card1, string card2)
        {
            Console.WriteLine($"Oh no! The drawn card was {card1} which is the same as {card2}. Game over.");
            gameOver = true;
            hasLost = true;
        }

        private void CorrectAnswer(string comparison, string card1, string card2)
        {
            Console.WriteLine($"You guessed correct! The card was {card1} which is {comparison} than {card2}");
            points++;
            streak++;
        }

        private void WrongAnswer(string comparison, string card1, string card2)
        {
            Console.WriteLine($"You guessed wrong. The card was {card1} which is {comparison} than {card2}");
            streak = 0;
        }


        static public void ConfirmProceed()
        {
            Console.WriteLine("Press any key to proceed . . .");
            Console.ReadKey();
        }

        private List<string> GenerateNewDeck()
        {
            var cards = Enum.GetValues(typeof(CardDeck));
            var list = new List<string>();

            foreach(CardDeck card in cards)
                for (int i = 0; i < 4; i++)
                    list.Add(card.ToString());

            return list;
        }

        private string GetRandomCard()
        {
            int rand = new Random().Next(deck.Count);
            string value = deck[rand];

            // The drawn card must be discarded from the deck ensuring that card is not drawn again
            deck.RemoveAt(rand);

            return value;
        }

        public static void Tutorial()
        {
            new Menu("You begin with a standard deck of 52 cards. You start off by drawing a card and looking at it. Then you guess if the next card will be higher or lower.\nThere are 13 draws in 4 rounds.\nYou gain 1 point by guessing correctly, if you guess correctly all 13 draws you gain 50 points.\nThe ace acts both valur 14 or 1 so you will always gain 1 point however if you draw a pair you lose the game no matter what.")
                .EnableNumbering(false)
                .AddOptions(new string[] { "Got it" })
                .GetOption();
        }
    }
}
