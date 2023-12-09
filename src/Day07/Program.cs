var hands = File.ReadAllLines("input.txt")
    .Select(line => line.Split(" "))
    .Select(parts => new Hand(parts[0], int.Parse(parts[1])))
    .ToList();

hands.Sort(Part1);

var rank = 1;
var part1 = hands.Sum(hand => hand.Bid * rank++);
Console.WriteLine($"Part 1: {part1}");

rank = 1;
hands.Sort(Part2);
var part2 = hands.Sum(hand => hand.Bid * rank++);
Console.WriteLine($"Part 2: {part2}");

return;

int Part1(Hand a, Hand b)
{
    const string cardIndex = "23456789TJQKA";
    var scoreA = Part1Score(a);
    var scoreB = Part1Score(b);
    return scoreA != scoreB ? scoreA.CompareTo(scoreB) : SecondaryCompare(a.Cards, b.Cards, cardIndex);
}

HandType Part1Score(Hand hand)
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

int Part2(Hand a, Hand b)
{
    const string cardIndex = "J23456789TQKA";
    var scoreA = Part2Score(a);
    var scoreB = Part2Score(b);
    return scoreA != scoreB ? scoreA.CompareTo(scoreB) : SecondaryCompare(a.Cards, b.Cards, cardIndex);
}

HandType Part2Score(Hand hand)
{
    if (hand.Cards.IndexOf('J') < 0) return Part1Score(hand);

    // We're dealing with Jokers at this point 🃏
    var jokerCount = hand.Cards.Count(c => c == 'J');
    if (jokerCount >= 4) return HandType.FiveOfAKind;

    var groups = hand.Cards
        .Where(c => c != 'J')
        .GroupBy(c => c)
        .Select(g => (g.Key, g.Count()))
        .OrderByDescending(g => g.Item2)
        .ToList();

    return jokerCount switch
    {
        3 => groups switch
        {
            [{Item2: 2}] => HandType.FiveOfAKind,
            [{Item2: 1}, {Item2: 1}] => HandType.FourOfAKind,
            _ => throw new InvalidOperationException()
        },
        2 => groups switch
        {
            [{Item2: 3}] => HandType.FiveOfAKind,
            [{Item2: 2}, _] => HandType.FourOfAKind,
            [{Item2: 1}, {Item2: 1}, {Item2: 1}] => HandType.ThreeOfAKind,
            _ => throw new InvalidOperationException()
        },
        _ => groups switch
        {
            [{Item2: 4}] => HandType.FiveOfAKind,
            [{Item2: 3}, {Item2: 1}] => HandType.FourOfAKind,
            [{Item2: 2}, {Item2: 2}] => HandType.FullHouse,
            [{Item2: 2}, {Item2: 1}, {Item2: 1}] => HandType.ThreeOfAKind,
            [{Item2: 1}, {Item2: 1}, {Item2: 1}, {Item2: 1}] => HandType.OnePair,
            _ => throw new InvalidOperationException()
        }
    };
}

int SecondaryCompare(string a, string b, string cardIndex)
{
    for (var i = 0; i < 5; i++)
    {
        var indexA = cardIndex.IndexOf(a[i]);
        var indexB = cardIndex.IndexOf(b[i]);

        if (indexA != indexB)
            return indexA.CompareTo(indexB);
    }

    throw new Exception("Equal cards!?!");
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