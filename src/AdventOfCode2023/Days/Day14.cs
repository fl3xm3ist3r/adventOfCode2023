namespace AdventOfCode2023.Days;

public class Day14 : DayBase
{
    //111979
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(e => e.ToCharArray().ToList()).ToList();

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Count; x++)
            {
                if (input[y][x] == 'O')
                {
                    var tmpY = y - 1;

                    if (tmpY < 0 || input[tmpY][x] != '.')
                    {
                        continue;
                    }

                    while (tmpY - 1 >= 0 && input[tmpY - 1][x] == '.')
                    {
                        tmpY--;
                    }

                    input[y][x] = '.';
                    input[tmpY][x] = 'O';
                }
            }
        }

        var total = 0;
        for(var i = 0; i < input.Count; i++)
        {
            var currentRow = input[i];
            var stoneCount = currentRow.Count(c => c == 'O');

            total += stoneCount * (input.Count - i);
        }
        return new ValueTask<string>(total.ToString());
    }

    //102055
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(e => e.ToCharArray().ToList()).ToList();

        var cycles = new Dictionary<string, int>();
        cycles.Add(string.Join(string.Empty, input.Select(e => string.Join(' ', e))).Replace(" ", ""), 0);

        var newLoop = 0;
        for (int i = 0; i < 1000000000; i++)
        {
            input = North(input);
            input = West(input);
            input = South(input);
            input = East(input);

            var stringifiedInput = string.Join(string.Empty, input.Select(e => string.Join(' ', e))).Replace(" ", "");
            if (cycles.TryGetValue(stringifiedInput, out var cycleStart))
            {
                var cycleInterval = i - cycleStart;
                var billionMinusCurrent = 1000000000 - i;
                var leftOver = billionMinusCurrent % cycleInterval;
                newLoop = i + leftOver;
                break;
            }

            cycles.Add(stringifiedInput, i);
        }

        var newInput = GetInput(Input.Value).Select(e => e.ToCharArray().ToList()).ToList();
        for (int i = 0; i < newLoop; i++)
        {
            input = North(newInput);
            input = West(newInput);
            input = South(newInput);
            input = East(newInput);
        }
        input = newInput;

        var total = 0;
        for (var i = 0; i < input.Count; i++)
        {
            var currentRow = input[i];
            var stoneCount = currentRow.Count(c => c == 'O');

            total += stoneCount * (input.Count - i);
        }
        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private List<List<char>> North(List<List<char>> input)
    {
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Count; x++)
            {
                if (input[y][x] == 'O')
                {
                    var tmpY = y - 1;

                    if (tmpY < 0 || input[tmpY][x] != '.')
                    {
                        continue;
                    }

                    while (tmpY - 1 >= 0 && input[tmpY - 1][x] == '.')
                    {
                        tmpY--;
                    }

                    input[y][x] = '.';
                    input[tmpY][x] = 'O';
                }
            }
        }

        return input;
    }

    private List<List<char>> West(List<List<char>> input)
    {
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Count; x++)
            {
                if (input[y][x] == 'O')
                {
                    var tmpX = x - 1;

                    if (tmpX < 0 || input[y][tmpX] != '.')
                    {
                        continue;
                    }

                    while (tmpX - 1 >= 0 && input[y][tmpX - 1] == '.')
                    {
                        tmpX--;
                    }

                    input[y][x] = '.';
                    input[y][tmpX] = 'O';
                }
            }
        }

        return input;
    }

    private List<List<char>> South(List<List<char>> input)
    {
        for (var y = input.Count - 1; y >= 0; y--)
        {
            for (var x = input[0].Count - 1; x >= 0; x--)
            {
                if (input[y][x] == 'O')
                {
                    var tmpY = y + 1;

                    if (tmpY > input.Count - 1 || input[tmpY][x] != '.')
                    {
                        continue;
                    }

                    while (tmpY + 1 < input.Count && input[tmpY + 1][x] == '.')
                    {
                        tmpY++;
                    }

                    input[y][x] = '.';
                    input[tmpY][x] = 'O';
                }
            }
        }

        return input;
    }

    private List<List<char>> East(List<List<char>> input)
    {
        for (var y = input.Count - 1; y >= 0; y--)
        {
            for (var x = input[0].Count - 1; x >= 0; x--)
            {
                if (input[y][x] == 'O')
                {
                    var tmpX = x + 1;

                    if (tmpX > input[0].Count - 1 || input[y][tmpX] != '.')
                    {
                        continue;
                    }

                    while (tmpX + 1 < input[0].Count && input[y][tmpX + 1] == '.')
                    {
                        tmpX++;
                    }

                    input[y][x] = '.';
                    input[y][tmpX] = 'O';
                }
            }
        }

        return input;
    }
}
