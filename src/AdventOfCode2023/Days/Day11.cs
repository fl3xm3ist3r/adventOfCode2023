namespace AdventOfCode2023.Days;

public class Day11 : DayBase
{
    //9693756
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(line => line.ToCharArray().ToList()).ToList();
        input = DuplicateEmptyLines(input);

        var stars = ToStars(input);

        long totalDistances = 0;

        for (int i = 0; i < stars.Count; i++)
        {
            var currentStar = stars[i];
            for (int j = i + 1; j < stars.Count; j++)
            {
                var tmpStar = stars[j];
                var xDistance = currentStar.x - tmpStar.x;
                var yDistance = currentStar.y - tmpStar.y;

                var distance = Math.Abs(xDistance) + Math.Abs(yDistance);
                totalDistances += distance;
            }
        }


        return new ValueTask<string>(totalDistances.ToString());
    }

    //717878258016
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(line => line.ToCharArray().ToList()).ToList();

        var empty = GetEmptyIndexes(input);

        var stars = ToEmptyStars(input, empty);

        long totalDistances = 0;

        for (int i = 0; i < stars.Count; i++)
        {
            var currentStar = stars[i];
            for (int j = i + 1; j < stars.Count; j++)
            {
                var tmpStar = stars[j];
                var xDistance = currentStar.x - tmpStar.x;
                var yDistance = currentStar.y - tmpStar.y;

                var distance = Math.Abs(xDistance) + Math.Abs(yDistance);
                totalDistances += distance;
            }
        }

        return new ValueTask<string>(totalDistances.ToString());
    }


    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static List<List<Char>> DuplicateEmptyLines(List<List<Char>> map)
    {
        //rows
        var emptyRowIndexes = new List<int>();

        var isRowClean = true;
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == '#')
                {
                    isRowClean = false;
                }
            }

            if (isRowClean)
            {
                emptyRowIndexes.Add(y);
            }
            isRowClean = true;
        }

        var increment = 0;
        foreach(var emptyRowIndex in emptyRowIndexes)
        {
            map.Insert(emptyRowIndex + increment, GetDefaultCharList(map[emptyRowIndex + increment].Count, '.'));
            increment++;
        }

        //colums
        var emptyColumIndexes = new List<int>();

        var isColumnClean = true;
        for (var x = 0; x < map[0].Count; x++)
        {
            for (var y = 0; y < map.Count; y++)
            {
                if (map[y][x] == '#')
                {
                    isColumnClean = false;
                }
            }

            if (isColumnClean)
            {
                emptyColumIndexes.Add(x);
            }
            isColumnClean = true;
        }

        increment = 0;
        foreach(var emptyColumnIndex in emptyColumIndexes)
        {
            foreach(var line in map)
            {
                line.Insert(emptyColumnIndex + increment, '.');
            }

            increment++;
        }

        return map;
    }

    private static (List<int> emptyRowIndexes, List<int> emptyColumIndexes) GetEmptyIndexes(List<List<Char>> map)
    {
        var emptyRowIndexes = new List<int>();

        var isRowClean = true;
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == '#')
                {
                    isRowClean = false;
                }
            }

            if (isRowClean)
            {
                emptyRowIndexes.Add(y);
            }
            isRowClean = true;
        }

        var emptyColumIndexes = new List<int>();

        var isColumnClean = true;
        for (var x = 0; x < map[0].Count; x++)
        {
            for (var y = 0; y < map.Count; y++)
            {
                if (map[y][x] == '#')
                {
                    isColumnClean = false;
                }
            }

            if (isColumnClean)
            {
                emptyColumIndexes.Add(x);
            }
            isColumnClean = true;
        }

        return (emptyRowIndexes, emptyColumIndexes);
    }

    private static List<char> GetDefaultCharList(int rowIndex, char c)
    {
        var list = new List<char>();
        for (var i = 0; i < rowIndex; i++)
        {
            list.Add(c);
        }

        return list;
    }

    private static List<Star> ToStars(List<List<Char>> map)
    {
        var stars = new List<Star>();
        for (var y = 0; y < map.Count; y++)
        {
            for(var x = 0; x < map[y].Count; x++)
            {
                if (map[y][x] == '#')
                {
                    stars.Add(new Star(x, y));
                }
            }
        }

        return stars;
    }

    private static List<Star> ToEmptyStars(List<List<Char>> map, (List<int> emptyRowIndexes, List<int> emptyColumIndexes) empty)
    {
        var stars = new List<Star>();

        var emptyY = 0;
        for (var y = 0; y < map.Count; y++)
        {
            if (empty.emptyRowIndexes.Contains(y))
            {
                emptyY++;
            }

            var emptyX = 0;
            for (var x = 0; x < map[y].Count; x++)
            {
                if (empty.emptyColumIndexes.Contains(x))
                {
                    emptyX++;
                }

                if (map[y][x] == '#')
                {
                    stars.Add(new Star(x + 1000000 * emptyX - emptyX, y + 1000000 * emptyY - emptyY));
                }
            }
        }

        return stars;
    }

    private record Star(long x, long y);
}
