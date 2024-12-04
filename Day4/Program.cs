using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

int CountOccurrences(IEnumerable<char> haystack) {
    var str = string.Join("", haystack);
    return Regexes.XMas().Matches(str).Count
           + Regexes.SamX().Matches(str).Count;
}

int XMas(string[] input)
    => input.Sum(CountOccurrences)
       + input.Transpose()
           .Sum(CountOccurrences)
       + input.GetDiagonals()
           .Sum(CountOccurrences)
       + input.Select(x => x.Reverse())
           .GetDiagonals()
           .Sum(CountOccurrences);

IEnumerable<int> GetIndices(IEnumerable<char> haystack)
    => haystack.Zip(haystack.Skip(1), haystack.Skip(2))
        .Select((x, i) => (x, i: i + 1))
        .Where(x => x.x is ('M', 'A', 'S') or ('S', 'A', 'M'))
        .Select(x => x.i);

IEnumerable<(int, int)> GetMasCoordinates(IEnumerable<IEnumerable<char>> input)
    => input.SelectMany((v, y) => v.Select((v, x) => (v, x, y)))
        .GroupBy(x => x.x - x.y)
        .Select(x => (x.Key, o: x.Min(x => Math.Max(x.x, x.y)), i: GetIndices(x.Select(x => x.v))))
        .SelectMany(x => x.i.Select(v => ((x.Key >= 0 ? x.o : 0) + v, (x.Key < 0 ? x.o : 0) + v)));

int XDashMas(string[] input)
    => GetMasCoordinates(input)
        .Intersect(GetMasCoordinates(input.Select(x => x.Reverse()))
            .Select(x => (input[0].Length - 1 - x.Item1, x.Item2)))
        .Count();

Console.WriteLine(XMas(input));
Console.WriteLine(XDashMas(input));

public static class Extensions {
    public static IEnumerable<IEnumerable<char>> Transpose(this IEnumerable<IEnumerable<char>> input)
        => input.SelectMany(v => v.Select((v, x) => (v, x)))
            .GroupBy(x => x.x, x => x.v);

    public static IEnumerable<IEnumerable<char>> GetDiagonals(this IEnumerable<IEnumerable<char>> input)
        => input.SelectMany((v, y) => v.Select((v, x) => (v, d: x - y)))
            .GroupBy(x => x.d, x => x.v);
}

public static partial class Regexes {
    [GeneratedRegex("XMAS")]
    public static partial Regex XMas();

    [GeneratedRegex("SAMX")]
    public static partial Regex SamX();
}
