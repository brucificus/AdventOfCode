/// <summary>
/// Represents a continuous positively-concave piecewise linear function.
/// That is, a linear function of the form `Y = (m*x)+b`, which can be continuously scanned from left to right with the slope changing - but only _decreasing_ by a number of integer steps - at given X-intercepts.
/// </summary>
/// <remarks>
///  See: https://old.reddit.com/r/adventofcode/comments/rbeizh/2021_day_7_applying_slope_trick_from_competitive/
/// </remarks>
public readonly record struct PositiveSlopeTrickFunction<TNumber>(TNumber M0, TNumber B, IEnumerable<(TNumber X, nuint SlopeChange)> SlopeChanges)
    : IPositiveSlopeTrickFunction<PositiveSlopeTrickFunction<TNumber>, TNumber, TNumber, TNumber, TNumber, TNumber>
    where TNumber : INumber<TNumber>
{
    public static PositiveSlopeTrickFunction<TNumber> AdditiveIdentity { get; } = new(TNumber.AdditiveIdentity, TNumber.AdditiveIdentity, Enumerable.Empty<(TNumber X, nuint SlopeChange)>());

    public IEnumerable<(TNumber X, nuint SlopeChange)> SlopeChanges { get; } =
        SlopeChanges as IBuffer<(TNumber X, nuint SlopeChange)>
        ?? SlopeChanges as IReadOnlyList<(TNumber X, nuint SlopeChange)> as IEnumerable<(TNumber X, nuint SlopeChange)>
        ?? SlopeChanges.Memoize();

    public TNumber Calculate(TNumber x)
    {
        static TNumber AdjustCoefficient(TNumber coefficient, nuint steps)
        {
            for (nuint i = 0; i < steps; i++)
            {
                coefficient++;
            }

            return coefficient;
        }

        var m = SlopeChanges.TakeWhile(v => v.X < x).Aggregate(M0, (p, v) => AdjustCoefficient(p, v.SlopeChange));
        return (m * x) + B;
    }

    public static PositiveSlopeTrickFunction<TNumber> operator +(PositiveSlopeTrickFunction<TNumber> left, PositiveSlopeTrickFunction<TNumber> right)
    {
        var mergedSlopeChanges =
            SlopeTrickFunctionHelpers.MergeObstensiblySameSortedSlopeChanges(left.SlopeChanges, right.SlopeChanges).Memoize();

        return new PositiveSlopeTrickFunction<TNumber>(left.M0 + right.M0, left.B + right.B, mergedSlopeChanges);
    }
}
