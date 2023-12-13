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

var currentLocation = S;
var previousLocation = S;
var acc = 0;

while (true)
{
    var nextLocation = NextPipeLocation(currentLocation, previousLocation);
    if (nextLocation == S)
        break;
    acc++;
    previousLocation = currentLocation;
    currentLocation = nextLocation;
}

Console.WriteLine((acc + 1)/2);

return;

P NextPipeLocation(P current, P previous)
{
    var currentPipe = lines[current.Y][current.X];
    Console.WriteLine($"{currentPipe} {current}");
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

internal record P(int X, int Y) { public override string ToString() => $"({X+1},{Y+1})"; }