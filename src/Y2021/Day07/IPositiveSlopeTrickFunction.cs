public interface IPositiveSlopeTrickFunction<TSelf, out TCoefficient, TInput, TProduct, out TConstant, out TResult>
    :  ISlopeTrickFunction<TSelf, TCoefficient, TInput, TProduct, TConstant, TResult>
    where TCoefficient : IMultiplyOperators<TCoefficient, TInput, TProduct>, IAdditionOperators<TCoefficient, TCoefficient, TCoefficient>, IAdditiveIdentity<TCoefficient, TCoefficient>, IIncrementOperators<TCoefficient>
    where TInput : IComparisonOperators<TInput, TInput>
    where TProduct : IAdditionOperators<TProduct, TConstant, TResult>
    where TConstant : IAdditionOperators<TConstant, TConstant, TConstant>, IAdditiveIdentity<TConstant, TConstant>
    where TSelf : IAdditiveIdentity<TSelf, TSelf>, IAdditionOperators<TSelf, TSelf, TSelf>
{
}
