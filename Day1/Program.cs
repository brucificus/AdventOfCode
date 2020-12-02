using System.Linq;
using Shared;
using static System.Console;
using static System.IO.File;

var values = (await ReadAllLinesAsync("input.txt"))
            .Where(v => v is string { Length: > 0 })
            .Select(v => int.Parse(v));

var pairs = values.SelectUniquePairs();

var part1Answer = pairs.Where(p => p.Item1 + p.Item2 == 2020).Select(p => p.Item1 * p.Item2).First();

await Out.WriteLineAsync($"Part 1 Answer: {part1Answer}");

var triplets = values.SelectUniqueTriplets();

var part2Answer = triplets.Where(p => p.Item1 + p.Item2 + p.Item3 == 2020).Select(p => p.Item1 * p.Item2 * p.Item3).First();

await Out.WriteLineAsync($"Part 2 Answer: {part2Answer}");
