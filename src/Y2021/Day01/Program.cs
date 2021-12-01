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
        var slidingWindow = values.BufferWithoutPartials(2, 1);
        var depthIncreases = slidingWindow.Select(b => b[1] > b[0]).Count(v => v);

        depthIncreases.Should().Be(1162);
    }

    [Test]
    public void Part2()
    {
        var rollingSumsWindow = values.BufferWithoutPartials(3, 1).Select(b => b.Sum<int, int>());
        var sumsComparisonWindow = rollingSumsWindow.BufferWithoutPartials(2, 1);
        var depthIncreases = sumsComparisonWindow.Select(b => b[1] > b[0]).Count(v => v);

        depthIncreases.Should().Be(1190);
    }
}
