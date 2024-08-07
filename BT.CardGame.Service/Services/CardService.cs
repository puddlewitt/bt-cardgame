using BT.CardGame.Service.Models;

namespace BT.CardGame.Service.Services;

public class CardService : ICardService
{
    private const string JokerToken = "JK";

    private readonly Dictionary<string, Func<CardSuit, Card>> _cardFactory = new()
    {
        { "2", (suit) => new Number("2", suit) },
        { "3", (suit) => new Number("3", suit) },
        { "4", (suit) => new Number("4", suit) },
        { "5", (suit) => new Number("5", suit) },
        { "6", (suit) => new Number("6", suit) },
        { "7", (suit) => new Number("7", suit) },
        { "8", (suit) => new Number("8", suit) },
        { "9", (suit) => new Number("9", suit) },
        { "T", (suit) => new Number("10", suit) },
        { "J", (suit) => new Jack("J", suit) },
        { "Q", (suit) => new Queen("Q", suit) },
        { "K", (suit) => new King("K", suit) },
        { "A", (suit) => new Ace("A", suit) },
        { JokerToken, (_) => new Joker(JokerToken) },
    };

    private readonly Dictionary<char, CardSuit> _validCardSuits = new()
    {
        { 'C', CardSuit.Club },
        { 'D', CardSuit.Diamond },
        { 'H', CardSuit.Heart },
        { 'S', CardSuit.Spade }
    };

    private readonly HashSet<char> _validSeparator = new(new[] { ',', char.MinValue });

    public (int Score, string ErrorMessage) CalculateScore(string cards)
    {
        var parsedHand = ValidateAndParseCards(cards);

        if (!string.IsNullOrEmpty(parsedHand.ErrorMessage))
        {
            return new(0, parsedHand.ErrorMessage);
        }
        
        var score = parsedHand.Cards
            .Aggregate(0, (total, card) =>
            {
                var toAdd = card.CardValue;

                var multiplyBy = card.Suit switch
                {
                    CardSuit.Club => 1,
                    CardSuit.Diamond => 2,
                    CardSuit.Heart => 3,
                    CardSuit.Spade => 4,
                    _ => 1
                };

                var newTotal = total + toAdd * multiplyBy;

                return newTotal;
            });

        var numberOfJokers = parsedHand.Cards
            .Count(c => c is Joker);

        score *= (int)Math.Pow(2, numberOfJokers);

        return new(score, string.Empty);
    }

    private (string ErrorMessage, IEnumerable<Card> Cards) ValidateAndParseCards(string cards)
    {
        var cardPairs = cards
            .Where(char.IsLetterOrDigit);
        
        if (string.IsNullOrWhiteSpace(cards)
            || cards.Length < 2
            || cardPairs.Count() % 2 != 0
            || !cards.Any(char.IsLetterOrDigit)
            || !char.IsLetterOrDigit(cards[^1]))
        {
            return ("Invalid input - check card schema", Array.Empty<Card>()); 
        }
        
        var parsedCards = new Dictionary<string, Card>();
        var parsedJokers = new List<Card>();
        var cardChars = cards.ToCharArray();
        var index = 0;

        while (index + 2 <= cardChars.Length)
        {
            var suitIndex = index + 1;
            var separatorIndex = index + 2;
            var cardValue = cardChars[index].ToString();
            var cardSuit = cardChars[suitIndex];
            var cardSeparator = separatorIndex >= cardChars.Length
                ? char.MinValue
                : cardChars[separatorIndex];
            var cardKey = $"{cardValue}{cardSuit}";
            var isJoker = cardKey == JokerToken;

            if (!_cardFactory.ContainsKey(cardValue) 
                && !isJoker)
            {
                return ("Card not recognised", Array.Empty<Card>());
            }

            if (!_validCardSuits.ContainsKey(cardSuit) 
                && !isJoker)
            {
                return ("Card not recognised", Array.Empty<Card>());
            }

            if (!_validSeparator.Contains(cardSeparator))
            {
                return ("Invalid input string", Array.Empty<Card>());
            }
            
            var suit = isJoker
                ? CardSuit.None
                : _validCardSuits[cardSuit];
            var newCard = isJoker
                ? _cardFactory[JokerToken](suit)
                : _cardFactory[cardValue](suit);
            
            if (newCard is Joker)
            {
                if (parsedJokers.Count == 2)
                {
                    return ("A hand cannot contain more than two Jokers", Array.Empty<Card>());
                }

                parsedJokers.Add(newCard);
            }
            else
            {
                if (!parsedCards.TryAdd(cardKey, newCard))
                {
                    return ("Cards cannot be duplicated", Array.Empty<Card>());
                }
            }

            index += 3;
        }

        var allCards = parsedJokers
            .Concat(parsedCards
                .Select(v => v.Value));

        return (string.Empty, allCards);
    }
}