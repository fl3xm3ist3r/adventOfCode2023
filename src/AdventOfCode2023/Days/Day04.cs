namespace AdventOfCode2023.Days;

public class Day04 : DayBase
{
    //20407
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var total = 0;

        foreach (var item in input)
        {
            var cardValue = 0;
            var parts = item.Split(":")[1].Split("|");

            var winningNumbers = Array.ConvertAll(parts[0].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);
            var randomNumbers = Array.ConvertAll(parts[1].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);

            foreach (var number in randomNumbers)
            {
                if (winningNumbers.Contains(number))
                {
                    if (cardValue == 0)
                    {
                        cardValue++;
                    }
                    else
                    {
                        cardValue *= 2;
                    }
                }
            }

            total += cardValue;
        }

        return new ValueTask<string>(total.ToString());
    }

    //23806951
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);
        var cards = new List<(int[] winningNumbers, int[] randomNumbers)>();

        foreach (var item in input)
        {
            var parts = item.Split(":")[1].Split("|");

            var winningNumbers = Array.ConvertAll(parts[0].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);
            var randomNumbers = Array.ConvertAll(parts[1].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);

            cards.Add((winningNumbers, randomNumbers));
        }

        var occurrences = new int[cards.Count];
        for (var i = 0; i < cards.Count; i++)
        {
            occurrences[i] = 1;
        }


        for (var i = 0; i < cards.Count; i++)
        {
            var winners = cards[i].randomNumbers.Count(number => cards[i].winningNumbers.Contains(number));

            for (var j = i + 1; j <= i + winners; j++)
            {
                occurrences[j] += occurrences[i];
            }
        }

        return new ValueTask<string>(occurrences.Sum().ToString());
    }


    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }


    //-------------------------------------------------------
    // this would technically work but it is just way to slow and would take ages...
    //-------------------------------------------------------

    //public override ValueTask<string> Solve_2()
    //{
    //    var input = GetInput(Input.Value);

    //    var linkedList = new LinkedList<(int index, int[] winningNumbers, int[] randomNumbers)>();

    //    for (var i = 0; i < input.Count; i++)
    //    {
    //        var parts = input[i].Split(":")[1].Split("|");

    //        var winningNumbers = Array.ConvertAll(parts[0].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);
    //        var randomNumbers = Array.ConvertAll(parts[1].Split(" ").Select(v => v.Trim()).Where(v => v != "").ToArray(), int.Parse);

    //        linkedList.AddLast((i + 1, winningNumbers, randomNumbers));
    //    }

    //    for (var i = 0; i < linkedList.Count; i++)
    //    {
    //        var linkedListItem = GetElementAtIndex(linkedList, i);
    //        var occurrences = 0;

    //        foreach (var number in linkedListItem.randomNumbers)
    //        {
    //            if (linkedListItem.winningNumbers.Contains(number))
    //            {
    //                occurrences++;
    //            }
    //        }

    //        for (var x = occurrences; 0 < x; x--)
    //        {
    //            var item = GetFirstElementWithIndex(linkedList, linkedListItem.index + x);

    //            linkedList.AddAfter(item, new LinkedListNode<(int index, int[] winningNumbers, int[] randomNumbers)>(item.Value));
    //        }
    //    }

    //    return new ValueTask<string>(linkedList.Count.ToString());
    //}

    //private static (int index, int[] winningNumbers, int[] randomNumbers) GetElementAtIndex(LinkedList<(int index, int[] winningNumbers, int[] randomNumbers)> list, int index)
    //{
    //    LinkedListNode<(int index, int[] winningNumbers, int[] randomNumbers)> currentNode = list.First;

    //    for (int i = 0; i < index; i++)
    //    {
    //        currentNode = currentNode.Next;
    //    }

    //    return currentNode.Value;
    //}

    //private static LinkedListNode<(int index, int[] winningNumbers, int[] randomNumbers)> GetFirstElementWithIndex(LinkedList<(int index, int[] winningNumbers, int[] randomNumbers)> list, int index)
    //{
    //    LinkedListNode<(int index, int[] winningNumbers, int[] randomNumbers)> currentNode = list.First;

    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        currentNode = currentNode.Next;

    //        if (currentNode.Value.index == index)
    //        {
    //            break;
    //        }
    //    }

    //    return currentNode;
    //}
}
