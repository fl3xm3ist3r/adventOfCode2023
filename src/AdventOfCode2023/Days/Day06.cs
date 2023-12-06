using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace AdventOfCode2023.Days;

public class Day06 : DayBase
{
    //5133600
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var raceTimes = input[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var raceDistance = input[1].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        var totalSolutions = Enumerable.Repeat(0, raceTimes.Count).ToList(); ;

        for (var i = 0; i < raceTimes.Count; i++)
        {
            var time = raceTimes[i];
            var distance = raceDistance[i];

            for (var x = 0; x < time; x++)
            {
                if (x * (time - x) > distance)
                {
                    totalSolutions[i]++;
                }
            }
        }

        return new ValueTask<string>(totalSolutions.Aggregate((total, next) => total * next).ToString());
    }

    //40651271
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        var raceTime = long.Parse(input[0].Split(":")[1].Replace(" ", ""));
        var raceDistance = long.Parse(input[1].Split(":")[1].Replace(" ", ""));

        var totalSolutions = 0;

        for (var i = 0; i < raceTime; i++)
        {
            if (i * (raceTime - i) > raceDistance)
            {
                totalSolutions++;
            }
        }

        return new ValueTask<string>(totalSolutions.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }
}
