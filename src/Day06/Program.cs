using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var times = GetNumbers(lines[0]);
var distances = GetNumbers(lines[1]);
var raceCount = times.Length;

var part1 = 1;

for (var i = 0; i < raceCount; i++)
{
    var race = i + 1;
    var time = times[i];
    var distance = distances[i];

    Console.WriteLine($"Race {race}: {time} {distance}");

    var acc = 0;

    for (var x = 1; x < time; x++)
    {
        var travelled = x * (time - x);
        if (travelled > distance)
        {
            Console.WriteLine($"  {x:000}: {travelled:000}");
            acc++;
        }
    }

    part1 *= acc;
}

Console.WriteLine(part1);

return;

static int[] GetNumbers(string line) => Regex.Matches(line, "\\d+").Select(x => int.Parse(x.Value)).ToArray();