
// Copyright (c) Charlie Poole, Rob Prouse and Contributors.

using System.Reflection;
using System.Text;
using NUnit.Engine;

namespace AdventOfCode.Y2020.Shared.Hosting.NUnit;

internal static class ExceptionHelper
{
    /// <summary>
    /// Builds up a message, using the Message field of the specified exception
    /// as well as any InnerExceptions.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>A combined message string.</returns>
    /// <remarks>Taken and modified from: https://github.com/nunit/nunit-console/blob/37a1b6139294b534fcc7dd3bc62e393b5da21622/src/NUnitEngine/nunit.engine.core/Internal/ExceptionHelper.cs </remarks>
    public static string BuildMessage(Exception exception)
    {
        var sb = new StringBuilder();
        sb.AppendFormat("{0} : ", exception.GetType());
        sb.Append(GetExceptionMessage(exception));

        foreach (var inner in FlattenExceptionHierarchy(exception))
        {
            sb.Append(Environment.NewLine);
            sb.Append("  ----> ");
            sb.AppendFormat("{0} : ", inner.GetType());
            sb.Append(GetExceptionMessage(inner));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Builds up a message, using the Message field of the specified exception
    /// as well as any InnerExceptions.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>A combined stack trace.</returns>
    public static string BuildMessageAndStackTrace(Exception exception)
    {
        var sb = new StringBuilder("--");
        sb.AppendLine(exception.GetType().Name);
        sb.AppendLine(GetExceptionMessage(exception));
        sb.AppendLine(GetSafeStackTrace(exception));

        foreach (var inner in FlattenExceptionHierarchy(exception))
        {
            sb.AppendLine("--");
            sb.AppendLine(inner.GetType().Name);
            sb.AppendLine(GetExceptionMessage(inner));
            sb.AppendLine(GetSafeStackTrace(inner));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the stack trace of the exception. If no stack trace
    /// is provided, returns "No stack trace available".
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>A string representation of the stack trace.</returns>
    private static string GetSafeStackTrace(Exception exception)
    {
        try
        {
            return exception.StackTrace;
        }
        catch (Exception)
        {
            return "No stack trace available";
        }
    }

    private static List<Exception> FlattenExceptionHierarchy(Exception exception)
    {
        var result = new List<Exception>();

        var unloadException = exception as NUnitEngineUnloadException;
        if (unloadException?.AggregatedExceptions != null)
        {
            result.AddRange(unloadException.AggregatedExceptions);

            foreach (var aggregatedException in unloadException.AggregatedExceptions)
                result.AddRange(FlattenExceptionHierarchy(aggregatedException));
        }

        if (exception is ReflectionTypeLoadException reflectionException)
        {
            result.AddRange(reflectionException.LoaderExceptions);

            foreach (var innerException in reflectionException.LoaderExceptions)
                result.AddRange(FlattenExceptionHierarchy(innerException));
        }

        if (exception.InnerException != null)
        {
            result.Add(exception.InnerException);
            result.AddRange(FlattenExceptionHierarchy(exception.InnerException));
        }

        return result;
    }

    private static string GetExceptionMessage(Exception ex)
    {
        if (string.IsNullOrEmpty(ex.Message))
        {
            // Special handling for Mono 5.0, which returns an empty message
            return ex is FileNotFoundException fnfEx
                ? "Could not load assembly. File not found: " + fnfEx.FileName
                : "No message provided";
        }

        return ex.Message;
    }
}
