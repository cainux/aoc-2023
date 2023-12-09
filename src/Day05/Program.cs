using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var seeds = Array.Empty<long>();
Map? start = null;
Map? currentMap = null;

foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line.Trim())) continue;

    if (line.StartsWith("seeds:"))
    {
        seeds = Regex.Matches(line, "\\d+").Select(x => long.Parse(x.Value)).ToArray();
        continue;
    }

    if (Regex.Match(line, ".+-to-.+ map:").Success)
    {
        var newMap = new Map();

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
            long.Parse(m[1].Value),
            long.Parse(m[0].Value),
            long.Parse(m[2].Value)
        );
        currentMap.Ranges.Add(range);
    }
}

Debug.Assert(start != null);

// Part 1
Console.WriteLine(seeds.Min(s => Part1(s, start)));

// Part 2
Debug.Assert(seeds.Length % 2 == 0);
// Split the seeds array into a collection of pairs
var pairs = seeds.Select((s, i) => (s, i)).GroupBy(x => x.i / 2).Select(g => g.Select(x => x.s).ToArray()).ToArray(); // this line was AI generated 🤖
var part2Input = pairs.Select(x => new SeedRange(x[0], x[1])).ToList();
var part2Output = Part2(part2Input, start);
Console.WriteLine(part2Output.Min(x => x.Start));

return;

long Part1(long seed, Map map)
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

List<SeedRange> Part2(List<SeedRange> input, Map? map)
{
    if (map == null) return input;

    map.Ranges.Sort((a, b) => a.SourceStart.CompareTo(b.SourceStart));

    var output = new List<SeedRange>();

    foreach (var seedRange in input)
    {
        // Any overlaps?
        if (seedRange.End < map.Ranges[0].SourceStart || seedRange.Start > map.Ranges[^1].SourceEnd)
        {
            output.Add(seedRange);
            continue;
        }

        var cursor = seedRange.Start;
        var rangeIndex = 0;

        while (cursor < seedRange.End)
        {
            // Section to the right of all the ranges
            if (rangeIndex > map.Ranges.Count - 1)
            {
                var finalRange = new SeedRange(cursor, Math.Abs(seedRange.End - cursor) + 1);
                output.Add(finalRange);
                break;
            }

            var range = map.Ranges[rangeIndex];

            // That section left of the next range
            if (cursor < range.SourceStart)
            {
                var leftRange = new SeedRange(cursor, range.SourceStart - cursor);
                output.Add(leftRange);

                // Move to the start of the overlapping range
                cursor = range.SourceStart;
            }

            if (cursor >= range.SourceStart && cursor <= range.SourceEnd )
            {
                // Area covered by the range
                var minEnd = Math.Min(seedRange.End, range.SourceEnd);
                var rangeSlice = new SeedRange(cursor, Math.Abs(cursor - minEnd) + 1);
                var mappedRange = rangeSlice with { Start = rangeSlice.Start + range.Delta };
                output.Add(mappedRange);
            }

            // Move to the end of the overlapping range
            cursor = Math.Max(range.SourceEnd + 1, cursor);
            rangeIndex++;
        }
    }

    return Part2(output, map.Next);
}

class Map
{
    public List<MapRange> Ranges { get; } = new();
    public Map? Next { get; set; }
}

record MapRange(long SourceStart, long DestinationStart, long Length)
{
    public long SourceEnd => SourceStart + Length - 1;
    public long Delta => DestinationStart - SourceStart;
    public override string ToString() => $"MR({SourceStart},{Length}) [Delta={Delta}]";
};

record SeedRange(long Start, long Length)
{
    public long End => Start + Length - 1;
    public override string ToString() => $"SR({Start},{Length})";
}