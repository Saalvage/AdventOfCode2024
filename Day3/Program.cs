using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("input.txt");

var outputA = Regexes.Multiplication().Matches(input)
    .Sum(MulMatchToResult);
Console.WriteLine(outputA);

var outputB = Instruction.Execute(Regexes.Do().Matches(input).Select(x => (Instruction)new Do(x.Index))
    .Concat(Regexes.Dont().Matches(input).Select(x => (Instruction)new Dont(x.Index)))
    .Concat(Regexes.Multiplication().Matches(input)
        .Select(x => (Instruction)new Multiply(x.Index, MulMatchToResult(x))))
    .OrderBy(x => x.Index));
Console.WriteLine(outputB);

int MulMatchToResult(Match m) => int.Parse(m.Groups["a"].ValueSpan) * int.Parse(m.Groups["b"].ValueSpan);

static partial class Regexes {
    [GeneratedRegex(@"mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)")]
    public static partial Regex Multiplication();

    [GeneratedRegex(@"do\(\)")]
    public static partial Regex Do();

    [GeneratedRegex(@"don't\(\)")]
    public static partial Regex Dont();
}

// Poor man's discriminated union.
abstract class Instruction {
    public int Index { get; }

    protected Instruction(int index) {
        Index = index;
    }

    public static int Execute(IEnumerable<Instruction> instrs) {
        var enabled = true;
        var sum = 0;
        foreach (var i in instrs) {
            switch (i) {
                case Multiply m when enabled:
                    sum += m.Result;
                    break;
                case Do:
                    enabled = true;
                    break;
                case Dont:
                    enabled = false;
                    break;
            }
        }
        return sum;
    }
}

class Multiply : Instruction {
    public int Result { get; }

    public Multiply(int index, int result) : base(index) {
        Result = result;
    }
}

class Do : Instruction {
    public Do(int index) : base(index) { }
}

class Dont : Instruction {
    public Dont(int index) : base(index) { }
}
