using AdventOfCode.Y2020.Shared.Lifetime;
using NUnit.Engine;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

public class NUnitApplicationBuilder
{
    public TestRunnerOptions Options { get; } = new()
    {
        InputFiles = new string[] { System.Reflection.Assembly.GetEntryAssembly().Location }
    };

    public NUnitApplication Build()
    {
        var engineBroker = new DisposableLifetimeBroker<ITestEngine>(TestEngineActivator.CreateInstance);

        var options = (Options ?? new TestRunnerOptions());

        return new NUnitApplication(engineBroker, options);
    }
}
