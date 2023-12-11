namespace AdventOfCode2023.Days;

public class Day08 : DayBase
{
    //17873
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var instructions = input[0].ToCharArray();
        Dictionary<string, Direction> list = input[1].Split($"{Environment.NewLine}").Select(ToDirection).ToDictionary(e => e.Key, e => e.Value);
        var startNode = list.GetValueOrDefault("AAA")!;

        var total = FollowPath(instructions, list, startNode, "ZZZ");

        return new ValueTask<string>(total.ToString());
    }

    // 15746133679061
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        var instructions = input[0].ToCharArray();
        Dictionary<string, Direction> list = input[1].Split($"{Environment.NewLine}").Select(ToDirection).ToDictionary(e => e.Key, e => e.Value);

        var nodesLoops = list.Where(e => e.Key[^1] == 'A').Select(e => e.Value).Select(node => FollowPath(instructions, list, node, "Z", true)).ToList();

        var result = CalculateLeastCommonMultipleOfNumberList(nodesLoops);

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
    }

    private static long FollowPath(char[] instructions, Dictionary<string, Direction> list, Direction startValue, string endCondition, bool part2 = false)
    {
        var steps = 0;

        var nextElement = startValue;
        for (int i = 0; i < instructions.Length;)
        {
            if (nextElement.Key == endCondition && !part2 ||
                nextElement.Key[^1].ToString() == endCondition && part2)
            {
                break;
            }

            if (instructions[i] == 'L')
            {
                nextElement = list.GetValueOrDefault(nextElement.Left)!;
            }
            else if (instructions[i] == 'R')
            {
                nextElement = list.GetValueOrDefault(nextElement.Right)!;
            }

            i++;
            steps++;
            if (i == instructions.Length)
            {
                i = 0;
            }
        }

        return steps;
    }

    private static long CalculateLeastCommonMultipleOfNumberList(List<long> numbers)
    {
        long lcm = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            lcm = LeastCommonMultiple(lcm, numbers[i]);
        }

        return lcm;
    }

    private static long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonMultiple(a, b)) * b;
    }

    private static long GreatestCommonMultiple(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private KeyValuePair<string, Direction> ToDirection(string input)
    {
        var key = input.Split(" = ")[0];

        var tmp = input.Split(" = ")[1].Trim('(', ')').Split(", ");
        var direction = new Direction(key, tmp[0], tmp[1]);

        return new KeyValuePair<string, Direction>(key, direction); ;
    }

    private record Direction(string Key, string Left, string Right);
}
