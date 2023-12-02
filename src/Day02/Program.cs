var input = File.ReadAllLines("input.txt");

// Part 01
var constraint = ParseHand("12 red, 13 green, 14 blue");

Console.WriteLine(input.Sum(x => Test(constraint, x)));

return;

short Test(Hand c, string gameLine)
{
    var g = ParseGame(gameLine);
    return g.Hands.Any(h => h.Red > c.Red || h.Green > c.Green || h.Blue > c.Blue) ? (short) 0 : g.Number;
}

Game ParseGame(string gameLine)
{
    var parts = gameLine.Split(':');
    var number = short.Parse(parts[0].Split(' ')[1]);
    var hands = parts[1].Split(';').Select(x => x.Trim()).Select(ParseHand).ToArray();
    return new Game(number, hands);
}

Hand ParseHand(string handLine)
{
    short r = 0, g = 0, b = 0;
    var hands = handLine.Split(',').Select(x => x.Trim()).ToArray();

    foreach (var set in hands)
    {
        var parts = set.Split(' ');
        var colour = parts[1];
        var count = short.Parse(parts[0]);

        switch (colour)
        {
            case "red":
                r += count;
                break;
            case "green":
                g += count;
                break;
            case "blue":
                b += count;
                break;
            default:
                throw new InvalidDataException($"Unknown color: {colour}");
        }
    }

    return new Hand(r, g, b);
}

internal record Game(short Number, Hand[] Hands);

internal record Hand(short Red, short Green, short Blue);