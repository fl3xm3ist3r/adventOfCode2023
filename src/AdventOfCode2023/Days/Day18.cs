using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day18 : DayBase
{
    // 46394
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(l => ToPath(l)).ToList();

        var (map, startPoint) = InitMap(input);
        var currentCoordinate = startPoint;
        map[currentCoordinate.Y][currentCoordinate.X] = '#';

        foreach (var path in input)
        {
            for (int i = 0; i < path.stepCount; i++)
            {
                switch (path.direction)
                {
                    case Direction.DOWN:
                        currentCoordinate.Y++;
                        break;
                    case Direction.UP:
                        currentCoordinate.Y--;
                        break;
                    case Direction.LEFT:
                        currentCoordinate.X--;
                        break;
                    case Direction.RIGHT:
                        currentCoordinate.X++;
                        break;
                }

                map[currentCoordinate.Y][currentCoordinate.X] = '#';
            }
        }

        var innerHashTagCount = InnerHashTagCount(map);

        var outerHashTagCount = map.Sum(l => l.Count(c => c == '#'));

        var result = outerHashTagCount + innerHashTagCount;

        return new ValueTask<string>(result.ToString());
    }

    // 201398068194715
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(l => ToPath(l, true)).ToList();

        var edges = new List<(long X, long Y)>();
        (long X, long Y) current = (0, 0);
        long totalSteps = 0;

        foreach (var path in input)
        {
            switch (path.direction)
            {
                case Direction.DOWN:
                    current.Y += path.stepCount;
                    break;
                case Direction.UP:
                    current.Y -= path.stepCount;
                    break;
                case Direction.LEFT:
                    current.X -= path.stepCount;
                    break;
                case Direction.RIGHT:
                    current.X += path.stepCount;
                    break;
            }

            totalSteps += path.stepCount;
            edges.Add(current);
        }

        var tmp = edges[^3];

        /* ------ Disclaimer ------ */
        // Sadly, my first attempt to calculate all tiles inside the form didn't worked anymore :(
        // After several hours of trying, I decided to take @Rootix's (https://github.com/rootix) calculation Solution
        // Reference: https://github.com/rootix/AdventOfCode/commit/7ff83766055def8c325f7d1019267760152eca93#diff-2201dde4a0be914497e1fb104105fd12b47b53d17526a69ad53a5fb13b6cbe84 (Line 32)
        /* ------------------------ */

        // Shoelace Theorem
        // https://en.wikipedia.org/wiki/Shoelace_formula
        double A = 0;
        for (var i = 0; i < edges.Count; i++)
        {
            var nextIndex = (i + 1) % edges.Count;
            A += edges[i].X * edges[nextIndex].Y - edges[i].Y * edges[nextIndex].X;
        }
        A = A / 2;

        // Pick's Theorem
        // https://en.wikipedia.org/wiki/Pick%27s_theorem
        // i = A - b/2 + 1
        var insideTiles = Math.Round(A - totalSteps / 2 + 1);
        var outsideTiles = totalSteps;
        var result = insideTiles + outsideTiles;

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static int InnerHashTagCount(List<List<char>> map)
    {
        var innerHashTagCount = 0;
        for (var y = 0; y < map.Count; y++)
        {
            int? hashTagStart = null;
            int? hashTagEnd = null;

            var line = "";

            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == '#')
                {
                    if (hashTagStart != null)
                    {
                        hashTagEnd = x;
                    }
                    else
                    {
                        hashTagStart = x;
                        hashTagEnd = x;
                    }
                }
                else
                {
                    if (hashTagStart != null)
                    {
                        line += GetHashTag(y, hashTagStart.Value, hashTagEnd.Value, map);
                        hashTagStart = null;
                        hashTagEnd = null;
                    }

                    line += ".";
                }
            }

            var hashTagCount = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    hashTagCount++;
                }
                else if (hashTagCount % 2 == 1)
                {
                    innerHashTagCount++;
                }
            }
        }

        return innerHashTagCount;
    }

    private static string GetHashTag(int y, int xStart, int xEnd, List<List<char>> map)
    {
        if (y == 0 || y == map.Count - 1)
        {
            return "##";
        }

        var lowerRow = map[y - 1];
        var lowerRage = new string(lowerRow.Skip(xStart).Take(xEnd - xStart + 1).ToArray());
        var higherRow = map[y + 1];
        var higherRange = new string(higherRow.Skip(xStart).Take(xEnd - xStart + 1).ToArray());

        if (lowerRage.Contains('#') && higherRange.Contains('#'))
        {
            return "#";
        }

        return "##";
    }

    private static (List<List<char>>, Point) InitMap(List<Path> paths)
    {
        var minHeight = 0;
        var maxHeight = 0;
        var minWidth = 0;
        var maxWidth = 0;

        var current = new Point(0, 0);

        foreach (var path in paths)
        {
            switch (path.direction)
            {
                case Direction.DOWN:
                    current.Y += path.stepCount;
                    break;
                case Direction.UP:
                    current.Y -= path.stepCount;
                    break;
                case Direction.LEFT:
                    current.X -= path.stepCount;
                    break;
                case Direction.RIGHT:
                    current.X += path.stepCount;
                    break;
            }

            if (current.Y < minHeight)
            {
                minHeight = current.Y;
            }
            if (maxHeight < current.Y)
            {
                maxHeight = current.Y;
            }

            if (current.X < minWidth)
            {
                minWidth = current.X;
            }
            if (maxWidth < current.X)
            {
                maxWidth = current.X;
            }
        }

        var height = minHeight * -1 + maxHeight;
        var width = minWidth * -1 + maxWidth;

        var result = new List<List<char>>();
        for(int y = 0; y < height + 1; y++)
        {
            var tmp = new List<char>();
            for (int x = 0; x < width + 1; x++)
            {
                tmp.Add('.');
            }
            result.Add(tmp);
        }

        return (result, new Point(minWidth * -1, minHeight * -1));
    }

    private Path ToPath(string input, bool part2 = false)
    {
        var splited = input.Split(" ");
        var direction = splited[0] switch
        {
            "R" => Direction.RIGHT,
            "L" => Direction.LEFT,
            "D" => Direction.DOWN,
            "U" => Direction.UP
        };
        var stepCount = int.Parse(splited[1]);
        var rgb = splited[2];

        if (part2)
        {
            var formattedHexNumber = splited[2].Replace("#", "").Replace("(", "").Replace(")", "");

            direction = formattedHexNumber[^1] switch
            {
                '0' => Direction.RIGHT,
                '1' => Direction.DOWN,
                '2' => Direction.LEFT,
                '3' => Direction.UP
            };

            formattedHexNumber = $"0{formattedHexNumber[..^1]}";

            var intValueString = int.Parse(formattedHexNumber, System.Globalization.NumberStyles.HexNumber);

            stepCount = intValueString;
        }
        return new Path(direction, stepCount, rgb);
    }

    private class Path(Direction direction, int stepCount, string rgb)
    {
       public Direction direction = direction;
        public int stepCount = stepCount;
        public string rgb = rgb;
    }

    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
