namespace AdventOfCode2023.Days;

public class Day16 : DayBase
{
    // 7496
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(l => l.ToCharArray().ToList()).ToList();

        var result = MoveThroughMaze((0, -1, Direction.RIGHT), input);

        return new ValueTask<string>(result.ToString());
    }

    // 7932
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(l => l.ToCharArray().ToList()).ToList();

        var result = new List<int>();

        for(int x = 0; x < input[0].Count; x++)
        {
            result.Add(MoveThroughMaze((-1, x, Direction.DOWN), input));
            result.Add(MoveThroughMaze((input.Count, x, Direction.UP), input));
        }

        for (int y = 0; y < input.Count; y++)
        {
            result.Add(MoveThroughMaze((y, -1, Direction.RIGHT), input));
            result.Add(MoveThroughMaze((y, input[0].Count, Direction.LEFT), input));
        }

        return new ValueTask<string>(result.Max().ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static int MoveThroughMaze((int Y, int X, Direction Direction) startPath, List<List<char>> input)
    {
        var pointCache = new List<(int Y, int X, Direction Direction)>();

        var paths = new List<(int Y, int X, Direction Direction)>() { startPath };

        for (var i = 0; i < paths.Count; i++)
        {
            var path = paths[i];

            var active = true;
            while (active)
            {
                path = MovePath(path);

                // out of bounds
                if (path.X < 0 || path.X >= input[0].Count ||
                    path.Y < 0 || path.Y >= input.Count)
                {
                    break;
                }

                //already visited this place
                var x = pointCache.IndexOf((path.Y, path.X, path.Direction));
                if (x != -1)
                {
                    break;
                }

                var currentField = input[path.Y][path.X];
                pointCache.Add((path.Y, path.X, path.Direction));

                switch (currentField)
                {
                    case '.':
                        break;
                    case '-':
                        switch (path.Direction)
                        {
                            case Direction.LEFT:
                            case Direction.RIGHT:
                                break;
                            case Direction.UP:
                            case Direction.DOWN:
                                path.Direction = Direction.LEFT;
                                paths.Add(path);
                                path.Direction = Direction.RIGHT;
                                paths.Add(path);
                                active = false;
                                break;
                        }
                        break;
                    case '|':
                        switch (path.Direction)
                        {
                            case Direction.UP:
                            case Direction.DOWN:
                                break;
                            case Direction.LEFT:
                            case Direction.RIGHT:
                                path.Direction = Direction.UP;
                                paths.Add(path);
                                path.Direction = Direction.DOWN;
                                paths.Add(path);
                                active = false;
                                break;
                        }
                        break;
                    case '/':
                        switch (path.Direction)
                        {
                            case Direction.UP:
                                path.Direction = Direction.RIGHT;
                                break;
                            case Direction.DOWN:
                                path.Direction = Direction.LEFT;
                                break;
                            case Direction.LEFT:
                                path.Direction = Direction.DOWN;
                                break;
                            case Direction.RIGHT:
                                path.Direction = Direction.UP;
                                break;
                        }
                        break;
                    case '\\':
                        switch (path.Direction)
                        {
                            case Direction.UP:
                                path.Direction = Direction.LEFT;
                                break;
                            case Direction.DOWN:
                                path.Direction = Direction.RIGHT;
                                break;
                            case Direction.LEFT:
                                path.Direction = Direction.UP;
                                break;
                            case Direction.RIGHT:
                                path.Direction = Direction.DOWN;
                                break;
                        }
                        break;
                }
            }
        }

        List<List<char>> map = new List<List<char>>();
        for (int y = 0; y < input.Count; y++)
        {
            var currentList = new List<char>();
            for (int x = 0; x < input[y].Count; x++)
            {
                currentList.Add('.');
            }
            map.Add(currentList);
        }

        foreach (var point in pointCache)
        {
            map[point.Y][point.X] = '#';
        }

        var total = map.Sum(l => l.Count(c => c == '#'));

        return total;
    }

    private static (int Y, int X, Direction Direction) MovePath((int Y, int X, Direction Direction) move) {
        switch (move.Direction)
        {
            case Direction.LEFT:
                move.X--;
                break;
            case Direction.RIGHT:
                move.X++;
                break;
            case Direction.UP:
                move.Y--;
                break;
            case Direction.DOWN:
                move.Y++;
                break;
        }

        return move;
    }

    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
