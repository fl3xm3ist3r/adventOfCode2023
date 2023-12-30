namespace AdventOfCode2023.Days;

public class Day20 : DayBase
{
    // 788081152
    public override ValueTask<string> Solve_1()
    {
        var modules = GetInput(Input.Value).Select(ToBaseModule).ToList();
        modules.Add(ToBaseModule("rx -> ")); //missing as a mapping
        foreach (var module in modules)
        {
            module.Initialize(modules);
        }

        var highSignalCount = 0;
        var lowSignalCount = 0;

        var broadcaster = modules.First(m => m.Name == "broadcaster");

        var queue = new List<Package>();
        for (int i = 0; i < 1000; i++)
        {
            queue.Add(new Package(broadcaster, SignalType.Low, broadcaster));
            lowSignalCount++;

            while (queue.Count > 0)
            {
                var currentPackage = queue[0];
                var newSender = currentPackage.Recipient;

                var newSignal = newSender.GetOutputSignal(currentPackage);

                if(newSignal != null)
                {
                    foreach (var newRecipient in newSender.Recipients)
                    {
                        if(newSignal == SignalType.High)
                        {
                            highSignalCount++;
                        }
                        else
                        {
                            lowSignalCount++;
                        }

                        queue.Add(new Package(newSender, newSignal.Value, newRecipient));
                    }
                }

                queue.Remove(currentPackage);
            }
        }

        var result = highSignalCount * lowSignalCount;

        return new ValueTask<string>(result.ToString());
    }

    // 224602011344203
    public override ValueTask<string> Solve_2()
    {
        var modules = GetInput(Input.Value).Select(ToBaseModule).ToList();
        var rx = ToBaseModule("rx -> ");
        modules.Add(rx);
        foreach (var module in modules)
        {
            module.Initialize(modules);
        }

        var conditionalModule = modules.First(m => m.Recipients.Contains(rx));
        var ConditionalModuleList = modules.Where(m => m.Recipients.Contains(conditionalModule)).ToList();

        var queue = new List<Package>();
        var broadcaster = modules.First(m => m.Name == "broadcaster");

        var lcmResulte = new List<ulong>() { 0, 0, 0, 0};

        ulong count = 0;
        while (lcmResulte.Any(l => l == 0))
        {
            queue.Add(new Package(broadcaster, SignalType.Low, broadcaster));
            count++;

            while (queue.Count > 0 && lcmResulte.Any(l => l == 0))
            {
                var currentPackage = queue[0];
                var newSender = currentPackage.Recipient;

                var newSignal = newSender.GetOutputSignal(currentPackage);

                if (newSignal != null)
                {
                    foreach (var newRecipient in newSender.Recipients)
                    {
                        //find all 4 "High" circle values of the rx Conjunction to calculate least common multiple
                        if (newSignal == SignalType.High)
                        {
                            if (lcmResulte[0] == 0 && newSender == ConditionalModuleList[0])
                        {
                                lcmResulte[0] = count;
                            }
                            if (lcmResulte[1] == 0 && newSender == ConditionalModuleList[1])
                            {
                                lcmResulte[1] = count;
                            }
                            if (lcmResulte[2] == 0 && newSender == ConditionalModuleList[2])
                            {
                                lcmResulte[2] = count;
                            }
                            if (lcmResulte[3] == 0 && newSender == ConditionalModuleList[3])
                            {
                                lcmResulte[3] = count;
                                Console.WriteLine(count.ToString());
                            }
                        }

                        queue.Add(new Package(newSender, newSignal.Value, newRecipient));
                    }
                }

                queue.Remove(currentPackage);
            }
        }

        /* ------ Disclaimer ------ */
        // Hint that lcm (kgv) works on this problem was found in @Lulukas (https://github.com/lulukas) code
        // Reference: https://github.com/lulukas/advent-of-code/commit/7d0ab4e65871e6c1ad06c977828d6cab08f3173f#diff-3f0f3e835409819e17ec067c8fc7ac5837524c379fa5f758395f76d850306011 (Line 134)
        /* ------------------------ */

        //since all numbers are already prime numbers we can just multiply them with each other
        var result = lcmResulte.Aggregate((a, b) => a * b);

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private Module ToBaseModule(string input)
    {
        var splited = input.Split(" -> ");
        var name = splited[0];
        var destinations = splited[1].Split(", ").ToList();
        Module module = new Module(name, destinations, ModuleType.Broadcaster);

        if (input[0] == '&')
        {
            module.Name = module.Name.Substring(1);
            module.Type = ModuleType.Conjunction;
        }
        else if (input[0] == '%')
        {
            module.Name = module.Name.Substring(1);
            module.Type = ModuleType.FlipFlop;
        }

        return module;
    }

    private class Module(string name, List<string> tmpRecipients, ModuleType type)
    {
        public string Name { get; set; } = name;

        public ModuleType Type = type;

        public List<string> TmpRecipients = tmpRecipients;

        public List<Module> Recipients {  get; set; } = new List<Module>();

        public void Initialize(List<Module> recipients)
        {
            foreach(var tmpRecipient in TmpRecipients)
            {
                if(tmpRecipient != "")
                {
                    var recipient = recipients.First(m => m.Name == tmpRecipient);
                    Recipients.Add(recipient);

                    recipient.cache.Add(Name, SignalType.Low);
                }
            }
        }

        public bool IsOn { get; set; }

        public Dictionary<string, SignalType> cache = new Dictionary<string, SignalType>();

        public void UpdateCache(string name, SignalType type)
        {
            cache[name] = type;
        }

        public SignalType? GetOutputSignal(Package package)
        {
            package.Recipient.UpdateCache(package.Sender.Name, package.Signal);

            if (Type == ModuleType.FlipFlop)
            {
                if(package.Signal == SignalType.High)
                {
                    return null;
                }

                IsOn = !IsOn;
                return IsOn == true ? SignalType.High : SignalType.Low;
            }
            else if (Type == ModuleType.Conjunction)
            {
                var containsLowSignal = package.Recipient.cache.Any(c => c.Value == SignalType.Low);

                if (containsLowSignal)
                {
                    return SignalType.High;
                }

                return SignalType.Low;
            }

            return package.Signal;
        }
    }

    private class Package(Module sender, SignalType signal, Module recipient)
    {
        public Module Sender { get; set; } = sender;

        public SignalType Signal { get; set; } = signal;

        public Module Recipient { get; set; } = recipient;
    }

    enum ModuleType
    {
        FlipFlop,
        Conjunction,
        Broadcaster
    }

    enum SignalType
    {
        High,
        Low
    }
}
