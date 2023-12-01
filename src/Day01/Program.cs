using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

// Part 1
Console.WriteLine(lines.Sum(x => Part01(x)));

// Part 2
var numbers = new Dictionary<string, string>
{
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" },
};

Console.WriteLine(lines.Sum(x => Part02(x)));

return;

short Part01(string line) => Convert.ToInt16($"{line.First(char.IsDigit)}{line.Last(char.IsDigit)}");

short Part02(string line)
{
    // var og = line;
    var inserts = new List<(int p, string c)>();

    foreach (var n in numbers)
        foreach (Match m in Regex.Matches(line, n.Key))
            inserts.Add((m.Index, n.Value));

    foreach (var (p, c) in inserts.OrderByDescending(x => x.p))
        line = line.Insert(p, $"[{c}]");

    // Console.WriteLine($"{Part01(line)} | {og} | {line}");

    return Part01(line);
}