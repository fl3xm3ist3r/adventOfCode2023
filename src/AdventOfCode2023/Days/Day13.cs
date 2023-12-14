using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

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

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
    }

    private static int GetMirroringIndex(List<string> maze)
    {
        for(var row = 0; row < maze.Count - 1; row++)
        {
            var topIndex = row;
            var botIndex = row + 1;

            var isMirrored = true;
            while(topIndex >= 0 && botIndex < maze.Count)
            {
                if (maze[topIndex] != maze[botIndex])
                {
                    isMirrored = false;
                    break;
                }

                topIndex--;
                botIndex++;
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
            while (leftIndex >= 0 && rightIndex < maze[0].Length)
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
}
