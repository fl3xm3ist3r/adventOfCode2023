using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day21 : DayBase
{
    //3737
    public override ValueTask<string> Solve_1()
    {
        var map = GetInput(Input.Value).Select(l => l.ToCharArray().ToList()).ToList();
        var lastPoints = new List<Point> { GetStartPoint(map) };
        var newPoints = new List<Point>();

        for(int i = 0; i < 64; i++)
        {
            foreach(var point in lastPoints)
            {
                MovePointIfPossible(point, newPoints, map);
            }

            lastPoints = newPoints;
            newPoints = new List<Point>();
        }

        var result = lastPoints.Count;

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static void MovePointIfPossible(Point point, List<Point> newPoints, List<List<char>> map)
    {
        for(var y = -1; y < 2; y++)
        {
            for (var x = -1; x < 2; x++)
            {
                if(y == 0 && x == 0 ||
                    y == -1 && x == -1 ||
                    y == 1 && x == 1 ||
                    y == -1 && x == 1 ||
                    y == 1 && x == -1)
                {
                    continue;
                }

                var newX = point.X + x;
                var newY = point.Y + y;

                if (newX < 0 || newX >= map[0].Count ||
                    newY < 0 || newY >= map.Count)
                {
                    continue;
                }

                if (map[newY][newX] != '#')
                {
                    var newPoint = new Point(newX, newY);

                    if (!newPoints.Contains(newPoint))
                    {
                        newPoints.Add(newPoint);
                    }
                }
            }
        }
    }

    private static Point GetStartPoint(List<List<char>> map)
    {
        for(var y = 0; y < map.Count; y++)
        {
            for(var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == 'S')
                {
                    return new Point(x, y);
                }
            }
        }

        return new Point(0, 0);
    }
}
