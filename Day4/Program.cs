using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Shared;
using static System.Console;

var input = await System.IO.File.ReadAllTextAsync("input.txt");
var emptyLines = new Regex("^(?:(?:\r ?\n |\r))+", RegexOptions.Multiline);

var records = (from block in input.Split(emptyLines)
               let blockPieces = block.Split(new Regex("\\s+")).Where(s => !string.IsNullOrWhiteSpace(s))
               let parsedBlockPieces =
                   from blockPiece in blockPieces
                   let blockPieceParts = blockPiece.Split(':')
                   select (fieldName: blockPieceParts[0], fieldValue: blockPieceParts[1])
                select parsedBlockPieces.ToImmutableDictionary(t => t.fieldName, t => t.fieldValue))
                .ToImmutableList();

var requiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
requiredFields = requiredFields.Except(new[] { "cid" }).ToArray();
var part1ValidRecords = records.Where(r => requiredFields.All(rf => r.ContainsKey(rf)));

await Out.WriteLineAsync($"Part 1 Answer: {part1ValidRecords.Count()}");

var height = new Regex("^(\\d+)(cm|in)$", RegexOptions.Compiled);
bool IsValidHeight(string value)
{
    var match = value.Matches(height).SingleOrDefault(m => m.Success);
    if (match is null) return false;

    return match.Groups[2].Value switch
    {
        "cm" => int.Parse(match.Groups[1].Value) is (>= 150 and <= 193),
        "in" => int.Parse(match.Groups[1].Value) is (>= 59 and <= 76),
        _ => throw new InvalidOperationException()
    };
}

var validEyeColors = new HashSet<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

var requiredFieldValidations = new Dictionary<string, Func<string, bool>>()
{
    { "byr", v => v.IsMatch(new Regex("^\\d{4}$")) && int.Parse(v) is (>= 1920 and <= 2002) },
    { "iyr", v => v.IsMatch(new Regex("^\\d{4}$")) && int.Parse(v) is (>= 2010 and <= 2020) },
    { "eyr", v => v.IsMatch(new Regex("^\\d{4}$")) && int.Parse(v) is (>= 2020 and <= 2030) },
    { "hgt", IsValidHeight },
    { "hcl", v => v.IsMatch(new Regex("^#[0-9a-f]{6}$", RegexOptions.None)) },
    { "ecl", v => validEyeColors.Contains(v) },
    { "pid", v => v.IsMatch(new Regex("^\\d{9}$")) },
}.ToImmutableDictionary();

bool passesAllRequiredFieldValidations(IReadOnlyDictionary<string, string> record)
{
    return requiredFieldValidations.Keys.All(fn => requiredFieldValidations[fn](record[fn]));
}

var part2ValidRecords = part1ValidRecords.Where(passesAllRequiredFieldValidations);

await Out.WriteLineAsync($"Part 2 Answer: {part2ValidRecords.Count()}");
