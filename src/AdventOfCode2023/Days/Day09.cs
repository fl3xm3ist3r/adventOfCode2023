namespace AdventOfCode2023.Days;

public class Day09 : DayBase
{
    //1934898178
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(s => s.Split(" ").Select(int.Parse).ToList()).ToList();

        var total = 0;

        foreach (var item in input)
        {
            var steps = GetSteps(item);
            steps.Reverse();

            var currentValue = 0;
            foreach (var step in steps) {
                currentValue += step[^1];
            }

            total += currentValue;
        }

        return new ValueTask<string>(total.ToString());
    }

    //1129
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(s => s.Split(" ").Select(int.Parse).ToList()).ToList();

        var total = 0;

        foreach (var item in input)
        {
            var steps = GetSteps(item);
            steps.Reverse();

            var currentValue = 0;
            foreach (var step in steps)
            {
                currentValue = step[0] - currentValue;
            }

            total += currentValue;
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static List<List<int>> GetSteps(List<int> start)
    {
        var steps = new List<List<int>>();

        var step = start;
        while (step.Any(x => x != step[0]))
        {
            steps.Add(step);
            var difference = new List<int>();
            for(int i = 0; i < step.Count - 1; i++)
            {
                difference.Add(step[i + 1] - step[i]);
            }

            step = difference;
        }

        steps.Add(step);
        return steps;
    }
}
