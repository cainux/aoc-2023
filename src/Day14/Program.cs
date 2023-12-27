using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var result = Tilt(lines);
Console.WriteLine($"Score: {Score(result)}");

return;

int Score(string[] input) => input.Select((t, y) => Regex.Matches(t, "O").Count * (input.Length - y)).Sum();

string[] Tilt(string[] input)
{
    for (var y = 0; y < input.Length; y++)
    for (var x = 0; x < input[0].Length; x++)
        if (input[y][x] == 'O')
            RollUp(x, y, input);

    return input;
}

void RollUp(int x, int y, string[] input)
{
    var done = false;

    while (!done)
    {
        if (y == 0)
            return;

        var current = input[y][x];
        var next = input[y - 1][x];

        if (current == 'O' && next == '.')
        {
            input[y] = input[y].Remove(x, 1).Insert(x, ".");
            input[y - 1] = input[y - 1].Remove(x, 1).Insert(x, "O");
            y--;
        }
        else
        {
            done = true;
        }
    }
}