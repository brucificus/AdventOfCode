using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using static System.IO.File;

[TestFixture]
public class Day2
{
    private IEnumerable<(int[] policyNumbers, char policyCharacter, string password)> inputEntries;

    [SetUp]
    public async Task SetUp()
    {
        inputEntries = (await ReadAllLinesAsync("input.txt"))
                .Where(v => v is string { Length: > 0 })
                .Select(v => v.Split(' '))
                .Select(sv => (unparsedPolicyNumbers: sv[0].Split('-'), policyCharacter: sv[1][0], password: sv[2]))
                .Select(e => (policyNumbers: e.unparsedPolicyNumbers.Select(int.Parse).ToArray(), e.policyCharacter, e.password));
    }

    [Test(ExpectedResult = 620)]
    public int Part1()
    {
        var part1Interpretation = inputEntries
                    .Select(e => (policyRange: e.policyNumbers, e.policyCharacter, e.password, passwordPolicyCharacterOccurences: e.password.Count(c => c == e.policyCharacter)));

        return part1Interpretation
                    .Where(e => e.passwordPolicyCharacterOccurences >= e.policyRange[0] && e.passwordPolicyCharacterOccurences <= e.policyRange[1])
                    .Count();
    }

    [Test(ExpectedResult = 727)]
    public int Part2()
    {
        var part2Interpretation = inputEntries
                .Select(e => (policyOrdinals: e.policyNumbers, e.policyCharacter, e.password))
                .Select(e => (e.policyOrdinals, e.policyCharacter, e.password, policyCompliance: e.policyOrdinals.Select(o => e.password[o - 1] == e.policyCharacter).ToArray()));

        return part2Interpretation
                    .Where(e => e.policyCompliance.Count(c => c) == 1)
                    .Count();
    }
}
