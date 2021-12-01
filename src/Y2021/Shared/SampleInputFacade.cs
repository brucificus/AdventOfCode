using System.IO.Abstractions;

namespace AdventOfCode.Y2021.Shared;

public abstract record SampleInputFacadeBase()
{
    public FileInfoBase File { get; init; } =
        new FileInfoWrapper(new FileSystem(), new("sampleInput.txt"));

    public async Task<IReadOnlyList<string>> ReadAllLinesAsync(CancellationToken cancellationToken = default) =>
        (await System.IO.File.ReadAllLinesAsync(File.FullName, cancellationToken)).ToImmutableArray();

    public async Task<string> ReadAllTextAsync(CancellationToken cancellationToken = default) =>
        await System.IO.File.ReadAllTextAsync(File.FullName, cancellationToken);
}

public sealed record SampleInputFacade() : SampleInputFacadeBase()
{
    public Lazy<Task<TResult>> LazilyAwait<TResult>(Func<SampleInputFacadeBase, Task<TResult>> action) =>
        new(async () => await action(this));
}
