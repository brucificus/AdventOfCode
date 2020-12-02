using System.Linq;
using static System.Console;
using static System.IO.File;

var inputEntries = (await ReadAllLinesAsync("input.txt"))
            .Where(v => v is string { Length: > 0 })
            .Select(v => v.Split(' '))
            .Select(sv => (unparsedPolicyNumbers: sv[0].Split('-'), policyCharacter: sv[1][0], password: sv[2]))
            .Select(e => (policyNumbers: e.unparsedPolicyNumbers.Select(int.Parse).ToArray(), e.policyCharacter, e.password));

var part1Interpretation = inputEntries
            .Select(e => (policyRange: e.policyNumbers, e.policyCharacter, e.password, passwordPolicyCharacterOccurences: e.password.Count(c => c == e.policyCharacter)));

var part1EntriesWithValidPasswords = part1Interpretation
            .Where(e => e.passwordPolicyCharacterOccurences >= e.policyRange[0] && e.passwordPolicyCharacterOccurences <= e.policyRange[1])
            .Count();

await Out.WriteLineAsync($"Part 1 Answer: {part1EntriesWithValidPasswords}");

var part2Interpretation = inputEntries
            .Select(e => (policyOrdinals: e.policyNumbers, e.policyCharacter, e.password))
            .Select(e => (e.policyOrdinals, e.policyCharacter, e.password, policyCompliance: e.policyOrdinals.Select(o => e.password[o - 1] == e.policyCharacter).ToArray()));

var part2EntriesWithValidPasswords = part2Interpretation
            .Where(e => e.policyCompliance.Count(c => c) == 1)
            .Count();

await Out.WriteLineAsync($"Part 2 Answer: {part2EntriesWithValidPasswords}");
