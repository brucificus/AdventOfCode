namespace AdventOfCode.Y2020.Shared.Lifetime
{
    public readonly record struct LifetimeBrokerAdapter<TInnerService, TDerivedService>(ILifetimeBroker<TInnerService> Adapted, Func<TInnerService, TDerivedService> ServiceProjection)
        : ILifetimeBroker<TDerivedService>
    {
        public async ValueTask Use(Action<TDerivedService> action)
        {
            var @this = this;
            await Adapted.Use(innerService => action(@this.ServiceProjection(innerService)));
        }

        public async ValueTask<TResult> Use<TResult>(Func<TDerivedService, TResult> action)
        {
            var @this = this;
            return await Adapted.Use(innerService => action(@this.ServiceProjection(innerService)));
        }

        public async Task<TResult> Use<TResult>(Func<TDerivedService, Task<TResult>> action)
        {
            var @this = this;
            return await Adapted.Use(innerService => action(@this.ServiceProjection(innerService)));
        }

        public async ValueTask<TResult> Use<TResult>(Func<TDerivedService, ValueTask<TResult>> action)
        {
            var @this = this;
            return await Adapted.Use(innerService => action(@this.ServiceProjection(innerService)));
        }
    }
}
