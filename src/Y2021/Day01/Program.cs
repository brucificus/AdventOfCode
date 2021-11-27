await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture]
public partial class Program
{
    private IReadOnlyList<string> lines = null!;

    [SetUp]
    public async Task SetUp()
    {
        lines = await new InputFileFacade().ReadAllLinesAsync().WhereNot(string.IsNullOrEmpty);
    }

    [Test(ExpectedResult = "")]
    public string Part1()
    {
        throw new NotImplementedException();
    }

    [Test(ExpectedResult = "")]
    public string Part2()
    {
        throw new NotImplementedException();
    }
}
