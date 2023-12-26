namespace AdventOfCode2023.Days;

public class Day22 : DayBase
{
    //512
    public override ValueTask<string> Solve_1()
    {
        var input = GetInput(Input.Value);
        var forms = input.Select(ToForm).ToList();

        // Very important to sort!!!!!
        // Could've saved me 2 hours of painfull debugging xD
        forms = forms.OrderBy(f => f.Coordinates.Max(c => c.Z)).ToList();

        var map = GetMap(input);

        LetTheFormsFall(map, forms);

        var allDuplicatedHoldingForms = forms.Where(f => f.AboveForms.Count == 0 || f.AboveForms.All(fAbove => fAbove.UnderneathForms.Count >= 2)).ToList();

        var result = allDuplicatedHoldingForms.Count;
        return new ValueTask<string>(result.ToString());
    }

    //98167
    public override ValueTask<string> Solve_2()
    {
        var input = GetInput(Input.Value);
        var forms = input.Select(ToForm).ToList();

        // Very important to sort!!!!!
        // Could've saved me 2 hours of painfull debugging xD
        forms = forms.OrderBy(f => f.Coordinates.Max(c => c.Z)).ToList();

        var map = GetMap(input);

        LetTheFormsFall(map, forms);

        var allDuplicatedHoldingForms = forms.Where(f => f.AboveForms.Count == 0 || f.AboveForms.All(fAbove => fAbove.UnderneathForms.Count >= 2)).ToList();

        // Invert of allDuplicatedHoldingForms
        var formsToFall = forms.Where(f => !allDuplicatedHoldingForms.Contains(f)).ToList();

        // recursively get all forms that are based on the initial form and that they have only one the the initial form as a "base"
        // otherwise they wouldn't fall because they have 2 "legs" and you only cut one of them
        var result = 0;
        foreach (var f in formsToFall)
        {
            var tmpListOfAlreadyFallenForms = new List<Form>();

            foreach (var formAbove in f.AboveForms)
            {
                result += GetFallenFormsCount(formAbove, tmpListOfAlreadyFallenForms, f);
            }
        }

        return new ValueTask<string>(result.ToString());
    }

    private static List<string> GetInput(string input)
    {
        return input.Split($"{Environment.NewLine}").ToList();
    }

    private static int GetFallenFormsCount(Form f, List<Form> tmpListOfAlreadyFallenForms, Form initialForm)
    {
        if(!tmpListOfAlreadyFallenForms.Contains(f) && AllUnderneathAreOnlyBasedOnInitialForm(f, initialForm))
        {
            tmpListOfAlreadyFallenForms.Add(f);

            var tmpTotal = 1;
            foreach(var formAbove in f.AboveForms)
            {
                tmpTotal += GetFallenFormsCount(formAbove, tmpListOfAlreadyFallenForms, initialForm);
            }

            return tmpTotal;
        }

        return 0;
    }

    private static bool AllUnderneathAreOnlyBasedOnInitialForm(Form f, Form initialForm)
    {
        // list empy => we are at the bottom so not based on initial
        if (f.UnderneathForms.Count == 0)
        {
            return false;
        }

        foreach(var underneathFrom in f.UnderneathForms)
        {
            if(underneathFrom == initialForm && f.UnderneathForms.Count == 1)
            {
                return true; // form is initial and we only have one underneath form => we are basen on initial
            }

            if (!AllUnderneathAreOnlyBasedOnInitialForm(underneathFrom, initialForm))
            {
                return false; // one of the lowers wasn't based on initial => we are not based on initial
            }
        }

        return true; // we are not the lowest one and no lower form said that it wasn't based on inital => we are based on initial
    }

    private static void LetTheFormsFall(List<List<List<char>>> map, List<Form> forms)
    {
        foreach (var form in forms)
        {
            var zReduction = 0;
            while (IsFormFallValid(form, zReduction, map, forms))
            {
                zReduction++;
            }

            foreach (var point in form.Coordinates)
            {
                point.Z = point.Z - zReduction;
                map[point.Z][point.Y][point.X] = '#';
            }
        }
    }

    private static bool IsFormFallValid(Form form, int zReduction, List<List<List<char>>> map, List<Form> forms)
    {
        var result = true;
        foreach(var point in form.Coordinates)
        {
            if (map[point.Z - zReduction - 1][point.Y][point.X] == '#')
            {
                var formThatIsUnderneath = forms.First(f => f.Coordinates.Any(c => c.Z == point.Z - zReduction - 1 && c.Y == point.Y && c.X == point.X));
                if (!form.UnderneathForms.Contains(formThatIsUnderneath))
                {
                    form.UnderneathForms.Add(formThatIsUnderneath);
                }

                if (!formThatIsUnderneath.AboveForms.Contains(form))
                {
                    formThatIsUnderneath.AboveForms.Add(form);
                }

                result = false;
            }

            if(point.Z - zReduction - 1 <= 0){
                result = false;
            }
        }

        return result;
    }

    private static List<List<List<char>>> GetMap(List<string> input)
    {
        var xMax = input.Max(s => int.Parse(s.Split('~')[1].Split(',')[0]));
        var yMax = input.Max(s => int.Parse(s.Split('~')[1].Split(',')[1]));
        var zMax = input.Max(s => int.Parse(s.Split('~')[1].Split(',')[2]));

        var list = new List<List<List<char>>>();

        for(int z = 0; z <= zMax; z++)
        {
            var height = new List<List<char>>();
            for(int y = 0; y <= yMax; y++)
            {
                var length = new List<char>();
                for(int x = 0; x <= xMax; x++)
                {
                    length.Add('.');
                }
                height.Add(length);
            }
            list.Add(height);
        }

        return list;
    }

    public static Form ToForm(string input)
    {
        var splited = input.Split('~');

        var first = splited[0].Split(',').Select(int.Parse).ToList();
        var second = splited[1].Split(',').Select(int.Parse).ToList();

        var form = new Form();

        for (int z = first[2]; z <= second[2]; z++)
        {
            for(int y = first[1]; y <= second[1]; y++)
            {
                for(int x = first[0]; x <= second[0]; x++)
                {
                    form.Coordinates.Add(new Point3D(x, y, z));
                }
            }
        }

        return form;
    }

    public class Form()
    {
        public List<Point3D> Coordinates { get; set; } = new List<Point3D>();

        public List<Form> UnderneathForms { get; set; } = new List<Form>();

        public List<Form> AboveForms { get; set; } = new List<Form>();
    }

    public class Point3D(int x, int y, int z)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;
    }
}
