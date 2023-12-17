using System.Text;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt")
    .Select(x => x.Split(" "))
    .Select(x => (x[0], x[1].Split(',').Select(int.Parse).ToArray()));

var part1 = lines.Sum(l => ValidArrangements(l.Item1, l.Item2, 0));

Console.WriteLine($"Part 1: {part1}");
return;

int ValidArrangements(string input, IReadOnlyList<int> constraint, int acc)
{
    var i = input.IndexOf('?');
    if (i <= -1)
        return Check(input, constraint) ? acc + 1 : acc;

    return ValidArrangements(new StringBuilder(input) {[i] = '#'}.ToString(), constraint, acc)
           + ValidArrangements(new StringBuilder(input) {[i] = '.'}.ToString(), constraint, acc);
}

bool Check(string input, IReadOnlyList<int> constraint)
{
    var broken = Regex.Matches(input, "\\#+");
    if (broken.Count != constraint.Count) return false;
    for (var i = 0; i < broken.Count; i++)
        if (broken[i].Length != constraint[i]) return false;

    return true;
}