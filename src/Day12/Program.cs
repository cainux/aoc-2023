var lines = File.ReadAllLines("input.txt").Select(x => x.Split(" ")).ToArray();
var input1 = lines.Select(x => (x[0], x[1].Split(',').Select(long.Parse).ToArray()));

Dictionary<string, long> cache = new();
var part1 = input1.Sum(l => Walk(string.Empty, l.Item1, l.Item2));
Console.WriteLine($"Part 1: {part1}");

var input2 = lines.Select(x =>
    (
        string.Join("?", Enumerable.Repeat(x[0], 5)),
        string.Join(",", Enumerable.Repeat(x[1], 5)).Split(',').Select(long.Parse).ToArray()
    ));

var part2 = input2.Sum(l => Walk(string.Empty, l.Item1, l.Item2));
Console.WriteLine($"Part 2: {part2}");
return;

long Walk(string damaged, string input, long[] groups)
{
    if (groups.Length == 0)
    {
        if (input.Contains('#')) return 0;
        return 1;
    }

    if (input.Length == 0 && damaged.Length == groups[0] && groups.Length == 1) return 1;
    if (input.Length == 0) return 0;

    return input[0] switch
    {
        '.' => damaged.Length == groups[0] ? CachedCall(Walk, string.Empty, input[1..], groups[1..])
            : damaged.Length == 0 ? CachedCall(Walk, damaged, input[1..], groups)
            : 0,
        '#' => damaged.Length < groups[0] ? CachedCall(Walk, damaged + '#', input[1..], groups)
            : 0,
        '?' => CachedCall(Walk, damaged, '#' + input[1..], groups) + CachedCall(Walk, damaged, '.' + input[1..], groups),
        _ => throw new System.Diagnostics.UnreachableException()
    };
}

long CachedCall(Func<string, string, long[], long> func, string damaged, string input, long[] groups)
{
    var key = $"{damaged}|{input}|{string.Join(",", groups)}";
    if (cache.TryGetValue(key, out var value))
        return value;

    value = func(damaged, input, groups);
    cache.Add(key, value);
    return value;
}