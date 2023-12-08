using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

// Part 1
Console.WriteLine(lines.Sum(Part01));

return;

int Part01(string line)
{
    var game = line.Split(':')[1].Split('|');
    var winningNumbers = Regex.Matches(game[0], "\\d+").Select(x => x.Value).ToHashSet();
    var numbersPlayed = Regex.Matches(game[1], "\\d+").Select(x => x.Value).ToHashSet();

    var matchCount = winningNumbers.Intersect(numbersPlayed).Count();
    var acc = 0;

    for (var i = 0; i < matchCount; i++)
        acc = acc == 0 ? 1 : acc * 2;

    return acc;
}