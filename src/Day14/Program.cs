using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var grid = lines.Select(l => l.ToCharArray()).ToArray();

var cache = new Dictionary<char, (int, int)[]>();
var seen = new HashSet<string>();

Dump(grid);
Console.WriteLine($"Part 1: {Score(grid)}");

var functions = new (char, Action<int, int, char[][]>)[] {('N', North), ('W', West), ('S', South), ('E', East)};
const int cycles = 1000000000;

for (var i = 0; i < cycles; i++)
{
    Cycle(grid, functions);
    var flattened = string.Join(' ', grid.Select(y => string.Join(string.Empty, y)));
    if (!seen.Add(flattened))
    {
        var cycle = i + 1;

        if (cycles % cycle == 0)
        {
            Console.WriteLine($"Seen {cycle}: {Score(grid)}");
        }
    }
}

return;

void Dump(char[][] input)
{
    foreach (var l in input.Select(y => string.Join(string.Empty, y)))
        Console.WriteLine(l);
    Console.WriteLine();
}

void Cycle(char[][] input, (char, Action<int, int, char[][]>)[] cycle)
{
    foreach (var func in cycle)
        Tilt(input, func);
}

IEnumerable<(int x, int y)> CachedCall(Func<char[][], char, IEnumerable<(int x, int y)>> func, char[][] input, char direction)
{
    if (cache.TryGetValue(direction, out var value))
        return value;

    value = func(input, direction).ToArray();
    cache.Add(direction, value);
    return value;
}

IEnumerable<(int x, int y)> Path(char[][] input, char direction)
{
    switch (direction)
    {
        case 'N':
        {
            for (var y = 0; y < input.Length; y++)
            for (var x = 0; x < input[0].Length; x++)
                yield return (x, y);
            break;
        }
        case 'W':
        {
            for (var y = 0; y < input.Length; y++)
            for (var x = 0; x < input[0].Length; x++)
                yield return (x, y);
            break;
        }
        case 'S':
        {
            for (var y = input.Length - 1; y >= 0; y--)
            for (var x = 0; x < input[0].Length; x++)
                yield return (x, y);
            break;
        }
        case 'E':
        {
            for (var y = 0; y < input.Length; y++)
            for (var x = input[0].Length - 1; x >= 0; x--)
                yield return (x, y);
            break;
        }
    }
}

void Tilt(char[][] input, (char d, Action<int, int, char[][]> f) func)
{
    foreach (var (x, y) in CachedCall(Path, input, func.d))
    {
        if (input[y][x] == 'O')
        {
            func.f(x, y, input);
        }
    }
}

void North(int x, int y, char[][] input)
{
    var step = 0;
    var nextY = y - 1;

    while (nextY >= 0)
    {
        if (input[nextY][x] != '.') break;

        step++;
        nextY = y - step - 1;
    }

    if (step <= 0) return;

    input[y - step][x] = 'O';
    input[y][x] = '.';
}

void West(int x, int y, char[][] input)
{
    var step = 0;
    var nextX = x - 1;

    while (nextX >= 0)
    {
        if (input[y][nextX] != '.') break;

        step++;
        nextX = x - step - 1;
    }

    if (step <= 0) return;

    input[y][x - step] = 'O';
    input[y][x] = '.';
}

void South(int x, int y, char[][] input)
{
    var step = 0;
    var nextY = y + 1;

    while (nextY < input.Length)
    {
        if (input[nextY][x] != '.') break;

        step++;
        nextY = y + step + 1;
    }

    if (step <= 0) return;

    input[y + step][x] = 'O';
    input[y][x] = '.';
}

void East(int x, int y, char[][] input)
{
    var step = 0;
    var nextX = x + 1;

    while (nextX < input[0].Length)
    {
        if (input[y][nextX] != '.') break;

        step++;
        nextX = x + step + 1;
    }

    if (step <= 0) return;

    input[y][x + step] = 'O';
    input[y][x] = '.';
}

int Score(char[][] input) => input
    .Select(y => string.Join(string.Empty, y))
    .Select((t, y) => Regex.Matches(t, "O").Count * (input.Length - y))
    .Sum();