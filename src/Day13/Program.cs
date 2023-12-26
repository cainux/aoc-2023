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
return;

int GetValue(string[] terrain)
{
    var result = FindReflectionPoint(terrain);
    if (result == -1)
    {
        result = FindReflectionPoint(SliceByColumns(terrain));
        if (result != -1) return result;
    }
    else
    {
        return result * 100;
    }

    return -1;
}

int FindReflectionPoint(string[] terrain)
{
    var half = terrain.Length / 2;
    for (var i = 0; i < terrain.Length - 1; i++)
    {
        var take = i >= half ? terrain.Length - 1 - i : i + 1;
        var topSlice = terrain[(i + 1 - take)..(i + 1)].Reverse().ToArray();
        var bottomSlice = terrain[(i + 1)..(i + 1 + take)];
        if (topSlice.SequenceEqual(bottomSlice))
            return i + 1;
    }
    return -1;
}

string[] SliceByColumns(string[] terrain)
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