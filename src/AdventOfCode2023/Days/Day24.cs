namespace AdventOfCode2023.Days;

public class Day24 : DayBase
{
    //20336
    public override ValueTask<string> Solve_1()
    {
        var lines = GetInput(Input.Value).Select(l => ToLine(l.Replace(" @ ", ", ").Split(", "))).ToArray();

        double min = 200000000000000;
        double max = 400000000000000;

        var result = 0;

        for(int i = 0; i < lines.Length - 1; i++)
        {
            var currentLine = lines[i];
            for(int j = i + 1; j < lines.Length; j++)
            {
                var tmpLine = lines[j];

                var (x, y) = BerechneSchnittpunkt(currentLine.GeradenGleichung, tmpLine.GeradenGleichung);

                var isInPastOfCurrent = IsInPast(currentLine, x, y);
                var isInPastOfTmp = IsInPast(tmpLine, x, y);

                if(min <= x && x <= max &&
                   min <= y && y <= max &&
                   !isInPastOfCurrent && !isInPastOfTmp)
                {
                    result++;
                }
            }
        }

        return new ValueTask<string>(result.ToString());
    }

    //public override ValueTask<string> Solve_2()
    //{
    //    var lines = GetInput(Input.Value).Select(l => ToLine(l.Replace(" @ ", ", ").Split(", "))).ToArray();

    //    double min = 200000000000000;
    //    double max = 400000000000000;

    //    var result = 0;

    //    for (int i = 0; i < lines.Length - 1; i++)
    //    {
    //        var currentLine = lines[i];
    //        for (int j = i + 1; j < lines.Length; j++)
    //        {
    //            var tmpLine = lines[j];

    //            var (x, y) = BerechneSchnittpunkt(currentLine.GeradenGleichung, tmpLine.GeradenGleichung);

    //            var isInPastOfCurrent = IsInPast(currentLine, x, y);
    //            var isInPastOfTmp = IsInPast(tmpLine, x, y);

    //            if (min <= x && x <= max &&
    //               min <= y && y <= max &&
    //               !isInPastOfCurrent && !isInPastOfTmp)
    //            {
    //                result++;
    //            }
    //        }
    //    }

    //    return new ValueTask<string>(result.ToString());
    //}

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static bool IsInPast(Line line, double x, double y)
    {
        if(line.Point1.X < line.Point2.X)
        {
            if(x < line.Point1.X)
            {
                return true;
            }
        }
        else
        {
            if (x > line.Point1.X)
            {
                return true;
            }
        }

        if (line.Point1.Y < line.Point2.Y)
        {
            if (y < line.Point1.Y)
            {
                return true;
            }
        }
        else
        {
            if (y > line.Point1.Y)
            {
                return true;
            }
        }

        return false;
    }

    // Berechnet die GeradenGleichung anhand 2 Punkte einer Gerade
    static (double a, double b) GeradenGleichung(Point3D point1, Point3D point2)
    {
        // y = ax + b

        // Berechnung der Steigung a
        double a = (double)(point2.Y - point1.Y) / (point2.X - point1.X);

        // Verwenden Sie einen der Punkte, um den y-Achsenabschnitt b zu berechnen: y = ax + b
        double b = point1.Y - a * point1.X;

        return (a, b);
    }

    // Berechnet den Schnittpunkt zweier Geraden
    static (double x, double y) BerechneSchnittpunkt((double a, double b) geradenGleichung1, (double a, double b) geradenGleichung2)
    {
        // Gleichung lösen: a1*x + b1 = a2*x + b2
        // Umformen: (a1 - a2) * x = b2 - b1
        // x = (b2 - b1) / (a1 - a2)
        double x = (geradenGleichung2.b - geradenGleichung1.b) / (geradenGleichung1.a - geradenGleichung2.a);

        // y-Wert für den Schnittpunkt berechnen
        double y = geradenGleichung1.a * x + geradenGleichung1.b;

        return (x, y);
    }

    public static Line ToLine(string[] input)
    {
        var line = new Line(input);
        line.GeradenGleichung = GeradenGleichung(line.Point1, line.Point2);

        return line;
    }

    public class Line(string[] input)
    {
        public Point3D Point1 { get; set; } = new Point3D(long.Parse(input[0]), long.Parse(input[1]), long.Parse(input[2]));

        public Point3D Point2 { get; set; } = new Point3D(long.Parse(input[0]) + long.Parse(input[3]), long.Parse(input[1]) + long.Parse(input[4]), long.Parse(input[2]) + long.Parse(input[5]));

        public (double a, double b) GeradenGleichung { get; set; }
    }

    public class Point3D(long x, long y, long z)
    {
        public long X { get; set; } = x;
        public long Y { get; set; } = y;
        public long Z { get; set; } = z;
    }
}
