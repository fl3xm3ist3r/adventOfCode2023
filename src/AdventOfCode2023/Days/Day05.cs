namespace AdventOfCode2023.Days;

public class Day05 : DayBase
{
    //240320250
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var seeds = input[0][0].Split(":")[1].Split(" ").Skip(1).Select(long.Parse).ToList();
        var mappingList = input.Skip(1).Select(MapToMapping).ToList();

        var locations = new List<long>();

        foreach (var seed in seeds)
        {
            var lastValue = seed;

            foreach (var mappings in mappingList)
            {
                foreach (var mapping in mappings)
                {
                    if (mapping.InputStart <= lastValue && lastValue < mapping.InputStart + mapping.Range)
                    {
                        lastValue += mapping.OutputStart - mapping.InputStart;
                        break;
                    }
                }
            }

            locations.Add(lastValue);
        }

        return new ValueTask<string>(locations.Min().ToString());
    }

    //28580589
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        var tmpSeeds = input[0][0].Split(":")[1].Split(" ").Skip(1).Select(long.Parse).ToList();
        var seeds = new List<(long start, long range)>();
        for (int i = 0; i < tmpSeeds.Count / 2; i += 2)
        {
            seeds.Add((tmpSeeds[i], tmpSeeds[i+1]));
        }

        var mappingList = input.Skip(1).Select(MapToMapping).Reverse().ToList();

        long? lowestSeedLocation = null;

        //sort locations so it starts with lowest location possible
        mappingList[0].Sort((x, y) => x.OutputStart.CompareTo(y.OutputStart));
        //loop from 0 because it would be the smallest location possible and it doesn't necessary needs to have a location mapping
        for (var i = 0; true; i++)
        {
            long lastValue = i;

            foreach (var mappings in mappingList)
            {
                foreach (var mapping in mappings)
                {
                    if (mapping.OutputStart <= lastValue && lastValue < mapping.OutputStart + mapping.Range)
                    {
                        lastValue += mapping.InputStart - mapping.OutputStart;
                        break;
                    }
                }
            }

            foreach (var seedRange in seeds)
            {
                if (seedRange.start <= lastValue && lastValue < seedRange.start + seedRange.range)
                {
                    lowestSeedLocation = i;
                    break;
                }
            }

            if (lowestSeedLocation != null)
            {
                break;
            }
        }

        return new ValueTask<string>(lowestSeedLocation.ToString());
    }

    private static List<List<string>> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList()
            .Select(s => s.Split($"{Environment.NewLine}").ToList()).ToList();
    }

    private List<Mapping> MapToMapping(List<string> inputs)
    {
        var mappings = new List<Mapping>();
        foreach (var input in inputs.Skip(1))
        {
            var splitInput = input.Split(" ").Select(long.Parse).ToList();
            mappings.Add(new Mapping(splitInput[0], splitInput[1], splitInput[2]));
        }
        return mappings;
    }

    private record Mapping(long OutputStart, long InputStart, long Range);
}
