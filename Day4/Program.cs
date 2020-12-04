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
var validRecords = records.Count(r => requiredFields.All(rf => r.ContainsKey(rf)));

await Out.WriteLineAsync($"Part 1 Answer: {validRecords}");
