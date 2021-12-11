/// <summary>
/// Represents a continuous positively-concave piecewise linear function.
/// That is, a linear function of the form `Y = (m*x)+b`, which can be continuously scanned from left to right with the slope changing - but only _decreasing_ by a number of integer steps - at given X-intercepts.
/// </summary>
/// <remarks>
///  See: https://old.reddit.com/r/adventofcode/comments/rbeizh/2021_day_7_applying_slope_trick_from_competitive/
/// </remarks>
public readonly record struct PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult>(TCoefficient M0, TConstant B, IEnumerable<(TInput X, nuint SlopeChange)> SlopeChanges)
    : IAdditiveIdentity<PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult>, PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult>>
    where TCoefficient : IMultiplyOperators<TCoefficient, TInput, TProduct>, IAdditionOperators<TCoefficient, TCoefficient, TCoefficient>, IIncrementOperators<TCoefficient>, IAdditiveIdentity<TCoefficient, TCoefficient>
    where TInput : IComparisonOperators<TInput, TInput>
    where TProduct : IAdditionOperators<TProduct, TConstant, TResult>
    where TConstant : IAdditionOperators<TConstant, TConstant, TConstant>, IAdditiveIdentity<TConstant, TConstant>
{
    public static PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult> AdditiveIdentity { get; } = new(TCoefficient.AdditiveIdentity, TConstant.AdditiveIdentity, Enumerable.Empty<(TInput X, nuint SlopeChange)>());

    public IEnumerable<(TInput X, nuint SlopeChange)> SlopeChanges { get; } =
        SlopeChanges as IBuffer<(TInput X, nuint SlopeChange)>
        ?? SlopeChanges as IReadOnlyList<(TInput X, nuint SlopeChange)> as IEnumerable<(TInput X, nuint SlopeChange)>
        ?? SlopeChanges.Share();

    public TResult Calculate(TInput x)
    {
        static TCoefficient AdjustCoefficient(TCoefficient coefficient, nuint steps)
        {
            for (nuint i = 0; i < steps; i++)
            {
                coefficient++;
            }

            return coefficient;
        }

        var m = SlopeChanges.TakeWhile(v => v.X <= x).Aggregate(M0, (p, v) => AdjustCoefficient(p, v.SlopeChange));
        return (m * x) + B;
    }

    public static PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult> operator +(PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult> left, PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult> right)
    {
        var mergedSlopeChanges =
            SlopeTrickFunctionHelpers.MergeObstensiblySameSortedSlopeChanges(left.SlopeChanges, right.SlopeChanges).Memoize();

        return new PositiveSlopeTrickFunction<TCoefficient, TInput, TProduct, TConstant, TResult>(left.M0 + right.M0, left.B + right.B, mergedSlopeChanges);
    }
}
