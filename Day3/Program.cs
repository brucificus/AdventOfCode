using System;
using System.Linq;
using Shared;
using Shared.Mapping;
using static System.Console;
using TobogganPosition = Shared.Mapping.ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette;

var map = await TextualSemisesquiBoundedInfiniteIntegralPlane.FromFileAsync("input.txt");
const char tree = '#';

var tobogganPosition = map.Origin;
var part1Slope = new Slope(3, 1);

Func<int, TobogganPosition, int> CountTrees = (p, c) => c.Peek() == tree ? p + 1 : p;

var part1TreeCount = tobogganPosition.RepeatedlySlideToBottomOfMap<int>(part1Slope, 0, CountTrees);

await Out.WriteLineAsync($"Part 1 Answer: {part1TreeCount}");

var part2Slopes = new Slope[]
{
    new Slope(1, 1),
    part1Slope,
    new Slope(5, 1),
    new Slope(7, 1),
    new Slope(1, 2)
};

var part2TreeCounts = part2Slopes.Select(s => tobogganPosition.RepeatedlySlideToBottomOfMap<int>(s, 0, CountTrees));

var part2Answer = part2TreeCounts.Multiply();

await Out.WriteLineAsync($"Part 2 Answer: {part2Answer}");
