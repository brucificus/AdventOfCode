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
        var slidingWindow = values.Buffer(2, 1).Where(b => b.Count == 2);
        var depthIncreases = slidingWindow.Select(b => b[1] > b[0]).Count(v => v);
        depthIncreases.Should().Be(1162);
    }

    [Test]
    public void Part2()
    {
        var rollingSumsWindow = values.Buffer(3, 1).Where(b => b.Count == 3).Select(b => b.Sum());
        var sumsComparisonWindow = rollingSumsWindow.Buffer(2, 1).Where(b => b.Count == 2);
        var depthIncreases = sumsComparisonWindow.Select(b => b[1] > b[0]).Count(v => v);
        depthIncreases.Should().Be(1190);
    }
}
