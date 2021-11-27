// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

using System.Xml;
using NUnit;
using NUnit.Engine;
using Spectre.Console;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

/// <summary>
/// TestEventHandler processes events from the running
/// test for the console runner.
/// </summary>
/// <remarks>Taken and modified from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitConsole/nunit3-console/TestEventHandler.cs </remarks>
internal class TestEventHandler : ITestEventListener
{
    private readonly ExtendedTextWriter _outWriter;

    private readonly bool _displayBeforeTest;
    private readonly bool _displayAfterTest;
    private readonly bool _displayBeforeOutput;

    private string _lastTestOutput;
    private bool _wantNewLine = false;

    public TestEventHandler(ExtendedTextWriter outWriter, string labelsOption)
    {
        _outWriter = outWriter;

        labelsOption = labelsOption.ToUpperInvariant();
        _displayBeforeTest = labelsOption is "BEFORE" or "BEFOREANDAFTER";
        _displayAfterTest = labelsOption is "AFTER" or "BEFOREANDAFTER";
        _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || labelsOption == "ONOUTPUTONLY";
    }

    public void OnTestEvent(string report)
    {
        var doc = new XmlDocument();
        doc.LoadXml(report);

        var testEvent = doc.FirstChild;
        switch (testEvent.Name)
        {
            case "start-test":
                TestStarted(testEvent);
                break;

            case "test-case":
                TestFinished(testEvent);
                break;

            case "test-suite":
                SuiteFinished(testEvent);
                break;

            case "test-output":
                TestOutput(testEvent);
                break;
        }
    }

    private void TestStarted(XmlNode testResult)
    {
        var testName = testResult.Attributes["fullname"].Value;

        if (_displayBeforeTest)
            WriteLabelLine(testName);
    }

    private void TestFinished(XmlNode testResult)
    {
        var testName = testResult.Attributes["fullname"].Value;
        var status = testResult.GetAttribute("label") ?? testResult.GetAttribute("result");
        var outputNode = testResult.SelectSingleNode("output");

        if (outputNode != null)
        {
            if (_displayBeforeOutput)
                WriteLabelLine(testName);

            FlushNewLineIfNeeded();
            WriteOutputLine(testName, outputNode.InnerText);
        }

        if (_displayAfterTest)
            WriteLabelLineAfterTest(testName, status);
    }

    private void SuiteFinished(XmlNode testResult)
    {
        var suiteName = testResult.Attributes["fullname"].Value;
        var outputNode = testResult.SelectSingleNode("output");

        if (outputNode != null)
        {
            if (_displayBeforeOutput)
                WriteLabelLine(suiteName);

            FlushNewLineIfNeeded();
            WriteOutputLine(suiteName, outputNode.InnerText);
        }
    }

    private void TestOutput(XmlNode outputNode)
    {
        var testName = outputNode.GetAttribute("testname");
        var stream = outputNode.GetAttribute("stream");

        if (_displayBeforeOutput && testName != null)
            WriteLabelLine(testName);

        WriteOutputLine(testName, outputNode.InnerText, stream == "Error" ? ColorStyle.Error : ColorStyle.Output);
    }

    private string _currentLabel;

    private void WriteLabelLine(string label)
    {
        if (label != _currentLabel)
        {
            FlushNewLineIfNeeded();
            _lastTestOutput = label;

            _outWriter.WriteLine(ColorStyle.SectionHeader, $"=> {label}");

            _currentLabel = label;
        }
    }

    private void WriteLabelLineAfterTest(string label, string? status)
    {
        FlushNewLineIfNeeded();
        _lastTestOutput = label;

        if (status != null)
        {
            _outWriter.Write(GetStyleForResultStatus(status), $"{status} ");
        }

        _outWriter.WriteLine(ColorStyle.SectionHeader, $"=> {label}");

        _currentLabel = label;
    }

    private void WriteOutputLine(string testName, string text, ColorStyle style = ColorStyle.Output)
    {
        var currentBackgroundColor = Console.BackgroundColor;
        var spectreStyle = new Style(style.AsForegroundColorOn(currentBackgroundColor), currentBackgroundColor, style.AsDecoration());
        WriteOutputLine(testName, text, spectreStyle);
    }

    private void WriteOutputLine(string testName, string text, Style color)
    {
        if (_lastTestOutput != testName)
        {
            FlushNewLineIfNeeded();
            _lastTestOutput = testName;
        }

        _outWriter.Write(color, text);

        // If the text we just wrote did not have a new line, flag that we should eventually emit one.
        if (!text.EndsWith("\n"))
        {
            _wantNewLine = true;
        }
    }

    private void FlushNewLineIfNeeded()
    {
        if (_wantNewLine)
        {
            _outWriter.WriteLine();
            _wantNewLine = false;
        }
    }

    private static Style GetStyleForResultStatus(string status)
    {
        const string passed = "Passed";
        const string failed = "Failed";
        const string error = "Error";
        const string invalid = "Invalid";
        const string cancelled = "Cancelled";
        const string warning = "Warning";
        const string ignored = "Ignored";

        var colorStyle = status switch
        {
            passed => ColorStyle.Pass,
            failed => ColorStyle.Failure,
            error => ColorStyle.Error,
            invalid => ColorStyle.Error,
            cancelled => ColorStyle.Error,
            warning => ColorStyle.Warning,
            ignored => ColorStyle.Warning,
            _ => ColorStyle.Output
        };

        var backgroundColor = Console.BackgroundColor;
        var foregroundColor = colorStyle.AsForegroundColorOn(Console.BackgroundColor);
        var decoration = colorStyle.AsDecoration();

        decoration = status switch
        {
            passed => decoration,
            failed => decoration,
            error => decoration,
            invalid => decoration | Decoration.Italic,
            cancelled => decoration | Decoration.Dim,
            warning => decoration,
            ignored => decoration | Decoration.Strikethrough,
            _ => decoration
        };

        return new Style(foregroundColor, backgroundColor, decoration);
    }
}
