namespace AdventOfCode2023.Days;

public class Day12 : DayBase
{
    // 7857
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

    /* ------ Disclaimer ------ */
    // Even after several hours and the hints from @Shoedler (https://github.com/shoedler) to use recursion and memoization I couldn't figure out the solution for the second part by myself.
    // So I decided to recode @Rootix's (https://github.com/rootix) solution and understand how it works
    // Reference: https://github.com/rootix/AdventOfCode/commit/57d4320fe87e63444bbedcf0b8b1dc76270eabfd#diff-b73df4600111de4991e27386320fb4db21afe12a42c75f66548104589df7944f
    /* ------------------------ */

    // 28606137449920

    private readonly Dictionary<string, long> _memoizationCache = [];
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        long total = 0;

        foreach (var line in input)
        {
            var springs = MultiplyString(line.Split(' ')[0], '?');
            var groups = MultiplyString(line.Split(' ')[1], ',').Split(',').Select(int.Parse).ToArray();

            total += GenerateMemoizedPossibleCombinations(springs, groups);
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
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

    private static List<string> GenerateCombinations(string baseString)
    {
        List<string> combinations = new List<string>();
        GenerateCombinationsRecursive(baseString.ToCharArray(), 0, combinations);
        return combinations;
    }

    private long GenerateMemoizedPossibleCombinations(string springs, int[] groups)
    {
        var cacheKey = $"{springs},{string.Join(',', groups)}";
        if (_memoizationCache.TryGetValue(cacheKey, out var value))
        {
            return value;
        }

        value = GeneratePossibleCombinations(springs, groups);
        _memoizationCache[cacheKey] = value;

        return value;
    }

    private long GeneratePossibleCombinations(string springs, int[] groups)
    {
        while (true)
        {
            if (groups.Length == 0)
            {
                return !springs.Contains('#') ? 1 : 0; // No groups to match anymore so it's valid if we have no springs left
            }

            if (string.IsNullOrEmpty(springs))
            {
                return 0; // No springs anymore but still groups available so it's not valid
            }

            if (springs.StartsWith('.'))
            {
                springs = springs.Trim('.'); // Remove '.' at start to go to next spring group
                continue;
            }

            if (springs.StartsWith('?'))
            {
                // Use memoization to be faster if the same thing comes again becasue it is "cached"
                return GenerateMemoizedPossibleCombinations("." + springs[1..], groups) + GenerateMemoizedPossibleCombinations("#" + springs[1..], groups); // Go both new Paths
            }

            // We start with a group here so the first char is #

            if (springs.Length < groups[0])
            {
                return 0; // We have no match because springs contains not enough chars for the next group
            }

            if (springs[..groups[0]].Contains('.'))
            {
                return 0; // We have no match because springs contains a '.' inside next group's range
            }

            if (groups.Length > 1)
            {
                if (springs.Length < groups[0] + 1)
                {
                    return 0; // We have no match because springs ends directly after this group and further groups are not included
                }

                if (springs[groups[0]] == '#')
                {
                    return 0; // We have no match because spring is longer than group's range
                }

                springs = springs[(groups[0] + 1)..]; // Skip char after group it's a '.' or a '?'
                groups = groups[1..];
                continue;
            }

            springs = springs[groups[0]..]; // Last group so no checks needed
            groups = groups[1..];
        }
    }

    private static string MultiplyString(string input, char separator, int multiplier = 5)
    {
        var output = input;

        for (int i = 0; i < multiplier - 1; i++)
        {
            output += $"{separator}{input}";
        }

        return output;
    }
}
