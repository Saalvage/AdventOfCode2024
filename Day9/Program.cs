var input = (await File.ReadAllTextAsync("input.txt")).TrimEnd();
var slots = input.Where((_, i) => i % 2 == 0).Sum(x => x - '0');
var baseEnum = input.SelectMany((x, i) => Enumerable.Repeat(i % 2 == 0 ? i / 2 : long.MaxValue, x - '0'));
using var enumerator = baseEnum.Reverse().Where(x => x != long.MaxValue).GetEnumerator();
var sum = baseEnum.Select((x, i) => (x, i))
    .Take(slots)
    .Sum(v => v.i * (v.x == long.MaxValue
        ? enumerator.MoveNext() ? enumerator.Current : 0
        : v.x));
Console.WriteLine(sum);
