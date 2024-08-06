using BT.CardGame.Service.Models;

namespace BT.CardGame.Service.Services;

public class CardService : ICardService
{
    private const string JokerToken = "JK";

    private readonly Dictionary<string, (int CardValue, CardType CardType)> _validCardValues = new()
    {
        { "2", (2, CardType.Number) },
        { "3", (3, CardType.Number) },
        { "4", (4, CardType.Number) },
        { "5", (5, CardType.Number) },
        { "6", (6, CardType.Number) },
        { "7", (7, CardType.Number) },
        { "8", (8, CardType.Number) },
        { "9", (9, CardType.Number) },
        { "T", (10, CardType.Number) },
        { "J", (11, CardType.Jack) },
        { "Q", (12, CardType.Queen) },
        { "K", (13, CardType.King) },
        { "A", (14, CardType.Ace) },
        { JokerToken, (0, CardType.Joker) },
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

        if (string.IsNullOrEmpty(parsedHand.ErrorMessage))
        {
            var score = parsedHand.Cards
                .Aggregate(0, (total, card) =>
                {
                    var toAdd = card.CardType switch
                    {
                        CardType.Number => card.CardValue,
                        CardType.Jack => 11,
                        CardType.Queen => 12,
                        CardType.King => 13,
                        CardType.Ace => 14,
                        _ => 0
                    };

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
                .Count(c => c.CardType == CardType.Joker);

            score *= (int)Math.Pow(2, numberOfJokers);

            return new(score, parsedHand.ErrorMessage);
        }

        return new(0, parsedHand.ErrorMessage);
    }

    private (string ErrorMessage, IEnumerable<Card> Cards) ValidateAndParseCards(string cards)
    {
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

            if (!_validCardValues.ContainsKey(cardValue))
            {
                if (!isJoker)
                {
                    return ("Card not recognised", Array.Empty<Card>());
                }
            }

            if (!_validCardSuits.ContainsKey(cardSuit))
            {
                if (!isJoker)
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
                Suit = isJoker
                    ? CardSuit.None
                    : _validCardSuits[cardSuit],
                CardType = isJoker
                    ? CardType.Joker
                    : _validCardValues[cardValue].CardType,
                CardValue = _validCardValues.TryGetValue(cardValue, out var card)
                    ? card.CardValue
                    : 0
            };

            if (newCard.CardType == CardType.Joker)
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