namespace AdventOfCode2023.Days;

public class Day01 : DayBase
{
    //57346
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var total = input.Select(line => GetFirstNumber(line) + GetFirstNumber(ReverseString(line))).Select(int.Parse).Sum();

        return new ValueTask<string>(total.ToString());
    }

    //57345
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Select(ReplaceStringNumberWithNumber).ToArray();

        var total = input.Select(line => GetFirstNumber(line) + GetFirstNumber(ReverseString(line))).Select(int.Parse).Sum();

        return new ValueTask<string>(total.ToString());
    }

    private static string[] GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}");
    }
    
    private static string GetFirstNumber(string inputString)
    {
        foreach (var inputChar in inputString)
        {
            if (char.IsDigit(inputChar))
            {
                return inputChar.ToString();
            }
        }

        throw new Exception("No digit found in the input string.");
    }

    private static string ReplaceStringNumberWithNumber(string input)
    {
        var onlyNumbers = "";
        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                onlyNumbers += input[i];
            }
            else
            {
                for (var x = 3; x <= 5 && i + x <= input.Length; x++)
                {
                    var number = input.Substring(i, x);
                    if (StringNumberMapping.TryGetValue(number, out var mappedNumber))
                    {
                        onlyNumbers += mappedNumber;
                        break;
                    }
                }
            }
        }

        return onlyNumbers;
    }

    private static readonly Dictionary<string, string> StringNumberMapping = new ()
    {
        { "zero", "0" },
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
    };

    static string ReverseString(string input)
    {
        return new string(input.Reverse().ToArray());
    }
}
