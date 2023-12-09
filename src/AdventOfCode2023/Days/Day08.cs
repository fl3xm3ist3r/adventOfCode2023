namespace AdventOfCode2023.Days;

public class Day08 : DayBase
{
    //17873
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        long total = 0;

        var instructions = input[0].ToCharArray();

        Dictionary<string, Direction> list = input[1].Split($"{Environment.NewLine}").Select(ToDirection).ToDictionary(e => e.Key, e => e.Value);

        var nextElement = list.GetValueOrDefault("AAA")!;

        for (int i = 0; i < instructions.Length;)
        {
            if (nextElement.Key == "ZZZ")
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
            total++;
            if (i == instructions.Length)
            {
                i = 0;
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    // no result after 220 minutes of runtime
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        long total = 0;

        var instructions = input[0].ToCharArray();

        Dictionary<string, Direction> list = input[1].Split($"{Environment.NewLine}").Select(ToDirection).ToDictionary(e => e.Key, e => e.Value);

        var nodes = list.Where(e => e.Key[^1] == 'A').Select(e => e.Value).ToList();

        for (int i = 0; i < instructions.Length;)
        {
            if (nodes.Where(e => e.Key[^1] == 'Z').Count() == nodes.Count)
            {
                break;
            }

            var instruction = instructions[i];
            for (int x = 0; x < nodes.Count; x++)
            {
                if (instruction == 'L')
                {
                    nodes[x] = list.GetValueOrDefault(nodes[x].Left)!;
                }
                else if (instruction == 'R')
                {
                    nodes[x] = list.GetValueOrDefault(nodes[x].Right)!;
                }
            }

            i++;
            total++;
            if (i == instructions.Length)
            {
                i = 0;
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
    }

    private KeyValuePair<string, Direction> ToDirection(string input)
    {
        var key = input.Split(" = ")[0];

        var tmp = input.Split(" = ")[1].Trim('(', ')').Split(", ");
        var direction = new Direction(key, tmp[0], tmp[1].Replace(")\r", ""));

        return new KeyValuePair<string, Direction>(key, direction); ;
    }

    private record Direction(string Key, string Left, string Right);
}
