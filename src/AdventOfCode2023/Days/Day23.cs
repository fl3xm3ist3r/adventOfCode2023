using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day23 : DayBase
{
    // 2314
    public override ValueTask<string> Solve_1()
    {
        var map = GetInput(Input.Value).Select(l => l.ToCharArray()).ToArray();

        var startPoint = new string(map[0]).IndexOf('.');
        var endPoint = new string(map[^1]).IndexOf('.');

        var startPath = new Path()
        {
            Current = new Point(startPoint, 0),
            End = new Point(endPoint, map.Length - 1)
        };

        var pathQueue = new Queue<Path>();
        pathQueue.Enqueue(startPath);
        var successfullPaths = new List<Path>();

        while (pathQueue.Count > 0)
        {
            var path = pathQueue.Dequeue();
            MoveInAllDirectionsIfPossible(path, pathQueue, map, successfullPaths);
        }

        var result = successfullPaths.Max(p => p.Cache.Count);
        return new ValueTask<string>(result.ToString());
    }

    // 6874
    // Takes approximately 6 minutes
    static char[][] visited = Array.Empty<char[]>();
    public override ValueTask<string> Solve_2()
    {
        var map = GetInput(Input.Value).Select(l => l.Replace('<', '.').Replace('>', '.').Replace('^', '.').Replace('v', '.').ToCharArray()).ToArray();

        var startPoint = new string(map[0]).IndexOf('.');
        var endPoint = new string(map[^1]).IndexOf('.');

        int maxDistance = 0;

        visited = new char[map.Length][];
        for (int i = 0; i < map.Length; i++)
        {
            visited[i] = new char[map[i].Length];
        }

        //instead of using a queue that blotes all my memory i used recursion this time and it worked
        FindLongestPath(map, 0, startPoint, map.Length - 1, endPoint, ref maxDistance, 0);

        var result = maxDistance;
        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    public static void FindLongestPath(char[][] map, int yStart, int xStart, int yEnd, int xEnd, ref int maxDistance, int distance)
    {
        if (yStart == yEnd && xStart == xEnd)
        {
            maxDistance = Math.Max(maxDistance, distance);
            return;
        }

        visited[yStart][xStart] = '.';

        if (IsValid(yStart + 1, xStart, map)) FindLongestPath(map, yStart + 1, xStart, yEnd, xEnd, ref maxDistance, distance + 1);
        if (IsValid(yStart - 1, xStart, map)) FindLongestPath(map, yStart - 1, xStart, yEnd, xEnd, ref maxDistance, distance + 1);
        if (IsValid(yStart, xStart + 1, map)) FindLongestPath(map, yStart, xStart + 1, yEnd, xEnd, ref maxDistance, distance + 1);
        if (IsValid(yStart, xStart - 1, map)) FindLongestPath(map, yStart, xStart - 1, yEnd, xEnd, ref maxDistance, distance + 1);

        visited[yStart][xStart] = '\0';
    }

    public static bool IsValid(int y, int x, char[][] map)
    {
        if (x >= 0 && x < map[0].Length && y >= 0 && y < map.Length && map[y][x] == '.' && visited[y][x] != '.') return true;
        return false;
    }

    public static void MoveInAllDirectionsIfPossible(Path path, Queue<Path> pathQueue, char[][] map, List<Path> successfullPaths)
    {
        if(path.Current.X == path.End.X && path.Current.Y == path.End.Y)
        {
            successfullPaths.Add(path);
            return;
        }

        // Up, Down, Left, Right
        var directions = new[] { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) };

        foreach (var direction in directions)
        {
            var newPoint = new Point(path.Current.X + direction.X, path.Current.Y + direction.Y);
            if (IsMoveValid(newPoint, direction, path.Cache, map))
            {
                var newPath = new Path
                {
                    Current = newPoint,
                    End = path.End,
                    Cache = new HashSet<Point>(path.Cache) { newPoint }
                };
                pathQueue.Enqueue(newPath);
            }
        }
    }

    public static bool IsMoveValid(Point point, Point direction, HashSet<Point> cache, char[][] map)
    {
        if (cache.Contains(point))
        {
            return false;
        }

        if(point.X < 0 || point.X >= map[0].Length ||
            point.Y < 0 || point.Y >= map.Length)
        {
            return false;
        }

        var currentField = map[point.Y][point.X];
        switch (currentField)
        {
            case '.':
                return true;
            case '^' when direction == new Point(0, -1):
            case 'v' when direction == new Point(0, 1):
            case '<' when direction == new Point(-1, 0):
            case '>' when direction == new Point(1, 0):
                return true;
        }

        return false;
    }

    public class Path()
    {
        public HashSet<Point> Cache { get; set; } = new HashSet<Point>();

        public Point Current { get; set; } = Point.Empty;

        public Point End {  get; set; } = Point.Empty;
    }
}
