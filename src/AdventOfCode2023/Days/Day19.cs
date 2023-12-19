using System.Drawing;

namespace AdventOfCode2023.Days;

public class Day19 : DayBase
{
    //348378
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);

        var mappings = input[0].Split($"{Environment.NewLine}").Select(ToMapping).ToDictionary();
        var routes = input[1].Split($"{Environment.NewLine}").Select(ToRoute).ToList();

        var approvedRoutes = new List<Route>();

        foreach (var route in routes)
        {
            while(route.destination != "A" && route.destination != "R")
            {
                Mapping mapping = mappings.GetValueOrDefault(route.destination)!;

                foreach(var condition in mapping.conditions)
                {
                    var conditionResult = condition.HasNewDestination(route);

                    if (conditionResult)
                    {
                        break;
                    }
                }
            }

            if(route.destination == "A")
            {
                approvedRoutes.Add(route);
            }
        }

        var result = approvedRoutes.Sum(r => r.GetTotal());

        return new ValueTask<string>(result.ToString());
    }

    //121158073425385
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);

        var mappings = input[0].Split($"{Environment.NewLine}").Select(ToMapping).ToDictionary();
        var routes = new List<RouteRange>() { new RouteRange(new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), new Range(1, 4000), "in") };

        var approvedRoutes = new List<RouteRange>();

        while(routes.Count > 0)
        {
            var route = routes[0];

            while (route.destination != "A" && route.destination != "R")
            {
                Mapping mapping = mappings.GetValueOrDefault(route.destination)!;

                foreach (var condition in mapping.conditions)
                {
                    //last mapping so whole range gets assigned
                    if(condition.operation == "=")
                    {
                        route.destination = condition.destination;
                        break;
                    }

                    var isFirstValue = condition.first == "x" || condition.first == "m" || condition.first == "a" || condition.first == "s";

                    //value smaller than conditionValue => x < 123 || 123 > x
                    if (condition.operation == "<" && isFirstValue ||
                        (condition.operation == ">" && !isFirstValue))
                    {
                        var newRoute = SplitRoute(route, isFirstValue ? condition.first : condition.second, isFirstValue ? int.Parse(condition.second) : int.Parse(condition.first), true, condition.destination);
                        routes.Add(newRoute);
                    }
                    //value bigger than condition value => x > 123 || 123 < x
                    else
                    {
                        var newRoute = SplitRoute(route, isFirstValue ? condition.first : condition.second, isFirstValue ? int.Parse(condition.second) : int.Parse(condition.first), false, condition.destination);
                        routes.Add(newRoute);
                    }
                }
            }

            if (route.destination == "A")
            {
                approvedRoutes.Add(route);
            }

            routes.Remove(route);
        }

        var result = ulong.MinValue;
        foreach (var route in approvedRoutes)
        {
            result += route.GetTotal();
        }

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
    }

    private KeyValuePair<string, Mapping> ToMapping(string input)
    {
        var splitedKey = input.Split("{");
        var key = splitedKey[0];

        var splitedConditions = splitedKey[1].Replace("}", "").Split(",");
        var conditions = new List<Condition>();

        // not for last
        for(var i = 0; i < splitedConditions.Length - 1; i++)
        {
            var stringCondition = splitedConditions[i];
            var operation = stringCondition.Contains('<') ? "<" : ">";
            var destination = stringCondition.Split(":")[1];
            var first = stringCondition.Split(":")[0].Split(['<', '>'])[0];
            var second = stringCondition.Split(":")[0].Split(['<', '>'])[1];

            conditions.Add(new Condition(first, operation, second, destination));
        }

        // last
        conditions.Add(new Condition("", "=", "", splitedConditions[^1]));

        return new KeyValuePair<string, Mapping>(key, new Mapping(key, conditions));
    }

    private class Mapping(string key, List<Condition> conditions)
    {
        public string key { get; set; } = key;
        public List<Condition> conditions { get; set; } = conditions;
    }

    private class Condition(string first, string operation, string second, string destination)
    {
        public string first { get; set; } = first;
        public string operation { get; set; } = operation;
        public string second { get; set; } = second;
        public string destination { get; set; } = destination;

        public bool HasNewDestination(Route path)
        {
            if (operation == "=")
            {
                path.destination = destination;
                return true;
            }

            if (first == "x" || first == "m" || first == "a" || first == "s")
            {
                var value = path.GetValue(first);
                if(operation == "<")
                {
                    if (value < int.Parse(second))
                    {
                        path.destination = destination;
                        return true;
                    }
                }
                else
                {
                    if (value > int.Parse(second))
                    {
                        path.destination = destination;
                        return true;
                    }
                }
            }
            else
            {
                var value = path.GetValue(second);
                if (operation == "<")
                {
                    if (int.Parse(first) < value)
                    {
                        path.destination = destination;
                        return true;
                    }
                }
                else
                {
                    if (int.Parse(first) > value)
                    {
                        path.destination = destination;
                        return true;
                    }
                }
            }

            return false;
        }
    };

    private static RouteRange SplitRoute(RouteRange oldRoute, string attribute, int value, bool isSmallerThan, string newDestination)
    {
        var newRoute = CoppyRoutRange(oldRoute);

        var oldRouteValue = oldRoute.GetValue(attribute);
        var newRouteValue = newRoute.GetValue(attribute);

        // smaller than => x < 123 || 123 > x
        if (isSmallerThan)
        {
            // no need for old value
            if (oldRouteValue.end < value)
            {
                oldRoute.destination = "R";
                newRoute.destination = newDestination;
            }
            // no need for new value
            else if (oldRouteValue.start >= value)
            {
                newRoute.destination = "R";
            }
            //split
            else
            {
                oldRouteValue.start = value;
                newRouteValue.end = value - 1;
                newRoute.destination = newDestination;
            }
        }
        // bigger than => x > 123 || 123 < x
        else
        {
            // no need for old value
            if (oldRouteValue.start > value)
            {
                oldRoute.destination = "R";
                newRoute.destination = newDestination;
            }
            // no need for new value
            else if (oldRouteValue.end <= value)
            {
                newRoute.destination = "R";
            }
            //split
            else
            {
                oldRouteValue.end = value;
                newRouteValue.start = value + 1;
                newRoute.destination = newDestination;
            }
        }

        return newRoute;
    }

    private Route ToRoute(string input)
    {
        var values = input.Replace("{", "").Replace("}", "").Split(",").Select(s => int.Parse(s.Substring(2, s.Length-2))).ToList();
        return new Route(values[0], values[1], values[2], values[3]);
    }

    private class Route(int x, int m, int a, int s)
    {
        public int x { get; set; } = x;
        public int m { get; set; } = m;
        public int a { get; set; } = a;
        public int s { get; set; } = s;
        public string destination { get; set; } = "in";

        public int GetValue(string attribute)
        {
            return attribute switch
            {
                "x" => x,
                "m" => m,
                "a" => a,
                "s" => s
            };
        }

        public int GetTotal()
        {
            return x + m + a + s;
        }
    }

    private static RouteRange CoppyRoutRange(RouteRange old)
    {
        return new RouteRange(new Range(old.x.start, old.x.end), new Range(old.m.start, old.m.end), new Range(old.a.start, old.a.end), new Range(old.s.start, old.s.end), old.destination);
    }

    private class RouteRange(Range x, Range m, Range a, Range s, string destination)
    {
        public Range x { get; set; } = x;
        public Range m { get; set; } = m;
        public Range a { get; set; } = a;
        public Range s { get; set; } = s;
        public string destination { get; set; } = destination;

        public Range GetValue(string attribute)
        {
            return attribute switch
            {
                "x" => x,
                "m" => m,
                "a" => a,
                "s" => s
            };
        }

        public ulong GetTotal()
        {
            // + 1 => 1 - 1 = 0 but actual option would be 1
            ulong xRange = (ulong)(x.end - x.start + 1);
            ulong mRange = (ulong)(m.end - m.start + 1);
            ulong aRange = (ulong)(a.end - a.start + 1);
            ulong sRange = (ulong)(s.end - s.start + 1);

            return xRange * mRange * aRange * sRange;
        }
    }

    private class Range(int start, int end)
    {
        public int start { get; set; } = start;
        public int end { get; set; } = end;
    }
}
