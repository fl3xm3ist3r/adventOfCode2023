using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day02 : DayBase
{
    //2317
    public override ValueTask<string> Solve_1()
    {
        var games = GetInput(Input.Value);

        var total = games.Select((game, index) => IsGameValid(game) ? index + 1 :  0).Sum();

        return new ValueTask<string>(total.ToString());
    }

    //74804
    public override ValueTask<string> Solve_2()
    {
        var games = GetInput(Input.Value);

        var total = games.Select(GetMinimalGameQubes).Sum();

        return new ValueTask<string>(total.ToString());
    }

    private static List<List<List<string>>>  GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}")
                .Select(gameInput => gameInput.Split(":")[1]
                    .Split(";")
                    .Select(round => round.Split(",").ToList())
                    .ToList())
                .ToList();
    }

    private static bool IsGameValid(List<List<string>> game)
    {
        return game.All(IsGameRoundValid);
    }

    private static bool IsGameRoundValid(List<string> round)
    {
        return round.All(IsQubeValid);
    }

    private static bool IsQubeValid(string qube)
    {
        var number = int.Parse(Regex.Match(qube, @"\d+").Value);

        return !(qube.Contains("red") && number > 12 ||
                qube.Contains("green") && number > 13 ||
                qube.Contains("blue") && number > 14);
    }

    private static int GetMinimalGameQubes(List<List<string>> game) {
        var colors = new[] {"red", "green", "blue"};

        var result = colors.Select(color =>
            game.SelectMany(round => round.Where(qube => qube.Contains(color)))
            .Select(qube => int.Parse(Regex.Match(qube, @"\d+").Value))
            .Max()).ToArray();

        return result[0] * result[1] * result[2];
    }
}
