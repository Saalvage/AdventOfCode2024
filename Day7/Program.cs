var input = await File.ReadAllLinesAsync("input.txt");

var equations = input.Select(x => x.Split(": "))
    .Select(x => (g: long.Parse(x[0]), v: x[1].Split(' ').Select(long.Parse).ToArray()))
    .ToArray();

Console.WriteLine(SumSatisfiable(equations, false));
Console.WriteLine(SumSatisfiable(equations, true));

static long SumSatisfiable(IEnumerable<(long g, long[] v)> equations, bool hasConcatenation)
    => equations
        .Where(x => Satisfiable(x.g, x.v, hasConcatenation))
        .Sum(x => x.g);

static bool Satisfiable(long goal, long[] values, bool hasConcatenation = true) {
    if (values.Length == 1) {
        return values[0] == goal;
    }

    // Stop early if value is too large.
    if (values[0] > goal) {
        return false;
    }

    return Satisfiable(goal, [values[0] + values[1], ..values[2..]], hasConcatenation)
        || Satisfiable(goal, [values[0] * values[1], ..values[2..]], hasConcatenation)
        || hasConcatenation && long.TryParse(values[0].ToString() + values[1], out var res)
                            && Satisfiable(goal, [res, ..values[2..]], hasConcatenation);
}
