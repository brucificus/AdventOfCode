using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2020.Shared
{
    public static class LazyTaskExtensions
    {
        public static ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter GetAwaiter<T>(this Lazy<Task<T>> lazyAsyncWork) => lazyAsyncWork.Value.ConfigureAwait(false).GetAwaiter();
    }
}
