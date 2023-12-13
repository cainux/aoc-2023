using System.Diagnostics;

var lines = File.ReadAllLines("input.txt");

var maxX = lines[0].Length - 1;
var maxY = lines.Length - 1;

var allowedPipes = new Dictionary<string, string>
{
    // Start
    { "SN", "S|-7F" }, { "SE", "S-J7" }, { "SS", "S|JL" }, { "SW", "S-LF" },

    // Pipes: | - L J 7 F
    { "|N", "S|7F" }, { "|S", "S|LJ" },
    { "-E", "S-J7" }, { "-W", "S-LF" },
    { "LN", "S|7F" }, { "LE", "S-J7" },
    { "JN", "S|7F" }, { "JW", "S-LF" },
    { "7S", "S|LJ" }, { "7W", "S-LF" },
    { "FS", "S|LJ" }, { "FE", "S-J7" }
};

var S = lines.Select((line, i) => new P(line.IndexOf('S'), i)).Single(x => x.X >= 0);

var curr = S;
var prev = S;
var loop = new List<P>();

while (true)
{
    var nextLocation = NextPipeLocation(curr, prev);
    if (nextLocation == S)
        break;
    loop.Add(nextLocation);
    prev = curr;
    curr = nextLocation;
}

Console.WriteLine($"Part 1: {(loop.Count + 1)/2}");

var loopSet = loop.Select(p => $"{p.X},{p.Y}").ToHashSet();
var polygon = loop.Select(p => new PF(p.X, p.Y)).ToArray();
var acc = 0;

for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[0].Length; x++)
    {
        if (loopSet.Contains($"{x},{y}"))
            continue;

        if (PointInsidePolygon(new PF(x, y), polygon))
            acc++;
    }

Console.WriteLine($"Part 2: {acc}");

return;

P NextPipeLocation(P current, P previous)
{
    var currentPipe = lines[current.Y][current.X];
    // Console.WriteLine($"{currentPipe} {current}");
    var (x, y) = current;
    var east = new P(x + 1, y); var west = new P(x - 1, y);
    var north = new P(x, y - 1); var south = new P(x, y + 1);

    if (previous != west && x > 0 && CanGoTo(currentPipe + "W", west)) return west;
    if (previous != east && x < maxX && CanGoTo(currentPipe + "E", east)) return east;
    if (previous != north && y > 0 && CanGoTo(currentPipe + "N", north)) return north;
    if (previous != south && y < maxY && CanGoTo(currentPipe + "S", south)) return south;

    Debugger.Break();
    throw new UnreachableException();
}

bool CanGoTo(string k, P p) => allowedPipes.ContainsKey(k) && allowedPipes[k].Contains(lines[p.Y][p.X]);

static bool PointInsidePolygon(PF testPoint, PF[] polygon) // Point Inside Polygon Algorithm, taken from https://stackoverflow.com/a/14998816
{
    var result = false;
    var j = polygon.Length - 1;

    for (var i = 0; i < polygon.Length; i++)
    {
        if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y ||
            polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
        {
            if (polygon[i].X + (testPoint.Y - polygon[i].Y) /
               (polygon[j].Y - polygon[i].Y) *
               (polygon[j].X - polygon[i].X) < testPoint.X)
            {
                result = !result;
            }
        }
        j = i;
    }

    return result;
}

internal record P(int X, int Y) { public override string ToString() => $"({X+1},{Y+1})"; }

internal record PF(float X, float Y) { public override string ToString() => $"({X+1},{Y+1})"; }