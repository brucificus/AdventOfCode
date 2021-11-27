// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using NUnit;
using NUnit.Engine;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

/// <summary>
/// Provides the nunit3-console text-based user interface, running the tests and reporting the results.
/// </summary>
/// <remarks>Taken and modified from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitConsole/nunit3-console/Program.cs </remarks>
internal class ConsoleTestRunner
{
    private readonly ITestEngine _engine;
    private readonly ExtendedTextWriter _outWriter;
    private readonly IFileSystem _fileSystem;
    private readonly TestRunnerOptions _options;

    public ConsoleTestRunner(ITestEngine engine, TestRunnerOptions options)
        : this(engine, options, new ExtendedTextWriter(), new FileSystem())
    {
    }

    public ConsoleTestRunner(ITestEngine engine, TestRunnerOptions options, ExtendedTextWriter writer, IFileSystem fileSystem)
    {
        _engine = engine;
        _options = options;
        _outWriter = writer;
        _fileSystem = fileSystem;

        var workDirectory = options.WorkDirectory ?? _fileSystem.Directory.GetCurrentDirectory();

        if (!_fileSystem.Directory.Exists(workDirectory))
            _fileSystem.Directory.CreateDirectory(workDirectory);

        _engine.Services.GetService<IResultService>();
        _engine.Services.GetService<IExtensionService>();
    }

    /// <summary>
    /// Executes tests according to the provided commandline options.
    /// </summary>
    /// <returns></returns>
    public ConsoleTestRunnerResult Execute(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DisplayRuntimeEnvironment();

        cancellationToken.ThrowIfCancellationRequested();
        DisplayTestFiles();

        cancellationToken.ThrowIfCancellationRequested();
        var package = MakeTestPackage(_options);

        cancellationToken.ThrowIfCancellationRequested();
        return RunTests(package);
    }

    private void DisplayTestFiles()
    {
        _outWriter.WriteLine(ColorStyle.SectionHeader, "Test Files");
        foreach (var file in _options.InputFiles)
            _outWriter.WriteLine(ColorStyle.Default, "    " + file);
        _outWriter.WriteLine();
    }

    private ConsoleTestRunnerResult RunTests(TestPackage package)
    {
        var labels = (_options.DisplayTestLabels ?? LabelsOutputMode.On).ToString().ToUpperInvariant();

        XmlNode? result = null;
        NUnitEngineUnloadException? unloadException = null;
        NUnitEngineException? engineException = null;

        try
        {
            using (new SaveConsoleOutput())
            using (var runner = _engine.GetRunner(package))
            {
                var eventHandler = new TestEventHandler(_outWriter, labels);
                var testFilter = new TestFilterBuilder().GetFilter();

                result = runner.Run(eventHandler, testFilter);
            }
        }
        catch (NUnitEngineUnloadException ex)
        {
            unloadException = ex;
        }
        catch (NUnitEngineException ex)
        {
            engineException = ex;
        }

        if (result != null)
        {
            var reporter = new ResultReporter(result, _outWriter, _options);
            reporter.ReportResults();

            if (engineException != null)
            {
                _outWriter.WriteLine(ColorStyle.Error, Environment.NewLine + ExceptionHelper.BuildMessage(engineException));
                return new ConsoleTestRunnerResult(new UnexpectedError(engineException));
            }

            if (unloadException != null)
            {
                _outWriter.WriteLine(ColorStyle.Warning, Environment.NewLine + ExceptionHelper.BuildMessage(unloadException));
            }

            if (reporter.Summary.UnexpectedError)
                return new ConsoleTestRunnerResult(default(UnexpectedError));

            if (reporter.Summary.InvalidAssemblies > 0)
                return new ConsoleTestRunnerResult(default(InvalidAssembly));

            if (reporter.Summary.InvalidTestFixtures > 0)
                return new ConsoleTestRunnerResult(default(InvalidTestFixture));

            var failureCount = reporter.Summary.FailureCount + reporter.Summary.ErrorCount + reporter.Summary.InvalidCount;

            if (failureCount == 0) return new ConsoleTestRunnerResult(new SomeFailures(reporter.Summary));

            return new ConsoleTestRunnerResult(new AllSuccess(reporter.Summary));
        }

        // If we got here, it's because we had an exception, but check anyway
        if (engineException != null)
        {
            _outWriter.WriteLine(ColorStyle.Error, ExceptionHelper.BuildMessage(engineException));
            _outWriter.WriteLine();
            _outWriter.WriteLine(ColorStyle.Error, ExceptionHelper.BuildMessageAndStackTrace(engineException));
        }

        return new ConsoleTestRunnerResult(new UnexpectedError(engineException));
    }

    private void DisplayRuntimeEnvironment()
    {
        _outWriter.WriteLine(ColorStyle.SectionHeader, "Runtime Environment");
        _outWriter.WriteLabelLine("   OS Version: ", RuntimeInformation.OSDescription);
        _outWriter.WriteLine();
    }

    private TestPackage MakeTestPackage(TestRunnerOptions options)
    {
        var package = new TestPackage(options.InputFiles);

        package.AddSetting(EnginePackageSettings.DisposeRunners, true);

        if (options.ShadowCopyFiles)
            package.AddSetting(EnginePackageSettings.ShadowCopyFiles, true);

        if (options.SkipNonTestAssemblies)
            package.AddSetting(EnginePackageSettings.SkipNonTestAssemblies, true);

        if (options.DefaultTimeout >= 0)
            package.AddSetting(FrameworkPackageSettings.DefaultTimeout, options.DefaultTimeout);

        if (options.InternalTraceLevel.HasValue)
            package.AddSetting(FrameworkPackageSettings.InternalTraceLevel, options.InternalTraceLevel.Value);

        if (options.ActiveConfig.HasValue)
            package.AddSetting(EnginePackageSettings.ActiveConfig, options.ActiveConfig.Value);

        // Always add work directory, in case current directory is changed
        var workDirectory = options.WorkDirectory ?? _fileSystem.Directory.GetCurrentDirectory();
        package.AddSetting(FrameworkPackageSettings.WorkDirectory, workDirectory);

        if (options.StopOnError)
            package.AddSetting(FrameworkPackageSettings.StopOnError, true);

        if (options.RandomSeed.HasValue)
            package.AddSetting(FrameworkPackageSettings.RandomSeed, options.RandomSeed.Value);

        if (System.Diagnostics.Debugger.IsAttached)
        {
            package.AddSetting(FrameworkPackageSettings.DebugTests, true);

            package.AddSetting(FrameworkPackageSettings.NumberOfTestWorkers, 0);
        }

        if (options.PauseBeforeRun)
            package.AddSetting(FrameworkPackageSettings.PauseBeforeRun, true);

        if (options.TestParameters.Count != 0)
            AddTestParametersSetting(package, options.TestParameters);

        if (options.ConfigurationFile != null)
            package.AddSetting(EnginePackageSettings.ConfigurationFile, options.ConfigurationFile.Value);

        return package;
    }

    /// <summary>
    /// Sets test parameters, handling backwards compatibility.
    /// </summary>
    private static void AddTestParametersSetting(TestPackage testPackage, IDictionary<string, string> testParameters)
    {
        testPackage.AddSetting(FrameworkPackageSettings.TestParametersDictionary, testParameters);

        if (testParameters.Count != 0)
        {
            // This cannot be changed without breaking backwards compatibility with old frameworks.
            // Reserializes the way old frameworks understand, even if this runner's parsing is changed.

            var oldFrameworkSerializedParameters = new StringBuilder();
            foreach (var (key, value) in testParameters)
                oldFrameworkSerializedParameters.Append(key).Append('=').Append(value).Append(';');

            testPackage.AddSetting(FrameworkPackageSettings.TestParameters, oldFrameworkSerializedParameters.ToString(0, oldFrameworkSerializedParameters.Length - 1));
        }
    }
}

