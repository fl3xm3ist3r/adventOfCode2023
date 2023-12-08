namespace AdventOfCode2023.Days;

public class Day03 : DayBase
{
    //514969
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var total = 0;

        for (var y = 0; y < input.Count; y++)
        {
            int? start = null;
            int? end = null;
            var digits = "";
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '.')
                    continue;

                if (char.IsNumber(input[y][x]))
                {
                    digits += input[y][x];
                    if (digits.Length <= 1)
                    {
                        start = x;
                        x++;
                    }

                    while (x < input[y].Length && char.IsNumber(input[y][x]))
                    {
                        digits += input[y][x];
                        x++;
                    }

                    end = x - 1;

                    if (HasSymbolAround(start.Value, end ?? start.Value, y, input))
                    {
                        total += int.Parse(digits);
                    }
                    start = null;
                    end = null;
                    digits = "";
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    //78915902
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        var total = 0;

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '*')
                {
                    total += FindGearRatio(y, x, input);
                }
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static bool HasSymbolAround(int left, int right, int row, List<string> map)
    {
        //top row
        var y = row - 1;
        if (y >= 0)
        {
            for (var x = left - 1; x <= right + 1; x++)
            {
                if (x >= 0 && x < map[y].Length)
                {
                    var currentField = map[y][x];
                    if (currentField != '.')
                    {
                        return true;
                    }
                }
            }
        }

        //left
        y = row;
        if (left - 1 >= 0)
        {
            var currentField = map[y][left - 1];
            if (currentField != '.')
            {
                return true;
            }
        }

        //right
        y = row;
        if (right + 1 < map[y].Length)
        {
            var currentField = map[y][right + 1];
            if (currentField != '.')
            {
                return true;
            }
        }

        //bottom row
        y = row + 1;
        if (y < map.Count)
        {
            for (var x = left - 1; x <= right + 1; x++)
            {
                if (x >= 0 && x < map[y].Length)
                {
                    var currentField = map[y][x];
                    if (currentField != '.')
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static int FindGearRatio(int y, int x, List<string> map)
    {
        List<(int y, int x)> sourroundings = new List<(int y, int x)>
        {
            (y - 1, x - 1), (y - 1, x    ), (y - 1, x + 1),
            (y    , x - 1), /*    *     */  (y    , x + 1),
            (y + 1, x - 1), (y + 1, x    ), (y + 1, x + 1)
        };

        var numbers = new List<int>();
        for(int i = 0; i < sourroundings.Count; i++)
        {
            var s = sourroundings[i];
            if (s.y >= 0 && s.y < map.Count &&
               s.x >= 0 && s.x < map[y].Length
               && char.IsNumber(map[s.y][s.x]))
            {
                numbers.Add(FindNumber(s.y, s.x, map, sourroundings));
            }
        }

        if(numbers.Count > 1)
        {
            return numbers.Aggregate((acc, num) => acc * num);
        }
        return 0;
    }

    private static int FindNumber(int y, int x, List<string> map, List<(int y, int x)> sourroundings)
    {
        var leftPart = "";
        var middle = map[y][x];
        var rightPart = "";

        var leftX = x - 1;
        while (leftX >= 0 && char.IsNumber(map[y][leftX]))
        {
            leftPart = map[y][leftX] + leftPart;
            if(sourroundings.Contains((y, leftX)))
            {
                sourroundings.Remove((y, leftX));
            }
            leftX--;
        }

        var rightX = x + 1;
        while (rightX < map[y].Length && char.IsNumber(map[y][rightX]))
        {
            rightPart += map[y][rightX];
            if (sourroundings.Contains((y, rightX)))
            {
                sourroundings.Remove((y, rightX));
            }
            rightX++;
        }


        return int.Parse(leftPart + middle + rightPart);
    }
}
