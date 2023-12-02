var input = File.ReadAllLines("input.txt");

// Part 01
var constraint = ParseHand("12 red, 13 green, 14 blue");
Console.WriteLine(input.Sum(x => Test(constraint, x)));

// Part 02
Console.WriteLine(input.Sum(x => Power(ParseGame(x))));
return;

int Test(Hand c, string gameLine)
{
    var g = ParseGame(gameLine);
    return g.Hands.Any(h => h.Red > c.Red || h.Green > c.Green || h.Blue > c.Blue) ? 0 : g.Number;
}

Game ParseGame(string gameLine)
{
    var parts = gameLine.Split(':');
    var number = int.Parse(parts[0].Split(' ')[1]);
    var hands = parts[1].Split(';').Select(x => x.Trim()).Select(ParseHand).ToArray();
    return new Game(number, hands);
}

Hand ParseHand(string handLine)
{
    int r = 0, g = 0, b = 0;
    var hands = handLine.Split(',').Select(x => x.Trim()).ToArray();

    foreach (var set in hands)
    {
        var parts = set.Split(' ');
        var colour = parts[1];
        var count = int.Parse(parts[0]);

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

int Power(Game game)
{
    var h = MinSet(game.Hands);
    return h.Red * h.Green * h.Blue;
}

Hand MinSet(Hand[] hands)
{
    int r = 0, g = 0, b = 0;

    foreach (var hand in hands)
    {
        r = Math.Max(r, hand.Red);
        g = Math.Max(g, hand.Green);
        b = Math.Max(b, hand.Blue);
    }

    return new Hand(r, g, b);
}

internal record Game(int Number, Hand[] Hands);

internal record Hand(int Red, int Green, int Blue);