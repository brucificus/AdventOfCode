using System.IO.Abstractions;

namespace AdventOfCode.Y2020.Shared;

public abstract record InputFileFacadeBase()
{
    public FileInfoBase File { get; init; } =
        new FileInfoWrapper(new FileSystem(), new("input.txt"));

    public async Task<IReadOnlyList<string>> ReadAllLinesAsync(CancellationToken cancellationToken = default) =>
        (await System.IO.File.ReadAllLinesAsync(File.FullName, cancellationToken)).ToImmutableArray();

    public async Task<string> ReadAllTextAsync(CancellationToken cancellationToken = default) =>
        await System.IO.File.ReadAllTextAsync(File.FullName, cancellationToken);
}

public sealed record InputFileFacadeFacade() : InputFileFacadeBase()
{
    public Lazy<Task<TResult>> LazilyAwait<TResult>(Func<InputFileFacadeBase, Task<TResult>> action) =>
        new(async () => await action(this));
}
