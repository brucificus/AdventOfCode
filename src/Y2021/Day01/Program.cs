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

    [Test]
    public void Part1()
    {
        var depthIncreases = Enumerable.Range(1, values.Count - 1).Select(i => values[i] > values[i - 1]).Count(v => v);
        depthIncreases.Should().Be(7);
    }

    [Test(ExpectedResult = "")]
    public string Part2()
    {
        throw new NotImplementedException();
    }
}
