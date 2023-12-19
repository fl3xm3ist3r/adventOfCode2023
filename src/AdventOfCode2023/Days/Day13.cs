namespace AdventOfCode2023.Days;

public class Day13 : DayBase
{
    //31739
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Select(e => e.Split($"{Environment.NewLine}").ToList()).ToList();
        var total = 0;
        foreach (var maze in input)
        {
            var result = GetMirroringIndex(maze);
            total += result;
        }

        return new ValueTask<string>(total.ToString());
    }

    //31539
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(e => e.Split($"{Environment.NewLine}").ToList()).ToList();
        var total = 0;
        foreach (var maze in input)
        {
            var result = GetMirrorIndexOfByOneChar(maze);
            total += result;
        }

        return new ValueTask<string>(total.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
    }


    // Idea on how to detect mirrors by @Rootix (https://github.com/rootix)
    // Reference: https://github.com/rootix/AdventOfCode/commit/fb911386b97880a0d13319432334519432aebf54#diff-7d6e89b83a11bf7f5da6295a64bd5a94c4f000dad2e66cdeffda5e7ca2b712d2 (Line 21)
    private static int GetMirroringIndex(List<string> maze)
    {
        for(var row = 0; row < maze.Count - 1; row++)
        {
            var botIndex = row;
            var topIndex = row + 1;

            var isMirrored = true;
            while(botIndex >= 0 && topIndex < maze.Count)
            {
                if (maze[botIndex] != maze[topIndex])
                {
                    isMirrored = false;
                    break;
                }

                botIndex--;
                topIndex++;
            }

            if(isMirrored) {
                return (row + 1) * 100;
            }
        }

        for (var column = 0; column < maze[0].Length - 1; column++)
        {
            var leftIndex = column;
            var rightIndex = column + 1;

            var isMirrored = true;
            while (leftIndex >= 0 && rightIndex < maze[0].Length && isMirrored)
            {
                foreach(var row in maze)
                {
                    if(row[leftIndex] != row[rightIndex])
                    {
                        isMirrored = false;
                        break;
                    }
                }

                leftIndex--;
                rightIndex++;
            }

            if (isMirrored)
            {
                return column + 1;
            }
        }

        throw new Exception("You surely need a hug ʕ⁠っ⁠•⁠ᴥ⁠•⁠ʔ⁠っ");
    }

    // Idea on how to detect mirrors by @Rootix (https://github.com/rootix)
    // Reference: https://github.com/rootix/AdventOfCode/blob/main/2023/src/AdventOfCode2023/Days/Day13.cs#L21
    private static int GetMirrorIndexOfByOneChar(List<string> maze)
    {
        for (var row = 0; row < maze.Count - 1; row++)
        {
            var botIndex = row;
            var topIndex = row + 1;

            var wrongCharsCount = 0;
            while (botIndex >= 0 && topIndex < maze.Count && wrongCharsCount < 2)
            {
                for(var i = 0; i < maze[botIndex].Length; i++)
                {
                    if (wrongCharsCount > 1)
                    {
                        break;
                    }

                    if (maze[botIndex][i] != maze[topIndex][i])
                    {
                        wrongCharsCount++;
                    }
                }

                botIndex--;
                topIndex++;
            }

            if (wrongCharsCount == 1)
            {
                return (row + 1) * 100;
            }
        }

        for (var column = 0; column < maze[0].Length - 1; column++)
        {
            var leftIndex = column;
            var rightIndex = column + 1;

            var wrongCharsCount = 0;
            while (leftIndex >= 0 && rightIndex < maze[0].Length && wrongCharsCount < 2)
            {
                foreach (var row in maze)
                {
                    if(wrongCharsCount > 1)
                    {
                        break;
                    }

                    if (row[leftIndex] != row[rightIndex])
                    {
                        wrongCharsCount ++;
                    }
                }

                leftIndex--;
                rightIndex++;
            }

            if (wrongCharsCount == 1)
            {
                return column + 1;
            }
        }

        throw new Exception("You surely need a hug ʕ⁠っ⁠•⁠ᴥ⁠•⁠ʔ⁠っ");
    }
}
