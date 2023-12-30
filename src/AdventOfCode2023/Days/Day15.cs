namespace AdventOfCode2023.Days;

public class Day15 : DayBase
{
    // 511215
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value).Split(",").ToList();

        var total = 0;
        foreach (var item in input)
        {
            total += Hash(item);
        }

        return new ValueTask<string>(total.ToString());
    }

    // 236057
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value).Split(",").ToList();
        var boxes = new List<Box>();
        for(var i = 0; i < 256; i++)
        {
            boxes.Add(new Box());
        }

        foreach (var item in input)
        {
            if (item.Contains('='))
            {
                var label = item.Split("=")[0];
                var boxNumber = Hash(label);
                var focalLength = int.Parse(item.Split('=')[1]);
                var lense = boxes[boxNumber].Lenses.FirstOrDefault(l => l.Label == label);

                if (lense != null)
                {
                    // "replace" old lense with new lense
                    lense.FocalLength = focalLength;
                }
                else
                {
                    boxes[boxNumber].Lenses.Add(new Lense(label, focalLength));
                }
            }
            else
            {
                var label = item.Split("-")[0];
                var boxNumber = Hash(label);
                var lense = boxes[boxNumber].Lenses.FirstOrDefault(l => l.Label == label);

                if (lense != null)
                {
                    boxes[boxNumber].Lenses.Remove(lense);
                }
            }
        }

        var total = 0;

        for(int boxCount = 0; boxCount < boxes.Count; boxCount++)
        {
            for(int lenseCount = 0; lenseCount < boxes[boxCount].Lenses.Count; lenseCount++)
            {
                total += ((boxCount + 1) * (lenseCount + 1) * boxes[boxCount].Lenses[lenseCount].FocalLength);
            }
        }

        return new ValueTask<string>(total.ToString());
    }

    private static string GetInput(string input)
    {
        return input.Replace("\n", "");
    }

    private int Hash(string input)
    {
        var current = 0;
        foreach (char c in input)
        {
            current += c;
            current *= 17;
            current %= 256;
        }

        return current;
    }

    private class Box() {
        public List<Lense> Lenses {  get; set; } = new List<Lense>();
    }

    private class Lense(string label, int focalLength)
    {
        public string Label { get; set;} = label;

        public int FocalLength { get; set; } = focalLength;
    }
}
