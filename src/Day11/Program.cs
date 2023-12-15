// ReSharper disable InconsistentNaming
global using P = (int X, int Y);
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var G = lines.SelectMany((row, y) => Regex.Matches(row, "\\#").Select(m => new P(m.Index, y))).ToArray();
var pairs = G.SelectMany((G1, i) => G.Skip(i + 1).Select(G2 => (G1, G2)));
var xs = new HashSet<int>(); var ys = new HashSet<int>();

for (var x = 0; x < lines[0].Length; x++)
    if (!string.Join(null, lines.Select(l => l[x]).ToArray()).Contains('#'))
        xs.Add(x);

for (var y = 0; y < lines.Length; y++)
    if (!lines[y].Contains('#'))
        ys.Add(y);

var paths = pairs.Select(x => ShortestPath(x.G1, x.G2, lines[0].Length, lines.Length)).ToArray();

Console.WriteLine($"Part 1: {paths.Sum(p => CalculateDistance(p, xs, ys, 2))}");
Console.WriteLine($"Part 2: {paths.Sum(p => CalculateDistance(p, xs, ys, 1000000))}");
return;

static IEnumerable<P> ShortestPath(P G1, P G2, int maxX, int maxY)
{
    var shortestPath = new List<P>();
    var current = G1;
    var prev = current;

    while (current != G2)
    {
        var next = GetNeighbours(current, prev, maxX, maxY).MinBy(p => ManhattanDistance(p, G2));
        shortestPath.Add(next);
        prev = current;
        current = next;
    }

    return shortestPath;
}

static IEnumerable<P> GetNeighbours(P G, P prev, int maxX, int maxY)
{
    if (prev != G with {X = G.X - 1} && G.X > 0) yield return G with {X = G.X - 1};
    if (prev != G with {X = G.X + 1} && G.X < maxX) yield return G with {X = G.X + 1};
    if (prev != G with {Y = G.Y - 1} && G.Y > 0) yield return G with {Y = G.Y - 1};
    if (prev != G with {Y = G.Y + 1} && G.Y < maxY) yield return G with {Y = G.Y + 1};
}

static int ManhattanDistance(P G1, P G2) => Math.Abs(G1.X - G2.X) + Math.Abs(G1.Y - G2.Y);

static long CalculateDistance(IEnumerable<P> path, IReadOnlySet<int> xs, IReadOnlySet<int> ys, int multiplier) =>
    path.Aggregate(0, (i, s) => i + 1 * (xs.Contains(s.X) || ys.Contains(s.Y) ? multiplier : 1));