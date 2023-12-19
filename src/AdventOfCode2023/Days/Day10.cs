using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days;

public class Day10 : DayBase
{
    //6682
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(e => e.ToCharArray().ToList()).ToList();

        var s = FindCharIndex('S', input);
        var followResult = FollowPipes(input, s, '-');
        var result = followResult.length / 2;

        return new ValueTask<string>(result.ToString());
    }

    //353
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(e => e.ToCharArray().ToList()).ToList();

        var s = FindCharIndex('S', input);
        var followResult = FollowPipes(input, s, '-');

        var cleanedMap = new List<List<char>>();
        for (int y = 0; y < input.Count; y++)
        {
            cleanedMap.Add(new List<char>());
            for (int x = 0; x < input[0].Count; x++)
            {
                if (followResult.loopMap.Contains((y, x)))
                {
                    cleanedMap[y].Add(input[y][x]);
                }
                else
                {
                    cleanedMap[y].Add('0');
                }
            }
        }

        var result = CalculateEnclosedFields(cleanedMap);

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static (int y, int x) FindCharIndex(char charInput, List<List<char>> field)
    {
        for(int y = 0; y < field.Count; y++)
        {
            for (int x = 0; x < field[y].Count; x++)
            {
                if (field[y][x] == charInput)
                {
                    return (y, x);
                }
            }
        }

        throw new Exception();
    }

    private static (long length, List<(int y, int x)> loopMap) FollowPipes(List<List<char>> field, (int y, int x) startPosition, char pipe)
    {
        long length = 0;
        Direction? lastDirection = null;
        var currentPosition = startPosition;
        field[startPosition.y][startPosition.x] = pipe;

        var visitedPlaces = new List<(int y, int x)>();

        while (true)
        {
            visitedPlaces.Add(currentPosition);

            if(length > 0 && currentPosition == startPosition)
            {
                return (length, visitedPlaces);
            }

            length++;

            var currentPipe = field[currentPosition.y][currentPosition.x];

            // Right
            if (currentPipe is '-' or 'F' or 'L' && lastDirection != Direction.LEFT)
            {
                currentPosition = (currentPosition.y, currentPosition.x + 1);
                lastDirection = Direction.RIGHT;
                continue;
            }

            // Left
            if (currentPipe is '-' or 'J' or '7' && lastDirection != Direction.RIGHT)
            {
                currentPosition = (currentPosition.y, currentPosition.x - 1);
                lastDirection = Direction.LEFT;
                continue;
            }

            // Up
            if (currentPipe is '|' or 'L' or 'J' && lastDirection != Direction.DOWN)
            {
                currentPosition = (currentPosition.y - 1, currentPosition.x);
                lastDirection = Direction.UP;
                continue;
            }

            // Down
            if (currentPipe is '|' or 'F' or '7' && lastDirection != Direction.UP)
            {
                currentPosition = (currentPosition.y + 1, currentPosition.x);
                lastDirection = Direction.DOWN;
                continue;
            }

            throw new Exception("Wrong strating pipe");
        }
    }

    // Idea on how to detect if 0 is inside or not @Rootix (https://github.com/rootix)
    // Reference: https://github.com/rootix/AdventOfCode/commit/abe45a26ed94761003a9b3daa1c4f10394a22065#diff-4303474bde68ebca12a70cd25c6526b4ee2867f92e2e063508b8e19ad1b77ffe (Line 137)
    private static int CalculateEnclosedFields(List<List<char>> map)
    {
        var totalEnclosed = 0;

        for (int y = 0; y < map.Count; y++)
        {
            var line = string.Join("", map[y]);
            line = Regex.Replace(line, "F-*J|L-*7", "|");
            map[y] = line.ToCharArray().ToList();

            var pairCount = 0;
            for (int x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == '|')
                {
                    pairCount++;
                }
                else if (map[y][x] == '0' && pairCount % 2 == 1)
                {
                    totalEnclosed++;
                }
            }
        }

        return totalEnclosed;
    }


    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}
