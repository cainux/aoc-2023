using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var instructions = lines[0];
var nodes = new Dictionary<string, (string L, string R)>();

for (var i = 2; i < lines.Length; i++)
{
    var r = Regex.Match(lines[i], @"(?<location>\D{3}) = \((?<L>\D{3}), (?<R>\D{3})\)");
    nodes.Add(r.Groups["location"].Value, (r.Groups["L"].Value, r.Groups["R"].Value));
}

// Part 1
var current = "AAA";
var j = 0;
var part1 = 0;

while (current != "ZZZ")
{
    var (L, R) = nodes[current];
    current = instructions[j] switch
    {
        'L' => L,
        'R' => R,
        _ => throw new Exception("Invalid instruction")
    };
    j = j < instructions.Length - 1 ? j + 1 : 0;
    part1++;
}

Console.WriteLine($"Part 1: {part1}");