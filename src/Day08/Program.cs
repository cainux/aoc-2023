using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var instructions = lines[0];
var nodes = new Dictionary<string, (string L, string R)>();

for (var i = 2; i < lines.Length; i++)
{
    var r = Regex.Match(lines[i], @"(?<location>\S{3}) = \((?<L>\S{3}), (?<R>\S{3})\)");
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

// Part 2
var ghosts = nodes.Where(x => x.Key.EndsWith('A')).Select(x => new Ghost(x.Key)).ToArray();

Console.WriteLine("Part 2:");
foreach (var ghost in ghosts)
{
    Search(ghost);
    Console.WriteLine($"  {ghost.Steps}");
}

// 🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄
// 🎄 Then _manually_ take those numbers and find the Least Common Multiple of them 🎄
// 🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄🎄

return;

void Search(Ghost ghost)
{
    while (!ghost.Node.EndsWith('Z'))
    {
        var (L, R) = nodes[ghost.Node];
        ghost.Node = instructions[ghost.InstructionPosition] switch
        {
            'L' => L,
            _ => R,
        };
        ghost.InstructionPosition = ghost.InstructionPosition < instructions.Length - 1 ? ghost.InstructionPosition + 1 : 0;
        ghost.Steps++;
    }
}

internal class Ghost(string node)
{
    public string Node { get; set; } = node;
    public int InstructionPosition { get; set; }
    public int Steps { get; set; }

    public override string ToString() => $"{Node}, {Steps} [{InstructionPosition}]";
}