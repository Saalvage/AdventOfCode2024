using System.Diagnostics;

async Task<(List<int>, List<int>)> ParseInputAsync() {
    await using var file = File.OpenRead("input.txt");
    using var reader = new StreamReader(file);

    List<int> a = [];
    List<int> b = [];
    while (await reader.ReadLineAsync() is { } line) {
        var split = line.Split("   ");
        Debug.Assert(split.Length == 2);
        a.Add(int.Parse(split[0]));
        b.Add(int.Parse(split[1]));
    }

    return (a, b);
}

async Task<int> PartA() {
    var (a, b) = await ParseInputAsync();
    Parallel.ForEach([a, b], x => x.Sort());

    return a.Zip(b)
        .Sum(x => Math.Abs(x.Second - x.First));
}

async Task<int> PartB() {
    var (a, b) = await ParseInputAsync();
    var lookup = b.GroupBy(x => x)
        .ToDictionary(x => x.Key, x => x.Count());
    return a.Sum(x => x * lookup.GetValueOrDefault(x, 0));
}

Console.WriteLine(await PartA());
Console.WriteLine(await PartB());
