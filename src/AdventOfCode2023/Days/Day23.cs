using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day23 : DayBase
{
    //2314
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

    // doesn't work because it takes way to long

    // 6874
    static int longestPath;
    //public override ValueTask<string> Solve_2()
    //{
    //    var map = GetInput(Input.Value).Select(l => l.ToCharArray()).ToArray();

    //    var startIndex = new string(map[0]).IndexOf('.');
    //    var endIndex = new string(map[^1]).IndexOf('.');

    //    var (startPoint, startNode) = (new Point(startIndex, 0), new Node());
    //    var (endPoint, endNode) = (new Point(endIndex, map.Length - 1), new Node());

    //    var nodePointList = new List<(Point, Node)>
    //    {
    //        (startPoint, startNode),
    //        (endPoint, endNode)
    //    };

    //    for(int y = 0; y < map.Length; y++)
    //    {
    //        for(int x = 0; x < map[0].Length; x++)
    //        {
    //            if (map[y][x] == '#')
    //            {
    //                continue;
    //            }

    //            var tmpPoint = new Point(x, y);
    //            if (IsNode(tmpPoint, map))
    //            {
    //                nodePointList.Add((tmpPoint, new Node()));
    //            }
    //        }
    //    }

    //    var nodeList = new List<Node>();
    //    foreach(var (point, node) in nodePointList)
    //    {
    //        var directions = new Point[] {
    //            new Point(-1, 0),
    //            new Point(1, 0),
    //            new Point(0, -1),
    //            new Point(0, 1),
    //        };

    //        foreach (var direction in directions)
    //        {
    //            var tmpPoint = new Point(point.X, point.Y);

    //            var stepCount = 0;
    //            while (tmpPoint.Y + direction.Y >= 0 && tmpPoint.Y + direction.Y < map.Length &&
    //                    tmpPoint.X + direction.X >= 0 && tmpPoint.X + direction.X < map[0].Length)
    //            {
    //                stepCount++;
    //                tmpPoint.X += direction.X;
    //                tmpPoint.Y += direction.Y;

    //                if (map[tmpPoint.Y][tmpPoint.X] == '#')
    //                {
    //                    break;
    //                }

    //                if (IsNode(tmpPoint, map) || tmpPoint == endPoint)
    //                {
    //                    var (pointAtCoordinate, nodeAtCoordinate) = nodePointList.First(n => n.Item1 == tmpPoint);
    //                    node.NearestNodes.Add((nodeAtCoordinate, stepCount));
    //                    break;
    //                }
    //            }
    //        }

    //        nodeList.Add(node);
    //    }

    //    var queue = new Queue<NodePath>();
    //    var startNodePath = new NodePath() {
    //        Cache = new HashSet<Node>() { startNode },
    //        Current = startNode
    //    };
    //    queue.Enqueue(startNodePath);

    //    while (queue.Count > 0)
    //    {
    //        var path = queue.Dequeue();
    //        MoveToNearestNodes(path, queue, endNode);
    //    }

    //    var result = longestPath;
    //    return new ValueTask<string>(result.ToString());
    //}

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static bool IsNode(Point point, char[][] map)
    {
        var possibleCorrectChars = new char[] { '.', '^', '>', 'v', '<' };

        var up = point.Y - 1 >= 0 && possibleCorrectChars.Contains(map[point.Y - 1][point.X]);
        var down = point.Y + 1 < map.Length && possibleCorrectChars.Contains(map[point.Y + 1][point.X]);
        var left = point.X - 1 >= 0 && possibleCorrectChars.Contains(map[point.Y][point.X - 1]);
        var right = point.X + 1 < map[0].Length && possibleCorrectChars.Contains(map[point.Y][point.X + 1]);

        if((up && left) ||
            (up && right))
        {
            return true;
        }

        if((down && left) ||
            (down && right))
        {
            return true;
        }

        return false;
    }

    private static void MoveToNearestNodes(NodePath path, Queue<NodePath> queue, Node endNode)
    {
        foreach (var (node, distance) in path.Current.NearestNodes.Where(n => !path.Cache.Contains(n.node)))
        {
            var totalDistance = path.Distance + distance;
            if (node == endNode)
            {
                if (totalDistance > longestPath)
                {
                    longestPath = totalDistance;
                }
                continue;
            }

            var newPath = new NodePath()
            {
                Cache = new HashSet<Node>(path.Cache) { node },
                Current = node,
                Distance = totalDistance
            };
            queue.Enqueue(newPath);
        }
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

    public class Node()
    {
        public HashSet<(Node node, int distance)> NearestNodes { get; set; } = new HashSet<(Node, int distance)>();
    }

    public class NodePath()
    {
        public HashSet<Node> Cache { get; set; } = new HashSet<Node>();
        public Node Current { get; set; } = new Node();
        public int Distance { get; set; }
    }
}
