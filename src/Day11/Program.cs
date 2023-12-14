// ReSharper disable InconsistentNaming
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var expanded = Expand(lines);

var G = expanded
    .SelectMany((row, y) => Regex.Matches(row, "\\#").Select(m => new P(m.Index, y)))
    .ToArray();

var pairs = G.SelectMany((G1, i) => G.Skip(i + 1).Select(G2 => (G1, G2))).ToArray();

Console.WriteLine($"There are {G.Length} galaxies and {pairs.Length} pairs");
Console.WriteLine();

// Part 1
var part1 = pairs
    .Select(x => ShortestPath(x.G1, x.G2, expanded[0].Length, expanded.Length))
    .Select(x => x.Length)
    .Sum();

Console.WriteLine($"Part 1: {part1}");

return;

static string[] Expand(IReadOnlyList<string> input)
{
    var output = new List<string>(input.Count);
    var emptyXs = new List<int>();

    for (var x = 0; x < input[0].Length; x++)
        if (!string.Join(null, input.Select(l => l[x]).ToArray()).Contains('#'))
            emptyXs.Add(x);

    emptyXs.Reverse();

    foreach (var row in input)
    {
        var newRow = emptyXs.Aggregate(row, (current, x) => current.Insert(x, "."));
        output.Add(newRow);

        if (!newRow.Contains('#')) output.Add(newRow);
    }

    return output.ToArray();
}

P[] ShortestPath(P G1, P G2, int maxX, int maxY)
{
    var shortestPath = new List<P>();
    var current = G1;

    while (current != G2)
    {
        var next = GetNeighbours(current, maxX, maxY)
            .OrderBy(p => ManhattanDistance(p, G2))
            .First();

        shortestPath.Add(next);
        current = next;
    }

    return shortestPath.ToArray();
}

static IEnumerable<P> GetNeighbours(P G, int maxX, int maxY)
{
    if (G.X > 0) yield return G with {X = G.X - 1};
    if (G.X < maxX) yield return G with {X = G.X + 1};
    if (G.Y > 0) yield return G with {Y = G.Y - 1};
    if (G.Y < maxY) yield return G with {Y = G.Y + 1};
}

static int ManhattanDistance(P G1, P G2) => Math.Abs(G1.X - G2.X) + Math.Abs(G1.Y - G2.Y);

internal record P(int X, int Y) { public override string ToString() => $"({X+1},{Y+1})"; }