using Spectre.Console;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

internal class ExtendedTextWriter
{
    private readonly Spectre.Console.IAnsiConsole _console;

    public ExtendedTextWriter(Spectre.Console.IAnsiConsole? console = default)
    {
        _console = console ?? Spectre.Console.AnsiConsole.Console;
    }

    public void WriteLine(ColorStyle style, string text)
    {
        var currentBackgroundColor = Console.BackgroundColor;
        var spectreStyle = new Style(style.AsForegroundColorOn(currentBackgroundColor), currentBackgroundColor, style.AsDecoration());
        _console.WriteLine(text, spectreStyle);
    }

    /// <summary>
    /// Writes the label and the option that goes with it.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="option">The option.</param>
    public void WriteLabel(string label, object option)
    {
        _console.Write(label);
        _console.Write(option.ToString());
    }

    /// <summary>
    /// Writes the label and the option that goes with it.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="option">The option.</param>
    /// <param name="valueStyle">The color to display the value with</param>
    public void WriteLabel(string label, object option, ColorStyle valueStyle)
    {
        WriteLabel(label, option);
    }

    /// <summary>
    /// Writes the label and the option that goes with it followed by a new line.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="option">The option.</param>
    public void WriteLabelLine(string label, object option)
    {
        WriteLabel(label, option);
        WriteLine();
    }

    /// <summary>
    /// Writes the label and the option that goes with it followed by a new line.
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="option">The option.</param>
    /// <param name="valueStyle">The color to display the value with</param>
    public void WriteLabelLine(string label, object option, ColorStyle valueStyle)
    {
        WriteLabelLine(label, option);
    }

    public void WriteLine()
    {
        _console.WriteLine();
    }

    public void Write(Style style, string text)
    {
        _console.Write(text, style);
    }
}
