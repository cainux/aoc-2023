using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var times = GetNumbers(lines[0]);
var distances = GetNumbers(lines[1]);
var raceCount = times.Length;

var part1 = 1;

for (var i = 0; i < raceCount; i++)
{
    var t = times[i];
    var d = distances[i];
    var acc = 0;

    for (var x = 1; x < t; x++)
    {
        var travelled = x * (t - x);
        if (travelled > d)
            acc++;
    }

    part1 *= acc;
}

Console.WriteLine(part1);

// Part 2
var time = ulong.Parse(string.Join("", times.Select(x => x.ToString())));
var distance = ulong.Parse(string.Join("", distances.Select(x => x.ToString())));

var c = 0;
for (var x = 1UL; x < time; x++)
{
    var travelled = x * (time - x);
    if (travelled > distance)
        c++;
}

Console.WriteLine(c);

return;

static int[] GetNumbers(string line) => Regex.Matches(line, "\\d+").Select(x => int.Parse(x.Value)).ToArray();