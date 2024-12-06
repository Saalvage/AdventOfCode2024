var lines = await File.ReadAllLinesAsync("input.txt");

var rules = lines.TakeWhile(x => x != "")
    .Select(x => x.Split('|').Select(int.Parse).ToArray())
    .Aggregate(new Dictionary<int, List<int>>(), (d, i) => {
        if (!d.TryGetValue(i[1], out var l)) {
            d.Add(i[1], [i[0]]);
        } else {
            l.Add(i[0]);
        }
        return d;
    });

var updates = lines.SkipWhile(x => x != "").Skip(1)
    .Select(x => x.Split(',').Select(int.Parse).ToArray());

var correctCount = updates.Where(x => !TryGetIncorrectIndices(x, out _))
    .Sum(x => x[x.Length / 2]);

var wrongFixedCount = updates.Where(x => TryGetIncorrectIndices(x, out _))
    .Sum(x => Fix(x)[x.Length / 2]);

Console.WriteLine(correctCount);
Console.WriteLine(wrongFixedCount);

bool TryGetIncorrectIndices(int[] update, out (int, int) indices) {
    var h = new Dictionary<int, int>();
    indices = update.Select((x, i) => (x, i))
        .SkipWhile(x => {
            if (rules.TryGetValue(x.x, out var forbidden)) {
                foreach (var e in forbidden) {
                    h.TryAdd(e, x.i);
                }
            }
            return !h.ContainsKey(x.x);
        }).Select(x => (x.i, h[x.x]))
        .FirstOrDefault();
    return indices != default;
}

int[] Fix(int[] update) {
    var ret = (int[])update.Clone();
    while (TryGetIncorrectIndices(ret, out var indices)) {
        (ret[indices.Item1], ret[indices.Item2]) = (ret[indices.Item2], ret[indices.Item1]);
    }
    return ret;
}
