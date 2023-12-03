using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var maxX = lines[0].Length - 1;
var maxY = lines.Length - 1;

// Part 1
var acc1 = 0;
for (var y = 0; y < lines.Length; y++)
    acc1 += Regex.Matches(lines[y], @"\d+").Sum(m => PartNumber(m, y));
Console.WriteLine(acc1);

// Part 2
var acc2 = 0;
for (var y = 0; y < lines.Length; y++)
    acc2 += Regex.Matches(lines[y], @"\*").Sum(m => Ratio(m.Index, y));
Console.WriteLine(acc2);

return;

int PartNumber(Match m, int y)
{
    const char period = '.';
    if (m.Index > 0 && lines[y][m.Index - 1] != period)
        return int.Parse(m.Value);
    if (m.Index + m.Length < maxX && lines[y][m.Index + m.Length] != period)
        return int.Parse(m.Value);

    foreach (var adjacentLine in GetAdjacentLines(y))
        for (var i = Math.Max(0, m.Index - 1); i <= Math.Min(maxX, m.Index + m.Length); i++)
            if (adjacentLine[i] != period)
                return int.Parse(m.Value);

    return 0;
}

int Ratio(int x, int y)
{
    var adjacentNumbers = new List<int>();
    var numbersOnLine = Regex.Matches(lines[y], @"\d+");

    foreach (Match number in numbersOnLine)
        if (number.Index == x + 1 || number.Index + number.Length == x)
            adjacentNumbers.Add(int.Parse(number.Value));

    foreach (var adjacentLine in GetAdjacentLines(y))
        foreach (Match n in Regex.Matches(adjacentLine, @"\d+"))
        {
            if (x >= n.Index - 1 && x <= n.Index + n.Length)
                adjacentNumbers.Add(int.Parse(n.Value));

            if (adjacentNumbers.Count > 2)
                return 0;
        }

    return adjacentNumbers.Count == 2 ? adjacentNumbers[0] * adjacentNumbers[1] : 0;
}

IEnumerable<string> GetAdjacentLines(int y)
{
    if (y > 0) yield return lines[y - 1];
    if (y < maxY) yield return lines[y + 1];
}