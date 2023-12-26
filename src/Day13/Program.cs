using System.Diagnostics;
using System.Text;

var lines = File.ReadAllLines("input.txt");
var inputs = new List<string[]>();
var cursor = 0;

for (var i = 0; i < lines.Length; i++)
{
    if (string.IsNullOrEmpty(lines[i]) || i == lines.Length - 1)
    {
        if (i < lines.Length - 1)
        {
            inputs.Add(lines[cursor..i]);
            cursor = i + 1;
        }
        else
        {
            inputs.Add(lines[cursor..(i + 1)]);
        }
    }
}

Console.WriteLine($"Part 1: {inputs.Sum(GetValue)}");
Console.WriteLine($"Part 2: {inputs.Sum(GetSmudgedValue)}");
return;

int GetSmudgedValue(string[] terrain)
{
    var skip = FindReflectionPoint(terrain);
    var result = FindReflectionPoint(terrain, skip, 1);

    if (result != -1)
        return result * 100;

    var rotated = Rotate(terrain);
    skip = FindReflectionPoint(rotated);
    result = FindReflectionPoint(rotated, skip, 1);

    if (result != -1)
        return result;

    throw new UnreachableException();
}

int GetValue(string[] terrain)
{
    var result = FindReflectionPoint(terrain);
    if (result == -1)
    {
        result = FindReflectionPoint(Rotate(terrain));
        if (result != -1) return result;
    }
    else
    {
        return result * 100;
    }

    return -1;
}

int FindReflectionPoint(string[] terrain, int skip = -1, int allowance = 0)
{
    var half = terrain.Length / 2;
    for (var i = 0; i < terrain.Length - 1; i++)
    {
        var take = i >= half ? terrain.Length - 1 - i : i + 1;
        var slice1 = terrain[(i + 1 - take)..(i + 1)].Reverse().ToArray();
        var slice2 = terrain[(i + 1)..(i + 1 + take)];
        var diffCount = 0;

        for (var y = 0; y < slice1.Length; y++)
            for (var x = 0; x < slice1[0].Length; x++)
                diffCount += slice1[y][x] == slice2[y][x] ? 0 : 1;

        if (diffCount == allowance && i + 1 != skip)
            return i + 1;
    }
    return -1;
}

string[] Rotate(string[] terrain)
{
    var result = new string[terrain[0].Length];
    for (var i = 0; i < terrain[0].Length; i++)
    {
        var sb = new StringBuilder();
        foreach (var t in terrain)
            sb.Append(t[i]);
        result[i] = sb.ToString();
    }
    return result;
}