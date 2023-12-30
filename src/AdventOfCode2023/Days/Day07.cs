namespace AdventOfCode2023.Days;

public class Day07 : DayBase
{
    // 250946742
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        long total = 0;

        var sortedHands = new List<(HandType type, Hand hand)>();

        var hands = input.Select(i => new Hand(i.Split(" ")[0], int.Parse(i.Split(" ")[1]))).ToList();

        foreach (var hand in hands)
        {
            var handType = GetHandTypeFromHand(hand.Cards);
            sortedHands.Add((handType, hand));
        }

        sortedHands.Sort((x, y) =>
        {
            var typeComparison = y.type.CompareTo(x.type);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            return CompareHands(x.hand.Cards, y.hand.Cards, "23456789TJQKA");
        });

        for (var i = 0; i < sortedHands.Count; i++)
        {
            total += sortedHands[i].hand.Bid * (i + 1);
        }

        return new ValueTask<string>(total.ToString());
    }

    // 251824095
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        long total = 0;

        var sortedHands = new List<(HandType type, Hand hand)>();

        var hands = input.Select(i => new Hand(i.Split(" ")[0], int.Parse(i.Split(" ")[1]))).ToList();

        foreach (var hand in hands)
        {
            var handType = hand.Cards.Contains("J") ? GetHandTypeFromJokerHand(hand.Cards) : GetHandTypeFromHand(hand.Cards);
            sortedHands.Add((handType, hand));
        }

        sortedHands.Sort((x, y) =>
        {
            var typeComparison = y.type.CompareTo(x.type);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            return CompareHands(x.hand.Cards, y.hand.Cards, "J23456789TQKA");
        });

        for (var i = 0; i < sortedHands.Count; i++)
        {
            total += sortedHands[i].hand.Bid * (i + 1);
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private record Hand(string Cards, int Bid);

    private enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard,
    }

    private static  HandType GetHandTypeFromHand(string hand)
    {
        var pairs = hand.GroupBy(c => c).ToList();
        var count = pairs.Select(g => g.Count()).ToList();

        return pairs.Count switch
        {
            1 => // Five of a Kind
                HandType.FiveOfAKind,
            2 => // Full House or Four of a Kind
                count.Contains(4) ? HandType.FourOfAKind : HandType.FullHouse,
            3 => //Three of a Kind or Two Pair
                count.Contains(3) ? HandType.ThreeOfAKind : HandType.TwoPair,
            4 => // One Pair
                HandType.OnePair,
            5 => // High Card
                HandType.HighCard,
        };
    }

    private static HandType GetHandTypeFromJokerHand(string hand)
    {
        var jokerCount = hand.Count(c => c == 'J');
        var handWithoutJ = hand.Replace("J", "");

        var pairs = handWithoutJ.GroupBy(c => c).ToList();
        var count = pairs.Select(g => g.Count()).ToList();

        return pairs.Count switch
        {
            0 => // Five of a Kind
                HandType.FiveOfAKind,
            1 => // Five of a Kind
                HandType.FiveOfAKind,
            2 => // Full House or Four of a Kind
                count.Contains(1) ? HandType.FourOfAKind : HandType.FullHouse,
            3 => //Three of a Kind
                HandType.ThreeOfAKind,
            4 => // One Pair
                HandType.OnePair
        };
    }

    private static int CompareHands(string firstHand, string secondHand, string cardOrder)
    {
        for (var i = 0; i < firstHand.Length; i++)
        {
            var firstCard = firstHand[i];
            var secondCard = secondHand[i];

            var firstIndex = cardOrder.IndexOf(firstCard);
            var secondIndex = cardOrder.IndexOf(secondCard);

            if (firstIndex != secondIndex)
                return firstIndex.CompareTo(secondIndex);
        }

        return 0; // The hands are equal
    }
}
