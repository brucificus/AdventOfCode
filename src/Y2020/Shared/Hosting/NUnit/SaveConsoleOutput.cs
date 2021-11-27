
// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

/// <summary>
/// Saves Console.Out and Console.Error and restores them when the object
/// is destroyed
/// </summary>
/// <remarks>Taken from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitConsole/nunit3-console/Utilities/SaveConsoleOutput.cs </remarks>
internal sealed class SaveConsoleOutput : IDisposable
{
    private readonly TextWriter _savedOut = Console.Out;
    private readonly TextWriter _savedError = Console.Error;

    /// <summary>
    /// Restores Console.Out and Console.Error
    /// </summary>
    public void Dispose()
    {
        Console.SetOut(_savedOut);
        Console.SetError(_savedError);
    }
}
