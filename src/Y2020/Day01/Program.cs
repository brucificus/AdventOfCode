await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture]
public partial class Program
{
    private IReadOnlyList<int> values = null!;

    [SetUp]
    public async Task SetUp()
    {
        values = await new InputFileFacade().ReadAllLinesAsync().WhereNot(string.IsNullOrEmpty).Select(int.Parse);
    }

    [Test(ExpectedResult = 786811)]
    public int Part1()
    {
        var pairs = values.SelectUniquePairs();

        return pairs.Where(p => p.Item1 + p.Item2 == 2020).Select(p => p.Item1 * p.Item2).First();
    }

    [Test(ExpectedResult = 199068980)]
    public int Part2()
    {
        var triplets = values.SelectUniqueTriplets();

        return triplets.Where(p => p.Item1 + p.Item2 + p.Item3 == 2020).Select(p => p.Item1 * p.Item2 * p.Item3).First();
    }
}
