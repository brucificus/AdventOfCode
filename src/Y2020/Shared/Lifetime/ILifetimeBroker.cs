namespace AdventOfCode.Y2020.Shared.Lifetime;

/// <summary>
/// Provides initialize-execute-release semantics for instances of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of service to provide initialize-execute-release semantics for.</typeparam>
public interface ILifetimeBroker<out T>
{
    ValueTask Use(Action<T> action);
    ValueTask<TResult> Use<TResult>(Func<T, TResult> action);
    Task<TResult> Use<TResult>(Func<T, Task<TResult>> action);
    ValueTask<TResult> Use<TResult>(Func<T, ValueTask<TResult>> action);

    ILifetimeBroker<TDerived> Project<TDerived>(Func<T, TDerived> serviceProjection) => new LifetimeBrokerAdapter<T,TDerived>(this, serviceProjection);
}
