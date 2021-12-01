using System.Runtime.Versioning;

[RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
public readonly struct Undefined : IEqualityOperators<Undefined, Undefined>, IEqualityOperators<Undefined, object>
{
    public bool Equals(Undefined other) => throw new NotImplementedException();

    public override bool Equals(object? obj) => false;

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Undefined left, Undefined right) => false;

    public static bool operator !=(Undefined left, Undefined right) => true;

    public static bool operator ==(Undefined left, object right) => false;

    public static bool operator !=(Undefined left, object right) => true;
}
