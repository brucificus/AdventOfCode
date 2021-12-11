internal static class SlopeTrickFunctionHelpers
{
    public static IEnumerable<(TNumber X, nuint SlopeChange)> MergeObstensiblySameSortedSlopeChanges<TNumber>(IEnumerable<(TNumber X, nuint SlopeChange)> left, IEnumerable<(TNumber X, nuint SlopeChange)> right)
        where TNumber : IComparisonOperators<TNumber, TNumber>
    {
        var publishedLeft = left.Publish();
        var publishedRight = right.Publish();

        var nextLeft = publishedLeft.FirstOrDefault();
        var nextRight = publishedRight.FirstOrDefault();

        while (nextLeft != default || nextRight != default)
        {
            if (nextLeft != default && nextRight != default)
            {
                if (nextLeft.X < nextRight.X)
                {
                    yield return nextLeft;
                    nextLeft = publishedLeft.FirstOrDefault();
                    continue;
                }

                if (nextLeft.X > nextRight.X)
                {
                    yield return nextRight;
                    nextRight = publishedRight.FirstOrDefault();
                    continue;
                }

                // nextLeft.X == nextRight.X
                yield return (nextLeft.X, nextLeft.SlopeChange + nextRight.SlopeChange);
                nextLeft = publishedLeft.FirstOrDefault();
                nextRight = publishedRight.FirstOrDefault();
            }

            if (nextLeft == default)
            {
                yield return nextRight;
                nextRight = publishedRight.FirstOrDefault();
            }

            if (nextRight == default)
            {
                yield return nextLeft;
                nextLeft = publishedLeft.FirstOrDefault();
            }
        }
    }
}
