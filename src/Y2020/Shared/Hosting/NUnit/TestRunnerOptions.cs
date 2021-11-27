namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

public record TestRunnerOptions(
        string? WorkDirectory = default,
        string[]? InputFiles = default,
        bool SkipNonTestAssemblies = false,
        int DefaultTimeout = 0,
        bool ShadowCopyFiles = false,
        InternalTraceLevelOption? InternalTraceLevel = null,
        ActiveConfigOption? ActiveConfig = null,
        bool StopOnError = false,
        RandomSeedOption? RandomSeed = default,
        bool PauseBeforeRun = false,
        IDictionary<string, string>? TestParameters = default,
        ConfigurationFileOption? ConfigurationFile = default,
        LabelsOutputMode? DisplayTestLabels = default
    )
{
#pragma warning disable IO0003 // Replace Directory class with IFileSystem.Directory for improved testability
    private static readonly string CurrentDirectoryOnEntry = Directory.GetCurrentDirectory();
#pragma warning restore IO0003 // Replace Directory class with IFileSystem.Directory for improved testability

    public string WorkDirectory { get; init; } = WorkDirectory ?? CurrentDirectoryOnEntry;

    public string[] InputFiles { get; init; } = InputFiles ?? System.Array.Empty<string>();

    public IDictionary<string, string> TestParameters { get; init; } = TestParameters ?? new Dictionary<string, string>();
}

public enum LabelsOutputMode
{
    Off, On, OnOutputOnly, Before, After, BeforeAndAfter
}

public readonly record struct RandomSeedOption(int Value);

public readonly record struct ActiveConfigOption(object Value);

public readonly record struct InternalTraceLevelOption(object Value);

public readonly record struct ConfigurationFileOption(object Value);
