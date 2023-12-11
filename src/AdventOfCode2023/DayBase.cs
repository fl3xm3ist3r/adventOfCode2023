namespace AdventOfCode2023;

public abstract class DayBase : BaseDay
{
    protected DayBase()
    {
        Input = new Lazy<string>(() => RemoveLastLineIfEmpty(File.ReadAllText(InputFilePath)));
    }

    public string? OverrideFileDirPath { get; set; }

    protected override string InputFileDirPath => OverrideFileDirPath ?? base.InputFileDirPath;

    protected Lazy<string> Input { get; private set; }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>("Not solved");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>("Not solved");
    }

    private static string RemoveLastLineIfEmpty(string text)
    {
        string[] lines = text.Split($"{Environment.NewLine}");

        if (lines.Length > 0 && string.IsNullOrWhiteSpace(lines[^1]))
        {
            Array.Resize(ref lines, lines.Length - 1);
        }

        return string.Join($"{Environment.NewLine}", lines);
    }
}
