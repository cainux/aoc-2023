var lines = File.ReadAllLines("input.txt").Select(x => x.Split(' ').Select(int.Parse).ToArray()).ToArray();
Console.WriteLine(lines.Sum(Part1));
int Part1(int[] input) => input.All(x => x == 0) ? 0 : input[^1] + Part1(input.Zip(input[1..]).Select(x => x.Second - x.First).ToArray());

Console.WriteLine(lines.Sum(Part2));
int Part2(int[] input) => input.All(x => x == 0) ? 0 : input[0] - Part2(input.Zip(input[1..]).Select(x => x.Second - x.First).ToArray());