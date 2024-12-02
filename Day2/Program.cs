
using System.Diagnostics;

async Task<List<int[]>> ParseInputAsync() {
    await using var file = File.OpenRead("input.txt");
    using var reader = new StreamReader(file);

    List<int[]> ret = [];
    while (await reader.ReadLineAsync() is { } line) {
        ret.Add(line.Split(" ").Select(int.Parse).ToArray());
    }

    return ret;
}

bool IsSafe(int[] report) {
    var sign = report[0] < report[1] ? -1 : 1;
    return report.Zip(report.Skip(1)).All(x => sign * (x.First - x.Second) is >= 1 and <= 3);
}

bool IsNearlySafe(int[] report) {
    return IsSafe(report) || report.Select((_, i) => report[..i].Concat(report[(i+1)..]).ToArray()).Any(IsSafe);
}

Console.WriteLine((await ParseInputAsync()).Count(IsSafe));
Console.WriteLine((await ParseInputAsync()).Count(IsNearlySafe));
