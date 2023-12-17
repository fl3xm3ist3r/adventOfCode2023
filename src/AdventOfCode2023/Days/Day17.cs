using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day17 : DayBase
{
    //1110
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToList()).ToList();

        var result = GetSmallestHeatPath(input, 3);

        return new ValueTask<string>(result.ToString());
    }

    //1294 (takes approximately 8 minutes and 30 seconds)
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToList()).ToList();

        var result = GetSmallestHeatPath(input, 10, true);

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static int GetSmallestHeatPath(List<List<int>> map, int maxForwardCount, bool part2 = false)
    {
        var cache = new List<(Point point, Direction direction, int forwardCount)>();
        var paths = new List<Path>();
        var end = new Point(map[0].Count - 1, map.Count - 1);

        var first = new Path(new Point(0, 0), Direction.RIGHT, 0, 0);
        paths.Add(first);
        cache.Add((first.point, first.direction, first.forwardCount));

        var second = new Path(new Point(0, 0), Direction.DOWN, 0, 0);
        paths.Add(second);
        cache.Add((second.point, second.direction, second.forwardCount));


        while (paths.Count > 0)
        {
            var current = paths.OrderBy(p => p.totalHeat).First();

            if (current.point.X == end.X && current.point.Y == end.Y)
            {
                return current.totalHeat;
            }

            if (current.forwardCount < maxForwardCount)
            {
                AddIfValid(CoppyPath(current, forward: true), cache, map, paths);
            }

            if (!part2 || part2 && current.forwardCount >= 4)
            {
                if (current.direction == Direction.LEFT || current.direction == Direction.RIGHT)
                {
                    AddIfValid(CoppyPath(current, Direction.UP), cache, map, paths);
                    AddIfValid(CoppyPath(current, Direction.DOWN), cache, map, paths);
                }
                else
                {
                    AddIfValid(CoppyPath(current, Direction.LEFT), cache, map, paths);
                    AddIfValid(CoppyPath(current, Direction.RIGHT), cache, map, paths);
                }
            }

            paths.Remove(current);
        }

        return 0;
    }

    private static void AddIfValid(Path path, List<(Point point, Direction direction, int forwardCount)> cache, List<List<int>> map, List<Path> paths)
    {
        MovePath(path);

        if(path.point.X < 0 || path.point.X >= map.Count ||
            path.point.Y < 0 || path.point.Y >= map[0].Count)
        {
            return;
        }

        if(cache.Contains((path.point, path.direction, path.forwardCount)))
        {
            return;
        }

        cache.Add((path.point, path.direction, path.forwardCount));
        path.totalHeat += map[path.point.Y][path.point.X];

        paths.Add(path);
    }

    private static Path CoppyPath(Path path, Direction? newDirection = null, bool forward = false)
    {
        return new Path(path.point, forward ? path.direction : newDirection.Value, forward ? path.forwardCount + 1 : 1, path.totalHeat);
    }

    private static void MovePath(Path path)
    {
        switch (path.direction)
        {
            case Direction.LEFT:
                path.point = new Point(path.point.X - 1, path.point.Y);
                break;
            case Direction.RIGHT:
                path.point = new Point(path.point.X + 1, path.point.Y);
                break;
            case Direction.UP:
                path.point = new Point(path.point.X, path.point.Y - 1);
                break;
            case Direction.DOWN:
                path.point = new Point(path.point.X, path.point.Y + 1);
                break;
        }
    }

    private class Path(Point point, Direction direction, int forwardCount, int totalHeat)
    {
        public Point point = point;
        public Direction direction = direction;
        public int forwardCount = forwardCount;
        public int totalHeat = totalHeat;
    }

    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
