using System.Collections;

public static class TranspositionCollectionExtensions
{
    public static IReadOnlyList<IReadOnlyList<T>> Transpose<T>(this IReadOnlyList<IReadOnlyList<T>> input)
    {
        var inputWidth = input.Max(i => i.Count);
        var inputHeight = input.Count;

        var resultWidth = inputHeight;
        var resultHeight = inputWidth;

        var resultRows = new List<IReadOnlyList<T>>();
        foreach (var resultRow in Enumerable.Range(0, resultHeight))
        {
            var row = new List<T>();
            foreach (var resultColumn in Enumerable.Range(0, resultWidth))
            {
                var inputRow = resultColumn;
                var inputColumn = resultRow;

                row.Add(input[inputRow][inputColumn]);
            }
            resultRows.Add(row.AsReadOnly());
        }

        return resultRows;
    }

    public static IReadOnlyList<BitArray> Transpose(this IReadOnlyList<BitArray> input)
    {
        var inputWidth = input.Max(i => i.Count);
        var inputHeight = input.Count;

        var resultWidth = inputHeight;
        var resultHeight = inputWidth;

        var resultRows = new List<BitArray>();
        foreach (var resultRow in Enumerable.Range(0, resultHeight))
        {
            var row = new BitArray(resultWidth);
            foreach (var resultColumn in Enumerable.Range(0, resultWidth))
            {
                var inputRow = resultColumn;
                var inputColumn = resultRow;

                row[resultColumn] = input[inputRow][inputColumn];
            }
            resultRows.Add(row);
        }

        return resultRows;
    }
}
