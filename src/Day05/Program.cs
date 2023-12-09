using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var seeds = Array.Empty<ulong>();
Map? start = null;
Map? currentMap = null;

foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line.Trim())) continue;

    if (line.StartsWith("seeds:"))
    {
        seeds = Regex.Matches(line, "\\d+").Select(x => ulong.Parse(x.Value)).ToArray();
        continue;
    }

    if (Regex.Match(line, ".+-to-.+ map:").Success)
    {
        var m = Regex.Match(line, "(?<source>.+)-to-(?<destination>.+) map:");

        var newMap = new Map
        {
            Source = m.Groups["source"].Value,
            Destination = m.Groups["destination"].Value
        };

        if (currentMap != null) currentMap.Next = newMap;
        else start = newMap;
        currentMap = newMap;

        continue;
    }

    if (Regex.Match(line, @"\d+\s?").Success)
    {
        if (currentMap == null) throw new Exception("No map defined");
        var m = Regex.Matches(line, "\\d+");
        if (m.Count != 3) throw new Exception("Invalid range");
        var range = new MapRange(
            ulong.Parse(m[1].Value),
            ulong.Parse(m[0].Value),
            ulong.Parse(m[2].Value)
        );
        currentMap.Ranges.Add(range);
    }
}

Debug.Assert(start != null);

// Part 1
Console.WriteLine(seeds.Min(s => Process(s, start)));

return;

ulong Process(ulong seed, Map map)
{
    var result = seed;
    var cursor = map;

    while (cursor != null)
    {
        foreach (var range in cursor.Ranges)
            if (result >= range.SourceStart && result < range.SourceStart + range.Length)
            {
                result += range.DestinationStart - range.SourceStart;
                break;
            }

        cursor = cursor.Next;
    }

    return result;
}

class Map
{
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public List<MapRange> Ranges { get; } = new();
    public Map? Next { get; set; }
}

record MapRange(ulong SourceStart, ulong DestinationStart, ulong Length);