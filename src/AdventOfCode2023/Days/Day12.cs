using System.Runtime.InteropServices.JavaScript;

namespace AdventOfCode2023.Days;

public class Day12 : DayBase
{
    //7857
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);
        var total = 0;

        foreach (var line in input)
        {
            var groups = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();
            var possibleCombinations = GenerateCombinations(line.Split(' ')[0]);

            foreach (var possibleCombination in possibleCombinations)
            {
                if (IsValid(possibleCombination, groups))
                {
                    total++;
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    //7857
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);
        var total = 0;

        var count = 0;
        foreach (var line in input)
        {
            var groups = MultiplyString(line.Split(' ')[1], ',').Split(',').Select(int.Parse).ToList();
            var possibleCombinations = GenerateCombinations(MultiplyString(line.Split(' ')[0], '?'));

            foreach (var possibleCombination in possibleCombinations)
            {
                if (IsValid(possibleCombination, groups))
                {
                    total++;
                }
            }

            count++;
            Console.WriteLine(count.ToString());
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static List<string> GenerateCombinations(string baseString)
    {
        List<string> combinations = new List<string>();
        GenerateCombinationsRecursive(baseString.ToCharArray(), 0, combinations);
        return combinations;
    }

    private static void GenerateCombinationsRecursive(char[] baseChars, int index, List<string> combinations)
    {
        if (index == baseChars.Length)
        {
            combinations.Add(new string(baseChars));
            return;
        }

        if (baseChars[index] == '?')
        {
            baseChars[index] = '.';
            GenerateCombinationsRecursive(baseChars, index + 1, combinations);
            baseChars[index] = '#';
            GenerateCombinationsRecursive(baseChars, index + 1, combinations);
            baseChars[index] = '?'; // Reset to '?' for backtracking
        }
        else
        {
            GenerateCombinationsRecursive(baseChars, index + 1, combinations);
        }
    }

    private static bool IsValid(string input, List<int> groups)
    {
        var stringGroups = input.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (stringGroups.Length != groups.Count)
        {
            return false;
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i] != stringGroups[i].Length)
            {
                return false;
            }
        }

        return true;
    }

    private string MultiplyString(string input, char separator, int multiplier = 5)
    {
        var output = input;

        for (int i = 0; i < multiplier - 1; i++)
        {
            output += $"{separator}{input}";
        }

        return output;
    }
}
