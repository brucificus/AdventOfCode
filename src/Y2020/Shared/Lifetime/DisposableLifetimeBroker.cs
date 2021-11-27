using System.Reflection;

namespace AdventOfCode.Y2020.Shared.Lifetime;

/// <summary>
/// Provides initialize-execute-release semantics for instances of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of service to provide initialize-execute-release semantics for.</typeparam>
/// <param name="Factory">A factory that creates instances of <typeparamref name="T"/>.</param>
/// <param name="DisposeInstance">Provides the release semantics for instances of <typeparamref name="T"/>. Optional if <typeparamref name="T"/> implements either <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>.</param>
///
public readonly record struct DisposableLifetimeBroker<T>(Func<T> Factory, Func<T,ValueTask>? DisposeInstance = default)
    : ILifetimeBroker<T>
{
    private static Func<T, ValueTask>? DefaultServiceDisposal { get; } = GetDefaultServiceAsyncDisposal() ?? GetDefaultServiceSyncDisposal();

    public Func<T, ValueTask> DisposeInstance { get; } = DisposeInstance ??
                                                         DefaultServiceDisposal ??
                                                         throw new ArgumentNullException(nameof(DisposeInstance));
    public async ValueTask Use(Action<T> action)
    {
        var instance = Factory();
        try
        {
            action(instance);
        }
        finally
        {
            await DisposeInstance(instance);
        }
    }

    public async ValueTask<TResult> Use<TResult>(Func<T, TResult> action)
    {
        var instance = Factory();
        try
        {
            return action(instance);
        }
        finally
        {
            await DisposeInstance(instance);
        }
    }

    public async Task<TResult> Use<TResult>(Func<T, Task<TResult>> action)
    {
        var instance = Factory();
        try
        {
            return await action(instance);
        }
        finally
        {
            await DisposeInstance(instance);
        }
    }

    public async ValueTask<TResult> Use<TResult>(Func<T, ValueTask<TResult>> action)
    {
        var instance = Factory();
        try
        {
            return await action(instance);
        }
        finally
        {
            await DisposeInstance(instance);
        }
    }

    private static Func<T, ValueTask>? GetDefaultServiceAsyncDisposal()
    {
        if (typeof(T).IsAssignableTo(typeof(IAsyncDisposable)))
        {
            return AsyncInterfaceDisposalInvocation;
        }

        var bareDisposalMethod = typeof(T).GetMethod(nameof(IAsyncDisposable.DisposeAsync), BindingFlags.Instance | BindingFlags.Public);

        if (bareDisposalMethod != null)
        {
            throw new NotImplementedException();
        }

        return default;
    }

    private static Func<T, ValueTask>? GetDefaultServiceSyncDisposal()
    {
        if (typeof(T).IsAssignableTo(typeof(IDisposable)))
        {
            return SyncInterfaceDisposalAsyncInvocationShunt;
        }

        var bareDisposalMethod = typeof(T).GetMethod(nameof(IDisposable.Dispose), BindingFlags.Instance | BindingFlags.Public);

        if (bareDisposalMethod != null)
        {
            throw new NotImplementedException();
        }

        return default;
    }

#pragma warning disable CS1998
    private static async ValueTask SyncInterfaceDisposalAsyncInvocationShunt(T instance) => (((instance ?? throw new ArgumentNullException(nameof(instance))) as IDisposable) ?? throw new InvalidOperationException()).Dispose();
#pragma warning restore CS1998

    private static async ValueTask AsyncInterfaceDisposalInvocation(T instance) => await (((instance ?? throw new ArgumentNullException(nameof(instance))) as IAsyncDisposable) ?? throw new InvalidOperationException()).DisposeAsync();
}
