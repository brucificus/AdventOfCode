public interface ISlopeTrickFunction<TSelf, out TCoefficient, TInput, TProduct, out TConstant, out TResult>
    :  IFunctionSimplex<TInput, TResult>,
      IAdditiveIdentity<TSelf, TSelf>,
      IAdditionOperators<TSelf, TSelf, TSelf>
    where TCoefficient : IMultiplyOperators<TCoefficient, TInput, TProduct>, IAdditionOperators<TCoefficient, TCoefficient, TCoefficient>, IAdditiveIdentity<TCoefficient, TCoefficient>
    where TInput : IComparisonOperators<TInput, TInput>
    where TProduct : IAdditionOperators<TProduct, TConstant, TResult>
    where TConstant : IAdditionOperators<TConstant, TConstant, TConstant>, IAdditiveIdentity<TConstant, TConstant>
    where TSelf : IAdditiveIdentity<TSelf, TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>
{
    TCoefficient M0 { get; }
    TConstant B { get; }
    IEnumerable<(TInput X, nuint SlopeChange)> SlopeChanges { get; }
}

public interface IFunctionSimplex<in TInput, out TResult>
{
    TResult Calculate(TInput x);
}
