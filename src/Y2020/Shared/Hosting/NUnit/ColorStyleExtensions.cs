// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

using Spectre.Console;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

/// <remarks>Taken and modified from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitConsole/nunit3-console/ColorConsole.cs </remarks>
internal static class ColorStyleExtensions
{
    public static Color AsForegroundColorOn(this ColorStyle style, ConsoleColor backgroundColor)
    {
        switch (backgroundColor)
        {
            case ConsoleColor.White:
                return style switch
                {
                    ColorStyle.Header => ConsoleColor.Black,
                    ColorStyle.SubHeader => ConsoleColor.Black,
                    ColorStyle.SectionHeader => ConsoleColor.Blue,
                    ColorStyle.Label => ConsoleColor.Black,
                    ColorStyle.Value => ConsoleColor.Blue,
                    ColorStyle.Pass => ConsoleColor.Green,
                    ColorStyle.Failure => ConsoleColor.Red,
                    ColorStyle.Warning => ConsoleColor.Black,
                    ColorStyle.Error => ConsoleColor.Red,
                    ColorStyle.Output => ConsoleColor.Black,
                    ColorStyle.Help => ConsoleColor.Black,
                    ColorStyle.Default => ConsoleColor.Black,
                    _ => ConsoleColor.Black
                };

            case ConsoleColor.Cyan:
            case ConsoleColor.Green:
            case ConsoleColor.Red:
            case ConsoleColor.Magenta:
            case ConsoleColor.Yellow:
                return style switch
                {
                    ColorStyle.Header => ConsoleColor.Black,
                    ColorStyle.SubHeader => ConsoleColor.Black,
                    ColorStyle.SectionHeader => ConsoleColor.Blue,
                    ColorStyle.Label => ConsoleColor.Black,
                    ColorStyle.Value => ConsoleColor.Black,
                    ColorStyle.Pass => ConsoleColor.Black,
                    ColorStyle.Failure => ConsoleColor.Red,
                    ColorStyle.Warning => ConsoleColor.Yellow,
                    ColorStyle.Error => ConsoleColor.Red,
                    ColorStyle.Output => ConsoleColor.Black,
                    ColorStyle.Help => ConsoleColor.Black,
                    ColorStyle.Default => ConsoleColor.Black,
                    _ => ConsoleColor.Black
                };

            case ConsoleColor.Black:
            case ConsoleColor.DarkBlue:
            case ConsoleColor.DarkGreen:
            case ConsoleColor.DarkCyan:
            case ConsoleColor.DarkRed:
            case ConsoleColor.DarkMagenta:
            case ConsoleColor.DarkYellow:
            case ConsoleColor.Gray:
            case ConsoleColor.DarkGray:
            case ConsoleColor.Blue:
            default:
                return style switch
                {
                    ColorStyle.Header => ConsoleColor.White,
                    ColorStyle.SubHeader => ConsoleColor.Gray,
                    ColorStyle.SectionHeader => ConsoleColor.Cyan,
                    ColorStyle.Label => ConsoleColor.Green,
                    ColorStyle.Value => ConsoleColor.White,
                    ColorStyle.Pass => ConsoleColor.Green,
                    ColorStyle.Failure => ConsoleColor.Red,
                    ColorStyle.Warning => ConsoleColor.Yellow,
                    ColorStyle.Error => ConsoleColor.Red,
                    ColorStyle.Output => ConsoleColor.Gray,
                    ColorStyle.Help => ConsoleColor.Green,
                    ColorStyle.Default => ConsoleColor.Green,
                    _ => ConsoleColor.Green
                };
        }
    }

    public static Decoration AsDecoration(this ColorStyle style)
    {
        return style switch
        {
            ColorStyle.Header => Decoration.Underline | Decoration.Bold,
            ColorStyle.SubHeader => Decoration.Bold,
            ColorStyle.SectionHeader => Decoration.Italic,
            ColorStyle.Default => Decoration.None,
            ColorStyle.Output => Decoration.Dim,
            ColorStyle.Help => Decoration.None,
            ColorStyle.Label => Decoration.None,
            ColorStyle.Value => Decoration.None,
            ColorStyle.Pass => Decoration.None,
            ColorStyle.Failure => Decoration.None,
            ColorStyle.Warning => Decoration.None,
            ColorStyle.Error => Decoration.None,
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };
    }
}
