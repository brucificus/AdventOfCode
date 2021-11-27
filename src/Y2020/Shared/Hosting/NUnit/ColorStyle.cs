// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

/// <summary>
/// ColorStyle enumerates the various styles used in the console display
/// </summary>
/// <remarks>Taken from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitConsole/nunit3-console/ColorStyle.cs </remarks>
internal enum ColorStyle
{
    /// <summary>
    /// Color for headers
    /// </summary>
    Header,
    /// <summary>
    /// Color for sub-headers
    /// </summary>
    SubHeader,
    /// <summary>
    /// Color for each of the section headers
    /// </summary>
    SectionHeader,
    /// <summary>
    /// The default color for items that don't fit into the other categories
    /// </summary>
    Default,
    /// <summary>
    /// Test output
    /// </summary>
    Output,
    /// <summary>
    /// Color for help text
    /// </summary>
    Help,
    /// <summary>
    /// Color for labels
    /// </summary>
    Label,
    /// <summary>
    /// Color for values, usually go beside labels
    /// </summary>
    Value,
    /// <summary>
    /// Color for passed tests
    /// </summary>
    Pass,
    /// <summary>
    /// Color for failed tests
    /// </summary>
    Failure,
    /// <summary>
    /// Color for warnings, ignored or skipped tests
    /// </summary>
    Warning,
    /// <summary>
    /// Color for errors and exceptions
    /// </summary>
    Error
}
