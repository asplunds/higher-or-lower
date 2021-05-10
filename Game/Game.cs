using System;
using System.Collections.Generic;


namespace HigherOrLower
{
    public partial class Game
    {
        private int round = 0;
        private int deal = 0;
        private const int maxRounds = 4;
        private const int dealsPerRound = 13;
        private int streak = 0;
        readonly private List<Card> deck;
        private int points = 0;
        private Card previous = null;
        internal bool gameOver = false;
        internal bool hasLost = false;
        private readonly Highscore hs;

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
            Card randCard1 = previous is null ? GetRandomCard() : previous;

            const string higher = "Higher";
            const string lower = "Lower";
            const string exit = "Exit to main menu";

            // ask the player for higher or lower
            string streak_ = streak > 1 ? $"Streak: {streak}\r\n" : "";
            string points_ = $"Your points: {points}\r\n";
            string rounds_ = $"Round {round + 1}/{maxRounds} - Deal {deal + 1}/{dealsPerRound}\r\n";
            string dealer_ = $"The card is: {randCard1.DisplayName}";
            string option = new Menu($"{streak_}{points_}{rounds_}{dealer_}")
                .AddOptions(new string[] {
                    higher,
                    lower,
                    exit
                })
                .GetOption();

            if (exit == option) return new Result(0, hs).Exited();

            // check if there are no cards left
            if (deck.Count == 0)
                return new Result(points, hs).HasWon();

            // draw another, hidden card, on which the player has to guess
            Card randCard2 = GetRandomCard();

            // get values of the randomly drawn cards
            int value1 = (int)Enum.Parse(typeof(CardDeck), randCard1.cardType.ToString());
            int value2 = (int)Enum.Parse(typeof(CardDeck), randCard2.cardType.ToString());
            
            // get the value of an ace
            int ace = (int)Enum.Parse(typeof(CardDeck), CardDeck.Ace.ToString()); 

            // ensure next card is the same as the one just drawn
            previous = randCard2;

            // Game over
            if (value1 == value2)
            {
                SameCards(randCard1.DisplayName, randCard2.DisplayName);
            }
            // card is an ace
            else if (value1 == 14)
            {
                CorrectAnswer("higher and lower", randCard2.DisplayName, randCard1.DisplayName);
            }
            else if (option == higher)
            {
                // Card must be higher to be correct
                if (value2 > value1)
                {
                    CorrectAnswer("higher", randCard2.DisplayName, randCard1.DisplayName);
                }
                else
                {
                    WrongAnswer("lower", randCard2.DisplayName, randCard1.DisplayName);
                }
            } else
            {
                // Card must be lower to be correct
                if (value2 < value1)
                {
                    CorrectAnswer("lower", randCard2.DisplayName, randCard1.DisplayName);
                }
                else
                {
                    WrongAnswer("higher", randCard2.DisplayName, randCard1.DisplayName);
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

        private List<Card> GenerateNewDeck()
        {
            List<Card> list = new List<Card>();

            // 13 card types 
            foreach (CardDeck card in Enum.GetValues(typeof(CardDeck)))
                // times 4 colors = 52 cards in total
                foreach (Colors color in Enum.GetValues(typeof(Colors)))
                    list.Add(new Card(color, card));

            return list;
        }
        /*
            Something to note: as the game is played in real life where the deck is shuffled
            is not needed in this implementation because the card can be drawn from anywhere in the deck
            in this program which would be the same as shuffling, this is in contrast in real life where you want
            to draw from the top of the deck. Therefore, shuffling is unnecessarily complex and cpu intensive.
        */
        private Card GetRandomCard()
        {
            int rand = new Random().Next(deck.Count);
            Card card = deck[rand];

            // The drawn card must be discarded from the deck ensuring that card is not drawn again
            deck.RemoveAt(rand);

            return card;
        }

        public static void Tutorial()
        {
            new Menu($@"You begin with a standard deck of 52 cards.
You start off by drawing a card and looking at it. Then you guess if the next card will be higher or lower.
There are 13 draws in 4 rounds.

You gain 1 point by guessing correctly.

The ace acts both value 14 or 1 so you will always gain 1 point however, if you draw a pair
of the same card type you lose the game no matter what.

If you guess correctly for all {maxRounds} rounds you gain 50 points.

After 4 rounds (when there are no cards left) you finish the game. Your objective is to obtain
the most points to climb the leaderboard")
                .EnableNumbering(false)
                .AddOptions(new string[] { "Got it" })
                .GetOption();
        }
    }
}
