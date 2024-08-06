using BT.CardGame.Service.Models;

namespace BT.CardGame.Service.Services;

public class CardService : ICardService
{
    private readonly Dictionary<char, int> _validCardValues = new()
    {
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
    };
    private readonly Dictionary<string, CardType> _validCardTypes = new()
    {
        { "J", CardType.Jack },
        { "Q", CardType.Queen },
        { "K", CardType.King },
        { "A", CardType.Ace },
        { JokerToken, CardType.Joker },
    };
    private readonly Dictionary<char, CardSuit> _validCardSuits = new()
    {
        { 'C', CardSuit.Club },
        { 'D', CardSuit.Diamond },
        { 'H', CardSuit.Heart },
        { 'S', CardSuit.Spade }
    };
    private readonly HashSet<char> _validSeparator = new(new[] { ',', char.MinValue });
    private const string JokerToken = "JK";

    public (int Score, string ErrorMessage) CalculateScore(string cards)
    {
        var validCards = ValidateCards(cards);

        return new(0, validCards.ErrorMessage);
    }

    private (string ErrorMessage, IEnumerable<Card> Cards) ValidateCards(string cards)
    {
        var parsedCards = new Dictionary<string, Card>();
        var parsedJokers = new List<Card>();
        var cardChars = cards.ToCharArray();
        var index = 0;

        while (index + 2 <= cardChars.Length)
        {
            var suitIndex = index + 1;
            var separatorIndex = index + 2;
            var cardValue = cardChars[index];
            var cardSuit = cardChars[suitIndex];
            var cardSeparator = separatorIndex >= cardChars.Length
                ? char.MinValue
                : cardChars[separatorIndex];
            var cardKey = $"{cardValue}{cardSuit}";

            if (!_validCardValues.ContainsKey(cardValue))
            {
                if (!_validCardTypes.ContainsKey(cardKey))
                {
                    return ("Card not recognised", Array.Empty<Card>());
                }
            }

            if (!_validCardSuits.ContainsKey(cardSuit))
            {
                if (!_validCardTypes.ContainsKey(cardKey))
                {
                    return ("Card not recognised", Array.Empty<Card>());
                }
            }

            if (!_validSeparator.Contains(cardSeparator))
            {
                return ("Invalid input string", Array.Empty<Card>());
            }

            var newCard = new Card()
            {
                Suit = cardKey == JokerToken 
                    ? CardSuit.None
                    : _validCardSuits[cardSuit],
                Type = _validCardValues.ContainsKey(cardValue)
                    ? CardType.Number
                    : cardKey == JokerToken
                        ? CardType.Joker
                        : _validCardTypes[cardValue.ToString()],
                Value = _validCardValues.GetValueOrDefault(cardValue, 0)
            };
            
            if (newCard.Type == CardType.Joker)
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