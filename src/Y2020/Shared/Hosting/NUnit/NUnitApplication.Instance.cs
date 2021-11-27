using AdventOfCode.Y2020.Shared.Lifetime;
using NUnit.Engine;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

public partial class NUnitApplication
{
    private readonly ILifetimeBroker<ITestEngine> _testEngineBroker;
    private readonly TestRunnerOptions _options;

    internal NUnitApplication(ILifetimeBroker<ITestEngine> testEngineBroker, TestRunnerOptions options)
    {
        _testEngineBroker = testEngineBroker;
        _options = options;
    }

    /// <summary>
    /// Runs the tests and returns a Task that completes when either all the tests have finished or the token is triggered.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the entire runtime of the NUnit tests from startup to shutdown.
    /// </returns>
    public async Task<ConsoleTestRunnerResult> RunAsync(CancellationToken cancellationToken = default)
    {
        return await StartNewWorkTask(cancellationToken);
    }

    private Task<ConsoleTestRunnerResult> StartNewWorkTask(CancellationToken cancellationToken = default)
    {
        var consoleRunnerBroker = _testEngineBroker.Project(testEngine => new ConsoleTestRunner(testEngine, _options));

        return Task.Factory.StartNew<Task<ConsoleTestRunnerResult>>(
                async () => await consoleRunnerBroker.Use(consoleRunner => consoleRunner.Execute(cancellationToken)),
                TaskCreationOptions.LongRunning)
            .Unwrap();
    }
}
