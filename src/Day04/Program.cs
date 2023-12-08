using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

// Part 1
// Console.WriteLine(lines.Sum(Part01));

// Part 2
var cards = new Dictionary<int, (int, int)>();
var q = new Queue<(int CardNumber, int MatchCount)>();

foreach (var line in lines)
{
    var card = Part02(line);
    cards.Add(card.CardNumber, (card.CardNumber, card.MatchCount));
    q.Enqueue(card);
}

var acc = 0;

while (q.TryPeek(out var current))
{
    acc++;

    for (var i = 1; i <= current.MatchCount; i++)
        if (cards.TryGetValue(current.CardNumber + i, out var duplicate))
            q.Enqueue(duplicate);
}

Console.WriteLine(acc);

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

(int CardNumber, int MatchCount) Part02(string line)
{
    var lineParts = line.Split(':');
    var game = lineParts[1].Split('|');
    var winningNumbers = Regex.Matches(game[0], "\\d+").Select(x => x.Value).ToHashSet();
    var numbersPlayed = Regex.Matches(game[1], "\\d+").Select(x => x.Value).ToHashSet();

    var matchCount = winningNumbers.Intersect(numbersPlayed).Count();

    return (int.Parse(lineParts[0].Replace("Card ", string.Empty)), matchCount);
}