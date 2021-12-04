public static class TabularGridFlatteningExtensions
{
    public static IEnumerable<TResultItem> Disassemble<TCellValue, TResultItem>(this IReadOnlyList<IReadOnlyList<TCellValue>> table, Func<(int major, int minor, TCellValue value), TResultItem> projection, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Count;
        var maxMinorBreadth = table.Max(minor => minor.Count);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }

    public static IEnumerable<TResultItem> Disassemble<TCellValue, TResultItem>(this TCellValue[][] table, Func<(int major, int minor, TCellValue value), TResultItem> projection, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Length;
        var maxMinorBreadth = table.Max(minor => minor.Length);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }

    public static IEnumerable<TResultItem> Disassemble<TCellValue, TResultItem>(this ImmutableArray<ImmutableArray<TCellValue>> table, Func<(int major, int minor, TCellValue value), TResultItem> projection, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Length;
        var maxMinorBreadth = table.Max(minor => minor.Length);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return projection((major: major, minor: minor, value: table[major][minor]));
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }

    public static IEnumerable<(int major, int minor, TCellValue value)> Disassemble<TCellValue>(this IReadOnlyList<IReadOnlyList<TCellValue>> table, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Count;
        var maxMinorBreadth = table.Max(minor => minor.Count);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }

    public static IEnumerable<(int major, int minor, TCellValue value)> Disassemble<TCellValue>(this TCellValue[][] table, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Length;
        var maxMinorBreadth = table.Max(minor => minor.Length);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }

    public static IEnumerable<(int major, int minor, TCellValue value)> Disassemble<TCellValue>(this ImmutableArray<ImmutableArray<TCellValue>> table, TableDisassemblyOrdering disassemblyOrdering = TableDisassemblyOrdering.OutsideIn)
    {
        var majorBreadth = table.Length;
        var maxMinorBreadth = table.Max(minor => minor.Length);

        if (disassemblyOrdering == TableDisassemblyOrdering.OutsideIn)
        {
            foreach(var major in Enumerable.Range(0, majorBreadth))
            {
                foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else if (disassemblyOrdering == TableDisassemblyOrdering.InsideOut)
        {
            foreach (var minor in Enumerable.Range(0, maxMinorBreadth))
            {
                foreach(var major in Enumerable.Range(0, majorBreadth))
                {
                    yield return (major: major, minor: minor, value: table[major][minor]);
                }
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(disassemblyOrdering));
        }
    }
}
