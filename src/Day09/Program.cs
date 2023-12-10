var lines = File.ReadAllLines("input.txt").Select(x => x.Split(' ').Select(int.Parse).ToArray()).ToArray();
Console.WriteLine(lines.Sum(Predict));
int Predict(int[] input) => input.All(x => x == 0) ? 0 : input[^1] + Predict(input.Zip(input[1..]).Select(x => x.Second - x.First).ToArray());