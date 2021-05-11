namespace HigherOrLower
{
    public partial class Game
    {
        // A class to represent a card of a card deck
        private class Card {
            public Colors color;
            public CardDeck cardType;
            public Card(Colors color, CardDeck cardType) {
                this.color = color;
                this.cardType = cardType;
            }

            // lambda function to express the name to appear inside the game
            public string DisplayName => $"{cardType} of {color}";
        }

        private enum Colors {
            hearts      = 1,
            diamonds    = 2,
            clubs       = 3,
            spades      = 4
        }
        private enum CardDeck
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
    }
}
