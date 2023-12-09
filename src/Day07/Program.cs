var hands = File.ReadAllLines("input.txt")
    .Select(line => line.Split(" "))
    .Select(parts => new Hand(parts[0], int.Parse(parts[1])))
    .ToList();

hands.Sort(CompareHands);

var rank = 1;
var part1 = hands.Sum(hand => hand.Bid * rank++);
Console.WriteLine($"Part 1: {part1}");

return;

int CompareHands(Hand a, Hand b)
{
    var scoreA = Score(a);
    var scoreB = Score(b);
    return scoreA != scoreB ? scoreA.CompareTo(scoreB) : SecondaryCompare(a.Cards, b.Cards);
}

int SecondaryCompare(string a, string b)
{
    const string cards = "23456789TJQKA";
    
    for (var i = 0; i < 5; i++)
    {
        var indexA = cards.IndexOf(a[i]);
        var indexB = cards.IndexOf(b[i]);

        if (indexA != indexB)
            return indexA.CompareTo(indexB);
    }

    throw new Exception("Equal cards!?!");
}

HandType Score(Hand hand)
{
    var groups = hand.Cards
        .GroupBy(c => c)
        .Select(g => (g.Key, g.Count()))
        .OrderByDescending(g => g.Item2)
        .ToList();

    return groups switch
    {
        [{Item2: 5}] => HandType.FiveOfAKind,
        [{Item2: 4}, _] => HandType.FourOfAKind,
        [{Item2: 3}, {Item2: 2}] => HandType.FullHouse,
        [{Item2: 3}, _, _] => HandType.ThreeOfAKind,
        [{Item2: 2}, {Item2: 2}, _] => HandType.TwoPairs,
        [{Item2: 2}, _, _, _] => HandType.OnePair,
        _ => HandType.HighCard
    };
}

record Hand(string Cards, int Bid);

enum HandType
{
    FiveOfAKind = 7,
    FourOfAKind = 6,
    FullHouse = 5,
    ThreeOfAKind = 4,
    TwoPairs = 3,
    OnePair = 2,
    HighCard = 1
}