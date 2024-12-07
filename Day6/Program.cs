using Pos = (int X, int Y);

var input = File.ReadAllLines("input.txt");

var field = new Tile[input[0].Length, input.Length];

var (x, y, _) = input.SelectMany((l, y) => l.Select((c, x) => (x, y, t: field[x, y] = c switch {
    '.' => Tile.Empty,
    '#' => Tile.Obstruction,
    '^' => Tile.Start,
}))).Single(x => x.t == Tile.Start);

Console.WriteLine(Run(field, (x, y), (0, -1), true));

(int, int)? Run(Tile[,] field, Pos start, Pos dir, bool loop = false) {
    var visited = new HashSet<(Pos Pos, Pos Dir)>();
    var loops = new HashSet<Pos>();

    var (x, y) = start;
    do {
        if (!visited.Add(((x, y), dir))) {
            return null;
        }

        Pos newPos = (x + dir.X, y + dir.Y);
        if (newPos.X >= field.GetLength(0) || newPos.Y >= field.GetLength(1) || newPos.X < 0 || newPos.Y < 0) {
            break;
        }
        if (field[newPos.X, newPos.Y] == Tile.Obstruction) {
            dir = dir switch {
                (0, -1) => (1, 0),
                (1, 0) => (0, 1),
                (0, 1) => (-1, 0),
                (-1, 0) => (0, -1),
            };
        } else {
            if (loop) {
                var og = field[newPos.X, newPos.Y];
                field[newPos.X, newPos.Y] = Tile.Obstruction;
                if (!loops.Contains(newPos) && Run(field, start, (0, -1)) == null) {
                    loops.Add(newPos);
                }
                field[newPos.X, newPos.Y] = og;
            }

            (x, y) = newPos;
        }
    } while (true);
    return (visited.DistinctBy(v => v.Pos).Count(), loops.Count);
}

[Flags]
public enum Tile {
    Empty = 0,
    Obstruction = 1,
    Start = 2,
}
