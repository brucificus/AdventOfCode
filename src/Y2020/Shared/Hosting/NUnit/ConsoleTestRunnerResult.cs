using NUnit.Engine;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

public sealed class ConsoleTestRunnerResult : OneOfBase<AllSuccess, SomeFailures, InvalidArg, InvalidAssembly, InvalidTestFixture, UnexpectedError>, IHasExitCode
{
    public ConsoleTestRunnerResult(OneOf<AllSuccess, SomeFailures, InvalidArg, InvalidAssembly, InvalidTestFixture, UnexpectedError> input) : base(input)
    {
    }

    public int ExitCode => base.Match(
        x => x.ExitCode,
        x => x.ExitCode,
        x => x.ExitCode,
        x => x.ExitCode,
        x => x.ExitCode,
        x => x.ExitCode);

    public static implicit operator int(ConsoleTestRunnerResult self) => self.ExitCode;
}

public interface IHasExitCode
{
    int ExitCode { get; }
}

public interface IHasResultSummary
{
    ResultSummary Summary { get; }
    int CountFailures() => Summary.FailureCount + Summary.ErrorCount + Summary.InvalidCount;
}

public readonly record struct AllSuccess(ResultSummary Summary) : IHasResultSummary, IHasExitCode { public int ExitCode => 0; }

public readonly record struct SomeFailures(ResultSummary Summary) : IHasResultSummary, IHasExitCode
{
    // Some operating systems truncate the return code to 8 bits, which
    // only allows us a maximum of 127 in the positive range. We limit
    // ourselves so as to stay in that range.
    private const int MAXIMUM_RETURN_CODE_ALLOWED = 100; // In case we are running on Unix

    public int ExitCode
    {
        get
        {
            var failureCount = (this as IHasResultSummary).CountFailures();
            return Math.Min(failureCount, MAXIMUM_RETURN_CODE_ALLOWED);
        }
    }

}

public readonly record struct InvalidArg() : IHasExitCode { public int ExitCode => -1; }

public readonly record struct InvalidAssembly() : IHasExitCode { public int ExitCode => -2; }

public readonly record struct InvalidTestFixture() : IHasExitCode { public int ExitCode => -4; }

public readonly record struct UnexpectedError(NUnitEngineException? EngineException = default) : IHasExitCode { public int ExitCode => -100; }
