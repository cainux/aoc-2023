using System.Text.RegularExpressions;

const char period = '.';
var lines = File.ReadAllLines("input.txt");
var maxX = lines[0].Length - 1;
var maxY = lines.Length - 1;
var acc = 0;

for (var y = 0; y < lines.Length; y++)
    acc += Regex.Matches(lines[y], @"\d+").Sum(m => IsPartNumber(m.Index, y, m.Length) ? int.Parse(m.Value) : 0);

Console.WriteLine(acc);

return;

bool IsPartNumber(int x, int y, int length)
{
    if (x > 0 && lines[y][x - 1] != period) return true;
    if (x + length < maxX && lines[y][x + length] != period) return true;

    if (y > 0)
    {
        var lineAbove = lines[y - 1];
        var start = Math.Max(0, x - 1);
        var end = Math.Min(maxX, x + length);
        for (var i = start; i <= end; i++)
            if (lineAbove[i] != period) return true;
    }

    if (y + 1 < maxY)
    {
        var lineBelow = lines[y + 1];
        var start = Math.Max(0, x - 1);
        var end = Math.Min(maxX, x + length);
        for (var i = start; i <= end; i++)
            if (lineBelow[i] != period) return true;
    }

    return false;
}